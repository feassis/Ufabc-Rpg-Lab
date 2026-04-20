using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class VisualNovelController : MonoBehaviour
{
    [Header("Configurações de Scenes")]
    public string proximaCena;

    [Header ("Componentes")]
    public DialogueManager dialogueManager;
    public DialogueData[] cenaAtual;
    private int index = 0;
    public LogWindow logWindow;

    [Header("Configurações de Auto")]
    public bool autoMode = false;
    public TextMeshProUGUI autoButtonText;
    public float tempoPorCaractere = 1f;
    private float tempoMinimo = 1.5f;
    private float timerAuto = 0f;

    void Update()
    {
        bool espacoApertado = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
        bool cliqueApertado = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && !EventSystem.current.IsPointerOverGameObject();
        bool logAberto = logWindow != null && logWindow.panel != null && logWindow.panel.activeSelf;

        if (logAberto) return;

        if (espacoApertado || cliqueApertado)
        {
            if  (dialogueManager.isTyping)
            {
                dialogueManager.CompletarTexto(cenaAtual[index].dialogueText);
            }
            else
            {
                AvancarCena();
            }
            return;
        }

        string textoLimpo = cenaAtual[index].dialogueText.Replace(" ", "");
        int comprimentoReal = textoLimpo.Length;

        timerAuto += Time.deltaTime;
        float tempoParaAvancar = float.MaxValue;

        if (cenaAtual[index].ehAnimacao)
        {
            tempoParaAvancar = cenaAtual[index].tempoExibicao;
        }
        else if (autoMode)
        {
            tempoParaAvancar = Mathf.Max(tempoMinimo, comprimentoReal * tempoPorCaractere);
        }
        else
        {
            tempoParaAvancar = float.MaxValue;
        }
        Debug.Log("Tempo para avançar: " + tempoParaAvancar + "Esse é o tempo por caracter: " + tempoPorCaractere + "esse é o numero de caracteres: " + comprimentoReal);
        if (timerAuto >= tempoParaAvancar)
        {
            AvancarCena();
        }
    }

    public void ToggleAuto()
    {
        autoMode = !autoMode;
        timerAuto = 0f;

        AtualizarBotaoAuto();
    }

    private void AtualizarBotaoAuto()
    {
        if (autoButtonText != null)
        {
            autoButtonText.text = autoMode ? "AUTO: ON" : "AUTO: OFF";
        }
    }

    public void AvancarCena()
    {
        if (index < cenaAtual.Length - 1)
        {
            index++;
            dialogueManager.DisplayDialogue(cenaAtual[index]);
            timerAuto = 0f;
        }
        else
        {
            Debug.Log("Fim da cena atual.");
            autoMode = false;
            CarregarProximaScene();
        }
    }

    private void CarregarProximaScene()
    {
        if (!string.IsNullOrEmpty(proximaCena))
        {
            SceneManager.LoadScene(proximaCena);
        }
        else
        {
            Debug.LogWarning("Nome da próxima cena não foi definido no Inspector!");
        }
    }

    void Start()
    {   
        AtualizarBotaoAuto();
        if(cenaAtual != null && cenaAtual.Length > 0)
        {
            dialogueManager.DisplayDialogue(cenaAtual[index]);
        }
    }
}