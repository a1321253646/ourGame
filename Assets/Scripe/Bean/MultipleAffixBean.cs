using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultipleAffixBean
{
    private double mValue = 1;

    private Dictionary<long, double> mFloatValueList = new Dictionary<long, double>();

    public void AddFloat(long id, double value) {
        if (mFloatValueList.ContainsKey(id))
        {
            double old = mFloatValueList[id];
            mFloatValueList[id] = value;
            mValue = mValue / old * value;
        }
        else
        {
            mFloatValueList.Add(id, value);
            mValue = mValue * value;
        }
    }

    public void mimus(long id, double value) {
        if (mFloatValueList.ContainsKey(id))
        {
            mFloatValueList.Remove(id);
            mValue = mValue / value;
        }
        else
        {
            mValue = mValue / value;
        }
    }
    public void deletById(long id) {
        if (mFloatValueList.ContainsKey(id))
        {
            double value = mFloatValueList[id];
            mValue = mValue / value;
            mFloatValueList.Remove(id);
        }
    }
    public void clear() {
        mValue = 1;
        mFloatValueList.Clear();
    }
    public double getValue() {
        return mValue;
    }
}
