using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AffixJsonBean 
{
    public long id;
    public long value;
    public string dec;
    public string name;
    public string valueArea;

    public List<EquipKeyAndValue> mValueAreaList = null;
    public long getAffixValue(long type)
    {
        if (mValueAreaList == null)
        {
            mValueAreaList = EquipKeyAndValue.getListForString(valueArea);
        }
        EquipKeyAndValue e = mValueAreaList[(int)type - 1];
        int rangeRadomNum = Random.Range((int)e.key, (int)e.value);
        return rangeRadomNum;
    }
}
