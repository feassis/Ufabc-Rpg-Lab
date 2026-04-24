using System;

//configurção do spawn dos inimigos fixos
[Serializable]
public struct MustSpawnEnemies
{
    public float SpawnTime;
    public EnemyController Enemy;
}


