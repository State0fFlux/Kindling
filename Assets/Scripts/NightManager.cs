using System.Collections;
using UnityEngine;

public class NightManager : MonoBehaviour
{

    public static NightManager Instance; // Singleton pattern

    [Header("Night Settings")]
    [SerializeField] private AudioClip[] hourTolls;
    [SerializeField] private int hoursInNight = 8;
    [SerializeField] private int wavesPerHour = 3;
    // Stats
    private int hour = 0; // hours since 10:00 PM
    private int wave = 0;

    // Components
    private AudioSource audioSrc;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: persist across scenes
    }

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    public void ResetNight()
    {
        hour = 0;
        wave = -1;
    }

    public IEnumerator IncrementWave()
    {
        wave++;
        if (wave >= wavesPerHour)
        {
            wave = 0;
            hour++;
            GameManager.Instance.IncreaseDifficulty();
        }

        UIManager.Instance.UpdateTime();

        if (wave == 0)
        { // first wave of the hour
            yield return SceneTransitionManager.Instance.FadeMusic(1f, 0f);
            audioSrc.PlayOneShot(hourTolls[hour]);
            yield return new WaitForSeconds(hourTolls[hour].length);
            yield return SceneTransitionManager.Instance.FadeMusic(0f, 1f);
            yield return Furny.Instance.WorriedDialogue();
        }
    }

    public int GetHour() { return hour; }
    public int GetWave() { return wave; }

    public string GetTime()
    {

        if (hour == hoursInNight) { // because 0-indexed, this means final boss
            return "he's coming...";
        }
        float hourFloat = hour + (float)wave / wavesPerHour;
        int timeHour = 22 + Mathf.FloorToInt(hourFloat);
        int displayHour = timeHour % 12;
        if (displayHour == 0) displayHour = 12;

        int minutes = Mathf.RoundToInt((hourFloat % 1f) * 60);
        string period = timeHour < 24 ? "PM" : "AM";

        return $"{displayHour}:{minutes:00} {period}";
    }

    public int HoursInNight() { return hoursInNight; }
    public int WavesPerHour() { return wavesPerHour; }
}