using UnityEngine;
using UnityEngine.Events;

public class Difficulty : Singleton<Difficulty>
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
    [HideInInspector] public float enemyBulletSpeedMultiplier = 0.2f;
    [ReadOnly, SerializeField] public float enemyBulletSpeed = 0;
    [HideInInspector] public int enemyNumberOfBulletsMultiplier = 1;
    [ReadOnly, SerializeField] public int enemyNumberOfBullets = 1;

    [Header("Player")]
    [HideInInspector] public float playerHealthMultiplier;
    [ReadOnly, SerializeField] public float playerHealth;
    [ReadOnly, SerializeField] public float playerMaxHealth;
    [HideInInspector] public float playerDamageMultiplier;
    [ReadOnly, SerializeField] public float playerDamage;

    public float SpawnRateDifficulty => spawnRate += spawnRateMultiplier;
    public int EnemiesNumberDifficulty => numberOfEnemies += numberOfEnemiesMultiplier;

    public static event UnityAction EnemyDifficulty;
    public static void OnEnemyDifficultyChange() => EnemyDifficulty?.Invoke();

    protected override void Awake()
    {
        base.Awake();

        SetNumberOfEnemies();
        SetSpawnRate();
        SetSpecialWave();
        SetEnemies();
        SetPlayer();
    }

    private void Start()
    {
        EnemyDifficulty += UpdateEnemyValues;
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

        enemyBulletSpeedMultiplier = difficultyObject.enemyBulletSpeedMultiplier;
        enemyBulletSpeed = difficultyObject.enemyBulletSpeed;
        enemyNumberOfBulletsMultiplier = difficultyObject.enemyNumberOfBulletsMultiplier;
        enemyNumberOfBullets = difficultyObject.enemyNumberOfBullets;
    }

    private void SetPlayer()
    {
        playerHealthMultiplier = difficultyObject.playerHealthMultiplier;
        playerHealth = difficultyObject.playeryHealth;
        playerMaxHealth = difficultyObject.playeryMaxHealth;
        playerDamageMultiplier = difficultyObject.playerDamageMultiplier;
        playerDamage = difficultyObject.playerDamage;
    }

    public void UpdatePlayerValues()
    {
        playerHealth += playerHealthMultiplier;
        playerMaxHealth += playerHealthMultiplier;
        playerDamage += playerDamageMultiplier;
    }

    public void UpdateEnemyValues()
    {
        enemyHealth += enemyHealthMultiplier;
        enemyDamage += enemyDamageMultiplier;
        enemyCooldown -= enemyCooldownMultiplier;
        enemyBulletSpeed += enemyBulletSpeedMultiplier;
        enemyNumberOfBullets += enemyNumberOfBulletsMultiplier;
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
        difficultyObject.enemyBulletSpeedMultiplier = enemyBulletSpeedMultiplier;
        difficultyObject.enemyBulletSpeed = enemyBulletSpeed;
        difficultyObject.enemyNumberOfBulletsMultiplier = enemyNumberOfBulletsMultiplier;
        difficultyObject.enemyNumberOfBullets = enemyNumberOfBullets;

        difficultyObject.playerHealthMultiplier = playerHealthMultiplier;
        difficultyObject.playeryHealth = playerHealth;
        difficultyObject.playerDamageMultiplier = playerDamageMultiplier;
        difficultyObject.playerDamage = playerDamage;
    }
}