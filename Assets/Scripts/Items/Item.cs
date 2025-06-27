using System;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] protected int cost = 0;
    [SerializeField] protected float range;
    [SerializeField] protected int maxHits = 1;
    [SerializeField] protected int damage = 10;
    [SerializeField] protected int count = 100;
    [SerializeField] protected bool indestructible = false;

    [Header("UI Settings")]
    [SerializeField] private Sprite icon; // default icon, can be overridden by child classes

    public bool IsIndestructible()
    {
        return indestructible;
    }
    public int GetCost()
    {
        return cost;
    }

    public int GetCount()
    {
        return count;
    }

    public void Add()
    {
        count++;
    }

    public void Remove()
    {
        count--;
    }

    public void Use(bool facingRight, Transform parent)
    {
        // check for aim
        // some items will require aiming up or down to use (implement in the respective child class)

        // default implementation, can be overridden by child classes
        // this is where the item will be used, e.g., consume stamina, apply effects, etc.
        // after using the item, call the callback to signal completion
        if (!indestructible) Remove();
        OnUse(facingRight, parent);
    }

    protected abstract void OnUse(bool facingRight, Transform parent); // subclasses must implement this

    /*
        void Shoot()
    {
        animator.speed = 1f; // Reset animator speed
        inventory[fireball]--;
        UIManager.Instance.UpdateInventory(inventory); // Update inventory UI
        lastAction = 0f;
        immobilized = true;
        animator.SetTrigger("Shoot");
        Instantiate(fireball, transform.position, Quaternion.identity);
    }

    void Swing()
    {
        animator.speed = 1f; // Reset animator speed
        lastAction = 0f;
        immobilized = true;
        animator.SetTrigger("Swing");
        stamina.Hurt(swingCost);
    }
    */

    public Sprite GetIcon()
    {
        // return a default icon if not overridden
        return icon;
    }
}