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
    public string cost;
    public long quality;
    public string cost_up;
    public string strengthen;
    public string affix;
    public string affix_count;
    public string sale;
    public string sale_level;


    private long affixAll = 0;
    private long affixCountAll = 0;
    public BigNumber mCost;
    public BigNumber mSale;
    public BigNumber mSaleLevel;

    public BigNumber getSale()
    {
        if (mSale == null && sale!= null && sale.Length > 0)
        {
            mSale = BigNumber.getBigNumForString(sale);
            sale = null;
        }
        if (mSale == null) {
            mSale = new BigNumber();
        }
        return mSale;
    }
    public BigNumber getSaleLevel()
    {
        if (mSaleLevel == null && sale_level != null && sale_level.Length > 0)
        {
            mSaleLevel = BigNumber.getBigNumForString(sale_level);
            sale_level = null;
        }
        if (mSaleLevel == null)
        {
            mSaleLevel = new BigNumber();
        }
        return mSaleLevel;
    }


    public BigNumber getCost() {
        if (mCost == null && cost != null && cost.Length > 0) {
            mCost = BigNumber.getBigNumForString(cost);
            cost = null;
        }
        if (mCost == null) {
            mCost = new BigNumber();
        }
        return mCost;
    }

    public List<EquipKeyAndBig> mCostList;
    public BigNumber getCost(long level){
        if (mCostList == null) {
            mCostList = EquipKeyAndBig.getListForString(cost_up);
        }
        BigNumber cost = new BigNumber(); 
        long levelDo = 0;
        foreach (EquipKeyAndBig v in mCostList) {
            if (v.key > level)
            {
                BigNumber mul =  BigNumber.multiply(v.value, level - levelDo);

                return BigNumber.add(mul, cost);
            }
            else {
                BigNumber mul = BigNumber.multiply(v.value, v.key - levelDo);
                cost = BigNumber.add(mul, cost);
                levelDo = v.key;
            }
        }
        return cost;
    }


    public List<EquipKeyAndValue> mAffixCount;
    public long getAffixCount() {
        if (mAffixCount == null) {
            mAffixCount = EquipKeyAndValue.getListForString(affix_count);
            if (mAffixCount != null && mAffixCount.Count > 1)
            {
                foreach (EquipKeyAndValue k in mAffixCount)
                {
                    affixCountAll += k.value;
                }
            }
            else if (mAffixCount != null && mAffixCount.Count == 1)
            {
                affixCountAll = 10000;
            }
        }
        if (affixCountAll == 0)
        {
            return 0;
        }
        else {
            int rangeRadomNum = Random.Range(0, (int)affixCountAll);
            foreach (EquipKeyAndValue k in mAffixCount)
            {
                if (k.value > rangeRadomNum)
                {
                    return k.key;
                }
                else
                {
                    rangeRadomNum = rangeRadomNum - (int)k.value;
                }
            }
        }
        return 0;
    }
    public List<EquipKeyAndValue> mAffix;
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
            else if(mAffix != null && mAffix.Count > 1)
            {
                affixAll = 10000;
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
    Dictionary<long, List<EquipKeyAndDouble>> mStrengThen;
    public Dictionary<long, List<EquipKeyAndDouble>> getStrengThen() {
        if (mStrengThen == null) {
            mStrengThen = EquipKeyAndDouble.getDicForString(getAttributeList(), strengthen);
        }
        return mStrengThen;
    }
    public double getStrengthenByLevel(long type,long level)
    {
        List<EquipKeyAndDouble> list;
        Dictionary<long, List<EquipKeyAndDouble>> dir= getStrengThen();
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
        foreach (EquipKeyAndDouble ek in list) {
            Debug.Log(" getStrengthenByLevel key=" + ek.key + " value=" + ek.value);
        }

        double streng = 0;
        long levelDo = 0;
        foreach (EquipKeyAndDouble v in list)
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
    public List<EquipKeyAndDouble> mAttribute = new List<EquipKeyAndDouble>();
    public  List<EquipKeyAndDouble> getAttributeList() {
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
                EquipKeyAndDouble bean = new EquipKeyAndDouble();
                bean.key = long.Parse(array[i]);
                bean.value = double.Parse(array2[i]);
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
