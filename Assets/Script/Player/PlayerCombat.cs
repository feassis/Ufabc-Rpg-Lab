using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private PlayerCombatData data;
    [SerializeField] private PlayerTriggerColision enemyDetection;
    [SerializeField] private GameObject biteVisuals;

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

    private List<EnemyController> enemiesAttacked = new List<EnemyController>();


    public event Action<float> OnSpecialUpdate;

    private void Awake()
    {
        PlayerInputHandler.OnAttackInput += OnAttackInput;
        PlayerInputHandler.OnSpecialInput += OnSpecialInput;

        lazer1.positionCount = 2;
        lazer2.positionCount = 2;
    }

    private void OnDestroy()
    {
        PlayerInputHandler.OnAttackInput -= OnAttackInput;
        PlayerInputHandler.OnSpecialInput -= OnSpecialInput;
    }

    private void OnSpecialInput()
    {
        if(specialCooldownTimer <= 0)
        {
            specialCooldownTimer = data.SpecialCooldown;
            specialTimer = data.SpecialDuration;
            lazerVisuals.SetActive(true);
            OnSpecialUpdate?.Invoke(1 - specialCooldownTimer/ data.SpecialCooldown);
        }
    }

    private void OnAttackInput()
    {
        if( Mathf.Max(attackCooldownTimer, attackTimer) <= 0 )
        {
            attackTimer = data.AttackDuration;
            attackCooldownTimer = data.AttackCoolDown;
            enemiesAttacked.Clear();
            biteVisuals.SetActive(true);
        }
    }

    private void Update()
    {
        if( attackTimer > 0 )
        {
            attackTimer -= Time.deltaTime;

            var enemyList = new List<EnemyController>();
            enemyList.AddRange(enemyDetection.GetEnemies());

            foreach (var enemy in enemyList)
            {
                if (!enemiesAttacked.Contains(enemy))
                {
                    enemy.gameObject.GetComponent<Health>().TakeDamage(data.Damage);

                    enemiesAttacked.Add(enemy);
                }
            }

            if(attackTimer <= 0)
            {
                biteVisuals.SetActive(false);
            }
        }

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
                    lazerHit1.collider.gameObject.GetComponent<Health>().TakeDamage(data.SpecialDamage);
                    specialTickTime = data.SpecialTickTimer;
                }

                if (lazerHit2.collider != null)
                {
                    lazerHit2.collider.gameObject.GetComponent<Health>().TakeDamage(data.SpecialDamage);
                    specialTickTime = data.SpecialTickTimer;
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
        }

        if(specialCooldownTimer > 0 )
        {
            specialCooldownTimer -= Time.deltaTime;
            OnSpecialUpdate?.Invoke(1 - specialCooldownTimer / data.SpecialCooldown);
        }
    }

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

    public void AddSkill(SkillSetups skill)
    {
        var skillIns = Instantiate<Skill>(skill.Skill, transform);

        skillIns.SetPlayerTransform(transform);
        skillIns.OnEnemyHited += SkillIns_OnEnemyHited;
        skills.Add(skillIns);
    }

    private void SkillIns_OnEnemyHited(EnemyController enemy)
    {
        enemy.GetComponent<Health>().TakeDamage(data.Damage);
    }
}
