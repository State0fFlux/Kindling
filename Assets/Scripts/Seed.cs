using UnityEngine;

public class Seed : MonoBehaviour
{
    [Header("Seed Settings")]
    [SerializeField] private float speedMult = 1f;

    [Header("Game Objects")]
    [SerializeField] private GameObject tree;

    // Components
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocityY = -Global.speed * speedMult; // fall down at constant speed
    }

    void Update()
    {
        if (transform.position.y < Global.groundY)
        {
            SpawnTree();
            Destroy(gameObject);
        }
    }
    
    private void SpawnTree()
    {
        // Instantiate a tree at the seed's position
        Instantiate(tree, new Vector2(transform.position.x, Global.groundY), Quaternion.identity);
    }
}