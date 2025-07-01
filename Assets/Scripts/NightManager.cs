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
        audioSrc.PlayOneShot(hourTolls[0]);
    }

    public void Increment()
    {
        hour++;
        if (hour >= hoursInNight)
        {
            SceneTransitionManager.Instance.TransitionToWin();
        }
        else
        {
            audioSrc.PlayOneShot(hourTolls[hour]);
            UIManager.Instance.UpdateTime();
        }
    }

    public int GetHour() { return hour; }

    public string GetTime()
    {
    int timeHour = 22 + hour; // 22 is 10 PM in 24-hour time
    int displayHour = timeHour % 12;
    if (displayHour == 0) displayHour = 12;

    string period = timeHour < 24 ? "PM" : "AM";
    return $"{displayHour}:00 {period}";
    }

    public int HoursInNight() { return hoursInNight; }
}