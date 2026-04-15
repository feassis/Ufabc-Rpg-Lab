using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("Componentes de UI")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image backgroundDisplay;
    public Image[] portraitSlots; 

    public void DisplayDialogue(DialogueData data)
    {
        nameText.text = data.characterName;
        dialogueText.text = data.dialogueText;

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