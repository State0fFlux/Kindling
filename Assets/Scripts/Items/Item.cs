using System;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] protected int count = 0;
    [SerializeField] protected Sprite icon; // default icon, can be overridden by child classes

    public int GetCount()
    {
        return count;
    }

    public Sprite GetIcon()
    {
        // return a default icon if not overridden
        return icon;
    }

    public void Add()
    {
        Add(1);
    }

    public void Add(int amount)
    {
        count += amount;
    }

    public void Remove()
    {
        Remove(1);
    }

    public void Remove(int amount)
    {
        count -= amount;
    }

    public abstract void Use(Vector2 aimInput, Transform parent); // subclasses must implement this
    public abstract Vector2 GetAimDirection(Vector2 aimInput);
}