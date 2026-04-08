using System.Collections.Generic;
using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float damageInterval = 0.5f;

    [Header("Filter")]
    [SerializeField] private bool useTagFilter = true;
    [SerializeField] private string targetTag = "Player";

    [Header("Behavior")]
    [SerializeField] private bool damageOnTrigger = true;
    [SerializeField] private bool damageOnCollision = true;

    private readonly Dictionary<int, float> nextDamageTimes = new Dictionary<int, float>();
    private Health ownerHealth;

    private void Awake()
    {
        ownerHealth = GetComponentInParent<Health>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!damageOnTrigger)
        {
            return;
        }

        TryDamage(other.gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!damageOnTrigger)
        {
            return;
        }

        TryDamage(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        nextDamageTimes.Remove(other.gameObject.GetInstanceID());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!damageOnCollision)
        {
            return;
        }

        TryDamage(collision.gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!damageOnCollision)
        {
            return;
        }

        TryDamage(collision.gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        nextDamageTimes.Remove(collision.gameObject.GetInstanceID());
    }

    private void TryDamage(GameObject target)
    {
        if (target == null)
        {
            return;
        }

        if (useTagFilter && !target.CompareTag(targetTag))
        {
            return;
        }

        Health targetHealth = target.GetComponentInParent<Health>();

        if (targetHealth == null)
        {
            return;
        }

        if (ownerHealth != null && targetHealth == ownerHealth)
        {
            return;
        }

        int targetId = target.GetInstanceID();

        if (nextDamageTimes.TryGetValue(targetId, out float nextAllowedTime) && Time.time < nextAllowedTime)
        {
            return;
        }

        if (targetHealth.TakeDamage(damage))
        {
            nextDamageTimes[targetId] = Time.time + Mathf.Max(0f, damageInterval);
        }
    }
}
