using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "NovoPassoTutorial", menuName = "Tutorial/Passo")]
public class TutorialStep : ScriptableObject
{
    public string titulo;
    [TextArea(3, 5)]
    public string instrucao;
    public VideoClip videoExemplo;
}