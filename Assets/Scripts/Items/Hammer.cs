using UnityEngine;

public class Hammer : Melee
{


    public override void Use(Vector2 aimInput, Transform parent)
    {
        Vector2 aimDirection = GetAimDirection(aimInput);

        RaycastHit2D[] hits = Physics2D.RaycastAll(parent.position, aimDirection, range);
        foreach (var hit in hits)
        {
            if (hit.collider == null) continue;
            
            GameObject obj = hit.collider.gameObject;

            if (obj.CompareTag("Gift"))
            {
                GetComponent<AudioSource>().Play();
                Health health = obj.GetComponent<Health>();
                health.Hurt(health.GetStat()); // destroy gift entirely
            }

            if (obj.CompareTag("HouseWalls"))
            {
                GetComponent<AudioSource>().Play();
                obj.GetComponent<Health>().Heal(damage);
            }
        }
    }

    public bool CanUse(Vector2 aimInput, Transform parent)
    {
        Vector2 aimDirection = GetAimDirection(aimInput);

        RaycastHit2D[] hits = Physics2D.RaycastAll(parent.position, aimDirection, range);
        foreach (var hit in hits)
        {
            if (hit.collider == null) continue;

            GameObject obj = hit.collider.gameObject;

            if (obj.CompareTag("Gift") || obj.CompareTag("HouseWalls")) {
                return true;
            }
        }
        return false;
    }

    public override Vector2 GetAimDirection(Vector2 aimInput)
    {
        return new Vector2(aimInput.x, 0);
    }
}
