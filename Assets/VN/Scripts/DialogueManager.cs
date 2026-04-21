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

    public void DisplayDialogue(DialogueData data)
    {
        nameText.text = data.characterName;
        dialogueText.text = data.dialogueText;

        if (data.audioClip != null && audioSource != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(data.audioClip);
        }

        string entry = $"<b>{data.characterName}:</b> {data.dialogueText}";
        historyLog.Add(entry);

        if(typeWriterCoroutine != null) StopCoroutine(typeWriterCoroutine);
        if (data.ehAnimacao || data.finalDeAnimacao)
        {
            dialogueText.text = data.dialogueText;
            isTyping = false;
        }
        else
            typeWriterCoroutine = StartCoroutine(DigitarTexto(data.dialogueText));

        foreach (var slot in portraitSlots) slot.gameObject.SetActive(false);

        for (int i = 0; i < data.characterDisplay.Length; i++)
        {
            if (i >= portraitSlots.Length) break;

            var pData = data.characterDisplay[i];
            Image slot = portraitSlots[i];
            
            slot.sprite = pData.portrait;
            slot.gameObject.SetActive(true);

            RectTransform rt = slot.GetComponent<RectTransform>();
            ConfigurarPosicao(rt, pData.position);
        }

        if (data.backgroundSprite != null)
            backgroundDisplay.sprite = data.backgroundSprite;
    }

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

    public void CompletarTexto(string fullText)
    {
        if (typeWriterCoroutine != null) StopCoroutine(typeWriterCoroutine);
        dialogueText.text = fullText;
        isTyping = false;
    }

    public List<string> GetHistory()
    {
        return historyLog;
    }

    private void ConfigurarPosicao(RectTransform rt, CharacterPosition pos)
    {
        Vector2 anchor = Vector2.zero;
        switch(pos)
        {
            case CharacterPosition.Esquerda: anchor = new Vector2(0.2f, 0.5f); break;
            case CharacterPosition.Centro:   anchor = new Vector2(0.5f, 0.5f); break;
            case CharacterPosition.Direita:  anchor = new Vector2(0.8f, 0.5f); break;
        }
        rt.anchorMin = anchor;
        rt.anchorMax = anchor;
        rt.anchoredPosition = Vector2.zero;
    }
}