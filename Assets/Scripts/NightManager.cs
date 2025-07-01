using UnityEngine;

public class NightManager : MonoBehaviour
{

    public static NightManager Instance; // Singleton pattern

    [Header("Night Settings")]
    [SerializeField] private int hoursInNight = 8;
    [SerializeField] private AudioClip[] hourTolls;

    // Stats
    private int hour = 0; // hours since 10:00 PM

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
    }

    public void Increment()
    {
        print(hour + "->" + (hour + 1));
        hour++;
        if (hour > hoursInNight)
        {
            SceneTransitionManager.Instance.TransitionToWin();
        }
        else
        {
            audioSrc.PlayOneShot(hourTolls[hour]);
        }
    }

    public int GetHour() { return hour; }

    public int HoursInNight() { return hoursInNight; }
}