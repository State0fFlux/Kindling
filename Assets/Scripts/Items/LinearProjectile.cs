using Unity.VisualScripting;
using UnityEngine;

public class LinearProjectile : Ranged
{

    public override void Use(Vector2 aimInput, Transform parent)
    {
        Inventory.Instance.Remove(this);
        GameObject clone = Instantiate(gameObject, parent.position, Quaternion.identity);
        Rigidbody2D cloneRb = clone.GetComponent<Rigidbody2D>();
        cloneRb.linearVelocity = GetAimDirection(aimInput) *Global.speed * speedMult;
    }

    public override Vector2 GetAimDirection(Vector2 aimInput)
    {
        if (aimInput.y != 0)
        {
            return new Vector2(0, aimInput.y);
        }
        return aimInput;
    }
}
