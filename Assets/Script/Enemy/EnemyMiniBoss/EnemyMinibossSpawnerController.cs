using StatePattern.StateMachine;
using System.Collections.Generic;
using UnityEngine;

//classe que controla o miniboss
public class EnemyMinibossSpawnerController : DartThrowerEnemy
{
    [SerializeField] private List<EnemyController> enemyPrefabs = new List<EnemyController>();
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
        base.Update();

        //se o timer do spawn estiver zerado muda para o estado de spawn
        if (Time.time > nextSpawnTime)
        {
            stateMachine.ChangeState(States.SPAWNMINION);
            nextSpawnTime = Time.time + Mathf.Max(0.01f, Data.AbilityCooldown);
            return;
        }

    }

    //metodo que invoca o inimigo
    public void TrySpawnEnemy()
    {
        if(minions.Count > maxNumberOfEnemiesSpawned)
        {
            return;
        }

        var minion = levelManager.SpawnEnemy(enemyPrefabs.GetRandomEntry<EnemyController>(), transform.position + new Vector3(Random.Range(0f, spawnDistance), Random.Range(0f, spawnDistance), 0));

        minion.gameObject.GetComponent<Health>().OnDied += OnMinionDied;
        minions.Add(minion);

        stateMachine.ChangeState(States.KEEPDISTANCE);
    }

    //detecta a morte de um minion
    private void OnMinionDied(Health health)
    {
        health.OnDied -= OnMinionDied;
        var minion = health.gameObject.GetComponent<EnemyController>();

        minions.Remove(minion);
    }
}
