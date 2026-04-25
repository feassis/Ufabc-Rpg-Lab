using System;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI titleText;
    [SerializeField] private TMPro.TextMeshProUGUI descriptionText;
    [SerializeField] private GameObject visualAnchor;
    [SerializeField] private Button button;

    private PowerUpData powerUpData;
    private GameObject visualInstance;

    private void Awake()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }
    }

    public void Setup(PowerUpData data, Stats stats, Health health, Action<PowerUpData> onSelected)
    {
        powerUpData = data;

        if (titleText != null)
        {
            titleText.text = data.Title;
        }

        if (descriptionText != null)
        {
            descriptionText.text = data.GetDisplayText(stats, health);
        }

        if (visualInstance != null)
        {
            Destroy(visualInstance);
        }

        if (data.VisualEffectPrefab != null)
        {
            Transform parent = visualAnchor != null ? visualAnchor.transform : transform;
            visualInstance = Instantiate(data.VisualEffectPrefab, parent);
        }

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onSelected?.Invoke(powerUpData));
        }
    }
}
