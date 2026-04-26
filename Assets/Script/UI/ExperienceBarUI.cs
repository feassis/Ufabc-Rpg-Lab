using UnityEngine;
using UnityEngine.UI;
using TMPro;

//classe que controla a UI da barra de experiencia
public class ExperienceBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerExperience targetExperience;
    [SerializeField] private Image fillImage;

    [Header("Numbers")]
    [SerializeField] private bool showNumbers = true;
    [SerializeField] private TextMeshProUGUI valueText;

    private void Awake()
    {
        if (targetExperience == null)
        {
            targetExperience = GetComponentInParent<PlayerExperience>();
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

    public void SetTarget(PlayerExperience newTarget)
    {
        if (targetExperience == newTarget)
        {
            return;
        }

        Unsubscribe();
        targetExperience = newTarget;
        Subscribe();
        Refresh();
    }

    private void Subscribe()
    {
        if (targetExperience == null)
        {
            return;
        }

        targetExperience.OnExperienceChanged += OnExperienceChanged;
    }

    private void Unsubscribe()
    {
        if (targetExperience == null)
        {
            return;
        }

        targetExperience.OnExperienceChanged -= OnExperienceChanged;
    }

    private void OnExperienceChanged(int level, int currentExperience, int experienceToNextLevel)
    {
        UpdateBar(level, currentExperience, experienceToNextLevel);
    }

    public void Refresh()
    {
        if (targetExperience == null)
        {
            return;
        }

        UpdateBar(targetExperience.Level, targetExperience.CurrentExperience, targetExperience.ExperienceToNextLevel);
    }

    private void UpdateBar(int level, int currentExperience, int experienceToNextLevel)
    {
        float normalized = experienceToNextLevel <= 0f ? 0f : (float)currentExperience / experienceToNextLevel;

        if (fillImage != null)
        {
            fillImage.fillAmount = Mathf.Clamp01(normalized);
        }

        UpdateNumbers(level, currentExperience, experienceToNextLevel);
    }

    private void UpdateNumbers(int level, int currentExperience, int experienceToNextLevel)
    {
        if (!showNumbers)
        {
            SetText(string.Empty);
            return;
        }

        SetText($"{level}");
    }

    private void SetText(string value)
    {
        if (valueText != null)
        {
            valueText.text = value;
        }
    }
}
