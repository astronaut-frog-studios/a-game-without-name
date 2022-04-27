using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveSpawnerHelpers
{
    public readonly List<Wave> baseWavesList = new List<Wave>();

    private List<GameObject> normalEnemies() =>
        EnemiesTypes.Instance.enemies.Where(enemy => enemy.name != "Enemy4").ToList();
    
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

        var enemyHorde = Helpers.FindInArrayByName(EnemiesTypes.Instance.enemies, "Enemy4");
        randomEnemies.Add(enemyHorde);

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
            0 => new[] {EnemiesTypes.Instance.enemies[index]},
            1 => new[] {EnemiesTypes.Instance.enemies[0], GetRandomNormalEnemies(3, 1, from: 1).First()},
            _ => normalEnemies().ToArray()
        };
    }
}