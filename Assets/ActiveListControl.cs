using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveListControl : MonoBehaviour {

    ActiveButtonControl[] mButtonList;

    // Use this for initialization
    void Start () {
        mButtonList = GetComponentsInChildren<ActiveButtonControl>();
    }

    public void showAd(long adId, string count, bool isAddSql)
    {
        showAd(adId, count, isAddSql, false);

    }

    public void showAd(long adId, string count, bool isAddSql,bool isNewTime) {
        int i = 0;
        while (i < mButtonList.Length && !mButtonList[i].init(ActiveButtonControl.ACTIVE_BUTTON_TYPE_AD, adId, count, isAddSql))
        {
            i++;
        }
                
    }
    public void showVocation(bool isAddSql) {
     //   Debug.Log("=================================showVocation");
        int i = 0;
        while (i < mButtonList.Length && !mButtonList[i].init(ActiveButtonControl.ACTIVE_BUTTON_TYPE_VOCATION, 0, null,isAddSql)) {
            i++;
        }
    }
    public void removeAd()
    {
        int i = 0;
        while (i < mButtonList.Length && !mButtonList[i].removeShow(ActiveButtonControl.ACTIVE_BUTTON_TYPE_AD))
        {
            i++;
        }
        if (i < mButtonList.Length)
        {
            mButtonList[i].removeShowTime();
            updateIcon(i);
        }
    }
    public void removeVocation()
    {
        int i = 0;
        while (i < mButtonList.Length && !mButtonList[i].removeShow(ActiveButtonControl.ACTIVE_BUTTON_TYPE_VOCATION))
        {
            i++;
        }
        if (i < mButtonList.Length) {
            updateIcon(i);
        }
    }

    private void updateIcon(int index) {
        int i = index +1;
        if (i < mButtonList.Length) {
            ActiveButtonBean bean = mButtonList[i].mBean;
            mButtonList[index].init(bean.buttonType, bean.adType, bean.count, false);
            mButtonList[i].removeShow(bean.buttonType,false);
            updateIcon(i);
        }
    }

    public GameObject getVocation() {
        int i = 0;
        while (i < mButtonList.Length)
        {
            if (mButtonList[i].mBean.buttonType == ActiveButtonControl.ACTIVE_BUTTON_TYPE_VOCATION) {
                return mButtonList[i].gameObject;
            }
            i++;
        }
        return null;
    }

}
