using UnityEngine;

public struct BoolCounter
{
    private int m_counter;

    public BoolCounter(bool _value)
    {
        m_counter = _value ? 1 : 0;
    }
    
    public void Reset(bool _value)
    {
        m_counter = _value ? 1 : 0;
    }

    public void Set(bool _value)
    {
        m_counter += _value ? 1 : -1;
    }

    public bool Get()
    {
        return (m_counter > 0);
    }

    public static explicit operator bool(BoolCounter _value)
    {
        return _value.Get();
    }
    
    public static implicit operator BoolCounter(bool _value)
    {
        return new BoolCounter(_value);
    }
}

public struct BoolCounterPos
{
    private int m_counter;

    public BoolCounterPos(bool _value)
    {
        m_counter = _value ? 1 : 0;
    }

    public void Reset(bool _value)
    {
        m_counter = _value ? 1 : 0;
    }

    public void Set(bool _value)
    {
        m_counter += _value ? 1 : -1;

        if(m_counter < 0)
        {
            m_counter = 0;
            Debug.LogError("BoolCounterPos counter is negative");
        }
    }

    public bool Get()
    {
        return (m_counter > 0);
    }
    
    public static explicit operator bool(BoolCounterPos _value)
    {
        return _value.Get();
    }

    public static implicit operator BoolCounterPos(bool _value)
    {
        return new BoolCounterPos(_value);
    }
}

public struct BoolCounterNeg
{
    private int m_counter;

    public BoolCounterNeg(bool _value)
    {
        m_counter = _value ? 1 : 0;
    }

    public void Reset(bool _value)
    {
        m_counter = _value ? 1 : 0;
    }

    public void Set(bool _value)
    {
        m_counter += _value ? 1 : -1;

        if (m_counter > 1)
        {
            m_counter = 0;
            Debug.LogError("BoolCounterPos counter is greater than 1");
        }
    }

    public bool Get()
    {
        return (m_counter > 0);
    }
    
    public static explicit operator bool(BoolCounterNeg _value)
    {
        return _value.Get();
    }

    public static implicit operator BoolCounterNeg(bool _value)
    {
        return new BoolCounterNeg(_value);
    }
}