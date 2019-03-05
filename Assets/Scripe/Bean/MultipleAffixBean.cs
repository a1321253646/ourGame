using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultipleAffixBean
{
    private float mValue = 1;

    private Dictionary<long, float> mFloatValueList = new Dictionary<long, float>();

    public void AddFloat(long id, float value) {
        if (mFloatValueList.ContainsKey(id))
        {
            float old = mFloatValueList[id];
            mFloatValueList[id] = value;
            mValue = mValue / old * value;
        }
        else
        {
            mFloatValueList.Add(id, value);
            mValue = mValue * value;
        }
    }

    public void mimus(long id, float value) {
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
            float value = mFloatValueList[id];
            mValue = mValue / value;
            mFloatValueList.Remove(id);
        }
    }
    public void clear() {
        mValue = 1;
        mFloatValueList.Clear();
    }
    public float getValue() {
        return mValue;
    }
}
