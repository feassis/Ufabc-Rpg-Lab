using UnityEngine;

// Component that scales a SpriteRenderer based on a Health percentage
public class HealthSpriteScaler : MonoBehaviour
{
    [SerializeField] private Health targetHealth;
    [SerializeField] private SpriteRenderer targetSprite;

    [Header("Scale Options")]
    [SerializeField] private bool scaleX = true;
    [SerializeField] private bool scaleY = false;
    [SerializeField, Tooltip("Minimum scale multiplier (0-1)")] private float minScaleMultiplier = 0f;

    private Vector3 initialLocalScale;

    private void Awake()
    {
        if (targetHealth == null)
        {
            targetHealth = GetComponentInParent<Health>();
        }

        if (targetSprite == null)
        {
            targetSprite = GetComponent<SpriteRenderer>();
        }

        initialLocalScale = transform.localScale;
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
        if (targetSprite == null)
        {
            targetSprite = GetComponent<SpriteRenderer>();
        }
    }

    public void SetTarget(Health newTarget)
    {
        if (targetHealth == newTarget) return;

        Unsubscribe();
        targetHealth = newTarget;
        Subscribe();
        Refresh();
    }

    private void Subscribe()
    {
        if (targetHealth == null) return;
        targetHealth.OnHealthChanged += OnHealthChanged;
    }

    private void Unsubscribe()
    {
        if (targetHealth == null) return;
        targetHealth.OnHealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(float current, float max)
    {
        UpdateScale(current, max);
    }

    public void Refresh()
    {
        if (targetHealth == null) return;
        UpdateScale(targetHealth.CurrentHealth, targetHealth.MaxHealth);
    }

    private void UpdateScale(float current, float max)
    {
        float normalized = max <= 0f ? 0f : current / max;
        normalized = Mathf.Clamp01(normalized);

        Vector3 newScale = initialLocalScale;

        if (scaleX)
        {
            float scaled = Mathf.Max(minScaleMultiplier * initialLocalScale.x, normalized * initialLocalScale.x);
            newScale.x = scaled;
        }

        if (scaleY)
        {
            float scaled = Mathf.Max(minScaleMultiplier * initialLocalScale.y, normalized * initialLocalScale.y);
            newScale.y = scaled;
        }

        transform.localScale = newScale;
    }
}
