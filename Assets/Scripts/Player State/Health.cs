using System.Collections;
using UnityEngine;

public class Health : Stat
{
    [Header("Health Settings")]
    [SerializeField] private Color hurtColor = Color.white;
    [SerializeField] private float flashDuration = .25f;

    void Update()
    {
        if (GetStat() <= 0f)
        {
            Animator anim = gameObject.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("Die");
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public override void Hurt(float amount)
    {
        StopAllCoroutines();
        StartCoroutine(Flash()); // add visual feedback
        base.Hurt(amount); // apply normal health reduction
    }

    private IEnumerator Flash()
    {
        Material mat = GetComponent<SpriteRenderer>().material;
        mat.SetColor("_FlashColor", hurtColor);
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
}