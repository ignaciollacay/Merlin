using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reutilizable and independent counter class trying to follow S.O.L.I.D. pattern
/// Trying to decouple Count from SpellSO since feature may or may not be used.
/// Also to decouple logic from SpellBook (and eventually all scripts that use counter logic.
/// </summary>
public class Counter : MonoBehaviour
{
    private int count = 0;

    public int Count { get { return count; } }

    public bool BiggerThan(int value)
    {
        if (count > value)
            return true;
        else
            return false;
    }
    public bool SmallerThan(int value)
    {
        if (count < value)
            return true;
        else
            return false;
    }
    public bool BiggerOrEqualThan(int value)
    {
        if (count >= value)
            return true;
        else
            return false;
    }
    public bool SmallerOrEqualThan(int value)
    {
        if (count <= value)
            return true;
        else
            return false;
    }

    public int Increase()
    {
        return ++count;
    }
    public int Increase(int amount)
    {
        return count += amount;
    }
    public int Decrease()
    {
        return --count;
    }
    public int Decrease(int amount)
    {
        return count -= amount;
    }
    public int Reset()
    {
        return count = 0;
    }

    public int Next(int maxValue)
    {
        if (SmallerThan(maxValue - 1)) // Include 0 as value in count (Array & List index start at 0)
            return Increase();
        else
            return Reset();
    }
    public int Prev(int maxValue)
    {
        if (BiggerThan(0))
            return Decrease();
        else
            return count = maxValue - 1;  // Include 0 as value in count (Array & List index start at 0)
    }
}
