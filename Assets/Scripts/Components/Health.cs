using System.Collections;
using UnityEngine;

public class Health : Stat
{
    [Header("Health Settings")]
    [SerializeField] private Color flashColor;
    [SerializeField] private float flashDuration = 0.25f;

    private Material mat;
    private SpriteRenderer[] spriteRenderers;

    protected override void Start()
    {
        base.Start();

        // Get all SpriteRenderers in this GameObject and children
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        mat = Instantiate(spriteRenderers[0].material);

        if (flashColor != Color.clear)
        {
            mat.SetColor("_FlashColor", flashColor);
            mat.SetColor("_DissolveColor", flashColor);
        }

        // Assign the instantiated material to all SpriteRenderers
        foreach (var sr in spriteRenderers)
        {
            sr.material = mat;
        }
    }

    void Update()
    {
        if (GetStat() <= 0f)
        {
            StartCoroutine(Die());
        }
    }

    public override void Hurt(float amount)
    {
        StartCoroutine(Flash()); // add visual feedback
        base.Hurt(amount); // apply normal health reduction
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
