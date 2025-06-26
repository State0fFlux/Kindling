using UnityEngine;

public class Item : MonoBehaviour // make this into an interface??
{
    [SerializeField] private int cost;

    public int GetCost()
    {
        return cost;
    }

    public void Use()
    {
        // check for aim
        // some items will require aiming up or down to use (implement in the respective child class)
    }
}