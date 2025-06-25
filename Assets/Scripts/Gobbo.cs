using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gobbo : HPManager
{
    [Header("Player Settings")]
    [SerializeField] private float speedMult = 1f;
    [SerializeField] private int startingFireballs = 3; // initial number of fireballs;
    [SerializeField] private float actionCooldown = 1f;

    [Header("Game Objects")]
    [SerializeField] private GameObject fireball;

    // Stats
    private float lastAction = 0f;
    private bool immobilized = false;
    private Dictionary<GameObject, int> inventory = new Dictionary<GameObject, int>();

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

        inventory.Add(fireball, startingFireballs); // Initialize inventory with fireballs
        UIManager.Instance.UpdateInventory(inventory);
    }


    void Update()
    {
        Heal(Time.deltaTime / 4); // Heal slowly over time

        lastAction += Time.deltaTime;

        if (Input.GetButtonDown("Shoot") && lastAction >= actionCooldown && inventory[fireball] > 0)
        {
            Shoot();
        }
        else if (Input.GetButtonDown("Swing") && lastAction >= actionCooldown)
        {
            Swing();
        }
        else if (Input.GetButtonDown("Interact"))
        {
            print("C");
        }
    }

    void FixedUpdate()
    {
        if (immobilized)
        {
            rb.linearVelocityX = 0f; // Stop movement if immobilized
            return;
        }

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
        inventory[fireball]--;
        UIManager.Instance.UpdateInventory(inventory); // Update inventory UI
        lastAction = 0f;
        immobilized = true;
        animator.SetTrigger("Shoot");
        Instantiate(fireball, transform.position, Quaternion.identity);
    }

    void Swing()
    {
        lastAction = 0f;
        immobilized = true;
        animator.SetTrigger("Swing");
    }

    public void OnActionComplete()
    {
        immobilized = false; // Reset immobilization after action
    }
}
