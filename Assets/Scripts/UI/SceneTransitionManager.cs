using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    [Header("Transition Settings")]
    [SerializeField] private float transitionDuration = 1f;
    [SerializeField] private AudioClip opening;
    [SerializeField] private AudioClip closing;
    [SerializeField] private AudioClip lobbyTheme;
    [SerializeField] private AudioClip gameTheme;
    [SerializeField] private AudioClip prologueTheme;
    [SerializeField] private AudioClip winTheme;
    [SerializeField] private AudioClip loseTheme;

    // Components
    private AudioSource sfxSrc;
    private AudioSource musicSrc;
    private AudioClip currentMusic;

    private RawImage overlayImage;            // Assign your RawImage here in Inspector
    private Material overlayMaterial;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        overlayImage = GetComponentInChildren<RawImage>();
        overlayMaterial = Instantiate(overlayImage.material);
        overlayImage.material = overlayMaterial;
        overlayMaterial.SetFloat("_Radius", 1f); // Start fully covered

        sfxSrc = GetComponent<AudioSource>();

        musicSrc = gameObject.AddComponent<AudioSource>();
        musicSrc.loop = true;
        musicSrc.volume = 1f;
        
        musicSrc.clip = lobbyTheme;
        musicSrc.Play();
        currentMusic = lobbyTheme;
    }

    public void TransitionToLose() => StartCoroutine(TransitionToScene("LoseScene"));
    public void TransitionToWin() => StartCoroutine(TransitionToScene("WinScene"));
    public void TransitionToGame() => StartCoroutine(TransitionToScene("GameScene"));
    public void TransitionToLobby() => StartCoroutine(TransitionToScene("LobbyScene"));
    public void Quit() => StartCoroutine(QuitCoroutine());

    private IEnumerator QuitCoroutine()
    {
        sfxSrc.PlayOneShot(closing);

        yield return AnimateRadius(1f, 0f, transitionDuration / 2);
        Application.Quit();
    }

    private IEnumerator TransitionToScene(string sceneName)
    {
        AudioClip nextMusic = GetMusicForScene(sceneName);


        sfxSrc.PlayOneShot(closing);
        yield return AnimateRadius(1f, 0f, transitionDuration / 2f, nextMusic != currentMusic);

        musicSrc.clip = nextMusic;
        musicSrc.Play();
        SceneManager.LoadScene(sceneName);

        sfxSrc.PlayOneShot(opening);
        yield return AnimateRadius(0f, 1f, transitionDuration / 2f, nextMusic != currentMusic);
        currentMusic = nextMusic;
    }

    private IEnumerator AnimateRadius(float from, float to, float duration, bool affectAudio = false)
    {
        if (overlayMaterial == null)
            yield break;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float eased = Mathf.SmoothStep(from, to, t);
            overlayMaterial.SetFloat("_Radius", eased);

            if (affectAudio && musicSrc != null)
            {
                // Ease pitch down or up
                musicSrc.pitch = Mathf.Lerp(from, to, t);
            }

            yield return null;
        }

        overlayMaterial.SetFloat("_Radius", to);

        if (affectAudio && musicSrc != null)
        {
            musicSrc.pitch = to;
        }
    }


    private IEnumerator FadeOutMusic()
    {
        float duration = transitionDuration / 2f;
        float startVol = musicSrc.volume;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            musicSrc.volume = Mathf.Lerp(startVol, 0f, t / duration);
            yield return null;
        }
        musicSrc.volume = 0f;
    }

    private IEnumerator FadeInMusic()
    {
        float duration = transitionDuration / 2f;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            musicSrc.volume = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }
        musicSrc.volume = 1f;
    }
    
    private AudioClip GetMusicForScene(string sceneName)
    {
        return sceneName switch
        {
            "LobbyScene" => lobbyTheme,
            "CreditsScene" => lobbyTheme,
            "PrologueScene" => prologueTheme,
            "GameScene" => gameTheme,
            "HowToPlayScene" => lobbyTheme,
            "LoseScene" => loseTheme,
            "WinScene" => winTheme,
            // Add others as needed
            _ => null
        };
    }
}