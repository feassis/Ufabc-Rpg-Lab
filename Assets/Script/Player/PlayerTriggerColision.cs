using System.Collections.Generic;
using UnityEngine;

//classe de detexão que o player usa para detectar os inimigos 
public class PlayerTriggerColision : MonoBehaviour
{
    private List<EnemyController> enemies = new List<EnemyController> ();

    public List<EnemyController> GetEnemies() => enemies;

    //na entrada do trigger adiciona o inimigo a uma lista
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<EnemyController>(out EnemyController enemy))
        {
            enemies.Add(enemy);
        }
    }

    //na saida do trigger remove o inimigo a uma lista
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<EnemyController>(out EnemyController enemy))
        {
            enemies.Remove(enemy);
        }
    }
}