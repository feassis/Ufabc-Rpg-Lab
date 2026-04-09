using StatePattern.StateMachine;
using UnityEngine;

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
            Owner.SetVelocity(dir.normalized * Owner.Data.DashSpeed);
        }
    }
}
