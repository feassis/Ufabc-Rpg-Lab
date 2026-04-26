using StatePattern.StateMachine;

//state machine para o inimigo ranged
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
