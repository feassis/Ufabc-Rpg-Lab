using StatePattern.StateMachine;

public class SpawnEnemyState : IState
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
        if(Owner is EnemyMinibossSpawnerController spawner)
        {
            spawner.TrySpawnEnemy();
        }
    }
}