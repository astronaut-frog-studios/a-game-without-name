using UnityEngine;

[System.Serializable]
public struct Wave
{
    public string name;
    public GameObject[] enemiesToSpawn;
    public int numberOfEnemies;
    public float spawnRate;

    public Wave(string name, GameObject[] enemiesToSpawn, int numberOfEnemies, float spawnRate)
    {
        this.name = name;
        this.enemiesToSpawn = enemiesToSpawn;
        this.numberOfEnemies = numberOfEnemies;
        this.spawnRate = spawnRate;
    }
} 