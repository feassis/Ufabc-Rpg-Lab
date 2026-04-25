using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUpPowerUpUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Transform optionsHolder;
    [SerializeField] private Card optionButtonPrefab;

    private readonly List<Card> spawnedCards = new List<Card>();
    private Action<PowerUpData> onSelected;
    private float previousTimeScale = 1f;

    private void Awake()
    {
        Hide();
    }

    public void Show(IReadOnlyList<PowerUpData> options, Stats playerStats, Action<PowerUpData> onSelected)
    {
        if (panel == null || optionsHolder == null || optionButtonPrefab == null)
        {
            Debug.LogWarning("LevelUpPowerUpUI precisa de panel, optionsHolder e optionButtonPrefab configurados.");
            return;
        }

        if (options == null || options.Count == 0)
        {
            return;
        }

        this.onSelected = onSelected;
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        ClearCards();

        for (int i = 0; i < options.Count; i++)
        {
            PowerUpData option = options[i];
            Card card = Instantiate(optionButtonPrefab, optionsHolder);
            card.gameObject.SetActive(true);
            card.Setup(option, playerStats, Select);
            spawnedCards.Add(card);
        }

        panel.SetActive(true);
    }

    private void Select(PowerUpData powerUp)
    {
        Time.timeScale = previousTimeScale;
        Hide();
        onSelected?.Invoke(powerUp);
    }

    private void Hide()
    {
        ClearCards();

        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    private void ClearCards()
    {
        for (int i = spawnedCards.Count - 1; i >= 0; i--)
        {
            if (spawnedCards[i] != null)
            {
                Destroy(spawnedCards[i].gameObject);
            }
        }

        spawnedCards.Clear();
    }

}
