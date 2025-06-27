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
}