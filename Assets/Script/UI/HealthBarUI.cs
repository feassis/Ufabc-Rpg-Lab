using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Health targetHealth;
    [SerializeField] private Image fillImage;
    [SerializeField] private Gradient fillGradient;

    [Header("Numbers")]
    [SerializeField] private bool showNumbers = true;
    [SerializeField] private bool showMaxHealth = true;
    [SerializeField] private TextMeshProUGUI valueText;

    private void Awake()
    {
        if (targetHealth == null)
        {
            targetHealth = GetComponentInParent<Health>();
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

    public void SetTarget(Health newTarget)
    {
        if (targetHealth == newTarget)
        {
            return;
        }

        Unsubscribe();
        targetHealth = newTarget;
        Subscribe();
        Refresh();
    }

    private void Subscribe()
    {
        if (targetHealth == null)
        {
            return;
        }

        targetHealth.OnHealthChanged += OnHealthChanged;
    }

    private void Unsubscribe()
    {
        if (targetHealth == null)
        {
            return;
        }

        targetHealth.OnHealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(float current, float max)
    {
        UpdateBar(current, max);
    }

    public void Refresh()
    {
        if (targetHealth == null)
        {
            return;
        }

        UpdateBar(targetHealth.CurrentHealth, targetHealth.MaxHealth);
    }

    private void UpdateBar(float currentHealth, float maxHealth)
    {
        float normalized = maxHealth <= 0f ? 0f : currentHealth / maxHealth;

        if (fillImage != null)
        {
            fillImage.fillAmount = Mathf.Clamp01(normalized);
            fillImage.color = fillGradient.Evaluate(Mathf.Clamp01(normalized));
        }

        UpdateNumbers(currentHealth, maxHealth);
    }

    private void UpdateNumbers(float currentHealth, float maxHealth)
    {
        if (!showNumbers)
        {
            SetText(string.Empty);
            return;
        }

        string textValue = showMaxHealth
            ? $"{Mathf.CeilToInt(currentHealth)} / {Mathf.CeilToInt(maxHealth)}"
            : $"{Mathf.CeilToInt(currentHealth)}";

        SetText(textValue);
    }

    private void SetText(string value)
    {
        if (valueText != null)
        {
            valueText.text = value;
        }
    }
}
