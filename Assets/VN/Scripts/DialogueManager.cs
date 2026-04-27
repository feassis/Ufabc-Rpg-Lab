using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    [Header("Componentes de UI")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image backgroundDisplay;
    public Image[] portraitSlots; 

    [Header("Componentes de Audio")]
    public AudioSource audioSource;

    [Header("Configurações de TypeWriter")]
    public float velocidadeDigitacao = 0.05f;
    public bool isTyping = false;
    private List <string> historyLog =  new List<string>();
    private Coroutine typeWriterCoroutine;

    [Header("Configurações de Destaque")]
    public Color corAtiva = Color.white;
    public Color coloreInativa = new Color(0.5f, 0.5f, 0.5f, 1f);

    //Funcao para exibir personagens, background e dialogo na tela, de acordo com os dados do ScriptableObject
    public void DisplayDialogue(DialogueData data)
    {
        //Configura o nome do personagem e o texto do diálogo
        nameText.text = data.characterName;
        dialogueText.text = data.dialogueText;

        //Toca o áudio associado, se houver
        if (data.audioClip != null && audioSource != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(data.audioClip);
        }
        
        //Adiciona o diálogo ao histórico
        string entry = $"<b>{data.characterName}:</b> {data.dialogueText}";
        historyLog.Add(entry);

        //Exibe o texto com efeito de máquina de escrever
        if(typeWriterCoroutine != null) StopCoroutine(typeWriterCoroutine);
        if (data.ehAnimacao || data.finalDeAnimacao)
        {
            dialogueText.text = data.dialogueText;
            isTyping = false;
        }
        else
            typeWriterCoroutine = StartCoroutine(DigitarTexto(data.dialogueText));

        //Configura os retratos dos personagens
        foreach (var slot in portraitSlots) slot.gameObject.SetActive(false);

        for (int i = 0; i < data.characterDisplay.Length; i++)
        {
            if (i >= portraitSlots.Length) break;

            var pData = data.characterDisplay[i];
            Image slot = portraitSlots[i];
            
            slot.sprite = pData.portrait;
            slot.gameObject.SetActive(true);

            if (pData.isTalking)
            {
                slot.color = corAtiva;

                //Atualiza o nome do personagem que está falando
                nameText.text = pData.name;
                data.characterName = pData.name;
                //slot.transform.SetAsLastSibling(); // Garante que o retrato do personagem que está falando fique em destaque
            }
            else
            {
                slot.color = coloreInativa;
            }

            RectTransform rt = slot.GetComponent<RectTransform>();
            ConfigurarPosicao(rt, pData.position);
        }

        //Configura o background
        if (data.backgroundSprite != null)
            backgroundDisplay.sprite = data.backgroundSprite;
    }

    //Coroutine para o efeito de máquina de escrever
    IEnumerator DigitarTexto(string fullText)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in fullText.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(velocidadeDigitacao);
        }
        isTyping = false;
    }

    //Função para completar o texto imediatamente, caso o jogador queira pular a animação
    public void CompletarTexto(string fullText)
    {
        if (typeWriterCoroutine != null) StopCoroutine(typeWriterCoroutine);
        dialogueText.text = fullText;
        isTyping = false;
    }

    //Função para obter o histórico de diálogos
    public List<string> GetHistory()
    {
        return historyLog;
    }

    //Configura a posição do retrato com base na enumeração
    private void ConfigurarPosicao(RectTransform rt, CharacterPosition pos)
    {
        Vector2 anchor = Vector2.zero;
        switch(pos)
        {
            case CharacterPosition.Esquerda: anchor = new Vector2(0.2f, 0.42f); break;
            case CharacterPosition.Centro:   anchor = new Vector2(0.5f, 0.5f); break;
            case CharacterPosition.Direita:  anchor = new Vector2(0.8f, 0.5f); break;
        }
        rt.anchorMin = anchor;
        rt.anchorMax = anchor;
        rt.anchoredPosition = Vector2.zero;
    }
}