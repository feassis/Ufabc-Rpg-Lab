using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float startHealth = 100f;
    [SerializeField] private bool destroyOnDeath;

    [Header("Damage Settings")]
    [SerializeField] private bool canTakeDamage = true;
    [SerializeField] private float invulnerabilityAfterHit;

    private float currentHealth;
    private float invulnerabilityTimer;
    private bool isDead;

    public event Action<float, float> OnHealthChanged;
    public event Action<float> OnDamaged;
    public event Action<float> OnHealed;
    public event Action<Health> OnDied;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public float NormalizedHealth => maxHealth <= 0 ? 0 : currentHealth / maxHealth;
    public bool IsDead => isDead;

    private void Awake()
    {
        maxHealth = Mathf.Max(1f, maxHealth);
        currentHealth = Mathf.Clamp(startHealth, 0f, maxHealth);
    }

    private void Start()
    {
        NotifyHealthChanged();

        if (currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    private void Update()
    {
        if (invulnerabilityTimer > 0f)
        {
            invulnerabilityTimer -= Time.deltaTime;
        }
    }

    public bool TakeDamage(float amount)
    {
        if (!canTakeDamage || isDead)
        {
            return false;
        }

        if (invulnerabilityTimer > 0f)
        {
            return false;
        }

        amount = Mathf.Max(0f, amount);

        if (amount <= 0f)
        {
            return false;
        }

        currentHealth = Mathf.Max(0f, currentHealth - amount);
        invulnerabilityTimer = invulnerabilityAfterHit;

        OnDamaged?.Invoke(amount);
        NotifyHealthChanged();

        if (currentHealth <= 0f)
        {
            HandleDeath();
        }

        return true;
    }

    public bool Heal(float amount)
    {
        if (isDead)
        {
            return false;
        }

        amount = Mathf.Max(0f, amount);

        if (amount <= 0f)
        {
            return false;
        }

        float previousHealth = currentHealth;
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        float healedAmount = currentHealth - previousHealth;

        if (healedAmount <= 0f)
        {
            return false;
        }

        OnHealed?.Invoke(healedAmount);
        NotifyHealthChanged();
        return true;
    }

    public void SetMaxHealth(float newMaxHealth, bool fillCurrentHealth = true)
    {
        maxHealth = Mathf.Max(1f, newMaxHealth);

        currentHealth = fillCurrentHealth
            ? maxHealth
            : Mathf.Clamp(currentHealth, 0f, maxHealth);

        NotifyHealthChanged();
    }

    public void SetCanTakeDamage(bool value)
    {
        canTakeDamage = value;
    }

    public void Kill()
    {
        if (isDead)
        {
            return;
        }

        currentHealth = 0f;
        NotifyHealthChanged();
        HandleDeath();
    }

    private void NotifyHealthChanged()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void HandleDeath()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        OnDied?.Invoke(this);

        if (destroyOnDeath)
        {
            Destroy(gameObject);
        }
    }
}
