using System.Collections;
using System;

public class BTree<TKey, TValue, TTreeElement> where TKey : System.IComparable<TKey>
                                               where TTreeElement : BTreeElement<TKey, TValue>, new()
{
    const int m_min_elements = 2;
    int m_max_elements = m_min_elements;

    private BTreeElement<TKey, TValue> m_root;

    private void Init()
    {
        m_root = new TTreeElement();
        m_root.Init(m_max_elements);
    }

    public BTree()
    {
        Init();
    }

    public BTree(int _max_elements)
    {
        m_max_elements = (_max_elements < m_min_elements) ? m_min_elements : _max_elements;

        Init();
    }

    public int MaxElements()
    {
        return m_max_elements;
    }

    public Pair<TKey, TValue> Find(TKey _key)
    {
        return m_root.Find(_key);
    }

    public void Insert(TKey _key, TValue _value)
    {
        BinaryElement<TKey, TValue> insert = m_root.Insert(_key, _value);

        if (insert != null)
        {
            m_root = m_root.New();
            m_root.Init(m_max_elements, insert);
        }
    }

    protected BTreeElement<TKey, TValue> Root()
    {
        return m_root;
    }
}

public class Pair<TKey, TValue>
{
    public Pair(TKey _key, TValue _value)
    {
        Key = _key;
        Value = _value;
    }

    public TKey Key;
    public TValue Value;
}

public class BinaryElement<TKey, TValue> where TKey : System.IComparable<TKey>
{
    public BinaryElement(BTreeElement<TKey, TValue> _left, BTreeElement<TKey, TValue> _right, Pair<TKey, TValue> _value)
    {
        Left = _left;
        Right = _right;
        Value = _value;
    }

    public BTreeElement<TKey, TValue> Left;
    public BTreeElement<TKey, TValue> Right;

    public Pair<TKey, TValue> Value;
}

public class BTreeElement<TKey, TValue> where TKey : System.IComparable<TKey>
{
    int m_max_elements = 2;
    int m_elements_count = 0;

    Pair<TKey, TValue>[] m_keys;
    BTreeElement<TKey, TValue>[] m_neighbors;

    public virtual BTreeElement<TKey, TValue> New()
    {
        return new BTreeElement<TKey, TValue>();
    }

    private void Init()
    {
        m_keys = new Pair<TKey, TValue>[m_max_elements + 1];
        m_neighbors = new BTreeElement<TKey, TValue>[m_max_elements + 2];
    }

    public virtual void Init(int _max_elements)
    {
        m_max_elements = _max_elements;

        Init();
    }

    public virtual void Init(int _max_elements, BinaryElement<TKey, TValue> _b_element)
    {
        m_max_elements = _max_elements;

        Init();

        InsertElement(0, _b_element);
    }

    public BTreeElement()
    {
    }

    public BTreeElement(int _max_elements)
    {
        Init(_max_elements);
    }

    public BTreeElement(int _max_elements, BinaryElement<TKey, TValue> _b_element)
    {
        Init(_max_elements, _b_element);
    }

    public int MaxElements()
    {
        return m_max_elements;
    }

    public Pair<TKey, TValue> Find(TKey _key)
    {
        int lo = 0;
        int find = SearchInArray(_key, ref lo);

        if (find >= 0)
            return m_keys[find];
        else if (m_neighbors[lo] != null)
            return GetNeighbor(lo).Find(_key);
        else
            return default(Pair<TKey, TValue>);
    }

    protected Pair<TKey, TValue> GetKey(int _index)
    {
        return m_keys[_index];
    }

    protected void AddKey(Pair<TKey, TValue> _pair)
    {
        m_keys[m_elements_count++] = _pair;
    }

    protected void ClearKeys()
    {
        for (int i = 0; i < m_elements_count; ++i)
            m_keys[i] = null;

        m_elements_count = 0;
    }

    protected virtual BTreeElement<TKey, TValue> GetNeighbor(int _index)
    {
        return m_neighbors[_index];
    }

    protected void SetNeighbor(int _index, BTreeElement<TKey, TValue> _element)
    {
        m_neighbors[_index] = _element;
    }

    public BinaryElement<TKey, TValue> Insert(TKey _key, TValue _value)
    {
        int lo = 0;
        int find = SearchInArray(_key, ref lo);
        BinaryElement<TKey, TValue> insert = null;
        
        if (find >= 0)
            m_keys[find].Value = _value;
        else if (m_neighbors[lo] != null)
            insert = GetNeighbor(lo).Insert(_key, _value);
        else
            insert = new BinaryElement<TKey, TValue>(m_neighbors[lo], m_neighbors[lo + 1], new Pair<TKey, TValue>(_key, _value));

        if (insert != null)
            return InsertElement(lo, insert);

        return null;
    }
    
    private BinaryElement<TKey, TValue> InsertElement(int _index, BinaryElement<TKey, TValue> _b_element)
    {
        BinaryElement<TKey, TValue> result = null;

        InsertInArray(_index, _b_element);

        if(m_elements_count >= m_keys.Length)
        {
            int mid = m_elements_count / 2;
            Pair<TKey, TValue> median = m_keys[mid];
            m_keys[mid] = null;

            BTreeElement<TKey, TValue> right = New();
            right.Init(m_max_elements);

            for (int i = mid + 1, ir = 0; i < m_elements_count; ++i, ++ir)
            {
                right.m_keys[ir] = m_keys[i];
                m_keys[i] = null;
            }

            for (int i = mid + 1, ir = 0, count = m_elements_count + 1; i < count; ++i, ++ir)
            {
                right.m_neighbors[ir] = m_neighbors[i];
                m_neighbors[i] = null;
            }

            right.m_elements_count = m_elements_count - mid - 1;
            m_elements_count = mid;

            result = new BinaryElement<TKey, TValue>(this, right, median);
        }

        return result;
    }

    private int SearchInArray(TKey _key, ref int _lo)
    {
        int lo = 0;
        int hi = m_elements_count;
        int res = -1;

        while (lo < hi)
        {
            int mid = (hi + lo) / 2;

            int cmp = _key.CompareTo(m_keys[mid].Key);
            if (cmp > 0)
                lo = mid + 1;
            else if (cmp < 0)
                hi = mid;
            else
            {
                res = mid;
                break;
            }
        }

        _lo = lo;
        return res;
    }

    private void InsertInArray(int _index, BinaryElement<TKey, TValue> _b_element)
    {
        if (m_elements_count < m_keys.Length)
            ++m_elements_count;

        for (int i = m_elements_count - 1; i > _index; --i)
            m_keys[i] = m_keys[i - 1];

        for (int i = m_elements_count; i > _index; --i)
            m_neighbors[i] = m_neighbors[i - 1];

        m_keys[_index] = _b_element.Value;
        m_neighbors[_index] = _b_element.Left;
        m_neighbors[_index+1] = _b_element.Right;
    }
}