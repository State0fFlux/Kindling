using System.Collections.Generic;
using Unity.Multiplayer.Center.Common.Analytics;
using UnityEngine;

public class Gobbo : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float speedMult = 1f;
    [SerializeField] private int startingFireballs = 3; // initial number of fireballs;

    [Header("Stamina Settings")]
    [SerializeField] private float staminaRegenRate; // rate at which stamina regenerates
    [SerializeField] private float sprintCost;
    [SerializeField] private float swingCost;

    // Stats
    private bool immobilized = false;
    private bool sprinting = false;

    // Components
    private Health health;
    private Stamina stamina;
    private Inventory inventory;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        inventory = GetComponent<Inventory>();
        health = GetComponent<Health>();
        stamina = GetComponent<Stamina>();
    }


    void Update()
    {
        health.Regen(0.25f); // Heal slowly over time

        if (immobilized) // prevent any actions while immobilized
        {
            return;
        }

        // Handle actions
        if (Input.GetButtonDown("Use"))
        {
            Item item = inventory.GetEquipped();
            if (item != null && stamina.GetStat() >= item.GetCost())
            {
                item.Use();
            }
        }
        else
        {
            float cycle = Input.GetAxisRaw("Cycle Inventory");
            float equip = Input.GetAxisRaw("Equip");
            if (cycle > 0)
            {
                inventory.CycleRight();
            }
            else if (cycle < 0)
            {
                inventory.CycleLeft();
            }
            else if (equip > 0 && inventory.GetEquipped() == null)
            {
                inventory.SetEquipped(true);
            } else if (equip < 0 && inventory.GetEquipped() != null) {
                inventory.SetEquipped(false);
            }
            
        }

        if (Input.GetButtonDown("Sprint") && stamina.GetStat() > 0f && !sprinting)
        {
            sprinting = true; // Set sprinting flag
            speedMult *= 2f;
            animator.speed = speedMult;
        }
        else if ((Input.GetButtonUp("Sprint") || stamina.GetStat() <= 0f) && sprinting)
        {
            sprinting = false; // Reset sprinting flag
            speedMult /= 2f; // Reset speed multiplier
            animator.speed = speedMult;
        }

        if (sprinting)
        {
            stamina.Decay(sprintCost);
        }
        else
        {
            stamina.Regen(staminaRegenRate);
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

        if (moveX == 0 && Random.value <= 0.01f)
        { // randomly sneeze 1% of time when idle
            animator.SetTrigger("Sneeze");
        }

        rb.linearVelocityX = moveX * Global.speed * speedMult;
    }

    void Shoot()
    {
        animator.speed = 1f; // Reset animator speed
        inventory[fireball]--;
        UIManager.Instance.UpdateInventory(inventory); // Update inventory UI
        lastAction = 0f;
        immobilized = true;
        animator.SetTrigger("Shoot");
        Instantiate(fireball, transform.position, Quaternion.identity);
    }

    void Swing()
    {
        animator.speed = 1f; // Reset animator speed
        lastAction = 0f;
        immobilized = true;
        animator.SetTrigger("Swing");
        stamina.Hurt(swingCost);
    }

    public void OnActionComplete()
    {
        immobilized = false; // Reset immobilization after action
    }
}
