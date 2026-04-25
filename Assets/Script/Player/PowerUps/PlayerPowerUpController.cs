using UnityEngine;

public class PlayerPowerUpController : MonoBehaviour
{
    [SerializeField] private PlayerCombat combat;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private Health health;
    [SerializeField] private Stats stats;

    private void Awake()
    {
        if (combat == null)
        {
            combat = GetComponent<PlayerCombat>();
        }

        if (movement == null)
        {
            movement = GetComponent<PlayerMovement>();
        }

        if (health == null)
        {
            health = GetComponent<Health>();
        }

        if (stats == null)
        {
            stats = GetComponent<Stats>();
        }

        InitializeStatsBaseValues();
        SyncMaxHealth(true);
    }

    public void Apply(PowerUpData powerUp)
    {
        if (powerUp == null)
        {
            return;
        }

        switch (powerUp.Type)
        {
            case PowerUpType.AddDamage:
                stats?.AddDamageBonus(powerUp.Value);
                break;
            case PowerUpType.AddSpecialDamage:
                stats?.AddSpecialDamageBonus(powerUp.Value);
                break;
            case PowerUpType.ReduceAttackCooldown:
                stats?.AddAttackCooldownModifier(powerUp.Value);
                break;
            case PowerUpType.ReduceSpecialCooldown:
                stats?.AddSpecialCooldownModifier(powerUp.Value);
                break;
            case PowerUpType.IncreaseMoveSpeed:
                stats?.MultiplyMoveSpeed(1f + Mathf.Max(0f, powerUp.Value));
                break;
            case PowerUpType.IncreaseMaxHealth:
                stats?.AddMaxHealthBonus(powerUp.Value);
                SyncMaxHealth(false);
                break;
            case PowerUpType.AddSkill:
                combat?.AddSkill(powerUp.SkillPrefab);
                break;
            case PowerUpType.Heal:
                health?.Heal(powerUp.Value);
                break;
        }
    }

    private void SyncMaxHealth(bool fillCurrentHealth)
    {
        if (health == null || stats == null)
        {
            return;
        }

        health.SetMaxHealth(stats.MaxHealth, fillCurrentHealth);
    }

    private void InitializeStatsBaseValues()
    {
        if (stats == null)
        {
            return;
        }

        if (combat != null)
        {
            stats.SetBaseCombat(combat.Data);
        }

        if (movement != null)
        {
            stats.SetBaseMovement(movement.Data);
        }

        if (health != null)
        {
            stats.SetBaseMaxHealth(health.MaxHealth);
        }
    }
}
