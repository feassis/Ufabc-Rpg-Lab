using StatePattern.StateMachine;

public class EnemyMinibossSpawnerControllerMachine : GenericStateMachine<EnemyMinibossSpawnerController>
{
    public EnemyMinibossSpawnerControllerMachine(EnemyMinibossSpawnerController owner) : base(owner)
    {
        CreateState();
        SetOwner();
    }

    protected void CreateState()
    {
        States.Add(StatePattern.StateMachine.States.KEEPDISTANCE, new KeepDistanceState());
        States.Add(StatePattern.StateMachine.States.SHOOTING, new DartThrowingState());
        States.Add(StatePattern.StateMachine.States.SPAWNMINION, new SpawnEnemyState());
    }
}