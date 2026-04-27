using StatePattern.StateMachine;
using UnityEngine;

//classe do inimigo melee
public class MeleeEnemy : EnemyController
{
    [SerializeField] private AudioSource audioSource;
    private MeeleStateMachine stateMachine;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new MeeleStateMachine(this);
    }

    protected void Start()
    {
        stateMachine.ChangeState(StatePattern.StateMachine.States.CHASING);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.Update();

        bool inAttackRange = Vector3.Distance(GetPlayerPos(), transform.position) < Data.AttackRange;

        //muda para o estado de ataque caso esteja no range
        if (inAttackRange && stateMachine.currentState is not AttackingState)
        {
            if(audioSource != null)
            {
                audioSource.Play();
            }
            stateMachine.ChangeState(States.ATTACKING);
            return;
        }

        // muda para o estado de persegui��o caso saia do range
        if (!inAttackRange && stateMachine.currentState is AttackingState)
        {
            stateMachine.ChangeState(States.CHASING);
        }
    }
}
