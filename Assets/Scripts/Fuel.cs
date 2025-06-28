using UnityEngine;

public class Fuel : Item
{

    // Components
    private Rigidbody2D rb;
    private Health health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //rb.linearVelocityY = -Global.speed * speedMult; // fall down at constant speed
        rb.rotation = Random.Range(0, 360); // random initial rotation
        rb.angularVelocity = Random.Range(100, 200) * (Random.value > 0.5f ? 1 : -1); // random rotation speed
        health = GetComponent<Health>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Basket")) { // collect pinecone
            Inventory.Instance.Add(this);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Hero") || collision.collider.gameObject.CompareTag("Ground"))
        {
            health.Hurt(health.GetStat()); // deplete health
        }
    }

    void StopMoving()
    {
            rb.angularVelocity = 0f; // stop rotation when hitting the ground
            rb.linearVelocityY = 0f; // stop falling
    }
}