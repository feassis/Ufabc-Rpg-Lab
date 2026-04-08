using StatePattern.StateMachine;

public class FlyingDroneStateMachine : GenericStateMachine<FlyingDroneEnemy>
{
    public FlyingDroneStateMachine(FlyingDroneEnemy Owner) : base(Owner)
    {
        CreateState();
        SetOwner();
    }

    protected void CreateState()
    {
        States.Add(StatePattern.StateMachine.States.CHASING, new ChasingState());
        States.Add(StatePattern.StateMachine.States.DIVING, new DivingState());
    }
}
