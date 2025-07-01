using UnityEngine;

public class Fuel : Item
{

    [SerializeField] private AudioClip collectNoise;

    // Components
    private Rigidbody2D rb;
    private Health health;
    private AudioSource audioSrc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSrc = GetComponent<AudioSource>();

        audioSrc.pitch = 1.0f + Random.Range(-0.1f, 0.1f); // for variety

        rb.rotation = Random.Range(0, 360); // random initial rotation
        rb.angularVelocity = Random.Range(100, 200) * (Random.value > 0.5f ? 1 : -1); // random rotation speed
        health = GetComponent<Health>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.collider.gameObject;
        if (obj.CompareTag("Basket"))
        { // collect pinecone
            obj.GetComponent<AudioSource>().PlayOneShot(collectNoise);
            Inventory.Instance.Add(this);
            Destroy(gameObject);
        } else if (obj.CompareTag("Ground"))
        {
            health.Hurt(health.GetStat()); // deplete health
        }
    }
}