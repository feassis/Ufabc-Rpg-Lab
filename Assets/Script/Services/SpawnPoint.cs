using System;
using UnityEngine;

[Serializable]
public class SpawnPoint
{
    public float X; 
    public float Y;

    public float RandownRange;

    public Vector2 GetRandomizedSpawnPoint()
    {
        return new Vector2(X, Y) + UnityEngine.Random.insideUnitCircle * RandownRange;
    }
}
