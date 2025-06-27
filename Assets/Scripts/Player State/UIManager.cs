using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI Settings")]
    [SerializeField] private Color equippedTint;
    [SerializeField] private Color selectedTint;
    [SerializeField] private Color unselectedTint;
    [SerializeField] private Color unobtainedTint;


    [Header("Game Objects")]
    [SerializeField] private GameObject[] tiles;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        Transform tilesParent = transform.Find("Tiles");
        tiles = new GameObject[tilesParent.childCount];
        for (int i = 0; i < tilesParent.childCount; i++)
        {
            tiles[i] = tilesParent.GetChild(i).gameObject;
        }
    }

    public void UpdateStat(GameObject statBar, float currStat, float maxStat)
    {
        RectTransform rt = statBar.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(maxStat * 2, rt.sizeDelta.y); // adjust max health bar width

        Slider slider = statBar.GetComponent<Slider>();
        slider.maxValue = maxStat; // adjust max health bar fill
        slider.value = currStat; // adjust current health bar fill
    }

    public void UpdateInventory(Item[] inventory, int selectedIndex, bool equipped)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            Image image = tiles[i].GetComponent<Image>();
            TextMeshProUGUI text = tiles[i].GetComponentInChildren<TextMeshProUGUI>();
            image.sprite = inventory[i] ? inventory[i].GetIcon() : null;
            if (selectedIndex == i)
            {
                if (equipped)
                {
                    image.color = equippedTint;
                }
                else
                {
                    image.color = selectedTint;
                }
            }
            else
            {
                image.color = unselectedTint;
            }
            text.text = (inventory[i] && !inventory[i].IsIndestructible()) ? inventory[i].GetCount().ToString() : "";
        }
    }
}
