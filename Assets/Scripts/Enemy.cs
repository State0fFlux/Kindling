using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speedMult;

    [Header("Attack")]
    [SerializeField] private float damage;
    [SerializeField] private float cooldown;

    private Rigidbody2D rb;
    private float lastAttackTime = -Mathf.Infinity;
    private Vector2 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Decide direction based on spawn position and Global.furnyX
        float targetX = Global.FurnyX;
        direction = new Vector2(targetX - transform.position.x, 0).normalized;

        // Flip sprite if needed
        if (direction.x < 0)
            GetComponent<SpriteRenderer>().flipX = true;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = direction * Global.speed * speedMult;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (Time.time - lastAttackTime < cooldown)
        {
            return;
        }
        if (collision.collider.CompareTag("HouseWalls") || collision.collider.CompareTag("Furny"))
            {
                Health targetHealth = collision.collider.GetComponentInParent<Health>();
                if (targetHealth != null)
                {
                    targetHealth.Hurt(damage);
                    lastAttackTime = Time.time;
                }
            }
    }
}
