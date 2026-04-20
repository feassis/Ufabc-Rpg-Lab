using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LogWindow : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public TextMeshProUGUI logTextComponent;
    public GameObject panel;

    public void ToggleLog()
    {
        bool isActive = !panel.activeSelf;
        panel.SetActive(isActive);

        if (isActive)
        {
            AtualizarTextoLog();
        }
    }

    private void AtualizarTextoLog()
    {
        List<string> logs = dialogueManager.GetHistory();
        logTextComponent.text = string.Join("\n\n", logs);
    }
}