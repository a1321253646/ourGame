using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public abstract class GuideActionItem 
{

    public  string mDec;
    public EquipKeyAndValue mTarget;
    public GuideManager mManager;
    public List<EquipKeyAndValue> mList;
    public GuideControl mControl;
    public bool isStart = false;
    public void init(string str, EquipKeyAndValue target, GuideManager manager, List<EquipKeyAndValue> list, GuideControl control) {
        mDec = str;
        mTarget = target;
        mManager = manager;
        mList = list;
        mControl = control;
    }
    //返回值-1表示该事件不符合引导，返回值0表示该事件是引导，并由control来显示引导，大于0表示需要事件发生源进行显示引导,-2表示引导结束,-3表示引导结束并且直接开始下一个引导, <=1000表示重新引导，具体返回值用于表示重新开始的步骤
    public abstract long notificationDeal(int eventID, long eventValue);
    public abstract void start();

    public int mTargetX;
    public int mTargetY;
    public int mDecX;
    public int mDecY;


    public void setUiShow() {
        if (mControl.mData.id == 1)
        {
            GameManager.getIntance().uiManager.seHeroUpShow(true);
        }
        else if (mControl.mData.id == 2)
        {
            GameManager.getIntance().uiManager.setAutoStartBossShow(true);
        }
        else if (mControl.mData.id == 3)
        {
            GameManager.getIntance().uiManager.setBackpackShow(true);
        }
        else if (mControl.mData.id == 5)
        {
            GameManager.getIntance().uiManager.setCardShow(true);
            GameManager.getIntance().uiManager.setLunhuiShow(true);
        }
    }

    public void setDecLocal(int targetX, int targetY, int decX, int decY) {
        mTargetX = targetX;
        mTargetY = targetY;
        mDecX = decX;
        mDecY = decY;
    }
}