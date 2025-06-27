using System.Collections.Generic;
using Unity.Multiplayer.Center.Common.Analytics;
using UnityEngine;

public class Gobbo : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float speedMult = 1f;
    [SerializeField] private Transform centerOfBody;

    [Header("Stamina Settings")]
    [SerializeField] private float staminaRegenRate; // rate at which stamina regenerates
    [SerializeField] private float sprintCost;

    [Header("UI Settings")]
    [SerializeField] private float cycleCooldown; // reference to the health bar UI element

    // Stats
    private bool immobilized = false;
    private bool facingRight = true;
    private bool sprinting = false;
    private float cycleCounter = 0;

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

        print(immobilized);
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
                item.Use(facingRight, centerOfBody);
                immobilized = true;
                animator.speed = 1f; // Reset animator speed
                animator.SetTrigger(item.name);
                stamina.Hurt(item.GetCost());
                inventory.Use();
            }
        }
        else
        {
            float cycle = Input.GetAxisRaw("Cycle Inventory");
            float equip = Input.GetAxisRaw("Equip");
            if (cycle == 0)
            {
                cycleCounter = 0;
            }
            if (cycleCounter <= 0f)
            {
                if (cycle > 0)
                {
                    cycleCounter = cycleCooldown; // Reset cycle counter
                    inventory.CycleRight();
                }
                else if (cycle < 0)
                {
                    cycleCounter = cycleCooldown; // Reset cycle counter
                    inventory.CycleLeft();
                }
            }

            if (equip != 0 && inventory.GetSelected() != null)
            {
                if (equip > 0 && inventory.GetEquipped() == null)
                {
                    inventory.SetEquipped(true);
                }
                else if (equip < 0 && inventory.GetEquipped() != null)
                {
                    inventory.SetEquipped(false);
                }
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

        cycleCounter = cycleCounter - Time.deltaTime;
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
            facingRight = false;
        }
        else if (moveX > 0)
        {
            facingRight = true;
        }
        spriteRenderer.flipX = !facingRight;

        if (moveX == 0 && Random.value <= 0.01f)
        { // randomly sneeze 1% of time when idle
            animator.SetTrigger("Sneeze");
        }

        rb.linearVelocityX = moveX * Global.speed * speedMult;
    }

    public void OnActionComplete()
    {
        immobilized = false; // Reset immobilization after action
    }
}
