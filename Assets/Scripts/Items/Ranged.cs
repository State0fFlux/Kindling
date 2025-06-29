using UnityEngine;

public abstract class Ranged : Weapon
{
    [Header("Ranged Settings")]
    [SerializeField] protected float speedMult = 3f;
    [SerializeField] protected float spriteRotation = 0f;

    // Stats
    protected int currHits = 0;

    // Components
    protected Rigidbody2D rb;
    private Health health;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
    }

    protected void Update()
    {
        if (rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(-rb.linearVelocity.y, -rb.linearVelocity.x) * Mathf.Rad2Deg;
            angle -= spriteRotation + 90; // adjust for sprite's original direction
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        print(collision);
        GameObject obj = collision.collider.gameObject;
        if (obj.layer == LayerMask.NameToLayer("Goodwall") || obj.layer == LayerMask.NameToLayer("Uglywall"))
        {
            Explode();
            return;
        }

        obj.transform.GetComponent<Health>()?.Hurt(damage);
        currHits++;
        if (currHits == maxHits) Explode();
    }

    void Explode()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        health.Hurt(health.GetStat()); // kill off
    }
}