using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private PlayerCombatData data;
    [SerializeField] private PlayerTriggerColision enemyDetection;
    [SerializeField] private GameObject biteVisuals;
    private float attackTimer = 0;
    private float attackCooldownTimer = 0;

    private List<EnemyController> enemiesAttacked = new List<EnemyController>();

    private void Awake()
    {
        PlayerInputHandler.OnAttackInput += OnAttackInput;
    }

    private void OnDestroy()
    {
        PlayerInputHandler.OnAttackInput -= OnAttackInput;
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

        if( attackCooldownTimer > 0 )
        {
            attackCooldownTimer -= Time.deltaTime;
        }
    }
}
