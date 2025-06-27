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
    private SpriteRenderer sr;
    private Health health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();

        //sr.sortingOrder = Random.value > 0.5f ? 1 : -1; // Randomly set sorting order for visual variety
    }

    // Update is called once per frame
    void Update()
    {
        if (health.GetStat() <= 0f) Die();
            if (growth < 1f)
            {
                growth += Time.deltaTime * growthRate; // Increase growth over time
            }
        if (growth >= 1f)
        {
            //hive.GetComponent<SpriteRenderer>().sortingOrder = sr.sortingOrder; // Match hive sorting order
            Instantiate(hive, transform.position, Quaternion.identity); // Spawn hive when fully grown
            Destroy(gameObject); // Destroy the sapling when fully grown
        }
    }

    void FixedUpdate()
    {
        animator.SetFloat("Growth", growth); // Update animator with current growth
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
