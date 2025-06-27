using UnityEngine;

public abstract class Melee : Weapon
{
    [Header("Melee Settings")]
    [SerializeField] protected int staminaCost = 0; // stamina cost

    public int GetStaminaCost()
    {
        return staminaCost;
    }
}