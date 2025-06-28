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

    [Header("Game Objects")]
    [SerializeField] private GameObject basket; // this has the trigger collider


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
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();
        stamina = GetComponent<Stamina>();
    }


    void Update()
    {
        health.Regen(0.25f); // Heal slowly over time

        currItem = Inventory.Instance.GetEquipped();

        // Handle hold actions
        if (currItem is Basket)
        {
            if (Input.GetButton("Use"))
            {
                immobilized = true;
                basket.SetActive(true);
            }
            else
            {
                immobilized = false;
                basket.SetActive(false);
            }
        }

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

        if (Input.GetButtonDown("Use"))
        {
            if (currItem is Weapon) {
                aimInput = new Vector2(facingRight ? 1 : -1, Input.GetAxisRaw("Aim"));
                if (aimInput.y < 0) aimInput.y = 0; // Temporary fix, prevent downward aim
                if (currItem is Ranged || stamina.GetStat() >= ((Melee)currItem).GetStaminaCost()) // can afford to use weapon
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
                    Inventory.Instance.CycleRight();
                }
                else if (cycle < 0)
                {
                    cycleCounter = cycleCooldown; // Reset cycle counter
                    Inventory.Instance.CycleLeft();
                }
            }

            if (equip != 0 && Inventory.Instance.GetSelected() != null)
            {
                if (equip > 0 && Inventory.Instance.GetEquipped() == null)
                {
                    Inventory.Instance.SetEquipped(true);
                }
                else if (equip < 0 && Inventory.Instance.GetEquipped() != null)
                {
                    Inventory.Instance.SetEquipped(false);
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
        currItem = Inventory.Instance.GetEquipped();
        if (currItem is Weapon) rb.AddForce(-((Weapon)currItem).GetAimDirection(aimInput) * ((Weapon)currItem).GetRecoil(), ForceMode2D.Impulse);
        ((Weapon)currItem).Use(aimInput, centerOfBody);
        if (currItem is Melee) stamina.Hurt(((Melee)currItem).GetStaminaCost());
    }

    public void TrySneeze()
    {
        if (Random.value <= 0.4f) animator.SetTrigger("Sneeze");
    }
}
