using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private float birdInterval = 5f;       // Time between bird drops
    [SerializeField] private float eventInterval = 10f;     // Time between enemy/present events
    [SerializeField] private float difficultyIncreaseInterval = 60f;
    [SerializeField, Range(0f, 1f)] private float difficulty = 0f; // 0 = easy (more elves), 1 = hard (more trees)

    [Header("Audio Settings")]
    [SerializeField] private AudioClip[] presentSpawnNoises;

    [Header("Game Objects")]
    [SerializeField] private GameObject[] birds;
    [SerializeField] private GameObject[] presents;
    [SerializeField] private GameObject tree;
    [SerializeField] private GameObject[] elves;
    [SerializeField] private Transform[] presentSpawnpoints;

    // Components
    private AudioSource audioSrc;

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        NightManager.Instance.ResetNight();
        StartCoroutine(BirdLoop());
        StartCoroutine(EventLoop());
        StartCoroutine(DifficultyLoop());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneTransitionManager.Instance.TransitionToGame();
        }   
    }

    private IEnumerator BirdLoop()
    {
        while (true)
        {
            SpawnSeedDropper();
            yield return new WaitForSeconds(birdInterval);
        }
    }

    private IEnumerator EventLoop()
    {
        while (true)
        {
            // Half-interval: spawn presents
            yield return new WaitForSeconds(eventInterval / 2f);
            SpawnPresents();

            // Another half: spawn enemies
            yield return new WaitForSeconds(eventInterval / 2f);
            SpawnEnemyCluster();
        }
    }

    private void SpawnSeedDropper()
    {
        GameObject bird = birds[Random.Range(0, birds.Length)];
        Instantiate(bird);
    }

    private void SpawnPresents()
    {
        if (presentSpawnNoises.Length > 0)
        {
            audioSrc.PlayOneShot(presentSpawnNoises[Random.Range(0, presentSpawnNoises.Length)]);
        }

        int count = Random.Range(1, 4); // 1 to 3 presents
        float spacing = 1; // Adjust based on present size
        Vector3 basePosition = presentSpawnpoints[Random.Range(0, presentSpawnpoints.Length)].position;

        for (int i = 0; i < count; i++)
        {
            Vector3 offset = new Vector3(0, i * spacing, 0);
            Instantiate(presents[Random.Range(0, presents.Length)], basePosition + offset, Quaternion.identity);
        }
    }

    private void SpawnEnemyCluster()
    {
        int totalEnemies = Mathf.RoundToInt(Mathf.Lerp(1, 3, difficulty)); // 2-6 enemies depending on difficulty

        for (int i = 0; i < totalEnemies; i++)
        {
            GameObject enemy = Random.value < difficulty ? tree : elves[Random.Range(0, elves.Length)];
            Instantiate(enemy);
        }
    }

    private IEnumerator DifficultyLoop()
    {
        while (NightManager.Instance.GetHour() < NightManager.Instance.HoursInNight())
        {
            yield return new WaitForSeconds(difficultyIncreaseInterval);
            IncreaseDifficulty();
        }
    }

    public void IncreaseDifficulty()
    {
        NightManager.Instance.Increment();
        difficulty += 1f / NightManager.Instance.HoursInNight(); ;
        difficulty = Mathf.Clamp01(difficulty);
        Debug.Log($"Hour {NightManager.Instance.GetHour()} | Difficulty increased to {difficulty:F2}");
    }
}
