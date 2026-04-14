using UnityEngine;
using UnityEngine.UI;

public class SpecialUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private PlayerCombat combat;

    private void Start()
    {
        combat.OnSpecialUpdate += Combat_OnSpecialUpdate;
    }

    private void Combat_OnSpecialUpdate(float normalizedTimer)
    {
        icon.fillAmount = normalizedTimer;
    }

    private void OnDestroy()
    {
        combat.OnSpecialUpdate -= Combat_OnSpecialUpdate;
    }
}
