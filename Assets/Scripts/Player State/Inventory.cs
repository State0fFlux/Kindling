using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
public class Inventory : MonoBehaviour
{
    [SerializeField] private Item[] items; // array to hold items in the inventory
    
    // Stats
    private int slots = 8;
    private bool equipped = false;
    private int selectedIndex = 0;

    void Start()
    {
        Item[] initialItems = items; // initialize with empty items
        items = new Item[slots]; // initialize the inventory with the specified number of slots

        for (int i = 0; i < slots; i++)
        {
            if (i < initialItems.Length)
            {
                items[i] = initialItems[i]; // fill the inventory with initial items
            }
            else
            {
                items[i] = null; // fill remaining slots with null
            }
        }

        UIManager.Instance.UpdateInventory(items, selectedIndex, equipped);
    }

    public Item GetEquipped()
    {
        if (equipped)
        {
            return items[selectedIndex];
        }
        return null;
    }

    public Item GetSelected()
    {
        return items[selectedIndex];
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

    public void Use()
    {
        Item item = items[selectedIndex];
        if (item.GetCount() <= 0)
        {
            items[selectedIndex] = null;
            SetEquipped(false);
        }
        UIManager.Instance.UpdateInventory(items, selectedIndex, equipped);
    }
}