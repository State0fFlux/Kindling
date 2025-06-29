using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    [Header("Transition Settings")]
    [SerializeField] private float transitionDuration = 1f;

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
    }

    public void TransitionToLose() => StartCoroutine(TransitionToScene("LoseScene"));
    public void TransitionToWin() => StartCoroutine(TransitionToScene("WinScene"));
    public void TransitionToGame() => StartCoroutine(TransitionToScene("GameScene"));
    public void TransitionToLobby() => StartCoroutine(TransitionToScene("LobbyScene"));

    private IEnumerator TransitionToScene(string sceneName)
    {
        // Animate circle closing (radius 1.5 to 0)
        yield return AnimateRadius(1f, 0f, transitionDuration / 2f);

        // Load new scene
        SceneManager.LoadScene(sceneName);

        // Animate circle opening (radius 0 to 1.5)
        yield return AnimateRadius(0f, 1f, transitionDuration / 2f);
    }

    private IEnumerator AnimateRadius(float from, float to, float duration)
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
            yield return null;
        }
        overlayMaterial.SetFloat("_Radius", to);
    }
}