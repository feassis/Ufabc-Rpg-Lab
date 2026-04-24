using UnityEngine;

//classe para fazer algo seguir um alvo
class Follower : MonoBehaviour
{
    //alvo a ser seguido
    public Transform target;
    //Offset para melhor controle
    public Vector3 offset;

    
    void LateUpdate()
    {
        //atualiza a posição no LateUpdate
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}