using StatePattern.StateMachine;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMinibossSpawnerController : DartThrowerEnemy
{
    [SerializeField] private EnemyController enemyPrefab;
    [SerializeField] private float spawnDistance;
    [SerializeField] private int maxNumberOfEnemiesSpawned = 15;
    [SerializeField] private int enemyNumToSpawnPerCicle = 3;

    private float nextSpawnTime;
    private List<EnemyController> minions = new List<EnemyController>();

    private EnemyMinibossSpawnerControllerMachine stateMachine;
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyMinibossSpawnerControllerMachine(this);
    }

    protected override void Update()
    {
        stateMachine.Update();

        if (Time.time > nextSpawnTime)
        {
            stateMachine.ChangeState(States.SPAWNMINION);
            nextSpawnTime = Time.time + Mathf.Max(0.01f, Data.AbilityCooldown);
            return;
        }


        bool inAttackRange = Vector3.Distance(GetPlayerPos(), transform.position) <= Data.AttackRange;


        if (inAttackRange && stateMachine.currentState is not DartThrowingState && Time.time >= nextThrowTime)
        {
            stateMachine.ChangeState(States.SHOOTING);
            return;
        }

        else if (stateMachine.currentState is not KeepDistanceState && Time.time < nextThrowTime)
        {
            stateMachine.ChangeState(States.KEEPDISTANCE);
            return;
        }

    }

    public void TrySpawnEnemy()
    {
        if(minions.Count > maxNumberOfEnemiesSpawned)
        {
            return;
        }

        var minion = levelManager.SpawnEnemy(enemyPrefab, transform.position + new Vector3(Random.Range(0f, spawnDistance), Random.Range(0f, spawnDistance), 0));

        minion.gameObject.GetComponent<Health>().OnDied += OnMinionDied;
        minions.Add(minion);

        stateMachine.ChangeState(States.KEEPDISTANCE);
    }

    private void OnMinionDied(Health health)
    {
        health.OnDied -= OnMinionDied;
        var minion = health.gameObject.GetComponent<EnemyController>();

        minions.Remove(minion);
    }
}
