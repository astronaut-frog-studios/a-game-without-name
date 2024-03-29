using UnityEngine;

public enum RoundState
{
    WAVES_STARTED,
    ROUND_STARTED,
    WAITING,
    FINISHED
}

public class RoundsManager : Singleton<RoundsManager>
{
    [SerializeField] private RoundState state = RoundState.ROUND_STARTED;

    [SerializeField] private float timeBetweenRounds = 10f;
    [SerializeField] private float roundCountdown;
    private WavesSpawner wavesSpawner;
    public delegate void StartWaveHandler();
    public event StartWaveHandler StartedWave;

    public delegate void TurnLightsOffHandler();
    public event TurnLightsOffHandler TurnLightsOff;
    public delegate void TurnLightsOnHandler();
    public event TurnLightsOnHandler TurnLightsOn;

    protected override void Awake()
    {
        base.Awake();
        wavesSpawner = FindObjectOfType<WavesSpawner>();
    }

    private void Start()
    {
        AudioSystem.Instance.PlayMusic("rain");

        roundCountdown = timeBetweenRounds;

        wavesSpawner.WaveFinished += FinishedRound;
    }

    private void OnDestroy()
    {
        wavesSpawner.WaveFinished -= FinishedRound;
    }

    private void Update()
    {
        if (state == RoundState.FINISHED)
        {
            if (GameManager.Instance.canStartWaves)
            {
                gameManager.DestroyLevel(gameManager.getCurrentRound);
                return;
            }

            roundCountdown = timeBetweenRounds;
            state = RoundState.ROUND_STARTED;

            return;
        }

        if (state is RoundState.WAVES_STARTED) return;

        if (roundCountdown <= 0)
        {
            AudioSystem.Instance.StopPlaying("safe-area");
            // tocar a musica da fase

            if (!GameManager.Instance.canStartWaves)
            {
                gameManager.LoadLevel(gameManager.getCurrentRound);
                return;
            }

            TurnLightsOff?.Invoke();
            StartedWave?.Invoke();
            gameManager.displayRound.gameObject.SetActive(true);

            state = RoundState.WAVES_STARTED;

            return;
        }

        if (state == RoundState.ROUND_STARTED)
        {
            gameManager.SetCurrentRound();
            if (gameManager.getCurrentRound > LevelDesigns.Instance.getMaxRounds)
            {
                gameManager.displayRound.gameObject.SetActive(false);
                gameManager.OnGameEnded();
                return;
            }

            AudioSystem.Instance.PlayMusic("safe-area");
            TurnLightsOn?.Invoke();

            if (gameManager.getCurrentRound > 1)
            {
                PlayerEvents.OnPlayerDifficultyChange();
                Difficulty.OnEnemyDifficultyChange();
            }

            state = RoundState.WAITING;
        }

        roundCountdown -= Time.deltaTime;
        print("SafeZone time");
        gameManager.displayRound.gameObject.SetActive(false);
        // está entre os rounds, safe zone. Falar com npcs, comprar coisas e tudo mais
    }

    private void FinishedRound() => state = RoundState.FINISHED;

    private GameManager gameManager => GameManager.Instance;
}