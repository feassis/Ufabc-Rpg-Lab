using UnityEngine;
using TMPro;
using System.Collections.Generic;

//Script para gerenciar a janela de log do diálogo
public class LogWindow : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public TextMeshProUGUI logTextComponent;
    public GameObject panel;

    //Função para alternar a visibilidade do painel de log
    public void ToggleLog()
    {
        bool isActive = !panel.activeSelf;
        panel.SetActive(isActive);

        if (isActive)
        {
            AtualizarTextoLog();
        }
    }

    //Função para adicionar uma nova entrada ao log e atualizar o texto
    private void AtualizarTextoLog()
    {
        List<string> logs = dialogueManager.GetHistory();
        logTextComponent.text = string.Join("\n\n", logs);
    }
}