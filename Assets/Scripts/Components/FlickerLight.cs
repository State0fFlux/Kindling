using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlickerLight : MonoBehaviour
{
    [Header("Flicker Settings")]
    [SerializeField] private float flickerAmplitude = 0.2f; // Amplitude of flicker effect
    [SerializeField] private float flickerFrequency = 0.1f; // Frequency of flicker effect

    // Components
    Light2D fire;

    void Start()
    {
        fire = GetComponent<Light2D>();
        StartCoroutine(Flicker());
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