using StatePattern.StateMachine;
using UnityEngine;

public class DartThrowerEnemy : EnemyController
{
    [SerializeField] private DartProjectile dartPrefab;
    [SerializeField] private ProjectileData projectileData;
    [SerializeField] private Transform throwPoint;

    private DartThrowerStateMachine stateMachine;
    private float nextThrowTime;

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new DartThrowerStateMachine(this);
    }

    private void Start()
    {
        stateMachine.ChangeState(States.CHASING);
    }

    private void Update()
    {
        stateMachine.Update();

        bool inAttackRange = Vector3.Distance(GetPlayerPos(), transform.position) <= Data.AttackRange;

        if (inAttackRange && stateMachine.currentState is not DartThrowingState)
        {
            stateMachine.ChangeState(States.SHOOTING);
            return;
        }

        if (!inAttackRange && stateMachine.currentState is DartThrowingState)
        {
            stateMachine.ChangeState(States.CHASING);
        }
    }

    public void TryThrowDart()
    {
        if (Time.time < nextThrowTime)
        {
            return;
        }

        ThrowDart();
        nextThrowTime = Time.time + Mathf.Max(0.01f, Data.AttackCooldown);
    }

    private void ThrowDart()
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

        DartProjectile dart = Object.Instantiate(dartPrefab, origin, Quaternion.identity);
        dart.Initialize(direction, projectileData, gameObject);
    }
}

public class DartThrowingState : IState
{
    public EnemyController Owner { get; set; }

    public void OnStateEnter()
    {
    }

    public void OnStateExit()
    {
    }

    public void Update()
    {
        if (Owner is DartThrowerEnemy dartThrower)
        {
            dartThrower.TryThrowDart();
        }
    }
}
