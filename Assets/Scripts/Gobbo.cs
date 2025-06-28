using UnityEngine;
using UnityEngine.UIElements;

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
    private float moveX;
    private Vector2 aimInput;
    private Item currItem;

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

        moveX = Input.GetAxisRaw("Horizontal");
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

        /*
        if (moveX == 0 && Random.value <= 0.01f)
        { // randomly sneeze 1% of time when idle
            animator.SetTrigger("Sneeze");
        }*/

        if (Input.GetButtonDown("Use"))
        {
            aimInput = new Vector2(facingRight ? 1 : -1, Input.GetAxisRaw("Aim"));
            if (aimInput.y < 0) aimInput.y = 0; // Temporary fix, prevent downward aim
            currItem = inventory.GetEquipped();
            if (currItem != null && (!(currItem is Melee) || stamina.GetStat() >= ((Melee)currItem).GetStaminaCost()))
            {
                rb.linearVelocityX = 0;
                immobilized = true;
                animator.speed = 1f; // Reset animator speed

                if (currItem is LinearProjectile)
                {
                    animator.SetTrigger("Shoot");
                }
                else // Melee
                {
                    animator.SetTrigger(currItem.name); // includes a call to Use
                }
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
        if (!immobilized && moveX != 0)
        {
            rb.linearVelocityX = moveX * Global.speed * speedMult;
        }
    }

    public void OnActionComplete()
    {
        immobilized = false; // Reset immobilization after action
    }

    public void OnItemUsed()
    {
        currItem = inventory.GetEquipped();
        if (currItem is Weapon) rb.AddForce(-currItem.GetAimDirection(aimInput) * ((Weapon)currItem).GetRecoil(), ForceMode2D.Impulse);
        currItem.Use(aimInput, centerOfBody);
        if (currItem is Melee) stamina.Hurt(((Melee)currItem).GetStaminaCost());
        inventory.UpdateAfterUse();
    }
    
    public void OnDeath()
    {
        Destroy(gameObject);
    }
}
