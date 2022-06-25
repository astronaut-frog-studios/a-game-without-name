using System.Linq;
using UnityEngine;
using Cinemachine;

public class LevelDesigns : Singleton<LevelDesigns>
{
    public Collider2D[] LevelCameraConfiners;
    [SerializeField] private CinemachineConfiner CameraConfiner;
    public GameObject[] Levels;
    [ReadOnly, SerializeField] private int MaxRounds;
    public int getMaxRounds => MaxRounds;

    private void Start()
    {
        MaxRounds = Levels.Length - 1;
    }

    public void SetCameraConfiner(int index = 0) => CameraConfiner.m_BoundingShape2D = LevelCameraConfiners[index];

    public Vector3 GetPlayerSpawn(int index = 0)
    {
        var allTransforms = Levels[index].GetComponentsInChildren<Transform>();
        var spawnTransform = allTransforms.Where(k => k.name == "SpawnPlayer").FirstOrDefault();

        return spawnTransform.position;
    }

    public void SetActiveEnemySpawn(int index, bool setActive)
    {
        var allEnemySpawns = Levels[index].GetComponentsInChildren<Transform>();
        var enemySpawn = allEnemySpawns.Where(k => k.name.Contains("SpawnArea")).FirstOrDefault();

        if (enemySpawn)
            enemySpawn.GetComponent<LevelBounds>().enabled = setActive;

    }
}
