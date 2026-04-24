using System.Collections.Generic;

//classe basica para a criação de StateMachine dos inimigos
namespace StatePattern.StateMachine
{
    public class GenericStateMachine<T> where T : EnemyController
    {
        protected T Owner;
        public IState currentState {  get; protected set; }
        protected Dictionary<States, IState> States = new Dictionary<States, IState>();

        public GenericStateMachine(T Owner) => this.Owner = Owner;

        //atualiza o estado atual
        public void Update() => currentState?.Update();

        //faz a mudança dos estados da StateMachine
        protected void ChangeState(IState newState)
        {
            currentState?.OnStateExit();
            currentState = newState;
            currentState?.OnStateEnter();
        }

        public void ChangeState(States newState) => ChangeState(States[newState]);

        // da Set do owner da state machine
        protected void SetOwner()
        {
            foreach (IState state in States.Values)
            {
                state.Owner = Owner;
            }
        }
    }
}