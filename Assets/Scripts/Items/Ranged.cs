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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Dome")) Destroy(gameObject);

        other.transform.GetComponent<Health>()?.Hurt(damage);
        currHits++;
        if (currHits == maxHits) Destroy(gameObject); // Destroy the fireball
    }
}