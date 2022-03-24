using UnityEngine;

[CreateAssetMenu(fileName = "Difficulty_SO", menuName = "ScriptableObjects/Difficulty")]
public class DifficultyObject : ScriptableObject
{
    public int numberOfEnemiesMultiplier = 2;
    public int numberOfEnemies = 2;
    
    public float spawnRateMultiplier = 0.04f;
    public float spawnRate = 0.16f;

    public int specialRoundsNumber = 4;
    public int specialWaveLenght = 3;
}