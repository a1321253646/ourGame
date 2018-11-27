using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuideJsonBean
{
    public long id;
    public long guideId;
    public string qualification;
    public string guideTarget;
    public string str;

    private List<EquipKeyAndValue> mQualificationList = new List<EquipKeyAndValue>();
    private List<string> mStr = new List<string>();
    private EquipKeyAndValue mTargete ;

    public List<string> getDecString() {

        if (mStr.Count == 0 && str != null) {
            string[] array2 = str.Split('|');
            foreach (string str2 in array2)
            {
                if (str2 == null || str2.Length < 1)
                {
                    continue;
                }
                mStr.Add(str2);
            }
        }
        if (mStr.Count == 0)
        {
            return null;
        }
        else
        {
            return mStr;
        }
    }

    public EquipKeyAndValue getTarget() {
        Debug.Log("getTarget = " + guideTarget);
        if (mTargete == null && guideTarget != null) {
            string[] array2 = guideTarget.Split(',');
            mTargete = new EquipKeyAndValue();
            mTargete.key = long.Parse(array2[0]);
            mTargete.value = long.Parse(array2[1]);
            guideTarget = null;
        }
        return mTargete;
    }

    public List<EquipKeyAndValue> getQualificationList() {
        if (mQualificationList.Count == 0 && qualification != null) {
            if (str == null || str.Length < 1)
            {
                return null;
            }
            string[] array = qualification.Split(';');
            foreach (string str2 in array)
            {
                if (str2 == null || str2.Length < 1)
                {
                    continue;
                }
                string[] array2 = str2.Split(',');
                EquipKeyAndValue bean = new EquipKeyAndValue();
                bean.key = long.Parse(array2[0]);
                bean.value = long.Parse(array2[1]);
                mQualificationList.Add(bean);
            }
            qualification = null;
        }
        if (mQualificationList.Count == 0)
        {
            return null;
        }
        else
        {
            return mQualificationList;
        }
    }


}