using System;
using UnityEngine;

//usado pelo inimigo para detectar o player
public class EnemyTriggerCollision : MonoBehaviour
{

    public event Action<GameObject> OnPlayerEntry;
    public event Action<GameObject> OnPlayerExit;

    //detecta a entrada do player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            OnPlayerEntry?.Invoke(collision.gameObject);
        }
    }

    //detecta a saida do player
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            OnPlayerExit?.Invoke(collision.gameObject);
        }
    }
}
