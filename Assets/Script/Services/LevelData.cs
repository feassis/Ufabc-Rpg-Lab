using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName ="Setup/Level/Data")]
public class LevelData : ScriptableObject
{
    public GameObject Scenary;

    public SpawnPoint PlayerSpawnPoint;
    public List<SpawnPoint> EnemySpawnPoints;
    
    public List<Waves> Waves;

    public string NextLevel;

    public SpawnPoint GetRandomEnemySpawnPoint() => EnemySpawnPoints.GetRandomEntry();
}
