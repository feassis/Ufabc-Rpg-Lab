using StatePattern.StateMachine;
using UnityEngine;

public class DartThrowingState : IState
{
    public EnemyController Owner { get; set; }

    public void OnStateEnter()
    {
        Owner.SetVelocity(Vector3.zero);
    }

    public void OnStateExit()
    {
    }

    public void Update()
    {
        if (Owner is DartThrowerEnemy dartThrower)
        {
            dartThrower.TryThrowDart();
        }
    }
}
