using UnityEditor.SearchService;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public void Play()
    {
        SceneTransitionManager.Instance.TransitionToGame();
    }

    public void Lobby()
    {
        SceneTransitionManager.Instance.TransitionToLobby();
    }
}
