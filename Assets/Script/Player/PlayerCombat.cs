using System;
using System.Collections.Generic;
using UnityEngine;

public enum CombatCooldownType
{
    Attack,
    Special
}

//classe de gerenciamento do combate do jogador
public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private PlayerCombatData data;
    [SerializeField] private PlayerTriggerColision enemyDetection;
    [SerializeField] private GameObject biteVisuals;
    [SerializeField] private Stats stats;

    [Header("Special Setup")]
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private Transform rightLaser1;
    [SerializeField] private Transform rightLaser2;
    [SerializeField] private Transform leftLaser1;
    [SerializeField] private Transform leftLaser2;
    [SerializeField] private GameObject lazerVisuals;
    [SerializeField] private LineRenderer lazer1;
    [SerializeField] private LineRenderer lazer2;
    [SerializeField] private float maxDistance = 50f;
   
    private List<Skill> skills = new List<Skill>();
    private float attackTimer = 0;
    private float attackCooldownTimer = 0;
    private float specialTimer = 0;
    private float specialCooldownTimer = 0;
    private float specialTickTime = 0;
    private float damageBonus = 0;
    private float specialDamageBonus = 0;
    private float attackCooldownModifier = 0f;
    private float specialCooldownModifier = 0f;

    private List<EnemyController> enemiesAttacked = new List<EnemyController>();


    public event Action<float> OnSpecialUpdate;
    public event Action<CombatCooldownType, float> OnCooldownUpdate;
    public PlayerCombatData Data => data;

    public float GetDamage() => data.Damage;

    //se inscreve aos inputs de attack e special e configura o line renderes dos lazers do special
    private void Awake()
    {
        PlayerInputHandler.OnAttackInput += OnAttackInput;
        PlayerInputHandler.OnSpecialInput += OnSpecialInput;

        if (stats == null)
        {
            stats = GetComponent<Stats>();
        }

        lazer1.positionCount = 2;
        lazer2.positionCount = 2;
    }

    //se desincreve dos inputs de attack e special
    private void OnDestroy()
    {
        PlayerInputHandler.OnAttackInput -= OnAttackInput;
        PlayerInputHandler.OnSpecialInput -= OnSpecialInput;
    }

    //metodo que inicia o special
    private void OnSpecialInput()
    {
        if(specialCooldownTimer <= 0)
        {
            specialCooldownTimer = GetSpecialCooldown();
            specialTimer = GetSpecialDuration();
            lazerVisuals.SetActive(true);
            NotifyCooldownUpdate(CombatCooldownType.Special, specialCooldownTimer, GetSpecialCooldown());
        }
    }

    //metodo que inicia o attack
    private void OnAttackInput()
    {
        if( Mathf.Max(attackCooldownTimer, attackTimer) <= 0 )
        {
            attackTimer = GetAttackDuration();
            attackCooldownTimer = GetAttackCooldown();
            enemiesAttacked.Clear();
            biteVisuals.SetActive(true);
            NotifyCooldownUpdate(CombatCooldownType.Attack, GetAttackRecoveryTimer(), GetAttackRecoveryDuration());
        }
    }

    //gerencia os timers e executa dano nos inimigos e realiza o special
    private void Update()
    {
        //attack
        if ( attackTimer > 0 )
        {
            attackTimer -= Time.deltaTime;

            var enemyList = new List<EnemyController>();
            enemyList.AddRange(enemyDetection.GetEnemies());

            foreach (var enemy in enemyList)
            {
                if (!enemiesAttacked.Contains(enemy))
                {
                    enemy.gameObject.GetComponent<Health>().TakeDamage(GetDamage());

                    enemiesAttacked.Add(enemy);
                }
            }

            if(attackTimer <= 0)
            {
                biteVisuals.SetActive(false);
            }

            NotifyCooldownUpdate(CombatCooldownType.Attack, GetAttackRecoveryTimer(), GetAttackRecoveryDuration());
        }

        //special
        if(specialTimer > 0)
        {
            specialTimer -= Time.deltaTime;

            if(specialTickTime > 0 )
            {
                specialTickTime -= Time.deltaTime;
            }

            RaycastHit2D lazerHit1, lazerHit2;

            if (movement.GetMoveInput().x >= 0)
            {
                lazerHit1 = CastRayToEnemy(rightLaser1.transform.position, lazer1);
                lazerHit2 = CastRayToEnemy(rightLaser2.transform.position, lazer2);
            }
            else
            {
                lazerHit1 = CastRayToEnemy(leftLaser1.transform.position, lazer1);
                lazerHit2 = CastRayToEnemy(leftLaser2.transform.position, lazer2);
            }

            if(specialTickTime <= 0)
            {
                if (lazerHit1.collider != null)
                {
                    if(lazerHit1.collider.gameObject.TryGetComponent<Health>(out Health health))
                    {
                        health.TakeDamage(GetSpecialDamage());
                    }
                    specialTickTime = GetSpecialTickTimer();
                }

                if (lazerHit2.collider != null)
                {
                    if(lazerHit2.collider.gameObject.TryGetComponent<Health>(out Health health))
                    {
                        health.TakeDamage(GetSpecialDamage());
                    }
                    specialTickTime = GetSpecialTickTimer();
                }
            }

            if(specialTimer <= 0)
            {
                lazerVisuals.SetActive(false);
            }
        }


        if( attackCooldownTimer > 0 )
        {
            attackCooldownTimer -= Time.deltaTime;
            NotifyCooldownUpdate(CombatCooldownType.Attack, GetAttackRecoveryTimer(), GetAttackRecoveryDuration());
        }

        if(specialCooldownTimer > 0 )
        {
            specialCooldownTimer -= Time.deltaTime;
            NotifyCooldownUpdate(CombatCooldownType.Special, specialCooldownTimer, GetSpecialCooldown());
        }
    }

    //traça o raio do raio lazer para o inimigo
    private RaycastHit2D CastRayToEnemy(Vector3 initialPos, LineRenderer visuals)
    {
        Vector3 mouseWorld = PlayerInputHandler.GetMousePosInWorld();
        mouseWorld.z = 0f;

        Vector2 origin = initialPos;
        Vector2 direction = (mouseWorld - transform.position).normalized;

        float distance = Vector2.Distance(origin, mouseWorld);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, enemyLayer);

        Vector3 endPoint;

        if (hit.collider != null)
        {
            endPoint = hit.point;
            Debug.Log("Acertou: " + hit.collider.name);
        }
        else
        {
            endPoint = origin + direction * maxDistance;
        }

        visuals.SetPosition(0, origin);
        visuals.SetPosition(1, endPoint);

        return hit;
    }

    //adiciona uma skill
    public void AddSkill(SkillSetups skill)
    {
        AddSkill(skill.Skill);
    }

    public void AddSkill(Skill skillPrefab)
    {
        if (skillPrefab == null)
        {
            return;
        }

        var skillIns = Instantiate<Skill>(skillPrefab, transform);

        skillIns.SetPlayerTransform(transform);
        skillIns.OnEnemyHited += SkillIns_OnEnemyHited;
        skills.Add(skillIns);
    }

    private void SkillIns_OnEnemyHited(EnemyController enemy)
    {
        enemy.GetComponent<Health>().TakeDamage(GetDamage());
    }

    public void AddDamage(float amount)
    {
        if (stats != null)
        {
            stats.AddDamageBonus(amount);
            return;
        }

        damageBonus += Mathf.Max(0f, amount);
    }

    public void AddSpecialDamage(float amount)
    {
        if (stats != null)
        {
            stats.AddSpecialDamageBonus(amount);
            return;
        }

        specialDamageBonus += Mathf.Max(0f, amount);
    }

    public void AddAttackCooldownModifier(float amount)
    {
        if (stats != null)
        {
            stats.AddAttackCooldownModifier(amount);
            return;
        }

        attackCooldownModifier += Mathf.Max(0f, amount);
    }

    public void AddSpecialCooldownModifier(float amount)
    {
        if (stats != null)
        {
            stats.AddSpecialCooldownModifier(amount);
            return;
        }

        specialCooldownModifier += Mathf.Max(0f, amount);
    }

    public float GetCooldownDuration(CombatCooldownType cooldownType)
    {
        switch (cooldownType)
        {
            case CombatCooldownType.Attack:
                return GetAttackRecoveryDuration();
            case CombatCooldownType.Special:
                return GetSpecialCooldown();
            default:
                return 0f;
        }
    }

    public float GetCooldownProgress(CombatCooldownType cooldownType)
    {
        switch (cooldownType)
        {
            case CombatCooldownType.Attack:
                return GetNormalizedCooldownProgress(GetAttackRecoveryTimer(), GetAttackRecoveryDuration());
            case CombatCooldownType.Special:
                return GetNormalizedCooldownProgress(specialCooldownTimer, GetSpecialCooldown());
            default:
                return 1f;
        }
    }

    private void NotifyCooldownUpdate(CombatCooldownType cooldownType, float timer, float duration)
    {
        float normalizedCooldown = GetNormalizedCooldownProgress(timer, duration);

        OnCooldownUpdate?.Invoke(cooldownType, normalizedCooldown);

        if (cooldownType == CombatCooldownType.Special)
        {
            OnSpecialUpdate?.Invoke(normalizedCooldown);
        }
    }

    private float GetNormalizedCooldownProgress(float timer, float duration)
    {
        if (duration <= 0f)
        {
            return 1f;
        }

        return 1f - Mathf.Clamp01(timer / duration);
    }

    private float GetAttackRecoveryTimer() => Mathf.Max(attackCooldownTimer, attackTimer);

    private float GetAttackRecoveryDuration() => Mathf.Max(GetAttackCooldown(), GetAttackDuration());

    private float GetDamage() => stats != null ? stats.Damage : data.Damage + damageBonus;

    private float GetSpecialDamage() => stats != null ? stats.SpecialDamage : data.SpecialDamage + specialDamageBonus;

    private float GetAttackCooldown() => stats != null ? stats.CurrentAttackCooldown : CooldownFormula(attackCooldownModifier, data.AttackCoolDown);

    private float GetSpecialCooldown() => stats != null ? stats.SpecialCooldown : CooldownFormula(specialCooldownModifier, data.SpecialCooldown);

    private float GetAttackDuration() => stats != null ? stats.AttackDuration : data.AttackDuration;

    private float GetSpecialDuration() => stats != null ? stats.SpecialDuration : data.SpecialDuration;

    private float GetSpecialTickTimer() => stats != null ? stats.SpecialTickTimer : data.SpecialTickTimer;

    private float CooldownFormula(float x, float baseValue) => 1f + ((baseValue - 1f) * Mathf.Exp(-0.5f * x));
}
