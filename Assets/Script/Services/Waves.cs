using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Waves
{
    public float StartUpTimer;
    public float MinCoolDownBetweenEnemies;
    public float MaxCoolDownBetweenEnemies;

    [Header("Wave Settings")]
    public List<MustSpawnEnemies> FixedEnemies;
    public int PointsGoal;
    public List<EnemyWaveEntry> EnemyOptions;
}
