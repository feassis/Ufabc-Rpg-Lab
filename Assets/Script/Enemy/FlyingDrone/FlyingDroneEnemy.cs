using StatePattern.StateMachine;
using UnityEngine;

public class FlyingDroneEnemy : EnemyController
{
    private FlyingDroneStateMachine stateMachine;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new FlyingDroneStateMachine(this);
    }

    protected void Start()
    {
        stateMachine.ChangeState(StatePattern.StateMachine.States.CHASING);
    }

    private void Update()
    {
        stateMachine.Update();

        Debug.Log($"Distance {Vector3.Distance(GetPlayerPos(), transform.position)}");

        if(Vector3.Distance(GetPlayerPos(), transform.position) < Data.AttackRange)
        {
            stateMachine.ChangeState(States.DIVING);
        }
    }
}
