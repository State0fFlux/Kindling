public class Health : Stat
{
    void Update()
    {
        if (GetStat() <= 0f)
        {
            Destroy(gameObject);
        }
    }
}