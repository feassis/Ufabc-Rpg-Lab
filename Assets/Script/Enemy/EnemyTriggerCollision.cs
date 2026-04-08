using System;
using UnityEngine;

public class EnemyTriggerCollision : MonoBehaviour
{

    public event Action<GameObject> OnPlayerEntry;
    public event Action<GameObject> OnPlayerExit;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            OnPlayerEntry?.Invoke(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            OnPlayerExit?.Invoke(collision.gameObject);
        }
    }
}
