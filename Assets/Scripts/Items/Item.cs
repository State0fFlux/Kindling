using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] Sprite icon;

    public Sprite GetIcon() { return icon; }

    public override bool Equals(object obj)
    {
        if (obj is Item other)
            return this.icon == other.icon;
        return false;
    }

        public override int GetHashCode()
    {
        return icon.GetHashCode();
    }
}