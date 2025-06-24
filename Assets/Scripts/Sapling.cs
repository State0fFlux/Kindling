using UnityEngine;

public class Sapling : MonoBehaviour
{
    [Header("Sapling Settings")]
    [SerializeField] private float growthRate; // Growth rate per second
    [SerializeField] private float growth = 0; // Initial growth from 0 to 1

    // Components
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (growth < 1f)
        {
            growth += Time.deltaTime * growthRate; // Increase growth over time
            if (growth > 1f) growth = 1f; // Clamp to max growth
        }
    }

    void FixedUpdate()
    {
        animator.SetFloat("Growth", growth); // Update animator with current growth
    }
}
