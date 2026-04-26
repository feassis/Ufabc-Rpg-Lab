using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CooldownDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerCombat combat;
    [SerializeField] private Stats stats;
    [SerializeField] private Image fillImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI cooldownText;
    [SerializeField] private CombatCooldownType cooldownType = CombatCooldownType.Special;

    [Header("Colors")]
    [SerializeField] private Color chargingColor = new Color(0.35f, 0.35f, 0.35f, 1f);
    [SerializeField] private Color chargedColor = new Color(0.15f, 0.8f, 0.25f, 1f);

    [Header("Text")]
    [SerializeField] private bool hideTextWhenCharged = true;

    private float normalizedCooldown = 1f;

    private void Awake()
    {
        if (combat == null)
        {
            combat = GetComponentInParent<PlayerCombat>();
        }

        if (stats == null && combat != null)
        {
            stats = combat.GetComponent<Stats>();
        }
    }

    private void OnEnable()
    {
        Subscribe();
        Refresh();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void OnValidate()
    {
        if (fillImage == null)
        {
            fillImage = GetComponent<Image>();
        }
    }

    public void SetTarget(PlayerCombat newCombat)
    {
        if (combat == newCombat)
        {
            return;
        }

        Unsubscribe();
        combat = newCombat;
        stats = combat != null ? combat.GetComponent<Stats>() : null;
        Subscribe();
        Refresh();
    }

    private void Subscribe()
    {
        if (combat == null)
        {
            return;
        }

        combat.OnCooldownUpdate += OnCooldownUpdate;
    }

    private void Unsubscribe()
    {
        if (combat == null)
        {
            return;
        }

        combat.OnCooldownUpdate -= OnCooldownUpdate;
    }

    public void SetCooldownType(CombatCooldownType newCooldownType)
    {
        cooldownType = newCooldownType;
        Refresh();
    }

    public void Refresh()
    {
        float progress = combat != null ? combat.GetCooldownProgress(cooldownType) : 1f;
        UpdateDisplay(progress);
    }

    private void OnCooldownUpdate(CombatCooldownType updatedCooldownType, float normalizedTimer)
    {
        if (updatedCooldownType != cooldownType)
        {
            return;
        }

        UpdateDisplay(normalizedTimer);
    }

    private void UpdateDisplay(float normalizedTimer)
    {
        normalizedCooldown = Mathf.Clamp01(normalizedTimer);

        if (fillImage != null)
        {
            fillImage.fillAmount = normalizedCooldown;
        }

        bool isCharged = normalizedCooldown >= 1f;

        if (backgroundImage != null)
        {
            backgroundImage.color = isCharged ? chargedColor : chargingColor;
        }

        UpdateText(isCharged);
    }

    private void UpdateText(bool isCharged)
    {
        if (cooldownText == null)
        {
            return;
        }

        if (isCharged && hideTextWhenCharged)
        {
            cooldownText.text = string.Empty;
            return;
        }

        float remainingSeconds = (1f - normalizedCooldown) * GetCooldownDuration();
        cooldownText.text = $"{Mathf.CeilToInt(remainingSeconds)}s";
    }

    private float GetCooldownDuration()
    {
        if (combat != null)
        {
            return combat.GetCooldownDuration(cooldownType);
        }

        if (stats != null)
        {
            switch (cooldownType)
            {
                case CombatCooldownType.Attack:
                    return stats.CurrentAttackCooldown;
                case CombatCooldownType.Special:
                    return stats.SpecialCooldown;
            }
        }

        return 0f;
    }
}
