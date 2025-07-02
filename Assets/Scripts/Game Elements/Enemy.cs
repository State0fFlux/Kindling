using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speedMult;

    [Header("Attack")]
    [SerializeField] private float damage;
    [SerializeField] private float cooldown;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip[] spawnNoise;
    [SerializeField] private AudioClip[] attackNoise;

    // State
    private float lastAttackTime = -Mathf.Infinity;
    private Vector2 direction;

    // Components
    private Rigidbody2D rb;
    private Animator anim;
    private AudioSource audioSrc;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();

        if (spawnNoise.Length > 0)
        {
            audioSrc.PlayOneShot(spawnNoise[Random.Range(0, spawnNoise.Length)]);
        }

        // Randomly spawn on the left or right
        if (Random.value > 0.5f)
            transform.position = new Vector2(Global.borderLeft - 1f, -5);
        else
            transform.position = new Vector2(Global.borderRight + 1f, -5);

        // Decide direction based on spawn position and Global.furnyX
        float targetX = Global.FurnyX;
        direction = new Vector2(targetX - transform.position.x, 0).normalized;

        // Flip sprite if needed
        if (direction.x < 0)
            GetComponent<SpriteRenderer>().flipX = true;
    }

    void FixedUpdate()
    {
        if (rb.bodyType != RigidbodyType2D.Static)
        {
            rb.linearVelocity = direction * Global.speed * speedMult;
            anim.SetFloat("Speed", rb.linearVelocity.sqrMagnitude);
        }
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
                if (collision.collider.CompareTag("Furny"))
                {
                    StartCoroutine(Furny.Instance.HurtDialogue());
                }

                anim.SetTrigger("Attack");

                if (attackNoise.Length > 0)
                {
                    audioSrc.PlayOneShot(attackNoise[Random.Range(0, attackNoise.Length)]);
                }

                targetHealth.Hurt(damage);
                lastAttackTime = Time.time;
            }
        }
    }

    void OnDestroy()
    {
        if (CompareTag("Boss"))
        {
            SceneTransitionManager.Instance.TransitionToWin();
        }  
    }
}
