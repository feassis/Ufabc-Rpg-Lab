using UnityEngine;

[CreateAssetMenu(fileName = "New Player Combat Data", menuName = "Setup/Player/Combat Data")]
public class PlayerCombatData : ScriptableObject
{
    public float Damage;
    public float AttackDuration;
    public float AttackCoolDown;

    public float SpecialCooldown;
    public float SpecialDamage;
    public float SpecialTickTimer;
    public float SpecialDuration;
}
