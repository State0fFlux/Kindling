using UnityEngine;

public class Stat : MonoBehaviour
{
    [Header("Stat Settings")]
    [SerializeField] private GameObject statBar;
    [SerializeField] private float maxStat; // Maximum stat points
    [SerializeField] private float currStat; // Current stat points

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currStat = maxStat; // Initialize current stat to maximum
        UIManager.Instance.UpdateStat(statBar, currStat, maxStat); // Initialize HP in UI
    }

    public void Heal(float amount)
    {
        if (amount < 0f) throw new System.Exception("Healing amount must be positive");
        currStat += amount;
        if (currStat > maxStat) currStat = maxStat; // Cap HP at maximum value
        UIManager.Instance.UpdateStat(statBar, currStat, maxStat);
    }

    public void Hurt(float damage)
    {
        if (damage < 0f) throw new System.Exception("Damage amount must be positive");
        currStat -= damage;
        if (currStat < 0f) currStat = 0f; // Prevent negative HP
        UIManager.Instance.UpdateStat(statBar, currStat, maxStat);
    }

    public void Regen(float rate)
    {
        if (rate < 0f) throw new System.Exception("Regen rate must be positive");
        currStat += rate * Time.deltaTime; // Regenerate stat over time
        if (currStat > maxStat) currStat = maxStat; // Cap HP at maximum value
        UIManager.Instance.UpdateStat(statBar, currStat, maxStat);
    }

    public void Decay(float rate)
    {
        if (rate < 0f) throw new System.Exception("Decay rate must be positive");
        currStat -= rate * Time.deltaTime; // Decay stat over time
        if (currStat < 0f) currStat = 0f; // Prevent negative HP
        UIManager.Instance.UpdateStat(statBar, currStat, maxStat);
    }

    public float GetStat()
    {
        return currStat;
    }

    public float GetMax()
    {
        return maxStat;
    }
}
