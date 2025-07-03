using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton pattern

    [Header("Game Settings")]
    [SerializeField] private float birdInterval = 5f;
    [SerializeField] private float presentInterval = 7f;
    [SerializeField] private int minEnemies = 3;
    [SerializeField] private int maxEnemies = 6;
    [SerializeField] private float downTime = 5f; // time before first wave starts

    [Header("Audio Settings")]
    [SerializeField] private AudioClip[] presentSpawnNoises;

    [Header("Game Objects")]
    [SerializeField] private GameObject bird;
    [SerializeField] private GameObject[] presents;
    [SerializeField] private GameObject tree;
    [SerializeField] private GameObject[] elves;
    [SerializeField] private Transform[] presentSpawnpoints;
    [SerializeField] private GameObject santaBoss;
    [SerializeField] private GameObject pauseMenu;

    // Stats
    private AudioSource audioSrc;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private int enemyCount = 0;
    private bool isSpawningWave = false;
    private float difficulty = 0f;
    private bool isFinalBossActive = false;
    private bool paused = false;
    private GameObject santaInstance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        NightManager.Instance.ResetNight();

        StartCoroutine(BirdLoop());
        StartCoroutine(WaveSystem());
    }

    private void Update()
    {
        print(enemyCount);
        if (Input.GetKeyDown(KeyCode.Escape) && !paused)
        {
            FindFirstObjectByType<ButtonManager>().PlayRandButtonNoise();
            Pause();
        }
        /*
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (activeEnemies[i] == null)
            {
                activeEnemies.RemoveAt(i);
            }
        }*/
    }

    public void Pause()
    {
        SceneTransitionManager.Instance.PlayCloser();

        AudioSource[] allAudio = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (var audio in allAudio)
        {
            audio.Pause();
        }

        paused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        SceneTransitionManager.Instance.PlayOpener();

        AudioSource[] allAudio = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (var audio in allAudio)
        {
            audio.UnPause();
        }

        FindFirstObjectByType<AudioListener>().enabled = true;
        paused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    private IEnumerator WaveSystem()
    {
        yield return new WaitForSeconds(downTime);

        while (true)
        {
            yield return NightManager.Instance.IncrementWave();
            // Final boss logic
            if (IsFinalWave() && !isFinalBossActive)
            {
                if (enemyCount == 0) //(activeEnemies.Count == 0)
                {
                    isFinalBossActive = true;
                    santaInstance = Instantiate(santaBoss);
                    yield break;
                }
            }
            // Regular wave logic
            else if (!isSpawningWave && enemyCount == 0)//activeEnemies.Count == 0)
            {
                yield return StartCoroutine(HandleNextWave());
            }

            yield return null;
        }
    }

    private IEnumerator HandleNextWave()
    {
        if (NightManager.Instance.GetHour() == 0 && NightManager.Instance.GetWave() == 0)
        { // first wave of the night!
            StartCoroutine(PresentLoop());
        }

        isSpawningWave = true;

        StartCoroutine(SpawnEnemyCluster());
        yield return new WaitUntil(() => enemyCount == 0); //activeEnemies.Count == 0); // wait until all are dead
        isSpawningWave = false;
    }


    private bool IsFinalWave()
    {
        return NightManager.Instance.GetHour() == NightManager.Instance.HoursInNight(); // 0-indexed, surpassed the cap
    }



    private IEnumerator BirdLoop()
    {
        while (true)
        {
            SpawnSeedDropper();
            yield return new WaitForSeconds(birdInterval);
        }
    }

    private IEnumerator PresentLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(presentInterval);
            SpawnPresents();
        }
    }

    private void SpawnSeedDropper()
    {
        Instantiate(bird);
    }

    private void SpawnPresents()
    {
        if (presentSpawnNoises.Length > 0)
        {
            audioSrc.PlayOneShot(presentSpawnNoises[Random.Range(0, presentSpawnNoises.Length)]);
        }

        int count = Random.Range(1, 4);
        float spacing = 1;
        Vector3 basePosition = presentSpawnpoints[Random.Range(0, presentSpawnpoints.Length)].position;

        for (int i = 0; i < count; i++)
        {
            Vector3 offset = new Vector3(0, i * spacing, 0);
            Instantiate(presents[Random.Range(0, presents.Length)], basePosition + offset, Quaternion.identity);
        }
    }

    private IEnumerator SpawnEnemyCluster()
    {
        int totalEnemies = Mathf.RoundToInt(Mathf.Lerp(minEnemies, maxEnemies, difficulty));
        enemyCount += totalEnemies;
        for (int i = 0; i < totalEnemies; i++)
        {
            GameObject enemy = Random.value < difficulty ? tree : elves[Random.Range(0, elves.Length)];
            GameObject instance = Instantiate(enemy);
            if (instance.CompareTag("Boss"))
            {
                instance.GetComponent<Health>().onDeathCallback = () =>
                {
                    // trigger win or cutscene
                    SceneTransitionManager.Instance.TransitionToWin();
                };
            }
            else
            {
                instance.GetComponent<Health>().onDeathCallback = () => enemyCount--;
            } 

            //activeEnemies.Add(instance);
            yield return new WaitForSeconds(Random.Range(1, 3)); // stagger enemies
        }
    }

    public void IncreaseDifficulty()
    {
        difficulty += 1f / (NightManager.Instance.HoursInNight() - 1);
        difficulty = Mathf.Clamp01(difficulty);
    }
}
