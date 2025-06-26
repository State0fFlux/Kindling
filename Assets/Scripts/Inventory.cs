using UnityEditor.Search;
using UnityEngine;
public class Inventory : MonoBehaviour
{
    private int slots = 10;
    private Item[] items; // array to hold items in the inventory
    private bool equipped = false;
    private int selectedIndex = 0;

    void Start()
    {
        UIManager.Instance.UpdateInventory(items);
    }

    public Item GetEquipped()
    {
        if (equipped && selectedIndex < items.Length)
        {
            return items[selectedIndex];
        }
        return null;
    }

    public void SetEquipped(bool equipped)
    {
        this.equipped = equipped;
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
}