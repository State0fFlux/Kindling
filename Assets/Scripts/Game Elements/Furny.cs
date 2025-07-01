using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Furny : MonoBehaviour
{
    [Header("Furny Settings")]
    [SerializeField] private Item fireball;
    [SerializeField] private Item pinecone;
    [SerializeField] private float healAmount;
    [SerializeField] private float interval;

    // Stats
    private Coroutine activeCoroutine;
    bool dead = false;

    // Components
    Stat health;
    Light2D fire;
    Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fire = GetComponentInChildren<Light2D>();
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
        }
    }

    void FixedUpdate()
    {
        fire.shapeLightFalloffSize = health.GetStat() / health.GetMax() * 16; // reflect hp in light size
        fire.intensity = fire.shapeLightFalloffSize / 2;
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
            // Heal if pinecones are available
            if (Inventory.Instance.Contains(pinecone))
            {
                Inventory.Instance.Add(fireball, 2); // give 2 fireballs for a quality of life rebalance
                Inventory.Instance.Remove(pinecone);
                if (health.GetStat() + healAmount <= health.GetMax())
                {
                    health.Heal(healAmount);
                }
            }
            yield return new WaitForSeconds(interval);
        }
    }

    public void OnDeath()
    {
        SceneTransitionManager.Instance.TransitionToLose();
    }
}
