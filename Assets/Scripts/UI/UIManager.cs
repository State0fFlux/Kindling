using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI Settings")]
    [SerializeField] private Color equippedTint;
    [SerializeField] private Color selectedTint;
    [SerializeField] private Color unselectedTint;
    [SerializeField] private Color unobtainedTint;
    [SerializeField] private int padding;
    [SerializeField] private Sprite emptyIcon;


    [Header("Game Objects")]
    [SerializeField] private GameObject tile;
    [SerializeField] private GameObject inventoryBar;
    [SerializeField] private GameObject fuelBox;
    [SerializeField] private GameObject clock;

    // Stats
    private GameObject[] usableTiles;
    private GameObject fuelTile;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void UpdateStat(GameObject statBar, float currStat, float maxStat)
    {

        Slider slider = statBar.GetComponent<Slider>();
        slider.maxValue = maxStat; // adjust max health bar fill
        slider.value = currStat; // adjust current health bar fill
    }

    public void InitializeInventory(ItemStack[] items, int selectedIndex, bool equipped, int slots)
    {
        inventoryBar.GetComponent<RectTransform>().sizeDelta = new Vector2(padding + slots * (padding + tile.GetComponent<RectTransform>().sizeDelta.x), 2 * padding + tile.GetComponent<RectTransform>().sizeDelta.y);
        usableTiles = new GameObject[slots];
        for (int i = 0; i < slots; i++)
        {
            RectTransform rt = tile.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(padding + i * (padding + rt.sizeDelta.x), 0);
            usableTiles[i] = Instantiate(tile, inventoryBar.transform);
        }

        tile.GetComponent<RectTransform>().anchoredPosition = new Vector2(padding, 0);
        fuelTile = Instantiate(tile, fuelBox.transform);

        UpdateInventory(items, selectedIndex, equipped, null);

    }


    public void UpdateInventory(ItemStack[] inventory, int selectedIndex, bool equipped, ItemStack fuel)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            Image icon = usableTiles[i].transform.GetChild(0).GetComponent<Image>();
            TextMeshProUGUI text = usableTiles[i].GetComponentInChildren<TextMeshProUGUI>();
            icon.sprite = inventory[i] != null ? inventory[i].GetItem().GetIcon() : emptyIcon;
            if (selectedIndex == i)
            {
                if (equipped)
                {
                    icon.color = equippedTint;
                }
                else
                {
                    icon.color = selectedTint;
                }
            }
            else
            {
                icon.color = unselectedTint;
            }
            text.text = (inventory[i] != null && inventory[i].GetItem() is not Melee && inventory[i].GetItem() is not Basket) ? inventory[i].GetCount().ToString() : "";
        }

        Image fuelIcon = fuelTile.transform.GetChild(0).GetComponent<Image>();
        TextMeshProUGUI fuelText = fuelTile.GetComponentInChildren<TextMeshProUGUI>();
        if (fuel == null)
        {
            fuelIcon.sprite = emptyIcon;
            fuelIcon.color = unselectedTint;
            fuelText.text = "";
        }
        else
        {
            fuelIcon.sprite = fuel.GetItem().GetIcon();
            fuelIcon.color = selectedTint;
            fuelText.text = fuel.GetCount().ToString();
        }
    }

    public void UpdateTime()
    {
        clock.GetComponent<TextMeshProUGUI>().text = NightManager.Instance.GetTime();
    }
}
