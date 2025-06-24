using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private float dropInterval;          // Time between drops

    [Header("Game Objects")]
    [SerializeField] private GameObject[] birds;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnSeedDropper();
            yield return new WaitForSeconds(dropInterval);
        }
    }

    private void SpawnSeedDropper()
    {
        // Random dropper and seed
        GameObject dropper = birds[Random.Range(0, birds.Length)];


        

        // Spawn and configure
        Instantiate(dropper);
    }
}
