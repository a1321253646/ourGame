using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AccouterJsonBean2
{
    public long id;
    public string name;
    public long type;
    public long quality;
    public long stacking;
    public long tabid;
    public long sortID;
    public string icon;
    public long attribute_type;
    public long attribute_value;
    public long coat;
    public string coat_up;
    public string strengthen;
    public string affix;


    public AttributeBean mAttribute = null;
    public AttributeBean getAttributeList()
    {
        if (mAttribute == null)
        {
            mAttribute = new AttributeBean();
            mAttribute.type = attribute_type;
            mAttribute.value = attribute_value;
        }
        return mAttribute;
    }

    public List<EquipKeyAndValue> mAffixList = new List<EquipKeyAndValue>();
    public long allAffixCount = 0;
    public AffixJsonBean getAffix() {
        if (mAffixList == null) {
            mAffixList = EquipKeyAndValue.getListForString(affix);
            foreach (EquipKeyAndValue e in mAffixList) {
                allAffixCount += e.value;
            }
        }
        long rangeRadomNum = Random.Range((int)0, (int)allAffixCount);
        EquipKeyAndValue affixValue = null;
        foreach (EquipKeyAndValue e in mAffixList)
        {
            affixValue = e;
            if (rangeRadomNum > e.value)
            {                
                rangeRadomNum = rangeRadomNum - e.value;
            }
            else {
                break;
            }
        }

        return JsonUtils.getIntance().getAffixInfoById(affixValue .key);
    }


    public List<EquipKeyAndValue> mEquipCostUpList = null;
    public long getEquipCostUp(long level) {
        if (mEquipCostUpList == null) {
            mEquipCostUpList = EquipKeyAndValue.getListForString(coat_up);
        }
        return getValueByLevel(mEquipCostUpList,level) +coat;
    }
    public List<EquipKeyAndValue> mStrengThenList = null;
    public long getStrengThenValue(long level)
    {
        if (mStrengThenList == null)
        {
            mStrengThenList = EquipKeyAndValue.getListForString(strengthen);
        }
        return getValueByLevel(mStrengThenList, level) + attribute_value;
    }
    private long getValueByLevel(List<EquipKeyAndValue> list,long level) {
        long value = 0;
        foreach (EquipKeyAndValue v in list)
        {
            if (level < v.key)
            {
                value = value + (level - v.key) * v.value;
                break;
            }
            else
            {
                value = value + v.key * v.value;
            }
        }
        EquipKeyAndValue v2 = list[list.Count - 1];
        if (level > v2.key)
        {
            value = value + (level - v2.key) * v2.value;
        }
        return value;
    }
}
