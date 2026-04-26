using StatePattern.StateMachine;

//state machine do inimigo melee
public class MeeleStateMachine : GenericStateMachine<MeleeEnemy>
{
    public MeeleStateMachine(MeleeEnemy Owner) : base(Owner)
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
