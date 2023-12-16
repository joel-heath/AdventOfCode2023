namespace AdventOfCode2023;
public class JSArray<T> : List<T?>
{
    public int Length { get => Count; set => Count = value; }
    public new int Count
    {
        get => base.Count;
        set
        {
            if (value > base.Count)
            {
                for (int i = base.Count; i < value; i++) Add(default);
            }
            else
            {
                for (int i = base.Count - 1; i >= value; i--) RemoveAt(i);
            }
        }
    }

    public new T? this[int index]
    {
        get
        {
            if (index < Count) return base[index];
            return default;
        }
        set
        {
            if (index < Count) base[index] = value;
            for (int i = Count; i < index; i++)
            {
                Add(default);
            }
            Add(value);
        }
    }
}