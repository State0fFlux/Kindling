using UnityEngine;
public class Inventory : MonoBehaviour
{

    public static Inventory Instance { get; private set; } // Singleton pattern

    [SerializeField] private int slots;
    [SerializeField] private Item[] initialItems;
    [SerializeField] private int[] initialCounts;

    // Stats
    private bool equipped = false;
    private int selectedIndex = 0;
    private ItemStack[] items; // array to hold items in the inventory


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        items = new ItemStack[slots]; // initialize the inventory with the specified number of slots

        for (int i = 0; i < slots; i++)
        {
            if (i < initialItems.Length)
            {
                items[i] = new ItemStack(initialItems[i], initialCounts[i]); // fill the inventory with initial items
            }
            else
            {
                items[i] = null; // fill remaining slots with null
            }
        }

        UIManager.Instance.InitializeInventory(items, selectedIndex, equipped, slots);
    }

    public Item GetEquipped()
    {
        if (equipped)
        {
            return items[selectedIndex].GetItem();
        }
        return null;
    }

    public Item GetSelected()
    {
        return items[selectedIndex].GetItem();
    }

    public void SetEquipped(bool equipped)
    {
        if (items[selectedIndex] == null && equipped)
        {
            Debug.Log("you can't equip nothing!");
            this.equipped = false;
        }
        else
        {
            this.equipped = equipped;
        }
        UIManager.Instance.UpdateInventory(items, selectedIndex, this.equipped);
    }

    public void CycleLeft()
    {
        selectedIndex = (selectedIndex - 1 + items.Length) % items.Length; // wrap around to the last item if going left from the first item
        SetEquipped(true);
    }

    public void CycleRight()
    {
        selectedIndex = (selectedIndex + 1) % items.Length; // wrap around to the first item if going right from the last item
        SetEquipped(true);
    }

    public void Add(Item item)
    {
        // try to add to existing stack
        for (int i = 0; i < items.Length; i++)
        {
            ItemStack stack = items[i];
            if (stack != null && stack.GetItem().Equals(item))
            {
                stack.Add();
                if (i == selectedIndex)
                {
                    SetEquipped(true);
                }
                UIManager.Instance.UpdateInventory(items, selectedIndex, equipped);
                return;
            }
        }

        // no existing stack found, create a new one
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = new ItemStack(item);
                UIManager.Instance.UpdateInventory(items, selectedIndex, equipped);
                return;
            }
        }
    }

    public void Remove(Item item)
    {
    for (int i = 0; i < items.Length; i++)
    {
        ItemStack stack = items[i];
        if (stack != null && stack.GetItem().Equals(item))
        {
            stack.Remove();

            if (stack.GetCount() <= 0)
            {
                items[i] = null;

                // If this was the equipped item, unequip it
                if (i == selectedIndex)
                {
                    SetEquipped(false);
                }
            }

            UIManager.Instance.UpdateInventory(items, selectedIndex, equipped);
            return;
        }
    }
    }

    public bool Contains(Item item)
    {
        foreach (ItemStack stack in items)
        {
            if (stack != null && stack.GetItem().Equals(item))
            {
                return true;
            }
        }
        return false;
    }
}