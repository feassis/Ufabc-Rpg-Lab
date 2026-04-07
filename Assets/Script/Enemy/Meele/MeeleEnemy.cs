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
