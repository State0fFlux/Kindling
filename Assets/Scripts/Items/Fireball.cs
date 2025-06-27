using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Animations;

public class Fireball : Item
{
    [Header("Fireball Settings")]
    [SerializeField] private float speedMult = 3f;
    [SerializeField] private bool facingRight = true;

    // Stats
    private int currHits = 0;

    // Components
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        float aim = Input.GetAxisRaw("Aim");
        if (aim > 0)
        {
            rb.linearVelocity = Vector2.up;
        }
        else
        {
            rb.linearVelocity = facingRight ? Vector2.right : Vector2.left;
        }
        rb.linearVelocity *= Global.speed * speedMult;
    }

    void Update()
    {
        if (rb.linearVelocity.sqrMagnitude > 0.01f)
            transform.up = -rb.linearVelocity.normalized;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.transform.GetComponent<Health>()?.Hurt(damage);
        currHits++;
        if (currHits == maxHits) Destroy(gameObject); // Destroy the fireball
    }

    protected override void OnUse(bool facingRight, Transform parent)
    {
        this.facingRight = facingRight;
        Instantiate(gameObject, parent.position, Quaternion.identity);
    }
}
