using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("Painéis")]
    public GameObject panelCredits;

    // Função para abrir e fechar o painel
    public void SetCreditsActive(bool state)
    {
        if (panelCredits != null)
        {
            panelCredits.SetActive(state);
        }
    }

    // Função de Toggle (alternar)
    public void ToggleCredits()
    {
        if (panelCredits != null)
        {
            bool currentState = panelCredits.activeSelf;
            panelCredits.SetActive(!currentState);
        }
    }
}