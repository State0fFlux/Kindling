using UnityEngine;

public class Axe : Item
{
    protected override void OnUse(bool facingRight, Transform parent)
    {
        Vector2 direction = facingRight ? Vector2.right : Vector2.left;

        RaycastHit2D[] hits = Physics2D.RaycastAll(parent.position, direction, range, LayerMask.GetMask("Enemy"));
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

        Debug.DrawRay(parent.position, direction * range, Color.red, 0.3f); // debug line
    }
}
