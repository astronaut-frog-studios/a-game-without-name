using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public static bool historyMode = true;
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
        currentRound = SaveGameManager.Instance.getSavedRound;
        levelDesigns = LevelDesigns.Instance;
        GameEnded += EndGame;
    }

    public void SetCurrentRound()
    {
        currentRound++;
        displayRound.text = currentRound.ToString();
    }

    private void EndGame(bool win)
    {
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
        player.position = levelDesigns.GetPlayerSpawn(currentRound);
    }

    public void DestroyLevel(int currentRound)
    {
        levelDesigns.DestroyEnemySpawn(currentRound);
        levelDesigns.Levels[currentRound].SetActive(false);

        LoadSafeZone();
    }

    public void LoadSafeZone()
    {
        StartCoroutine(CallLoadingScreen(false));
        player.position = levelDesigns.GetPlayerSpawn();
    }

    private IEnumerator CallLoadingScreen(bool startWaves)
    {
        transitionPanel.SetActive(true);
        var animationLength = transitionPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(animationLength);

        transitionPanel.SetActive(false);
        canStartWaves = startWaves;
    }
    #endregion
}
