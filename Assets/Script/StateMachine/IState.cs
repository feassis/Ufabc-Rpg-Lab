namespace StatePattern.StateMachine
{
    //Interface que determina o que é um estado
    public interface IState
    {
        public EnemyController Owner { get; set; }
        public void OnStateEnter();
        public void Update();
        public void OnStateExit();
    }
}