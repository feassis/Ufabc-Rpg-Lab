using StatePattern.StateMachine;
using UnityEngine;

public class MeeleEnemy : EnemyController
{
    private MeeleStateMachine stateMachine;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new MeeleStateMachine(this);
    }

    protected void Start()
    {
        stateMachine.ChangeState(StatePattern.StateMachine.States.CHASING);
    }

    private void Update()
    {
        stateMachine.Update();

        bool inAttackRange = Vector3.Distance(GetPlayerPos(), transform.position) < Data.AttackRange;

        if (inAttackRange && stateMachine.currentState is not AttackingState)
        {
            stateMachine.ChangeState(States.ATTACKING);
            return;
        }

        if (!inAttackRange && stateMachine.currentState is AttackingState)
        {
            stateMachine.ChangeState(States.CHASING);
        }
    }
}

public class AttackingState : IState
{
    public EnemyController Owner { get; set; }
    private float nextAttackTime;
    private Health targetHealth;

    public void OnStateEnter()
    {
        nextAttackTime = 0f;

        GameObject player = Owner.GetPlayer();
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

        targetHealth.TakeDamage(Owner.Data.AttackDamage);
        nextAttackTime = Time.time + Mathf.Max(0.01f, Owner.Data.AttackCooldown);
    }
}