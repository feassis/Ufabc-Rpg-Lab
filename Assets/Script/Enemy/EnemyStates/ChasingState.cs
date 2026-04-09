using StatePattern.StateMachine;
using System.Buffers;
using UnityEngine;

public class ChasingState : IState
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

        Owner.SetVelocity(dir.normalized * Owner.Data.Speed);
    }
}