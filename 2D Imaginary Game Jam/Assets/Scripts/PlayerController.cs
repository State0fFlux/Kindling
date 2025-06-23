using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        animator.SetInteger("MoveX", (int)moveX);
        animator.SetInteger("MoveY", (int)moveY);

        if (moveX < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveX > 0)
        {
            spriteRenderer.flipX = false;
        }

        if (moveX == 0 && moveY == 0 && Random.Range(1, 100) == 1)
        { // randomly sneeze 1% of time when idle
            animator.SetTrigger("Sneeze");
        }

        rb.linearVelocity = new Vector2(moveX, moveY).normalized * speed;
    }
}
