using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Setup/Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public float Speed;
    public float DashSpeed;
    public float AttackRange;
    public float AttackDamage = 10f;
    public float AttackCooldown = 0.5f;
    public float AbilityCooldown = 10f;
}
