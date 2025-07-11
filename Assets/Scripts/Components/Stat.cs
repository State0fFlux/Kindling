using UnityEngine;

public abstract class Stat : MonoBehaviour
{
    [Header("Stat Settings")]
    [SerializeField] private GameObject statBar;
    [SerializeField] private float maxStat; // Maximum stat points
    [SerializeField] private float currStat; // Current stat points

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        if (statBar != null) UIManager.Instance.UpdateStat(statBar, currStat, maxStat); // Initialize HP in UI
    }

    public virtual void Heal(float amount)
    {
        if (amount < 0f) throw new System.Exception("Healing amount must be positive");
        currStat += amount;
        if (currStat > maxStat) currStat = maxStat; // Cap HP at maximum value
        if (statBar != null) UIManager.Instance.UpdateStat(statBar, currStat, maxStat);
    }

    public virtual void Hurt(float damage)
    {
        if (damage < 0f) throw new System.Exception("Damage amount must be positive");
        currStat -= damage;
        if (currStat < 0f) currStat = 0f; // Prevent negative HP
        if (statBar != null) UIManager.Instance.UpdateStat(statBar, currStat, maxStat);
    }

    public void Regen(float rate)
    {
        if (rate < 0f) throw new System.Exception("Regen rate must be positive");
        Heal(rate * Time.deltaTime);
    }

    public void Decay(float rate)
    {
        if (rate < 0f) throw new System.Exception("Decay rate must be positive");
        currStat -= rate * Time.deltaTime;
        if (currStat < 0f) currStat = 0f; // Prevent negative HP
        if (statBar != null) UIManager.Instance.UpdateStat(statBar, currStat, maxStat);
    }

    public float GetStat()
    {
        return currStat;
    }

    public float GetMax()
    {
        return maxStat;
    }

    public float GetRatio()
    {
        return currStat / maxStat;
    }

    public void SetMax(float maxStat)
    {
        this.maxStat = maxStat;
        if (statBar != null) UIManager.Instance.UpdateStat(statBar, currStat, maxStat);
    }

    public void SetStatAsRatio(float percentage)
    {
        currStat = percentage * maxStat;
        if (statBar != null) UIManager.Instance.UpdateStat(statBar, currStat, maxStat);
    }
}
