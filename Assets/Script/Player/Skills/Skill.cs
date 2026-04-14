using System;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public event Action<EnemyController> OnEnemyHited;
    protected Transform player;

    public void SetPlayerTransform(Transform player) => this.player = player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<EnemyController>(out EnemyController enemy))
        {
            OnEnemyHited?.Invoke(enemy);
        }
    }
}
