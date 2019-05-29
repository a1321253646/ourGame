using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AttributePre
{

    public static long aggressivity = 1;
    public static long defense = 2;
    public static long maxBloodVolume = 3;
    public static long hurt = 7;
    public static long crtHurt = 8;
    public static long readHurt = 9;
    public static long attackSpeed = 10;

    private AttributePre() {

    }
    public AttributePre(Attacker attacker) {
        mAttacker = attacker;
    }
    public Attacker mAttacker;

    private Dictionary<long, Dictionary<long, double>> mAttributePre = new Dictionary<long, Dictionary<long, double>>();

    Attribute mAll = new Attribute().setToPre();

    

    public void add(long id,long type,double value) {
//        Debug.Log("===============add id = " + id+ " type = "+type+" value = "+value);
        if (mAttributePre.ContainsKey(id))
        {
            Dictionary<long, double> dic = mAttributePre[id];
            if (dic.ContainsKey(type))
            {
                double old = dic[type];
                dic[type] = dic[type] + value;
                dealAll(type, old, dic[type]);
            }
            else {
                dic.Add(type, value + 10000);
                dealAll(type, 10000, 10000 + value);
            }
        }
        else {
            Dictionary<long, double> dic = new Dictionary<long, double>();
            dic.Add(type, value + 10000);
            mAttributePre.Add(id, dic);
            dealAll(type, 10000, 10000 + value);
        }
    }
    public void updateDebuff(long id, long type, double value) {
        if (mAttributePre.ContainsKey(id))
        {
            Dictionary<long, double> dic = mAttributePre[id];
            if (dic.ContainsKey(type))
            {
                dic.Remove(type);
            }
            else
            {
                dic.Add(type, 10000 - value);
                dealAll(type, 10000, 10000 - value);
            }
        }
        else
        {
            Dictionary<long, double> dic = new Dictionary<long, double>();
            dic.Add(type, 10000 - value);
            mAttributePre.Add(id, dic);
            dealAll(type, 10000, 10000 - value);
        }
    }

    public void minus(long id, long type, double value)
    {
        if (mAttributePre.ContainsKey(id))
        {
            Dictionary<long, double> dic = mAttributePre[id];
            if (dic.ContainsKey(type))
            {

                double old = dic[type];
                dic[type] = dic[type] - value;
                dealAll(type, old, dic[type]);
            }
            else
            {
                dic.Add(type, 10000- value);
                dealAll(type, 10000, 10000 - value);
            }
        }
        else
        {
            Dictionary<long, double> dic = new Dictionary<long, double>();
            dic.Add(type,  10000 - value);
            mAttributePre.Add(id, dic);
            dealAll(type,10000, 10000 - value);
        }
    }

    public void delete(long id) {
//        Debug.Log(" id ==" + id);
        if (!mAttributePre.ContainsKey(id)) {
            return;
        }
        Dictionary<long, double> dic = mAttributePre[id];
        foreach (long type in dic.Keys) {
            deleteAll(type, dic[type]/10000f);

        }
        mAttributePre.Remove(id);
        mAttacker.getAttribute(true);
    }

    private void deleteAll(long type, double value) {
        Debug.Log(" type =="+type+ " value" + value);
        if (type == aggressivity)
        {
            mAll.aggressivity = mAll.aggressivity / value;
        }
        else if (type == defense)
        {
            mAll.defense = mAll.defense / value;
        }
        else if (type == maxBloodVolume)
        {
            mAll.maxBloodVolume = mAll.maxBloodVolume / value;
        }
        else if (type == hurt)
        {
            mAll.hurt = mAll.hurt / value;
        }
        else if (type == crtHurt)
        {
            mAll.crtHurt = mAll.crtHurt / value;
        }
        else if (type == readHurt)
        {
            mAll.readHurt = mAll.readHurt / value;
        }
        else if (type == attackSpeed)
        {
            mAll.attackSpeed = mAll.attackSpeed / value;
        }
       
    }

    private void dealAll(long type, double oldValue, double newValue) {
//        Debug.Log("===============type = " + type + " oldValue = " + oldValue + " newValue = " + newValue);
        if (type == aggressivity) {
            mAll.aggressivity = mAll.aggressivity / oldValue * newValue;
//            Debug.Log("mAll.aggressivity =" + mAll.aggressivity);
        }
        else if (type == defense) {
            mAll.defense = mAll.defense / oldValue * newValue;
        }
        else if (type == maxBloodVolume)
        {
            mAll.maxBloodVolume = mAll.maxBloodVolume / oldValue * newValue;
        }
        else if (type == hurt)
        {
            mAll.hurt = mAll.hurt / oldValue * newValue;
        }
        else if (type == crtHurt)
        {
            mAll.crtHurt = mAll.crtHurt / oldValue * newValue;
        }
        else if (type == readHurt)
        {
            mAll.readHurt = mAll.readHurt / oldValue * newValue;
        }
        else if (type == attackSpeed)
        {
            mAll.attackSpeed = mAll.attackSpeed / oldValue * newValue;
        }
        mAttacker.getAttribute(true);
    }


    public Attribute getAll() {
        return mAll;
    }

    public void clear() {
        mAll.setToPre();
        mAttributePre.Clear();
    }
}
