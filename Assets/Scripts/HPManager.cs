using UnityEngine;

public class HPManager : MonoBehaviour
{
    [Header("HP Settings")]
    [SerializeField] private GameObject hpBar;
    [SerializeField] protected float currHP; // Current health points
    [SerializeField] protected float maxHP; // Maximum health points

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIManager.Instance.UpdateHP(gameObject, currHP, maxHP); // Initialize HP in UI
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Heal(float amount)
    {
        currHP += amount;
        if (currHP > maxHP) currHP = maxHP; // Cap HP at maximum value
        UIManager.Instance.UpdateHP(hpBar, currHP, maxHP);
    }

    public void Hurt(float damage)
    {
        currHP -= damage;
        if (currHP < 0f) currHP = 0f; // Prevent negative HP
        UIManager.Instance.UpdateHP(hpBar, currHP, maxHP);
    }

    public float GetHP()
    {
        return currHP;
    }
}
