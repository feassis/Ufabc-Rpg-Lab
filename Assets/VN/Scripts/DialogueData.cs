using UnityEngine;

public enum CharacterPosition { Esquerda, Centro, Direita }

[System.Serializable]
public class CharacterDisplay
{
    public Sprite portrait;
    public CharacterPosition position;
}

[CreateAssetMenu(fileName = "NovaFala", menuName = "VN/Lines")]

public class DialogueData : ScriptableObject
{
    public string characterName;
    [TextArea(3, 10)]
    public string dialogueText;
    public CharacterDisplay[] characterDisplay;
    public Sprite backgroundSprite;
}