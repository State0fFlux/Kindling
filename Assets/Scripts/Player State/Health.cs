using System.Collections;
using UnityEngine;

public class Health : Stat
{
    [Header("Health Settings")]
    private float flashDuration = 0.25f;
    private Material mat;

    protected override void Start()
    {
        base.Start();
        var renderer = GetComponent<SpriteRenderer>();
        renderer.material = Instantiate(renderer.material);
        mat = renderer.material;
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
        //StopAllCoroutines();
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