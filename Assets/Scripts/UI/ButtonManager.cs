using UnityEngine;
public class ButtonManager : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Transform howToPlayView;
    [SerializeField] private Transform creditsView;
    [SerializeField] private Transform lobbyView;

    private CameraMover cameraMover;

    void Start()
    {
        cameraMover = GetComponent<CameraMover>();
    }

    public void Play()
    {
        SceneTransitionManager.Instance.TransitionToGame();
    }

    public void Lobby()
    {
        SceneTransitionManager.Instance.TransitionToLobby();
    }

    public void HowToPlay()
    {
        cameraMover.MoveTo(howToPlayView);
    }

    public void Credits()
    {
        cameraMover.MoveTo(creditsView);
    }

    public void Quit()
    {
        SceneTransitionManager.Instance.Quit();
    }

    public void BackToLobby()
    {
        cameraMover.MoveTo(lobbyView);
    }
}
