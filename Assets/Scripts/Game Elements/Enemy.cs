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

    // State
    private float lastAttackTime = -Mathf.Infinity;
    private Vector2 direction;

    // Components
    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

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
        anim.SetFloat("Speed", rb.linearVelocity.sqrMagnitude);
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
                    anim.SetTrigger("Attack");
                    targetHealth.Hurt(damage);
                    lastAttackTime = Time.time;
                }
            }
    }
}
