using UnityEngine;

//classe para controlar a transparencia do shade de transparencia
public class TransparencyController : MonoBehaviour
{
    //referencia ao player
    public Transform player;
    //referencia ao material
    public Material material;
    //offset para melhor controle de posição
    public Vector3 offset;

    void Update()
    {
        material.SetVector("_PlayerPos", player.position + offset);
    }
}