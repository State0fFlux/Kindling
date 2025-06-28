using UnityEngine;

public class Seed : MonoBehaviour
{
    [Header("Seed Settings")]
    [SerializeField] private float speedMult = 1f;
    [SerializeField] private float damage = 25f; // damage dealt to Furny or Gobbo when hit

    [Header("Game Objects")]
    [SerializeField] private GameObject tree;

    // Components
    private Rigidbody2D rb;
    private Animator animator;
    private Health health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocityY = -Global.speed * speedMult; // fall down at constant speed
        rb.rotation = Random.Range(0, 360); // random initial rotation
        rb.angularVelocity = Random.Range(100, 200) * (Random.value > 0.5f ? 1 : -1); // random rotation speed

        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        print("A");
        StopMoving();

        if (collider.gameObject.CompareTag("Hero"))
        {
            collider.gameObject.GetComponent<Stat>().Hurt(damage); // TODO: gobbo eats birds to heal, furny eats wood to heal
            animator.SetTrigger("Die");
        }
        else if (collider.gameObject.CompareTag("Ground"))
        {
            animator.SetTrigger("Plant");
        }
    }

    void StopMoving()
    {
            rb.angularVelocity = 0f; // stop rotation when hitting the ground
            rb.linearVelocityY = 0f; // stop falling
    }

    public void OnPlanted()
    {
        // Instantiate a tree at the seed's position
        GameObject obj = Instantiate(tree, new Vector2(transform.position.x, Global.groundY), Quaternion.identity);
        obj.GetComponent<Health>().SetStatAsRatio(health.GetRatio());
        Destroy(gameObject); // Destroy the seed after planting the tree
    }

    public void OnDeath()
    {
        Destroy(gameObject);
    }
}