using System;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

//Classe que controla a Faze
public partial class LevelManager : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField] private LevelData Data;
    [SerializeField] private Transform enemyHolder;
    
    [Header("Wave UI")]
    [SerializeField] private GameObject startWavesPanel;
    [SerializeField] private TextMeshProUGUI startWavesText;

    [Header("Player Configs")]
    [SerializeField] private GameObject player;
    [SerializeField] private List<SkillSetups> skillSetups;
    
    [Header("End GAme UI")]
    [SerializeField] private EndGamePopup endGamePopup;

    private int waveIndex = 0;

    private int fixedEnemiesSpawned = 0;
    private int pointsSpawned = 0;

    private List<EnemyController> enemies = new List<EnemyController>();

    private void Start()
    {
        InitializePlayer();
        InitializeWave();
    }

    //Inicializa uma das ondas de inimigos
    private void InitializeWave()
    {
        StartCoroutine(WaveStartUpSequence());
    }

    //sequencia de setup da inicialização da onda de inimigos
    private IEnumerator WaveStartUpSequence()
    {
        fixedEnemiesSpawned = 0;
        enemies.Clear();
        pointsSpawned = 0;

        Waves wave = Data.Waves[waveIndex];

        var startUpIn = wave.StartUpTimer;

        startWavesPanel.SetActive(true);
        startWavesText.text = $"Starts in: {startUpIn}s";

        while(startUpIn > 0)
        {
            yield return new WaitForSeconds(1f);

            startUpIn -= 1f;


            startWavesText.text = $"Starts in: {startUpIn}s";
        }

        startWavesPanel.SetActive(false);

        StartWaveSequence(wave);
    }

    //sequencia da inicialização da onda de inimigos
    private void StartWaveSequence(Waves wave)
    {
        SpawnFixedEnemies(wave);
        StartCoroutine(SpawnRandomizedEnemies(wave));
    }

    //spawna um inimigo aleatorio e adiciona sua pontuação no contador
    private IEnumerator SpawnRandomizedEnemies(Waves wave)
    {
        while (pointsSpawned < wave.PointsGoal)
        {
            var enemy = wave.EnemyOptions.GetRandomEntry();
            var spawnPoint = Data.GetRandomEnemySpawnPoint();

            var point = spawnPoint.GetRandomizedSpawnPoint();

            SpawnEnemy(enemy.Enemy, new Vector3(point.x, point.y, 0f) + player.transform.position);

            pointsSpawned += enemy.Points;

            yield return new WaitForSeconds(Random.Range(wave.MinCoolDownBetweenEnemies, wave.MaxCoolDownBetweenEnemies));
        }
    }

    //spawna um inimigo fixo no tempo determinado
    private void SpawnFixedEnemies(Waves wave)
    {
        if(wave.FixedEnemies.Count == 0)
        {
            return;
        }

        foreach(var enemy in wave.FixedEnemies)
        {
            var spawnPoint = Data.GetRandomEnemySpawnPoint();

            var point = spawnPoint.GetRandomizedSpawnPoint();

            StartCoroutine(SpawnEnemyWithDelay(enemy.SpawnTime, enemy.Enemy, new Vector3(point.x, point.y, 0f) + player.transform.position, () =>
            {
                fixedEnemiesSpawned++;
            }));
        }
    }

    //spawna o inimigo com um delay
    private IEnumerator SpawnEnemyWithDelay(float time, EnemyController enemy, Vector3 spawnPos, Action onComplete = null)
    {
        yield return new WaitForSeconds(time);

        SpawnEnemy(enemy, spawnPos);

        onComplete?.Invoke();
    }

    //função que spawna o Inimigo e registra ele no level manager
    public EnemyController SpawnEnemy(EnemyController enemy, Vector3 SpawnPos)
    {
        var spawnedEnemy = Instantiate<EnemyController>(enemy);
        spawnedEnemy.transform.position = SpawnPos;

        spawnedEnemy.transform.SetParent(enemyHolder);

        spawnedEnemy.SetPlayer(player);
        spawnedEnemy.SetLevelManager(this);

        spawnedEnemy.gameObject.GetComponent<Health>().OnDied += OnEnemyDied;

        enemies.Add(spawnedEnemy);

        return spawnedEnemy;
    }

    //metodo chamado na morte do inimigo para chamar a verificar o estado da onda
    private void OnEnemyDied(Health health)
    {
        var deadEnemy = health.gameObject.GetComponent<EnemyController>();

        enemies.Remove(deadEnemy);

        CheckWaveState();
    }

    //metodo para checar se a onda acabou
    private void CheckWaveState()
    {
        var wave = Data.Waves[waveIndex];
        if(fixedEnemiesSpawned == wave.FixedEnemies.Count && enemies.Count == 0)
        {
            waveIndex++;
            ProcessEndOfWave();
        }
    }

    //Manda para a proxima onda ou termina a fase
    private void ProcessEndOfWave()
    {

        if (waveIndex >= Data.Waves.Count)
        {
            EndGame(true);
        }
        else
        {
            InitializeWave();
        }
    }

    //metodo para iniciar o loop de fim de fase
    private void EndGame(bool victory)
    {
        endGamePopup.gameObject.SetActive(true);
        endGamePopup.Setup(victory);
    }

    //inicialisa o jogador
    private void InitializePlayer()
    {
        // will spawn player

        player.GetComponent<Health>().OnDied += Player_OnDied;

        AddSkillToPlayer(SkillType.GnarBoomerang);
    }

    //adiciona uma habilidade ao jogador
    public void AddSkillToPlayer(SkillType skillType)
    {
        var skill = skillSetups.Find(s => s.Type == skillType);

        player.GetComponent<PlayerCombat>().AddSkill(skill);
    }

    //metodo inscrito na morte do jogador
    private void Player_OnDied(Health obj)
    {
        EndGame(false);
    }
}
