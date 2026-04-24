using StatePattern.StateMachine;
using UnityEngine;

//controller do inimigo ranged
public class DartThrowerEnemy : EnemyController
{
    [SerializeField] protected DartProjectile dartPrefab;
    [SerializeField] protected ProjectileData projectileData;
    [SerializeField] protected Transform throwPoint;

    private DartThrowerStateMachine stateMachine;
    protected float nextThrowTime;

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new DartThrowerStateMachine(this);
    }

    private void Start()
    {
        stateMachine.ChangeState(States.CHASING);
    }

    protected virtual void Update()
    {
        stateMachine.Update();

        bool inAttackRange = Vector3.Distance(GetPlayerPos(), transform.position) <= Data.AttackRange;

        //tenta atirar se o inimiigo estiver no range
        if (inAttackRange && stateMachine.currentState is not DartThrowingState)
        {
            stateMachine.ChangeState(States.SHOOTING);
            return;
        }

        //caso fora do range de attack persegue o jogador
        if (!inAttackRange && stateMachine.currentState is DartThrowingState)
        {
            stateMachine.ChangeState(States.CHASING);
        }
    }

    //tenta arremssar um projetil
    public void TryThrowDart()
    {
        if (Time.time < nextThrowTime)
        {
            return;
        }

        ThrowDart();
        nextThrowTime = Time.time + Mathf.Max(0.01f, Data.AttackCooldown);
    }

    //metodo de atirar um dado
    protected void ThrowDart()
    {
        if (dartPrefab == null || projectileData == null)
        {
            return;
        }

        GameObject player = GetPlayer();
        if (player == null)
        {
            return;
        }

        Vector3 origin = throwPoint != null ? throwPoint.position : transform.position;
        Vector2 direction = (player.transform.position - origin).normalized;

        if (direction.sqrMagnitude < 0.001f)
        {
            return;
        }

        //instancia o projetil
        DartProjectile dart = Object.Instantiate(dartPrefab, origin, Quaternion.identity);
        dart.Initialize(direction, projectileData, gameObject);
    }
}
