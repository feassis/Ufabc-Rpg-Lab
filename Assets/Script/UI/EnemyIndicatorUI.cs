using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Indicador na UI que aponta para inimigos fora da tela relativo ao jogador
public class EnemyIndicatorUI : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private RectTransform container; // container no Canvas (pivot central recomendado)
    [SerializeField] private GameObject indicatorPrefab; // prefab contendo uma Image/arrow
    [SerializeField] private Camera worldCamera;
    [Range(0.1f, 2f)] [SerializeField] private float radiusFraction = 0.45f; // fração do menor lado da tela
    [SerializeField] private float onScreenHideMargin = 0.05f; // margem para considerar on-screen

    private Dictionary<EnemyController, RectTransform> indicators = new Dictionary<EnemyController, RectTransform>();
    private Transform playerTransform;

    private void Awake()
    {
        if (worldCamera == null) worldCamera = Camera.main;
        if (levelManager == null) Debug.LogError("EnemyIndicatorUI precisa de referência para LevelManager.");
        if (container == null) Debug.LogError("EnemyIndicatorUI precisa de um RectTransform container.");
        if (indicatorPrefab == null) Debug.LogError("EnemyIndicatorUI precisa do prefab do indicador.");
    }

    private void Start()
    {
        if (levelManager != null && levelManager.GetPlayerObject() != null)
            playerTransform = levelManager.GetPlayerObject().transform;
    }

    private void Update()
    {
        if (levelManager == null || playerTransform == null || container == null || indicatorPrefab == null) return;

        var enemies = levelManager.GetEnemies();

        // remove indicadores para inimigos que morreram
        var toRemove = new List<EnemyController>();
        foreach (var kv in indicators)
        {
            if (!enemies.Contains(kv.Key) || kv.Key == null)
            {
                Destroy(kv.Value.gameObject);
                toRemove.Add(kv.Key);
            }
        }
        foreach (var e in toRemove) indicators.Remove(e);

        // criar indicadores para novos inimigos
        foreach (var enemy in enemies)
        {
            if (enemy == null) continue;
            if (!indicators.ContainsKey(enemy))
            {
                var go = Instantiate(indicatorPrefab, container);
                var rt = go.GetComponent<RectTransform>();
                if (rt == null) rt = go.AddComponent<RectTransform>();
                indicators[enemy] = rt;
            }
        }

        // atualizar posição/rotacao dos indicadores
        float containerW = container.rect.width;
        float containerH = container.rect.height;
        float radius = Mathf.Min(containerW, containerH) * 0.5f * radiusFraction;

        Vector2 center = Vector2.zero; // assume pivot do container no centro

        foreach (var kv in indicators)
        {
            var enemy = kv.Key;
            var rt = kv.Value;
            if (enemy == null || rt == null) continue;

            Vector3 enemyWorldPos = enemy.GetBodyPos();
            Vector3 screenPos = worldCamera.WorldToScreenPoint(enemyWorldPos);

            bool isInFront = screenPos.z > 0f;
            bool isOnScreen = isInFront && screenPos.x >= Screen.width * onScreenHideMargin && screenPos.x <= Screen.width * (1 - onScreenHideMargin) && screenPos.y >= Screen.height * onScreenHideMargin && screenPos.y <= Screen.height * (1 - onScreenHideMargin);

            if (isOnScreen)
            {
                rt.gameObject.SetActive(false);
                continue;
            }

            rt.gameObject.SetActive(true);

            Vector3 dir = (enemyWorldPos - playerTransform.position);
            dir.z = 0f;
            if (dir.sqrMagnitude <= Mathf.Epsilon)
            {
                rt.anchoredPosition = center;
                continue;
            }

            Vector2 dir2 = new Vector2(dir.x, dir.y).normalized;
            rt.anchoredPosition = center + dir2 * radius;

            float angle = Mathf.Atan2(dir2.y, dir2.x) * Mathf.Rad2Deg;
            rt.localEulerAngles = new Vector3(0f, 0f, angle - 90f);
        }
    }

    private void OnDestroy()
    {
        foreach (var kv in indicators)
        {
            if (kv.Value != null) Destroy(kv.Value.gameObject);
        }
        indicators.Clear();
    }
}
