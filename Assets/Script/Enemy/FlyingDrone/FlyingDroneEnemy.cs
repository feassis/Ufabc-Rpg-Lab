using StatePattern.StateMachine;
using UnityEngine;

//classe do inimigo voador
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

        //se chegar no range de ataque ele muda para o estado de mergulho
        if(Vector3.Distance(GetPlayerPos(), transform.position) < Data.AttackRange)
        {
            stateMachine.ChangeState(States.DIVING);
        }
    }
}
