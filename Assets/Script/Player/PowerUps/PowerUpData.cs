using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpData", menuName = "Setup/Player/Power Up")]
public class PowerUpData : ScriptableObject
{
    public string Title;
    [TextArea] public string Description;
    public PowerUpType Type;
    public float Value = 1f;
    public Skill SkillPrefab;
    public GameObject VisualEffectPrefab;

    public string GetDisplayText(Stats stats)
    {
        if (string.IsNullOrWhiteSpace(Description))
        {
            return Title;
        }

        string displayText = Description;

        if (Type == PowerUpType.AddDamage)
        {
            float currentValue = stats != null ? stats.Damage : 0f;
            displayText = displayText
                .Replace("{p0}", currentValue.ToString("0"))
                .Replace("{p1}", (currentValue + Value).ToString("0"));
        }

        if (Type == PowerUpType.ReduceAttackCooldown)
        {
            float currentValue = stats != null ? stats.CurrentAttackCooldown : 0f;
            float newValue = stats != null ? stats.AttackCooldownFormula(stats.AttackCooldownModifier + Value) : 0f;
            displayText = displayText
                .Replace("{p0}", $"{currentValue.ToString("0.0")}s")
                .Replace("{p1}", $"{newValue.ToString("0.0")}s");
        }

        if (Type == PowerUpType.ReduceSpecialCooldown)
        {
            float currentValue = stats != null ? stats.SpecialCooldown : 0f;
            float newValue = stats != null ? stats.SpecialCooldownFormula(stats.SpecialCooldownModifier + Value) : 0f;
            displayText = displayText
                .Replace("{p0}", $"{currentValue.ToString("0.0")}s")
                .Replace("{p1}", $"{newValue.ToString("0.0")}s");
        }

        return displayText;
    }
}
