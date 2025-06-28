using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Furny : MonoBehaviour
{
    [Header("Furny Settings")]
    [SerializeField] private float flickerAmplitude; // Amplitude of flicker effect
    [SerializeField] private float flickerFrequency; // Frequency of flicker effect

    // Components
    Stat health;
    Light2D fire;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fire = GetComponentInChildren<Light2D>();
        StartCoroutine(Flicker());
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        // Decrease health points over time
        health.Decay(1f);
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

    public void OnDeath()
    {
        Destroy(gameObject);
    }
}
