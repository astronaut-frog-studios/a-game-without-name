using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveSpawnerHelpers
{
    public readonly List<Wave> baseWavesList = new List<Wave>();

    private List<GameObject> normalEnemies() =>
        EnemyTypes.Instance.enemies.Where(enemy => enemy.name != "Enemy2_33").ToList();

    private List<GameObject> GetRandomNormalEnemies(int to, int numberOfEnemies, int from = 0)
    {
        var randomEnemiesNumber =
            Helpers.RandomNumberWithoutDuplicate(to, numberOfEnemies, from);
        var randomEnemies = randomEnemiesNumber.Select(numberIndex => normalEnemies()[numberIndex]).ToList();

        return randomEnemies;
    }

    public Wave CreateSpecialWave()
    {
        var randomEnemies = GetRandomNormalEnemies(normalEnemies().Count, Difficulty.Instance.specialWaveLenght - 1);

        var enemyLmg = Helpers.FindInArrayByName(EnemyTypes.Instance.enemies, "Enemy2_33");
        randomEnemies.Add(enemyLmg);

        return new Wave("4", randomEnemies.ToArray(),
            20 + Difficulty.Instance.numberOfEnemiesMultiplier,
            0.60f + Difficulty.Instance.spawnRateMultiplier);
    }

    public IEnumerable<Wave> CreateBaseWaves()
    {
        for (var i = 0; i < 3; i++)
        {
            var enemies = enemiesArray(i);

            var waveName = "Wave: " + (i + 1);
            var wave = new Wave(waveName, enemies, Difficulty.Instance.EnemiesNumberDifficulty,
                Difficulty.Instance.SpawnRateDifficulty);

            baseWavesList.Add(wave);
        }

        return baseWavesList;
    }

    public void UpdateWaves()
    {
        var updatedWave = baseWavesList
            .Select(w =>
            {
                w.numberOfEnemies = Difficulty.Instance.EnemiesNumberDifficulty;
                w.spawnRate = Difficulty.Instance.SpawnRateDifficulty;

                return w;
            }).ToList();

        baseWavesList.Clear();
        baseWavesList.AddRange(updatedWave);
    }

    private GameObject[] enemiesArray(int index)
    {
        return index switch
        {
            0 => new[] { EnemyTypes.Instance.enemies[index], GetRandomNormalEnemies(EnemyTypes.Instance.enemies.Length - 2, 1, from: 1).First() },
            1 => new[] { EnemyTypes.Instance.enemies[0], GetRandomNormalEnemies(EnemyTypes.Instance.enemies.Length - 2, 1, from: 1).First(), GetRandomNormalEnemies(EnemyTypes.Instance.enemies.Length - 1, 1, from: 1).First() },
            _ => normalEnemies().ToArray()
        };
    }
}