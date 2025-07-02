using UnityEngine;
public class ButtonManager : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Transform howToPlayView;
    [SerializeField] private Transform creditsView;
    [SerializeField] private Transform lobbyView;
    [SerializeField] private AudioClip[] buttonClicks;

    private CameraMover cameraMover;

    void Start()
    {
        cameraMover = GetComponent<CameraMover>();
    }

    public void Play()
    {
        GetComponent<AudioSource>().PlayOneShot(buttonClicks[Random.Range(0, buttonClicks.Length)]);
        SceneTransitionManager.Instance.TransitionToGame();
    }

    public void Lobby()
    {
        GetComponent<AudioSource>().PlayOneShot(buttonClicks[Random.Range(0, buttonClicks.Length)]);
        SceneTransitionManager.Instance.TransitionToLobby();
    }

    public void HowToPlay()
    {
        GetComponent<AudioSource>().PlayOneShot(buttonClicks[Random.Range(0, buttonClicks.Length)]);
        cameraMover.MoveTo(howToPlayView);
    }

    public void Credits()
    {
        GetComponent<AudioSource>().PlayOneShot(buttonClicks[Random.Range(0, buttonClicks.Length)]);
        cameraMover.MoveTo(creditsView);
    }

    public void Quit()
    {
        GetComponent<AudioSource>().PlayOneShot(buttonClicks[Random.Range(0, buttonClicks.Length)]);
        SceneTransitionManager.Instance.Quit();
    }

    public void BackToLobby()
    {
        GetComponent<AudioSource>().PlayOneShot(buttonClicks[Random.Range(0, buttonClicks.Length)]);
        cameraMover.MoveTo(lobbyView);
    }
}
