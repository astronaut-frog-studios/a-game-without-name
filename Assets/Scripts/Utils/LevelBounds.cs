using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBounds : StaticInstance<LevelBounds>
{
    [SerializeField] private MeshCollider spawnArea;
    public static float minBoundsX { get; private set; }
    public static float maxBoundsX { get; private set; }

    public static float minBoundsY { get; private set; }
    public static float maxBoundsY { get; private set; }

    public static Vector2 maxBounds { get; private set; }
    public static Vector2 minBounds { get; private set; }


    private void Start()
    {
        spawnArea = GetComponent<MeshCollider>();

        minBoundsX = spawnArea.bounds.min.x;
        maxBoundsX = spawnArea.bounds.max.x;
        minBoundsY = spawnArea.bounds.min.y;
        maxBoundsY = spawnArea.bounds.max.y;

        maxBounds = spawnArea.bounds.max;
        minBounds = spawnArea.bounds.min;
    }

    public static float RandomPosX => Random.Range(minBoundsX, maxBoundsX);
    public static float RandomPosY => Random.Range(minBoundsY, maxBoundsY);

}
