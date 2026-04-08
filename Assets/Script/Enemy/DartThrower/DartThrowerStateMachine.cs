using StatePattern.StateMachine;

public class DartThrowerStateMachine : GenericStateMachine<DartThrowerEnemy>
{
    public DartThrowerStateMachine(DartThrowerEnemy owner) : base(owner)
    {
        CreateState();
        SetOwner();
    }

    protected void CreateState()
    {
        States.Add(StatePattern.StateMachine.States.CHASING, new ChasingState());
        States.Add(StatePattern.StateMachine.States.SHOOTING, new DartThrowingState());
    }
}
