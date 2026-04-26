using UnityEngine;
using UnityEngine.UI;
using TMPro;

//classe que controla a UI da barra de vida
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

    //se o get do validate não pegar o componente ele tenta pegar no pai do gameobject
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

    //apos qualquer atualização do editor esse metodo é chamado e usamos para pegar o componente imagem do nosso gameobject
    private void OnValidate()
    {
        if (fillImage == null)
        {
            fillImage = GetComponent<Image>();
        }
    }

    //Configura qual componente Health ira seguir
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

    //se registra ao evento de mudança de vida no componente Health 
    private void Subscribe()
    {
        if (targetHealth == null)
        {
            return;
        }

        targetHealth.OnHealthChanged += OnHealthChanged;
    }

    //se remove ao evento de mudança de vida no componente Health
    private void Unsubscribe()
    {
        if (targetHealth == null)
        {
            return;
        }

        targetHealth.OnHealthChanged -= OnHealthChanged;
    }

    //metodo inscrito na mudança de vida
    private void OnHealthChanged(float current, float max)
    {
        UpdateBar(current, max);
    }

    //força a atulização da barra de vida
    public void Refresh()
    {
        if (targetHealth == null)
        {
            return;
        }

        UpdateBar(targetHealth.CurrentHealth, targetHealth.MaxHealth);
    }


    //metodo que pega a vida normaliza ela e atualiza a barra e sua cor
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

    //metodo que atualiza os valores numericos da vida
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

    //coloca o valor no texto de vida
    private void SetText(string value)
    {
        if (valueText != null)
        {
            valueText.text = value;
        }
    }
}
