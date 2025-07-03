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

    public void PlayRandButtonNoise()
    {
        GetComponent<AudioSource>().PlayOneShot(buttonClicks[Random.Range(0, buttonClicks.Length)]);
    }

    public void Play()
    {
        PlayRandButtonNoise();
        SceneTransitionManager.Instance.TransitionToGame();
    }

    public void Lobby()
    {
        PlayRandButtonNoise();
        SceneTransitionManager.Instance.TransitionToLobby();
    }

    public void HowToPlay()
    {
        PlayRandButtonNoise();
        cameraMover.MoveTo(howToPlayView);
    }

    public void Credits()
    {
        PlayRandButtonNoise();
        cameraMover.MoveTo(creditsView);
    }

    public void Quit()
    {
        PlayRandButtonNoise();
        SceneTransitionManager.Instance.Quit();
    }

    public void BackToLobby()
    {
        PlayRandButtonNoise();
        cameraMover.MoveTo(lobbyView);
    }

    
}
