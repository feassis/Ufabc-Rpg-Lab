using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerColision : MonoBehaviour
{
    private List<EnemyController> enemies = new List<EnemyController> ();

    public List<EnemyController> GetEnemies() => enemies;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<EnemyController>(out EnemyController enemy))
        {
            enemies.Add(enemy);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<EnemyController>(out EnemyController enemy))
        {
            enemies.Remove(enemy);
        }
    }
}