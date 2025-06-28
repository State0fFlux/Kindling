using UnityEngine;

public class Hammer : Melee
{

    public override void Use(Vector2 aimInput, Transform parent)
    {
        Vector2 aimDirection = GetAimDirection(aimInput);

        RaycastHit2D[] hits = Physics2D.RaycastAll(parent.position, aimDirection, range, LayerMask.GetMask("Enemy"));
        print(hits);
        int hitCount = 0;

        foreach (var hit in hits)
        {
            if (hit.collider != null)
            {
                var enemy = hit.collider.GetComponent<Health>();
                if (enemy != null)
                {
                    enemy.Hurt(damage);
                    hitCount++;

                    if (hitCount >= maxHits)
                        break;
                }
            }
        }

        Debug.DrawRay(parent.position, aimDirection * range, Color.red, 5f); // debug line
    }

    public override Vector2 GetAimDirection(Vector2 aimInput)
    {
        return new Vector2(aimInput.x, 0);
    }
}
