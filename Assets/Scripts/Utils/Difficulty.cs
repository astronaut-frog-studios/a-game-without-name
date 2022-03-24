using UnityEngine;

public class Difficulty : PersistentSingleton<Difficulty>
{
    [SerializeField] private DifficultyObject difficultyObject;

    [ReadOnly] public int numberOfEnemiesMultiplier;
    [ReadOnly] public float spawnRateMultiplier;

    [ReadOnly, SerializeField] private int numberOfEnemies;
    [ReadOnly, SerializeField] private float spawnRate;

    [HideInInspector] public int specialRoundsNumber;
    [HideInInspector] public int specialWaveLenght;
    
    public float SpawnRateDifficulty => spawnRate += spawnRateMultiplier;
    public int EnemiesNumberDifficulty => numberOfEnemies += numberOfEnemiesMultiplier;
    
    private void Start()
    {
        SetNumberOfEnemies();
        SetSpawnRate();
        SetSpecialWave();
    }

    private void SetNumberOfEnemies()
    {
        numberOfEnemiesMultiplier = difficultyObject.numberOfEnemiesMultiplier;
        numberOfEnemies = difficultyObject.numberOfEnemies;
    }

    private void SetSpawnRate()
    {
        spawnRateMultiplier = difficultyObject.spawnRateMultiplier;
        spawnRate = difficultyObject.spawnRate;
    }

    private void SetSpecialWave()
    {
        specialRoundsNumber = difficultyObject.specialRoundsNumber;
        specialWaveLenght = difficultyObject.specialWaveLenght;
    }

    public void OnSave()
    {
        difficultyObject.numberOfEnemiesMultiplier = numberOfEnemiesMultiplier;
        difficultyObject.numberOfEnemies = numberOfEnemies;

        difficultyObject.spawnRateMultiplier = spawnRateMultiplier;
        difficultyObject.spawnRate = spawnRate;
    }
}