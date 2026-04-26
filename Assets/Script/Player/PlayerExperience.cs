using System;
using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    [SerializeField] private int level = 1;
    [SerializeField] private int currentExperience;
    [SerializeField] private int baseExperienceToLevel = 50;
    [SerializeField] private float experienceGrowth = 1.25f;

    public event Action<int> OnLevelUp;
    public event Action<int, int, int> OnExperienceChanged;

    public int Level => level;
    public int CurrentExperience => currentExperience;
    public int ExperienceToNextLevel => GetExperienceToNextLevel(level);

    private void Start()
    {
        NotifyExperienceChanged();
    }

    public void AddExperience(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        currentExperience += amount;

        while (currentExperience >= ExperienceToNextLevel)
        {
            currentExperience -= ExperienceToNextLevel;
            level++;
            OnLevelUp?.Invoke(level);
        }

        NotifyExperienceChanged();
    }

    private int GetExperienceToNextLevel(int targetLevel)
    {
        return Mathf.Max(1, Mathf.RoundToInt(baseExperienceToLevel * Mathf.Pow(experienceGrowth, targetLevel - 1)));
    }

    private void NotifyExperienceChanged()
    {
        OnExperienceChanged?.Invoke(level, currentExperience, ExperienceToNextLevel);
    }
}