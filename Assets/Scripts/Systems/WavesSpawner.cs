using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaveState
{
    SPAWNING,
    WAITING,
    COUNTING,
    FINISHED,
    INACTIVE
}

public class WavesSpawner : MonoBehaviour
{
    [SerializeField] private WaveState state = WaveState.INACTIVE;

    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private float spawnRadius = 5f;

    [SerializeField] private List<Wave> waves = new List<Wave>();
    private int nextWaveIndex;

    [SerializeField] private float timeBetweenWaves = 2.4f;
    [SerializeField] private float waveCountdown;

    [SerializeField] private float enemySearchTime = 1.2f;
    private float enemySearchCountdown;

    public delegate void WaveFinishedHandler();
    public event WaveFinishedHandler WaveFinished;

    private readonly WaveSpawnerHelpers waveSpawnerHelpers = new WaveSpawnerHelpers();

    private void Start()
    {

        waveCountdown = timeBetweenWaves;
        enemySearchCountdown = enemySearchTime;

        RoundsManager.Instance.StartedWave += StartSpawning;

        waves.AddRange(waveSpawnerHelpers.CreateBaseWaves());
    }

    private void Update()
    {
        if (state == WaveState.FINISHED)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length != 0) return;

            waves.Clear();
            WaveFinished?.Invoke();
            state = WaveState.INACTIVE;
            return;
        }

        if (state == WaveState.INACTIVE) return;

        CheckWaveCountdown();

        if (state == WaveState.WAITING)
        {
            if (!canSpawnEnemies()) return;

            OnWaveCompleted();
            return;
        }

        if (state == WaveState.COUNTING)
        {
            StartCoroutine(SpawnWave(waves[nextWaveIndex]));
        }
    }

    private bool isSpecialRound() => GameManager.Instance.getCurrentRound % Difficulty.Instance.specialRoundsNumber == 0;

    private void StartSpawning()
    {
        if (GameManager.Instance.getCurrentRound != 1)
        {
            waveSpawnerHelpers.UpdateWaves();
            waves.AddRange(waveSpawnerHelpers.baseWavesList);
        }

        if (isSpecialRound())
        {
            waves.Add(waveSpawnerHelpers.CreateSpecialWave());
        }

        state = WaveState.COUNTING;
    }

    private void CheckWaveCountdown()
    {
        if (waveCountdown > 0)
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    private bool canSpawnEnemies()
    {
        enemySearchCountdown -= Time.deltaTime;
        if (enemySearchCountdown > 0) return false;

        enemySearchCountdown = enemySearchTime;
        return waveCountdown <= 0;
    }

    private void SpawnEnemy(GameObject _enemyToSpawn)
    {
        // Instantiate(_enemyToSpawn, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);

        var spawnPositionX = LevelBounds.RandomPosX;
        var spawnPositionY = LevelBounds.RandomPosY;

        Instantiate(_enemyToSpawn, new Vector2(spawnPositionX, spawnPositionY), Quaternion.identity);
    }

    private void OnWaveCompleted()
    {
        state = WaveState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if (nextWaveIndex + 1 > waves.Count - 1)
        {
            state = WaveState.FINISHED;
            nextWaveIndex = 0;
            return;
        }

        nextWaveIndex++;
    }

    private IEnumerator SpawnWave(Wave _wave)
    {
        state = WaveState.SPAWNING;

        for (var i = 0; i < _wave.numberOfEnemies; i++)
        {
            var timeToSpawn = i == 0 ? 0f : 1f / _wave.spawnRate;
            yield return new WaitForSeconds(timeToSpawn);
            SpawnEnemy(_wave.enemiesToSpawn[Random.Range(0, _wave.enemiesToSpawn.Length)]);
        }

        state = WaveState.WAITING;
        waveCountdown = timeBetweenWaves;

        yield return null;
    }
}