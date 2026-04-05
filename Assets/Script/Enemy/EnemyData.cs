using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Setup/Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public float Speed;
    public float DashSpeed;
    public float AttackRange;
}
