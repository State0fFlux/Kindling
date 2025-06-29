using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Furny : MonoBehaviour
{
    [Header("Furny Settings")]
    [SerializeField] private float flickerAmplitude; // Amplitude of flicker effect
    [SerializeField] private float flickerFrequency; // Frequency of flicker effect
    [SerializeField] private Item fireball;
    [SerializeField] private Item pinecone;
    [SerializeField] private float healAmount;
    [SerializeField] private float interval;

    // Stats
    private Coroutine activeCoroutine;
    private bool dead = false;

    // Components
    Stat health;
    Light2D fire;
    Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fire = GetComponentInChildren<Light2D>();
        StartCoroutine(Flicker());
        health = GetComponent<Health>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Decrease health points over time
        health.Decay(1f);
        if (health.GetStat() <= 0f && !dead)
        {
            dead = true;
            anim.SetTrigger("Die");
            SceneTransitionManager.Instance.TransitionToLose();
        }
    }

    void FixedUpdate()
    {
        fire.shapeLightFalloffSize = health.GetStat() / health.GetMax() * 12; // reflect hp in light size
        fire.intensity = fire.shapeLightFalloffSize / 3;
    }

    public IEnumerator Flicker()
    {
        float start = 0.75f;
        while (true)
        {
            float duration = flickerFrequency * Random.Range(0.7f, 1.3f); // randomize flicker duration
            float end = start + Random.Range(-flickerAmplitude / 2, flickerAmplitude / 2); // randomize flicker intensity
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / duration * 2; // half duration for flicker up
                fire.falloffIntensity = Mathf.Lerp(start, end, t);
                yield return null;
            }
            t = 0;
            while (t < 1f)
            {
                t += Time.deltaTime / duration * 2; // half duration for flicker down
                fire.falloffIntensity = Mathf.Lerp(end, start, t);
                yield return null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if (obj.CompareTag("Gobbo") && activeCoroutine == null)
        {
            activeCoroutine = StartCoroutine(GiveFireballAndUsePinecone());
            anim.SetBool("Open", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if (obj.CompareTag("Gobbo") && activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
            activeCoroutine = null;
            anim.SetBool("Open", false);
        }
    }

    private IEnumerator GiveFireballAndUsePinecone()
    {
        while (true)
        {
            // Always add a fireball
            Inventory.Instance.Add(fireball);

            // Heal if pinecones are available
            if (Inventory.Instance.Contains(pinecone) && health.GetStat() + healAmount <= health.GetMax())
            {
                Inventory.Instance.Remove(pinecone);
                health.Heal(healAmount);
            }
            yield return new WaitForSeconds(interval);
        }
    }
}
