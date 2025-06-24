using UnityEditor.Callbacks;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [Header("Fireball Settings")]
    [SerializeField] private float speedMult = 3f;
    
    // Components
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocityY = Global.speed * speedMult; // shoot upwards at constant speed
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sky Enemy"))
        {
            Destroy(other.gameObject);   // Kill the enemy
            Destroy(gameObject);         // Destroy the fireball
        }
    }
}
