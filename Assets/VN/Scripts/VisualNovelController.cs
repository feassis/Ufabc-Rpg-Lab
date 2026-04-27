using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class VisualNovelController : MonoBehaviour
{
    [Header("Configurações de Scenes")]
    public string proximaCena;
    public GameObject painelFimDeCena;

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

        //Avança o diálogo quando o espaço ou clique é pressionado, ou automaticamente se o modo auto estiver ativado
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

        //Calcula o tempo necessário para avançar automaticamente, baseado no comprimento do texto e se é uma animação
        string textoLimpo = cenaAtual[index].dialogueText.Replace(" ", "");
        int comprimentoReal = textoLimpo.Length;

        timerAuto += Time.deltaTime;
        float tempoParaAvancar = float.MaxValue;

        //Se for uma animação, usa o tempo definido no ScriptableObject. Se for auto mode, calcula com base no número de caracteres. Caso contrário, não avança automaticamente.
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
        //Debug.Log("Tempo para avançar: " + tempoParaAvancar + "Esse é o tempo por caracter: " + tempoPorCaractere + "esse é o numero de caracteres: " + comprimentoReal);
        //Avança automaticamente se o timer atingir o tempo necessário
        if (timerAuto >= tempoParaAvancar)
        {
            AvancarCena();
        }
    }

    //Função para alternar o modo automático
    public void ToggleAuto()
    {
        autoMode = !autoMode;
        timerAuto = 0f;

        AtualizarBotaoAuto();
    }

    //Função para atualizar o texto do botão de auto mode
    private void AtualizarBotaoAuto()
    {
        if (autoButtonText != null)
        {
            autoButtonText.text = autoMode ? "AUTO: ON" : "AUTO: OFF";
        }
    }

    //Função para avançar para a próxima fala ou cena
    public void AvancarCena()
    {
        //Se ainda houver falas na cena atual, avança para a próxima fala. Caso contrário, exibe o painel de fim de cena
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
            if (painelFimDeCena != null)
            {
                painelFimDeCena.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Painel de fim de cena não foi atribuído no Inspector!");
            }
        }
    }

    //Função para carregar a próxima cena usando o SceneManager
    public void CarregarProximaScene()
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
        //Inicia o diálogo com a primeira fala da cena atual e atualiza o botão de auto mode
        AtualizarBotaoAuto();
        if(cenaAtual != null && cenaAtual.Length > 0)
        {
            dialogueManager.DisplayDialogue(cenaAtual[index]);
        }
    }
}