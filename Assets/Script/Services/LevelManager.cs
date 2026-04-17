using System;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using Unity.VisualScripting.Dependencies.NCalc;

public partial class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelData Data;
    [SerializeField] private GameObject startWavesPanel;
    [SerializeField] private TextMeshProUGUI startWavesText;
    [SerializeField] private Transform enemyHolder;
    [SerializeField] private GameObject player;
    [SerializeField] private List<SkillSetups> skillSetups;

    private int waveIndex = 0;

    private int fixedEnemiesSpawned = 0;
    private int pointsSpawned = 0;

    private List<EnemyController> enemies = new List<EnemyController>();

    private void Start()
    {
        InitializePlayer();
        InitializeWave();
    }

    private void InitializeWave()
    {
        StartCoroutine(WaveStartUpSequence());
    }

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

    private void StartWaveSequence(Waves wave)
    {
        SpawnFixedEnemies(wave);
        StartCoroutine(SpawnRandomizedEnemies(wave));
    }

    private IEnumerator SpawnRandomizedEnemies(Waves wave)
    {
        while (pointsSpawned < wave.PointsGoal)
        {
            var enemy = wave.EnemyOptions.GetRandomEntry();
            var spawnPoint = Data.GetRandomEnemySpawnPoint();

            var point = spawnPoint.GetRandomizedSpawnPoint();

            SpawnEnemy(enemy.Enemy, new Vector3(point.x, point.y, 0f));

            pointsSpawned += enemy.Points;

            yield return new WaitForSeconds(Random.Range(wave.MinCoolDownBetweenEnemies, wave.MaxCoolDownBetweenEnemies));
        }
    }

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

            StartCoroutine(SpawnEnemyWithDelay(enemy.SpawnTime, enemy.Enemy, new Vector3(point.x, point.y, 0f), () =>
            {
                fixedEnemiesSpawned++;
            }));
        }
    }

    private IEnumerator SpawnEnemyWithDelay(float time, EnemyController enemy, Vector3 spawnPos, Action onComplete = null)
    {
        yield return new WaitForSeconds(time);

        SpawnEnemy(enemy, spawnPos);

        onComplete?.Invoke();
    }

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

    private void OnEnemyDied(Health health)
    {
        var deadEnemy = health.gameObject.GetComponent<EnemyController>();

        enemies.Remove(deadEnemy);

        CheckWaveState();
    }

    private void CheckWaveState()
    {
        var wave = Data.Waves[waveIndex];
        if(fixedEnemiesSpawned == wave.FixedEnemies.Count && enemies.Count == 0)
        {
            waveIndex++;
            ProcessEndOfWave();
        }
    }

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

    private void EndGame(bool victory)
    {
        if (victory)
        {
            Debug.Log("End Game");
        }
        else
        {
            Debug.Log("Captured");
        }
    }

    private void InitializePlayer()
    {
        // will spawn player

        player.GetComponent<Health>().OnDied += Player_OnDied;

        AddSkillToPlayer(SkillType.Boomereng);
    }

    public void AddSkillToPlayer(SkillType skillType)
    {
        var skill = skillSetups.Find(s => s.Type == skillType);

        player.GetComponent<PlayerCombat>().AddSkill(skill);
    }

    private void Player_OnDied(Health obj)
    {
        EndGame(false);
    }
}
