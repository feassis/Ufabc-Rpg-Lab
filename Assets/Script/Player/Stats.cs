using System;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("Base Combat")]
    [SerializeField] private float baseDamage = 20f;
    [SerializeField] private float baseAttackDuration = 0.2f;
    [SerializeField] private float baseAttackCooldown = 0.5f;
    [SerializeField] private float baseSpecialCooldown = 5f;
    [SerializeField] private float baseSpecialDamage = 5f;
    [SerializeField] private float baseSpecialTickTimer = 0.2f;
    [SerializeField] private float baseSpecialDuration = 2f;

    [Header("Base Movement")]
    [SerializeField] private float baseSpeed = 4f;
    [SerializeField] private float baseSprintSpeed = 6f;
    [SerializeField] private float baseDashSpeed = 10f;
    [SerializeField] private float baseDashDuration = 0.2f;
    [SerializeField] private float baseDashCooldown = 1f;

    [Header("Base Health")]
    [SerializeField] private float baseMaxHealth = 100f;

    [Header("Modifiers")]
    [SerializeField] private float damageBonus;
    [SerializeField] private float specialDamageBonus;
    [SerializeField] private float attackCooldownModifier = 0f;
    [SerializeField] private float specialCooldownModifier = 0f;
    [SerializeField] private float moveSpeedMultiplier = 1f;
    [SerializeField] private float maxHealthBonus;

    public event Action OnStatsChanged;

    public float BaseDamage => baseDamage;
    public float BaseAttackDuration => baseAttackDuration;
    public float BaseAttackCooldown => baseAttackCooldown;
    public float BaseSpecialCooldown => baseSpecialCooldown;
    public float BaseSpecialDamage => baseSpecialDamage;
    public float BaseSpecialTickTimer => baseSpecialTickTimer;
    public float BaseSpecialDuration => baseSpecialDuration;
    public float BaseSpeed => baseSpeed;
    public float BaseSprintSpeed => baseSprintSpeed;
    public float BaseDashSpeed => baseDashSpeed;
    public float BaseDashDuration => baseDashDuration;
    public float BaseDashCooldown => baseDashCooldown;
    public float BaseMaxHealth => baseMaxHealth;

    public float DamageBonus => damageBonus;
    public float SpecialDamageBonus => specialDamageBonus;
    public float AttackCooldownModifier => attackCooldownModifier;
    public float SpecialCooldownModifier => specialCooldownModifier;
    public float MoveSpeedMultiplier => moveSpeedMultiplier;
    public float MaxHealthBonus => maxHealthBonus;

    public float Damage => baseDamage + damageBonus;
    public float AttackDuration => baseAttackDuration;

    public float CurrentAttackCooldown => AttackCooldownFormula(AttackCooldownModifier);
    public float AttackCooldownFormula(float x) => CooldownFormula(x, baseAttackCooldown);
    public float CooldownFormula(float x, float baseValue) => 1f + ((baseValue - 1f) * Mathf.Exp(-0.5f * x));
    public float SpecialCooldownFormula(float x) => CooldownFormula(x, baseSpecialCooldown);
    public float SpecialCooldown => SpecialCooldownFormula(SpecialCooldownModifier);
    public float SpecialDamage => baseSpecialDamage + specialDamageBonus;
    public float SpecialTickTimer => baseSpecialTickTimer;
    public float SpecialDuration => baseSpecialDuration;
    public float Speed => baseSpeed * moveSpeedMultiplier;
    public float SprintSpeed => baseSprintSpeed * moveSpeedMultiplier;
    public float DashSpeed => baseDashSpeed * moveSpeedMultiplier;
    public float DashDuration => baseDashDuration;
    public float DashCooldown => baseDashCooldown;
    public float MaxHealth => baseMaxHealth + maxHealthBonus;


    public void SetBaseCombat(PlayerCombatData combatData)
    {
        if (combatData == null)
        {
            return;
        }

        baseDamage = combatData.Damage;
        baseAttackDuration = combatData.AttackDuration;
        baseAttackCooldown = combatData.AttackCoolDown;
        baseSpecialCooldown = combatData.SpecialCooldown;
        baseSpecialDamage = combatData.SpecialDamage;
        baseSpecialTickTimer = combatData.SpecialTickTimer;
        baseSpecialDuration = combatData.SpecialDuration;
        NotifyChanged();
    }

    public void SetBaseMovement(PlayerMovementData movementData)
    {
        if (movementData == null)
        {
            return;
        }

        baseSpeed = movementData.Speed;
        baseSprintSpeed = movementData.SprintSpeed;
        baseDashSpeed = movementData.DashSpeed;
        baseDashDuration = movementData.DashDuration;
        baseDashCooldown = movementData.DashCoolDown;
        NotifyChanged();
    }

    public void SetBaseMaxHealth(float value)
    {
        baseMaxHealth = Mathf.Max(1f, value);
        NotifyChanged();
    }

    public void AddDamageBonus(float amount)
    {
        damageBonus += Mathf.Max(0f, amount);
        NotifyChanged();
    }

    public void AddSpecialDamageBonus(float amount)
    {
        specialDamageBonus += Mathf.Max(0f, amount);
        NotifyChanged();
    }

    public void AddAttackCooldownModifier(float amount)
    {
        attackCooldownModifier += Mathf.Max(0f, amount);
        NotifyChanged();
    }

    public void AddSpecialCooldownModifier(float amount)
    {
        specialCooldownModifier += Mathf.Max(0f, amount);
        NotifyChanged();
    }

    public void MultiplyMoveSpeed(float multiplier)
    {
        moveSpeedMultiplier *= Mathf.Max(0.1f, multiplier);
        NotifyChanged();
    }

    public void AddMaxHealthBonus(float amount)
    {
        maxHealthBonus += Mathf.Max(0f, amount);
        NotifyChanged();
    }

    private void NotifyChanged()
    {
        OnStatsChanged?.Invoke();
    }
}
