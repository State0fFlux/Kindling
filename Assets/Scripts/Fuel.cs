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

    void OnCollisionEnter2D(Collision2D collision)
    {
        print("collision: " + collision.collider);
        if (collision.collider.gameObject.CompareTag("Basket"))
        { // collect pinecone
            Inventory.Instance.Add(this);
            Destroy(gameObject);
        } else if (!collision.collider.gameObject.CompareTag("HouseWalls"))
        {
            health.Hurt(health.GetStat()); // deplete health
        }
    }
}