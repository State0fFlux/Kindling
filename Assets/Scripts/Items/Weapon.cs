using UnityEngine;

public abstract class Weapon : Item
{
    [Header("Weapon Settings")]
    [SerializeField] protected float range;
    [SerializeField] protected int maxHits = 1;
    [SerializeField] protected int damage = 10;
    [SerializeField] protected float recoil;

    public float GetRecoil()
    {
        return recoil;
    }

    public abstract void Use(Vector2 aimInput, Transform parent); // subclasses must implement this
    public abstract Vector2 GetAimDirection(Vector2 aimInput);
}