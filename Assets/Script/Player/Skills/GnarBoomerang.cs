using System.Collections;
using UnityEngine;

public class GnarBoomerang : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    private float damage = 0;
    private Vector3 initialDirection;
    private float speed;
    private float goingTime;
    private float backingTime;
    private Transform owner;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Health>(out Health health) && !collision.gameObject.Equals(owner.gameObject))
        {
            health.TakeDamage(damage);
        }
    }

    public void Setup(Vector3 dir, Transform owner, float speed, float goingTime, float backingTime, float damage)
    {
        initialDirection = dir;
        this.speed = speed;
        this.goingTime = goingTime;
        this.backingTime = backingTime;
        this.owner = owner;
        this.damage = damage;

        StartCoroutine(Move());
    }

    private void OnValidate()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private IEnumerator Move()
    {
        rb.linearVelocity = initialDirection.normalized * speed;

        yield return new WaitForSeconds(goingTime);

        var newDir = owner.position - transform.position;

        rb.linearVelocity = newDir.normalized * speed;

        yield return new WaitForSeconds(backingTime);

        Destroy(gameObject);
    }
}
