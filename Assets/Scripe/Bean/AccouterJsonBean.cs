using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AccouterJsonBean
{
    public long id;
    public string name;
    public long type;
    public long stacking;
    public long tabid;
    public long sortID;
    public string icon;
    public string attribute_type;
    public string attribute_value;
    public long cost;
    public long quality;
    public string cost_up;
    public string strengthen;
    public string affix;

    private long affixAll = 0;

    public List<EquipKeyAndValue> mCostList;
    public long getCost(long level){
        if (mCostList == null) {
            mCostList = EquipKeyAndValue.getListForString(cost_up);
        }
        long cost = 0;
        long levelDo = 0;
        foreach (EquipKeyAndValue v in mCostList) {
            if (v.key > level)
            {
                return cost+(level - levelDo) *v.value;
            }
            else {
                cost = cost+ v.value*( v.key- levelDo);
                levelDo = v.key;
            }
        }
        return cost;
    }

    public List<EquipKeyAndValue> mAffix ;
    public long getAffix()
    {
        if (mAffix == null)
        {
            mAffix = EquipKeyAndValue.getListForString(affix);
            if (mAffix != null && mAffix.Count > 0)
            {
                foreach (EquipKeyAndValue k in mAffix)
                {
                    affixAll += k.value;
                }
            }
        }
        if (affixAll == 0)
        {
            return -1;
        }
        else {
            int rangeRadomNum = Random.Range(0, (int)affixAll);
            foreach (EquipKeyAndValue k in mAffix)
            {
                if (k.value > rangeRadomNum)
                {
                    return k.key;
                }
                else {
                    rangeRadomNum = rangeRadomNum - (int)k.value;
                }
            }
        }
        return -1;
    }
    Dictionary<long, List<EquipKeyAndValue>> mStrengThen;
    public Dictionary<long, List<EquipKeyAndValue>> getStrengThen() {
        if (mStrengThen == null) {
            mStrengThen = EquipKeyAndValue.getDicForString(getAttributeList(), strengthen);
        }
        return mStrengThen;
    }
    public long getStrengthenByLevel(long type,long level)
    {
        List<EquipKeyAndValue> list;
        Dictionary<long, List<EquipKeyAndValue>> dir= getStrengThen();
        if (dir.ContainsKey(type))
        {
            list = dir[type];
        }
        else {
            return 0;
        }
        
        if (list == null)
        {
            return 0;
        }
        foreach (EquipKeyAndValue ek in list) {
            Debug.Log(" getStrengthenByLevel key=" + ek.key + " value=" + ek.value);
        }

        long streng = 0;
        long levelDo = 0;
        foreach (EquipKeyAndValue v in list)
        {
            if (v.key > level)
            {
                return streng + (level - levelDo) * v.value;
            }
            else
            {
                streng = streng + v.value * (v.key - levelDo);
                levelDo = v.key;
            }
        }
        return streng;
    }
    public List<EquipKeyAndValue> mAttribute = new List<EquipKeyAndValue>();
    public  List<EquipKeyAndValue> getAttributeList() {
        if (mAttribute.Count > 0) {
            return mAttribute;
        }
        if (attribute_value == null || attribute_value.Length < 1)
        {
            return null;
        }
        string[] array = attribute_type.Split(',');
        string[] array2 = attribute_value.Split(',');
        if (array.Length == array2.Length) {
            for (int i = 0; i < array.Length; i++) {
                if (array[i] == null || array[i].Length < 1 || array2[i] == null || array2[i].Length < 1)
                {
                    continue;
                }
                EquipKeyAndValue bean = new EquipKeyAndValue();
                bean.key = long.Parse(array[i]);
                bean.value = long.Parse(array2[i]);
                mAttribute.Add(bean);
            }
        }
        if (mAttribute.Count == 0)
        {
            return null;
        }
        return mAttribute;
    }
  /*  public List<AttributeBean> getAttributeList()
    {
        if (mAttribute.Count == 0)
        {
            if (attribute_value == null || attribute_value.Length < 1)
            {
                return null;
            }

            string[] array = attribute_value.Split('}');
            foreach (string str2 in array)
            {
                if (str2 == null || str2.Length < 1)
                {
                    continue;
                }
                string str3 = str2.Replace("{", "");
                if (str3 == null || str3.Length < 1)
                {
                    continue;
                }
                string[] array2 = str3.Split(',');
                AttributeBean bean = new AttributeBean();
                bean.min = long.Parse(array2[0]);
                bean.max = long.Parse(array2[1]);
                mAttribute.Add(bean);
            }
            attribute_value = null;
            if (mAttribute.Count == 0)
            {
                return null;
            }
            string[] type = attribute_type.Split(',');
            for (int i = 0; i < type.Length; i++) {
                mAttribute[i].type = long.Parse(type[i]);
            }
        }
        return mAttribute;
    }*/

}
