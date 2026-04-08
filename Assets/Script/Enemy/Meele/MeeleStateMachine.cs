using StatePattern.StateMachine;

public class MeeleStateMachine : GenericStateMachine<MeeleEnemy>
{
    public MeeleStateMachine(MeeleEnemy Owner) : base(Owner)
    {
        CreateState();
        SetOwner();
    }

    protected void CreateState()
    {
        States.Add(StatePattern.StateMachine.States.CHASING, new ChasingState());
        States.Add(StatePattern.StateMachine.States.ATTACKING, new AttackingState());
    }
}
