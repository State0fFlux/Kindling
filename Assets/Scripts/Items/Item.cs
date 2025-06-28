using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] Sprite icon;

    public Sprite GetIcon() { return icon; }
}