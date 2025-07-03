using System;
using UnityEngine;
public class Inventory : MonoBehaviour
{

    public static Inventory Instance { get; private set; } // Singleton pattern

    [SerializeField] private int slots;
    [SerializeField] private Item[] initialItems;
    [SerializeField] private int[] initialCounts;
    [SerializeField] private AudioClip fireAdd;

    // Stats
    private bool equipped = true;
    private int selectedIndex = 0;
    private ItemStack[] usables; // array to hold items in the inventory
    private ItemStack fuelStack;

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
        usables = new ItemStack[slots]; // initialize the inventory with the specified number of slots

        for (int i = 0; i < slots; i++)
        {
            if (i < initialItems.Length)
            {
                usables[i] = new ItemStack(initialItems[i], initialCounts[i]); // fill the inventory with initial items
            }
            else
            {
                usables[i] = null; // fill remaining slots with null
            }
        }

        UIManager.Instance.InitializeInventory(usables, selectedIndex, equipped, slots);
    }

    public Item GetEquipped()
    {
        if (equipped)
        {
            return usables[selectedIndex].GetItem();
        }
        return null;
    }

    public Item GetSelected()
    {
        if (usables[selectedIndex] == null) return null;
        return usables[selectedIndex].GetItem();
    }

    public void SetEquipped(bool equipped)
    {
        if (usables[selectedIndex] == null && equipped)
        {
            this.equipped = false;
        }
        else
        {
            this.equipped = equipped;
        }
        UIManager.Instance.UpdateInventory(usables, selectedIndex, this.equipped, fuelStack);

        audioSrc.pitch = equipped ? 1.1f : 0.9f;
        audioSrc.Play();
    }

    public void CycleLeft()
    {
        selectedIndex = (selectedIndex - 1 + usables.Length) % usables.Length; // wrap around to the last item if going left from the first item
        SetEquipped(true);
    }

    public void CycleRight()
    {
        selectedIndex = (selectedIndex + 1) % usables.Length; // wrap around to the first item if going right from the last item
        SetEquipped(true);
    }

    public void Add(Item item)
    {
        Add(item, 1);
    }

    public void Add(Item item, int amount)
    {
        if (item is Fuel) {
            if (fuelStack == null)
            {
                fuelStack = new ItemStack(item, amount);
            }
            else
            {
                fuelStack.Add(amount);
            }
            UIManager.Instance.UpdateInventory(usables, selectedIndex, equipped, fuelStack);
            return;
        }

        // try to add to existing stack
        for (int i = 0; i < usables.Length; i++)
        {
            ItemStack stack = usables[i];
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
                UIManager.Instance.UpdateInventory(usables, selectedIndex, equipped, fuelStack);
                return;
            }
        }

        // no existing stack found, create a new one
        for (int i = 0; i < usables.Length; i++)
        {
            if (usables[i] == null)
            {
                if (item is Fireball)
                {
                    audioSrc.PlayOneShot(fireAdd);
                }
                usables[i] = new ItemStack(item, amount);
                if (i == selectedIndex)
                {
                    SetEquipped(true);
                }
                UIManager.Instance.UpdateInventory(usables, selectedIndex, equipped, fuelStack);
                return;
            }
        }
    }

    public void Remove(Item item)
    {
        if (item is Fuel && fuelStack != null)
        {
            fuelStack.Remove();
            if (fuelStack.GetCount() <= 0)
            {
                fuelStack = null;
            }
            UIManager.Instance.UpdateInventory(usables, selectedIndex, equipped, fuelStack);
            return;
        }

        for (int i = 0; i < usables.Length; i++)
        {
            ItemStack stack = usables[i];
            if (stack != null && stack.GetItem().Equals(item))
            {
                stack.Remove();

                if (stack.GetCount() <= 0)
                {
                    usables[i] = null;

                    // If this was the equipped item, unequip it
                    if (i == selectedIndex)
                    {
                        SetEquipped(false);
                    }
                }

                UIManager.Instance.UpdateInventory(usables, selectedIndex, equipped, fuelStack);
                return;
            }
        }
    }

    public bool Contains(Item item)
    {

        if (item is Fuel) {
            return fuelStack != null && fuelStack.GetCount() > 0;
        }

        foreach (ItemStack stack in usables)
        {
            if (stack != null && stack.GetItem().Equals(item))
            {
                return true;
            }
        }
        return false;
    }
}