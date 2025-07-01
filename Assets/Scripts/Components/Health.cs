using System.Collections;
using UnityEditor.Callbacks;
using UnityEngine;

public class Health : Stat
{
    [Header("Health Settings")]
    [SerializeField] private Color hurtColor = Color.red;
    [SerializeField] private Color healColor = Color.green;
    [SerializeField] private float flashDuration = 0.25f;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip[] hurtNoise;
    [SerializeField] private AudioClip healNoise;
    [SerializeField] private AudioClip[] deathNoise;

    // Stats
    private SpriteRenderer[] spriteRenderers;
    private bool hasDied = false;

    // Components
    private Material mat;
    private AudioSource audioSrc;
    private Collider2D[] cols;


    protected override void Start()
    {
        base.Start();

        audioSrc = GetComponent<AudioSource>();
        cols = GetComponentsInChildren<Collider2D>();

        // Get all SpriteRenderers in this GameObject and children
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        mat = Instantiate(spriteRenderers[0].material);
        mat.SetColor("_DissolveColor", hurtColor);

        // Assign the instantiated material to all SpriteRenderers
        foreach (var sr in spriteRenderers)
        {
            sr.material = mat;
        }
    }

    void Update()
    {
        if (GetStat() <= 0f && !hasDied)
        {
            StartCoroutine(Die());
        }
    }

    public override void Hurt(float amount)
    {
        mat.SetColor("_FlashColor", hurtColor);

        StartCoroutine(Flash()); // add visual feedback
        base.Hurt(amount); // apply normal health reduction

        if (GetStat() > 0f && hurtNoise.Length > 0) {
            print("hurt");
            audioSrc.PlayOneShot(hurtNoise[Random.Range(0, hurtNoise.Length)]);
        }
    }

    public override void Heal(float amount)
    {
        mat.SetColor("_FlashColor", healColor);

        StartCoroutine(Flash()); // add visual feedback
        base.Heal(amount); // apply normal health reduction

        if (healNoise != null)
        {
            audioSrc.pitch = 1 + Random.Range(-0.5f, 0.5f);
            audioSrc.PlayOneShot(healNoise);
            audioSrc.pitch = 1;
        }
    }

    private IEnumerator Flash()
    {
        float timer = 0f;

        while (timer < flashDuration)
        {
            float t = timer / flashDuration;
            mat.SetFloat("_FlashIntensity", 1 - t);

            timer += Time.deltaTime;
            yield return null;
        }

        mat.SetFloat("_FlashIntensity", 0);
    }

    private IEnumerator Die()
    {
        hasDied = true;

        // Disable all colliders
        foreach (var col in cols)
        {
            col.enabled = false;
        }

        if (TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.bodyType = RigidbodyType2D.Static;
        }
        
        if (deathNoise.Length > 0)
            {
                audioSrc.PlayOneShot(deathNoise[Random.Range(0, deathNoise.Length)]);
            }
        
        float timer = 0f;

        while (timer < flashDuration)
        {
            float t = timer / flashDuration;
            mat.SetFloat("_DissolveIntensity", t);

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
