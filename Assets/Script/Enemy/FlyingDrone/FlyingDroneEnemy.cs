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

public class DivingState : IState
{
    public EnemyController Owner { get; set; }

    private Vector3 direction;

    public void OnStateEnter()
    {
        direction = Owner.GetPlayerPos() - Owner.transform.position;
        direction = direction.normalized;
    }

    public void OnStateExit()
    {

    }

    public void Update()
    {
        var dir = Owner.GetShadowPos() - Owner.GetBodyPos();

        if (dir.sqrMagnitude >= 0.01)
        {
            Owner.transform.position = Owner.transform.position + direction.normalized * Owner.Data.DashSpeed * Time.deltaTime;
            Owner.SetBodyPos(Owner.GetBodyPos() + dir.normalized * Owner.Data.DashSpeed * Time.deltaTime);
        }
    }
}
