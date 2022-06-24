using System.Linq;
using UnityEngine;

public class LevelDesigns : Singleton<LevelDesigns>
{
    public GameObject[] Levels;
    [ReadOnly, SerializeField] private int MaxRounds;
    public int getMaxRounds => MaxRounds;

    private void Start()
    {
        MaxRounds = Levels.Length - 1;
    }

    public Vector3 GetPlayerSpawn(int index = 0)
    {
        var allTransforms = Levels[index].GetComponentsInChildren<Transform>();
        var spawnTransform = allTransforms.Where(k => k.name == "SpawnPlayer").First();

        return spawnTransform.position;
    }

    public void DestroyEnemySpawn(int index)
    {
        var allEnemySpawns = Levels[index].GetComponentsInChildren<Transform>();
        var enemySpawn = allEnemySpawns.Where(k => k.name == "SpawnArea").First();

        enemySpawn.GetComponent<LevelBounds>().enabled = false;
    }
}
