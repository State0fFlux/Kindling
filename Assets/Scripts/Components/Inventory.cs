using UnityEngine;
public class Inventory : MonoBehaviour
{

    public static Inventory Instance { get; private set; } // Singleton pattern

    [SerializeField] private int slots;
    [SerializeField] private Item[] initialItems;
    [SerializeField] private int[] initialCounts;
    [SerializeField] private AudioClip fireAdd;

    // Stats
    private bool equipped = false;
    private int selectedIndex = 0;
    private ItemStack[] items; // array to hold items in the inventory

    // Components
    private AudioSource audioSrc;


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
        audioSrc = GetComponent<AudioSource>();
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
        if (items[selectedIndex] == null) return null;
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

        audioSrc.pitch = equipped ? 1.1f : 0.9f;
        audioSrc.Play();
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
        Add(item, 1);
    }

    public void Add(Item item, int amount)
    {
        // try to add to existing stack
        for (int i = 0; i < items.Length; i++)
        {
            ItemStack stack = items[i];
            if (stack != null && stack.GetItem().Equals(item))
            {
                if (item is Fireball)
                {
                    audioSrc.PlayOneShot(fireAdd);
                }
                stack.Add(amount);
                if (i == selectedIndex && !equipped)
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
                if (item is Fireball)
                {
                    audioSrc.PlayOneShot(fireAdd);
                }
                items[i] = new ItemStack(item, amount);
                if (i == selectedIndex)
                {
                    SetEquipped(true);
                }
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