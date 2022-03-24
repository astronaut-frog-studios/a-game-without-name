using UnityEngine;

public enum RoundState
{
    WAVES_STARTED,
    ROUND_STARTED,
    WAITING,
    FINISHED
}

public class RoundsManager : PersistentSingleton<RoundsManager>
{
    [SerializeField] private RoundState state = RoundState.ROUND_STARTED;

    [SerializeField] private float timeBetweenRounds = 10f;
    [SerializeField] private float roundCountdown;

    [SerializeField] private int currentRound;
    public int getCurrentRound => currentRound;

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
            roundCountdown = timeBetweenRounds;
            state = RoundState.ROUND_STARTED;

            return;
        }

        if (state == RoundState.WAVES_STARTED) return;

        if (roundCountdown <= 0)
        {
            AudioSystem.Instance.StopPlaying("safe-area");
            // tocar a musica da fase

            TurnLightsOff?.Invoke();
            StartedWave?.Invoke();
            state = RoundState.WAVES_STARTED;

            return;
        }

        if (state == RoundState.ROUND_STARTED)
        {
            currentRound++;

            AudioSystem.Instance.PlayMusic("safe-area");
            TurnLightsOn?.Invoke();

            state = RoundState.WAITING;
        }

        roundCountdown -= Time.deltaTime;
        print("SafeZone time");
        // está entre os rounds, safe zone. Falar com npcs, comprar coisas e tudo mais
    }

    private void FinishedRound() => state = RoundState.FINISHED;
}