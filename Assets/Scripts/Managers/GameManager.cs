using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

internal enum GameState
{
    WAITING,
    STARTED,
    ENDED,
    TESTING
}

public class GameManager : Singleton<GameManager>
{
    public static bool historyMode = true;

    [SerializeField]
    private GameState state = GameState.WAITING;

    public bool isPaused;
    [SerializeField] private GameObject pausePanel;
    public bool canStartWaves;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject transitionPanel, winPanel, defeatPanel;
    [ReadOnly, SerializeField] private int currentRound;
    public int getCurrentRound => currentRound;
    [SerializeField] public Text displayRound;

    private LevelDesigns levelDesigns;

    public event UnityAction<bool> GameEnded;
    public void OnGameEnded(bool win = true) => GameEnded?.Invoke(win);

    private void Start()
    {
        state = GameState.STARTED;
        isPaused = false;

        currentRound = SaveGameManager.Instance.getSavedRound;
        levelDesigns = LevelDesigns.Instance;
        GameEnded += EndGame;
    }

    private void Update()
    {
        if (state is GameState.TESTING) return;

        if (isPaused)
        {
            if (state != GameState.ENDED)
            {
                pausePanel.SetActive(true);
            }

            AudioSystem.Instance.MuteAll();
            Time.timeScale = 0;
            return;
        }

        pausePanel.SetActive(false);
        AudioSystem.Instance.UnmuteAll();
        Time.timeScale = 1;
    }

    public void SetCurrentRound()
    {
        currentRound++;
        displayRound.text = currentRound.ToString();
    }

    private void EndGame(bool win)
    {
        isPaused = true;
        state = GameState.ENDED;

        if (win)
        {
            winPanel.SetActive(true);
            return;
        }

        defeatPanel.SetActive(true);
    }

    #region Levels
    public void LoadLevel(int currentRound)
    {
        StartCoroutine(CallLoadingScreen(true));

        levelDesigns.Levels[currentRound].SetActive(true);
        levelDesigns.SetActiveEnemySpawn(currentRound, true);
        levelDesigns.SetCameraConfiner(currentRound);

        player.position = levelDesigns.GetPlayerSpawn(currentRound);
    }

    public void DestroyLevel(int currentRound)
    {
        levelDesigns.SetActiveEnemySpawn(currentRound, false);
        levelDesigns.Levels[currentRound].SetActive(false);

        LoadSafeZone();
    }

    public void LoadSafeZone()
    {
        StartCoroutine(CallLoadingScreen(false));

        levelDesigns.SetCameraConfiner();
        player.position = levelDesigns.GetPlayerSpawn();
    }

    private IEnumerator CallLoadingScreen(bool startWaves)
    {
        transitionPanel.SetActive(true);
        var animationLength = transitionPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(animationLength * 0.7f);
        canStartWaves = startWaves;

        yield return new WaitForSeconds(animationLength);
        transitionPanel.SetActive(false);

    }
    #endregion
}
