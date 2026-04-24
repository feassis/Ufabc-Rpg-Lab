using UnityEngine;
using UnityEngine.UI;

//Ui que representa o estado do especial do jogador
public class SpecialUI : MonoBehaviour
{
    //referencia ao componente Image do Icone do especial
    [SerializeField] private Image icon;
    //referencia ao controlador das habilidades de combate do jogador
    [SerializeField] private PlayerCombat combat;

    private void Start()
    {
        //adiciona um metodo ao evento de update do special no PlayerCombat
        combat.OnSpecialUpdate += Combat_OnSpecialUpdate;
    }

    private void Combat_OnSpecialUpdate(float normalizedTimer)
    {
        //atualiza a propriedade de fill da imagem
        icon.fillAmount = normalizedTimer;
    }

    private void OnDestroy()
    {
        //remove um metodo ao evento de update do special no PlayerCombat
        combat.OnSpecialUpdate -= Combat_OnSpecialUpdate;
    }
}
