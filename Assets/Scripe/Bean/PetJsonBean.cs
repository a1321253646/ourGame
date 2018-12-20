﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PetJsonBean
{
    public long id;
    public string name;
    public long resouce;
    public string noactivateIcon;
    public string activateIcon;
    public string moactivateImage;
    public string dec;
    public string afficId;
    public string vlue;
    public long skillId;
    public string skillActivateIcon;
    public string skillNoactivateIcon;
    public long activateLevel;
    public string activateText;
    public string talkText;

    public List<EquipKeyAndValue> mAffix;

    public List<EquipKeyAndValue> getAffixList() {

        if (mAffix == null && afficId != null) {
            string[] affixIdList = afficId.Split(',');
            string[] affixValueList = vlue.Split(',');
            if (affixIdList != null && affixValueList != null && affixIdList.Length != 0 && affixIdList.Length == affixValueList.Length) {
                mAffix = new List<EquipKeyAndValue>();
                for (int i = 0; i < affixIdList.Length; i++) {
                    EquipKeyAndValue value = new EquipKeyAndValue();
                    value.key = int.Parse(affixIdList[i]);
                    value.value = int.Parse(affixValueList[i]);
                    mAffix.Add(value);
                }
            }
        }
        return mAffix;
    }

}
