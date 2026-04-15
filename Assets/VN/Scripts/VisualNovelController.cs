using UnityEngine;
using UnityEngine.InputSystem; // Adicione esta linha

public class VisualNovelController : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public DialogueData[] cenaAtual;
    private int index = 0;

    void Start()
    {
        if(cenaAtual != null && cenaAtual.Length > 0)
        {
            dialogueManager.DisplayDialogue(cenaAtual[index]);
        }
    }

    void Update()
    {
        bool espacoApertado = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
        bool cliqueApertado = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;

        if (espacoApertado || cliqueApertado)
        {
            if (index < cenaAtual.Length - 1)
        {
            index++;
            dialogueManager.DisplayDialogue(cenaAtual[index]);
        }
        else
        {
            Debug.Log("Fim da cena.");
        }
    }
        }
    }