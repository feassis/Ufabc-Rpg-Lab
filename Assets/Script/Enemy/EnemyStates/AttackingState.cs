using StatePattern.StateMachine;
using UnityEngine;

public class AttackingState : IState
{
    public EnemyController Owner { get; set; }
    private float nextAttackTime;
    private Health targetHealth;

    //na entrada do estado para o movimento do player
    public void OnStateEnter()
    {
        nextAttackTime = 0f;

        GameObject player = Owner.GetPlayer();
        Owner.SetVelocity(Vector3.zero);
        if (player != null)
        {
            targetHealth = player.GetComponentInParent<Health>();
        }
    }

    public void OnStateExit()
    {
        targetHealth = null;
    }

    public void Update()
    {
        if (!Owner.IsTouchingPlayer() || Time.time < nextAttackTime)
        {
            return;
        }

        //tenta pegar o componete de vida od player
        if (targetHealth == null)
        {
            GameObject player = Owner.GetPlayer();
            if (player != null)
            {
                targetHealth = player.GetComponentInParent<Health>();
            }
        }

        if (targetHealth == null)
        {
            return;
        }

        //aplica o dano no player
        targetHealth.TakeDamage(Owner.Data.AttackDamage);
        nextAttackTime = Time.time + Mathf.Max(0.01f, Owner.Data.AttackCooldown);
    }
}