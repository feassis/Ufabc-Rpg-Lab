using UnityEngine;

public class DartProjectile : MonoBehaviour
{
    private Vector2 direction;
    private ProjectileData data;
    private float lifeRemaining;
    private GameObject owner;

    public void Initialize(Vector2 shotDirection, ProjectileData projectileData, GameObject projectileOwner)
    {
        direction = shotDirection.normalized;
        data = projectileData;
        lifeRemaining = Mathf.Max(0.1f, data.Lifetime);
        owner = projectileOwner;
    }

    private void Update()
    {
        if (data == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position += (Vector3)(direction * Mathf.Max(0.1f, data.Speed) * Time.deltaTime);

        lifeRemaining -= Time.deltaTime;
        if (lifeRemaining <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryHit(collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryHit(collision.gameObject);
    }

    private void TryHit(GameObject target)
    {
        if (target == null)
        {
            return;
        }

        if (owner != null && target.transform.root == owner.transform.root)
        {
            return;
        }

        if (target.TryGetComponent<Health>(out Health targetHealth))
        {
            targetHealth.TakeDamage(Mathf.Max(0f, data != null ? data.Damage : 0f));
        }

        Destroy(gameObject);
    }
}
