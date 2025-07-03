using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Furny : MonoBehaviour
{

    public static Furny Instance;

    [Header("Furny Settings")]
    [SerializeField] private Item fireball;
    [SerializeField] private Item pinecone;
    [SerializeField] private float healAmount;
    [SerializeField] private float interval;

    [Header("Dialogue Settings")]
    private string[] fedDialogue = {"I'm hungry!", "Feed me, Gobbo!", "So hungry...", "Yummy!", "More pinecones, please.", "Thanks!", "Ooooh! That one had sap!", "Nom nom nom." };
    [SerializeField] AudioClip[] fedAudio;
    private string[] worriedDialogue = { "Something's coming...", "I smell trouble, and it's not burnt wood", "They're out there, I know it.", "Why does it always get worse?", "Hold me tighter. Wait, I’m a furnace.", "So much for a silent night." };
    [SerializeField] AudioClip[] worriedAudio;
    private string[] hurtDialogue = { "Ow!", "Owchie", "You broke my pilot light!", "My knobs! Be gentle!", "You ever punch a furnace? Hurts both of us!" };
    [SerializeField] AudioClip[] hurtAudio;
    private float lastSpoken = 0f;
    private float promptFrequency = 3f;
    [SerializeField] GameObject dialogueBubble;

    // Stats
    private Coroutine activeCoroutine;

    // Components
    Health health;
    Light2D fire;
    Animator anim;
    AudioSource audioSrc;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fire = GetComponentInChildren<Light2D>();
        health = GetComponent<Health>();
        anim = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();

        health.onDeathCallback = SceneTransitionManager.Instance.TransitionToLose;
    }

    // Update is called once per frame
    void Update()
    {
        // Decrease health points over time
        health.Decay(1f);
        lastSpoken += Time.deltaTime;
    }

    void FixedUpdate()
    {
        fire.shapeLightFalloffSize = health.GetStat() / health.GetMax() * 16; // reflect hp in light size
        fire.intensity = fire.shapeLightFalloffSize / 2;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if (obj.CompareTag("Gobbo") && activeCoroutine == null)
        {
            activeCoroutine = StartCoroutine(GiveFireballAndUsePinecone());
            anim.SetBool("Open", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if (obj.CompareTag("Gobbo") && activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
            activeCoroutine = null;
            anim.SetBool("Open", false);
        }
    }

    private IEnumerator GiveFireballAndUsePinecone()
    {
        while (true)
        {
            // Heal if pinecones are available
            if (Inventory.Instance.Contains(pinecone))
            {
                Inventory.Instance.Add(fireball, 2); // give 2 fireballs for a quality of life rebalance
                Inventory.Instance.Remove(pinecone);
                if (health.GetStat() + healAmount <= health.GetMax())
                {
                    StartCoroutine(FedDialogue());
                    health.Heal(healAmount);
                }
            }
            yield return new WaitForSeconds(interval);
        }
    }
    
    // I'm so sorry about this, this definitely could be consolidated to be more modular but I was lazy...

    public IEnumerator FedDialogue()
    {
        if (lastSpoken > promptFrequency)
        {
            AudioClip clip = fedAudio[Random.Range(0, fedAudio.Length)];
            string quip = fedDialogue[Random.Range(0, fedDialogue.Length)];
            lastSpoken = 0f;
            audioSrc.PlayOneShot(clip);
            dialogueBubble.GetComponentInChildren<TextMeshProUGUI>().text = quip;
            dialogueBubble.SetActive(true);

            yield return new WaitForSeconds(clip.length);
            yield return new WaitForSeconds(2f);
            dialogueBubble.SetActive(false);
        }
    }

    public IEnumerator WorriedDialogue()
    {
        if (lastSpoken > promptFrequency)
        {
            AudioClip clip = worriedAudio[Random.Range(0, worriedAudio.Length)];
            string quip = worriedDialogue[Random.Range(0, worriedDialogue.Length)];
            lastSpoken = 0f;
            audioSrc.PlayOneShot(clip);
            dialogueBubble.GetComponentInChildren<TextMeshProUGUI>().text = quip;
            dialogueBubble.SetActive(true);

            yield return new WaitForSeconds(clip.length);
            yield return new WaitForSeconds(2f);
            dialogueBubble.SetActive(false);
        }
    }

    public IEnumerator HurtDialogue()
    {
        if (lastSpoken > promptFrequency)
        {
            AudioClip clip = hurtAudio[Random.Range(0, hurtAudio.Length)];
            string quip = hurtDialogue[Random.Range(0, hurtDialogue.Length)];
            lastSpoken = 0f;
            audioSrc.PlayOneShot(clip);
            dialogueBubble.GetComponentInChildren<TextMeshProUGUI>().text = quip;
            dialogueBubble.SetActive(true);

            yield return new WaitForSeconds(clip.length);
            yield return new WaitForSeconds(5f);
            dialogueBubble.SetActive(false);
        }
    }
}
