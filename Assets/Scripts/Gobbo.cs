using UnityEngine;

public class Gobbo : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float speedMult = 1f;
    [SerializeField] private int fireballs;
    [SerializeField] private float shootCooldown = 1f;

    [Header("Game Objects")]
    [SerializeField] private GameObject fireball;

    // Stats
    private float lastShot = 0f;

    // Components
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


    void Update()
    {
        lastShot += Time.deltaTime;

        if (Input.GetButtonDown("Shoot") && lastShot >= shootCooldown && fireballs > 0)
        {
            Shoot();
        }
        else if (Input.GetButtonDown("Swing"))
        {
            print("B");
        }
        else if (Input.GetButtonDown("Interact"))
        {
            print("C");
        }
    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        animator.SetInteger("MoveX", (int)moveX);

        if (moveX < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveX > 0)
        {
            spriteRenderer.flipX = false;
        }

        if (moveX == 0 && Random.Range(1, 100) == 1)
        { // randomly sneeze 1% of time when idle
            animator.SetTrigger("Sneeze");
        }

        rb.linearVelocityX = moveX * Global.speed * speedMult;
    }

    void Shoot()
    {
        fireballs--;
        lastShot = 0f;
        animator.SetTrigger("Shoot");
        Instantiate(fireball, transform.position, Quaternion.identity);
    }
}
