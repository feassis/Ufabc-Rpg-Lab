using StatePattern.StateMachine;
using UnityEngine;

public class KeepDistanceState : IState
{
    public EnemyController Owner { get; set; }

    public void OnStateEnter()
    {

    }

    public void OnStateExit()
    {

    }

    public void Update()
    {
        var dir = Owner.GetPlayerPos() - Owner.transform.position;

        var distance = Mathf.Abs(dir.magnitude);

        if(distance < Owner.Data.AttackRange)
        {
            Owner.SetVelocity(-dir.normalized * Owner.Data.Speed);
        }
        else
        {
            Owner.SetVelocity(dir.normalized * Owner.Data.Speed);
        }

            
    }
}