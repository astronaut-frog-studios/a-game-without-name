using UnityEngine;

public class Difficulty : PersistentSingleton<Difficulty>
{
    [SerializeField] private DifficultyObject difficultyObject;

    [Header("Waves")]
    [ReadOnly] public int numberOfEnemiesMultiplier;
    [ReadOnly, SerializeField] private int numberOfEnemies;
    [HideInInspector] public int specialRoundsNumber;
    [HideInInspector] public int specialWaveLenght;
    [ReadOnly] public float spawnRateMultiplier;
    [ReadOnly, SerializeField] private float spawnRate;

    [Header("Enemies")]
    [HideInInspector] public float enemyHealthMultiplier;
    [ReadOnly, SerializeField] public float enemyHealth;
    [HideInInspector] public float enemyDamageMultiplier;
    [ReadOnly, SerializeField] public float enemyDamage;
    [HideInInspector] public float enemyCooldownMultiplier;
    [ReadOnly, SerializeField] public float enemyCooldown;

    [Header("Player")]
    [HideInInspector] public float playerHealthMultiplier;
    [ReadOnly, SerializeField] public float playeryHealth;
    [HideInInspector] public float playerDamageMultiplier;
    [ReadOnly, SerializeField] public float playerDamage;

    public float SpawnRateDifficulty => spawnRate += spawnRateMultiplier;
    public int EnemiesNumberDifficulty => numberOfEnemies += numberOfEnemiesMultiplier;

    private void Start()
    {
        SetNumberOfEnemies();
        SetSpawnRate();
        SetSpecialWave();
        SetEnemies();
        SetPlayer();
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

    private void SetEnemies()
    {
        enemyDamageMultiplier = difficultyObject.enemyDamageMultiplier;
        enemyDamage = difficultyObject.enemyDamage;
        enemyHealthMultiplier = difficultyObject.enemyHealthMultiplier;
        enemyHealth = difficultyObject.enemyHealth;
        enemyCooldownMultiplier = difficultyObject.enemyCooldownMultiplier;
        enemyCooldown = difficultyObject.enemyCooldown;
    }

    private void SetPlayer()
    {
        playerHealthMultiplier = difficultyObject.playerHealthMultiplier;
        playeryHealth = difficultyObject.playeryHealth;
        playerDamageMultiplier = difficultyObject.playerDamageMultiplier;
        playerDamage = difficultyObject.playerDamage;
    }

    public void OnSave()
    {
        difficultyObject.numberOfEnemiesMultiplier = numberOfEnemiesMultiplier;
        difficultyObject.numberOfEnemies = numberOfEnemies;
        difficultyObject.spawnRateMultiplier = spawnRateMultiplier;
        difficultyObject.spawnRate = spawnRate;

        difficultyObject.enemyDamageMultiplier = enemyDamageMultiplier;
        difficultyObject.enemyDamage = enemyDamage;
        difficultyObject.enemyHealthMultiplier = enemyHealthMultiplier;
        difficultyObject.enemyHealth = enemyHealth;
        difficultyObject.enemyCooldownMultiplier = enemyCooldownMultiplier;
        difficultyObject.enemyCooldown = enemyCooldown;

        difficultyObject.playerHealthMultiplier = playerHealthMultiplier;
        difficultyObject.playeryHealth = playeryHealth;
        difficultyObject.playerDamageMultiplier = playerDamageMultiplier;
        difficultyObject.playerDamage = playerDamage;
    }
}