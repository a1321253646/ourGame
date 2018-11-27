using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public abstract class GuideActionItem 
{

    public  string mDec;
    public EquipKeyAndValue mTarget;
    public GuideManager mManager;
    public List<EquipKeyAndValue> mList;

    public void init(string str, EquipKeyAndValue target, GuideManager manager, List<EquipKeyAndValue> list ) {
        mDec = str;
        mTarget = target;
        mManager = manager;
        mList = list;
    }
    //返回值-1表示该事件不符合引导，返回值0表示该事件是引导，并由control来显示引导，大于0表示需要事件发生源进行显示引导,-2表示引导结束,-3表示引导结束并且直接开始下一个引导, <=1000表示重新引导，具体返回值用于表示重新开始的步骤
    public abstract long notificationDeal(int eventID, long eventValue);
    public abstract void start();

    public int mTargetX;
    public int mTargetY;
    public int mDecX;
    public int mDecY;


    public void setDecLocal(int targetX, int targetY, int decX, int decY) {
        mTargetX = targetX;
        mTargetY = targetY;
        mDecX = decX;
        mDecY = decY;
    }
}