using UnityEngine;

//Enumeração para posições dos personagens
public enum CharacterPosition { Esquerda, Centro, Direita }

//Classe para configurar os retratos dos personagens
[System.Serializable]
public class CharacterDisplay
{
    public Sprite portrait;
    public CharacterPosition position;
}

//ScriptableObject para armazenar os dados de cada fala do diálogo
[CreateAssetMenu(fileName = "NovaFala", menuName = "VN/Lines")]
public class DialogueData : ScriptableObject
{
    [Header("Configurações de Diálogo")]
    public string characterName;
    [TextArea(3, 10)]
    public string dialogueText;
    public CharacterDisplay[] characterDisplay;
    public Sprite backgroundSprite;
    
    [Header("Configurações de Audio")]
    public AudioClip audioClip;

    [Header("Configurações de Animação")]
    public bool ehAnimacao;
    public bool finalDeAnimacao;
    public float tempoExibicao = 0.2f;
}