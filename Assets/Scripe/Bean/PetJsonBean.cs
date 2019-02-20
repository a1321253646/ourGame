using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PetJsonBean
{
    public long id;
    public string name;
    public string skil_name;
    public long resouce;
    public string noactivateIcon;
    public string activateIcon;
    public string moactivateImage;
    public string des;
    public string affixId;
    public string value;
    public long skillId;
    public string skillActivateIcon;
    public string skillNoactivateIcon;
    public long activateLevel;
    public string activateText;
    public string talkText;

    public List<PlayerAttributeBean> mAffix;

    public List<PlayerAttributeBean> getAffixList() {

        if (mAffix == null && affixId != null) {
            string[] affixIdList = affixId.Split(',');
            string[] affixValueList = value.Split(',');
            if (affixIdList != null && affixValueList != null && affixIdList.Length != 0 && affixIdList.Length == affixValueList.Length) {
                mAffix = new List<PlayerAttributeBean>();
                for (int i = 0; i < affixIdList.Length; i++) {
                    PlayerAttributeBean value = new PlayerAttributeBean();
                    value.type = int.Parse(affixIdList[i]);
                    value.value = double.Parse(affixValueList[i]);
                    mAffix.Add(value);
                }
            }
        }
        return mAffix;
    }

}
