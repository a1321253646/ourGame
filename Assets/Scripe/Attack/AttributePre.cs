using System.Collections.Generic;

public class AttributePre
{

    public static long aggressivity = 1;
    public static long defense = 2;
    public static long maxBloodVolume = 3;
    public static long rate = 4;
    public static long evd = 5;
    public static long crt = 6;
    public static long hurt = 7;
    public static long crtHurt = 8;
    public static long readHurt = 9;
    public static long attackSpeed = 10;

    private Dictionary<long, Dictionary<long, long>> mAttributePre = new Dictionary<long, Dictionary<long, long>>();

    Attribute mAll = new Attribute().setToPre();

    public void add(long id,long type,long value) {
        if (mAttributePre.ContainsKey(id))
        {
            Dictionary<long, long> dic = mAttributePre[id];
            if (dic.ContainsKey(type))
            {
                long old = dic[type];
                dic[type] = dic[type] + value;
                dealAll(type, old, dic[type]);
            }
            else {
                dic.Add(type, value + 10000);
                dealAll(type, 10000, 10000 + value);
            }
        }
        else {
            Dictionary<long, long> dic = new Dictionary<long, long>();
            dic.Add(type, value + 10000);
            mAttributePre.Add(id, dic);
            dealAll(type, 10000, 10000 + value);
        }
    }
    public void minus(long id, long type, long value)
    {
        if (mAttributePre.ContainsKey(id))
        {
            Dictionary<long, long> dic = mAttributePre[id];
            if (dic.ContainsKey(type))
            {
                long old = dic[type];
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
            Dictionary<long, long> dic = new Dictionary<long, long>();
            dic.Add(type,  10000 - value);
            mAttributePre.Add(id, dic);
            dealAll(type,10000, 10000 - value);
        }
    }

    public void delete(long id) {
        Dictionary<long, long> dic = mAttributePre[id];
        foreach (long type in dic.Keys) {
            deleteAll(type, dic[type]);
        }
        mAttributePre.Remove(id);
    }

    private void deleteAll(long type, long value) {
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
        else if (type == rate)
        {
            mAll.rate = mAll.rate - value;
        }
        else if (type == evd)
        {
            mAll.evd = mAll.evd  - value;
        }
        else if (type == crt)
        {
            mAll.crt = mAll.crt- value;
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

    private void dealAll(long type, long oldValue,long newValue) {
        if (type == aggressivity) {
            mAll.aggressivity = mAll.aggressivity / oldValue * newValue;
        }
        else if (type == defense) {
            mAll.defense = mAll.defense / oldValue * newValue;
        }
        else if (type == maxBloodVolume)
        {
            mAll.maxBloodVolume = mAll.maxBloodVolume / oldValue * newValue;
        }
        else if (type == rate)
        {
            mAll.rate = mAll.rate - oldValue + newValue;
        }
        else if (type == evd)
        {
            mAll.evd = mAll.evd - oldValue + newValue;
        }
        else if (type == crt)
        {
            mAll.crt = mAll.crt - oldValue + newValue;
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

    }


    public Attribute getAll() {
        return mAll;
    }
}
