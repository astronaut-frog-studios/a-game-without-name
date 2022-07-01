using UnityEngine;

[CreateAssetMenu(fileName = "Difficulty_SO", menuName = "ScriptableObjects/Difficulty")]
public class DifficultyObject : ScriptableObject
{
    [Header("Waves")]
    public int numberOfEnemiesMultiplier = 2;
    public int numberOfEnemies = 2;
    public float spawnRateMultiplier = 0.04f;
    public float spawnRate = 0.16f;
    public int specialRoundsNumber = 4;
    public int specialWaveLenght = 3;

    [Header("Enemies")]
    public float enemyHealthMultiplier = 2f;
    public float enemyHealth = 0;
    public float enemyDamageMultiplier = 0.6f;
    public float enemyDamage = 0;
    public float enemyCooldownMultiplier = 0.2f;
    public float enemyCooldown = 0;
    public float enemyBulletSpeedMultiplier = 0.4f;
    public float enemyBulletSpeed = 0;
    public int enemyNumberOfBulletsMultiplier = 1;
    public int enemyNumberOfBullets = 1;

    [Header("Player")]
    public float playerHealthMultiplier = 0.4f;
    public float playeryHealth = 0;
    public float playerDamageMultiplier = 0.2f;
    public float playerDamage = 0;
}