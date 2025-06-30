public class ItemStack
{
    private Item item;
    private int count;

    public ItemStack(Item item, int count)
    {
        this.item = item;
        this.count = count;
    }

    public ItemStack(Item item)
    {
        this.item = item;
        count = 1;
    }

    public Item GetItem()
    {
        return item;
    }

    public int GetCount()
    {
        return count;
    }

    public void Add()
    {
        Add(1);
    }

    public void Add(int amount)
    {
        count += amount;
    }

    public void Remove()
    {
        Remove(1);
    }

    public void Remove(int amount)
    {
        count -= amount;
    }
}