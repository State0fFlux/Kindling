using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Furny : HPManager
{
    [Header("Furny Settings")]
    [SerializeField] private float flickerAmplitude; // Amplitude of flicker effect
    [SerializeField] private float flickerFrequency; // Frequency of flicker effect

    // Components
    Light2D fire;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fire = GetComponentInChildren<Light2D>();
        StartCoroutine(Flicker());
    }

    // Update is called once per frame
    void Update()
    {
        // Decrease health points over time
        Hurt(Time.deltaTime);
    }

    void FixedUpdate()
    {
        fire.shapeLightFalloffSize = currHP / maxHP * 12; // reflect hp in light size
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
}
