using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Furny : MonoBehaviour
{
    [Header("Furny Settings")]
    [SerializeField] private float hp = 100f; // Health points, controls light

    // Components
    Light2D fire;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fire = GetComponentInChildren<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Decrease health points over time
        hp -= Time.deltaTime * 1f; // Adjust the rate as needed
        if (hp < 0f) hp = 0f; // Prevent negative health
    }

    void FixedUpdate()
    {
        fire.falloffIntensity = Mathf.Lerp(fire.falloffIntensity, 0.5f + Random.Range(-0.1f, 0.1f), 0.1f); // Interpolate light intensity based on health
        fire.shapeLightFalloffSize = Mathf.Lerp(fire.shapeLightFalloffSize, hp / 10, 0.4f); // reflect hp in light size
    }
}
