using UnityEngine;

public class Hammer : Melee
{

    public override void Use(Vector2 aimInput, Transform parent)
    {
        Vector2 aimDirection = GetAimDirection(aimInput);

        RaycastHit2D[] hits = Physics2D.RaycastAll(parent.position, aimDirection, range);
        foreach (var hit in hits)
        {
            print(hit.collider.gameObject);
            if (hit.collider == null) continue;

            if (hit.collider.gameObject.CompareTag("HouseWalls"))
            {
                GetComponent<AudioSource>().Play();
                hit.collider.GetComponent<Health>().Heal(damage);
            }
        }
    }

    public override Vector2 GetAimDirection(Vector2 aimInput)
    {
        return new Vector2(aimInput.x, 0);
    }
}
