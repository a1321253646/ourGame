using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VocationDecBean 
{
    public long id;
    public string name;
    public string dec;
    public string bust;
    public long skill;
    public string skillName;
    public string skillIcon;
    public string attribute;
    public string next;
    public string attributeName;
    public string tip_dec;
    public long resource;
    public float attack_range;

    private List<long> mNexts;
    private List<long> mAttribute;

    public List<long> getNexts() {
        if (mNexts == null) {
            mNexts = getLongs(next);
        }
        if (mNexts.Count == 0)
        {
            return null;
        }
        else {
            return mNexts;
        }
    }
    public List<long> getAttribute()
    {
        if (mAttribute == null)
        {
            mAttribute = getLongs(attribute);
        }
        if (mAttribute== null || mAttribute.Count == 0)
        {
            return null;
        }
        else
        {
            return mAttribute;
        }
    }

    private List<long> getLongs(string str) {
        List<long> list = new List<long>();
        if (str == null || str.Length == 0) {
            return list;
        }
        string[] strs = str.Split(',');
        foreach (string s in strs) {
            list.Add(long.Parse(s));
        }
        return list;
    }
}
