using System;
using System.Collections.Generic;
using UnityEngine;

//Define o que é uma onda de inimigos
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
