using System.Collections.Generic;
using UnityEngine;


//scriptable object para configurar o level
[CreateAssetMenu(fileName = "LevelData", menuName ="Setup/Level/Data")]
public class LevelData : ScriptableObject
{
    public GameObject Scenary;

    public SpawnPoint PlayerSpawnPoint;
    public List<SpawnPoint> EnemySpawnPoints;
    
    public List<Waves> Waves;


    public SpawnPoint GetRandomEnemySpawnPoint() => EnemySpawnPoints.GetRandomEntry();
}
