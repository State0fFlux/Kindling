using UnityEngine;

public class Sapling : MonoBehaviour
{
    [Header("Sapling Settings")]
    [SerializeField] private float growthRate; // Growth rate per second
    [SerializeField] private float growth = 0; // Initial growth from 0 to 1

    [Header("Game Objects")]
    [SerializeField] private GameObject hive;

    // Components
    private Animator animator;
    private Health health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (growth < 1f)
        {
            growth += Time.deltaTime * growthRate; // Increase growth over time
        }
        else // fully grown
        {
            GameObject obj = Instantiate(hive, transform.position, Quaternion.identity); // Spawn hive when fully grown
            obj.GetComponent<Health>().SetStatAsRatio(health.GetRatio());
            Destroy(gameObject); // Destroy the sapling when fully grown
        }
    }

    void FixedUpdate()
    {
        animator.SetFloat("Growth", growth); // Update animator with current growth
    }

    void OnDeath()
    {
        Destroy(gameObject);
    }
}
