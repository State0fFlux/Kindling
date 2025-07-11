using System;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [Header("Bird Settings")]
    [SerializeField] private float speedMult = 1f;
    [SerializeField] private int dropCount = 2; // number of seeds to drop

    [Header("Game Objects")]
    [SerializeField] private GameObject fuel;

    // Stats
    private float[] dropPoints;
    private bool[] hasDropped;
    private bool flyingRight;

    // Components
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 
        // initialize bird position and direction
        flyingRight = UnityEngine.Random.value > 0.5f;
        GetComponent<SpriteRenderer>().flipX = flyingRight;
        GetComponent<Animator>().speed = speedMult;

        float spawnX = flyingRight ? Global.borderLeft - 1f : Global.borderRight + 1f;
        float spawnY = Global.skyY * UnityEngine.Random.Range(0.8f, 1.2f); // Randomize Y position a bit
        transform.position = new Vector2(spawnX, spawnY);


        float spawnWidth = (Global.borderRight - Global.borderLeft) * 0.8f;
        float leftEdge = -spawnWidth / 2f;
        float segmentWidth = spawnWidth / dropCount;

        dropPoints = new float[dropCount];
        hasDropped = new bool[dropCount];

        for (int i = 0; i < dropCount; i++)
        {
            float segmentStart = leftEdge + i * segmentWidth;
            float segmentEnd = segmentStart + segmentWidth;
            dropPoints[i] = UnityEngine.Random.Range(segmentStart, segmentEnd);
            hasDropped[i] = false;
        }

        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocityX = flyingRight ? Global.speed * speedMult : -Global.speed * speedMult;
    }

    void Update()
    {
        if (transform.position.x < Global.borderLeft - 5f || transform.position.x > Global.borderRight + 5f)
        {
            Destroy(gameObject);
        }

        AttemptSeedDrop();
    }

    public void SetDirection(bool flyingRight)
    {
        this.flyingRight = flyingRight;
    }

    private void AttemptSeedDrop()
    {
        for (int i = 0; i < dropPoints.Length; i++)
        {
            if (!hasDropped[i] && Mathf.Abs(transform.position.x - dropPoints[i]) < 0.1f)
            {
                Instantiate(fuel, transform.position, Quaternion.identity);
                hasDropped[i] = true;
                return; // Only drop one seed per update
            }
        }
    }
}
