using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GuideControl
{
    public GuideJsonBean mData;
    GuideManager mManager;
    GuideActionItem mCurrentAction;
    List<GuideActionItem> mActionList = new List<GuideActionItem>();
    int mCurrentIndex = 0;
    public void init(GuideJsonBean data, GuideManager manager) {
        mData = data;
        mManager = manager;
        initGuideAction();
    }


    private void initGuideAction() {


        if (mData.id == 1)
        {//升级引导
            GuideActionButton item1 = new GuideActionButton();
            item1.init(
                mData.getDecString()[0]
                , null,
                mManager,
                null, this);
            item1.init(mData.getQualificationList()[0].value, GuideManager.EVENT_ENEMY_DEAL, GuideManager.BUTTON_START_HERO_UP, "Button_lvup");
            item1.setDecLocal(2, 1, 2, 3);
            mActionList.Add(item1);
            GameManager.getIntance().uiManager.seHeroUpShow(false);
        }
        else if (mData.id == 2)
        {//boss引导
            GuideActionButton item1 = new GuideActionButton();
            item1.init(
                mData.getDecString()[0]
                , null,
                mManager,
                null, this);
            item1.init(mData.getQualificationList()[0].value, GuideManager.EVENT_ENEMY_DEAL, GuideManager.BUTTON_START_BOSS, "Button_boss");
            item1.setDecLocal(3, 3, 3, 1);
            mActionList.Add(item1);
            GameManager.getIntance().uiManager.setAutoStartBossShow(false);
        }
        else if (mData.id == 3)
        {//准备穿戴
            GuideActionButton item1 = new GuideActionButton();
            item1.init(
                mData.getDecString()[0]
                , null,
                mManager,
                null,this);
            item1.init(mData.getQualificationList()[0].value, GuideManager.EVENT_OBJECT_DROP, GuideManager.BUTTON_START_OPEN_BACK, "pack_ui");
            item1.setDecLocal(3, 2, 1, 2);
            mActionList.Add(item1);
            GuideActionBackpackItem item2 = new GuideActionBackpackItem();
            item2.init(
                mData.getDecString()[1]
                , mData.getTarget(),
                mManager,
                null, this);
            mActionList.Add(item2);
            item2.setDecLocal(2, 3, 2, 1);
            ShowGuideBySelf item3 = new ShowGuideBySelf();
            item3.init(
                mData.getDecString()[2]
                , null,
                mManager,
                null, this);
            item3.init(GuideManager.SHOW_TIP, GuideManager.EVENT_SHOW , GuideManager.BUTTON_CLICK_TIP_SURE, "tip_button_list1_2");
            item3.setDecLocal(1, 1, 3, 3);
            mActionList.Add(item3);
            GameManager.getIntance().uiManager.setBackpackShow(false);
        }
        else if (mData.id == 4)//升级装备引导
        {
            GuideActionEquiteUp item1 = new GuideActionEquiteUp();
            item1.init(
                mData.getDecString()[0]
                , mData.getTarget(),
                mManager,
                mData.getQualificationList(), this);
            item1.setDecLocal(3, 3, 1, 3);
            mActionList.Add(item1);
        }
        else if (mData.id == 5) {//准备卡牌引导
            GuideActionButton item1 = new GuideActionButton();
            item1.init(
                mData.getDecString()[0]
                , null,
                mManager,
                null, this);
            item1.init(mData.getQualificationList()[0].value, GuideManager.EVENT_CLICK_BUTTON, GuideManager.BUTTON_CLICK_OPEN_CARD, "skilcard_ui");
            item1.setDecLocal(3, 2, 1, 2);
            mActionList.Add(item1);
            GuideActionCardUiBackItem item2 = new GuideActionCardUiBackItem();
            item2.init(
                mData.getDecString()[1]
                , mData.getTarget(),
                mManager,
                null, this);
            item2.setDecLocal(2, 3, 2, 1);
            mActionList.Add(item2);

            GuideActionUserCard item4 = new GuideActionUserCard();
            item4.init(
                mData.getDecString()[2]
                ,null,
                mManager,
                null, this);
            item4.setDecLocal(3, 3, 3, 1);
            mActionList.Add(item4);
            GameManager.getIntance().uiManager.setCardShow(false);
            GameManager.getIntance().uiManager.setLunhuiShow(false);
        }
        else if (mData.id == 6)//转职引导
        {
            GuideActionButton item1 = new GuideActionButton();
            item1.init(
                mData.getDecString()[0]
                , null,
                mManager,
                 null, this);
            item1.init(5, GuideManager.EVENT_SHOW_VOCATION, GuideManager.BUTTON_CLICK_OPEN_VOCATION, "find:vocation");
            item1.setDecLocal(1, 3, 3, 1);
            mActionList.Add(item1);

 /*           GuideActionButton item2 = new GuideActionButton();
            item2.init(
                mData.getDecString()[1]
                , null,
                mManager,
                 null, this);
            item2.init(GuideManager.SHOW_CARD_VOCATION, GuideManager.EVENT_SHOW, GuideManager.BUTTON_CLICK_CLICK_VOCATION, "vocation");
            item2.setDecLocal(1, 2, 3, 2);
            mActionList.Add(item2);
            */

        }
    }

    //返回值-1表示该事件不符合引导，返回值0表示该事件是引导，并由control来显示引导，大于0表示需要事件发生源进行显示引导,-2表示引导结束,-3表示引导结束并且直接开始下一个引导,-4表示重新引导
    public long notificationDeal(int eventID, long eventValue)
    {
        if (mCurrentAction != null)
        {
            long back =  mCurrentAction.notificationDeal(eventID, eventValue);
//            Debug.Log("notificationDeal back = " + back);
            if (back <= -1000)
            {
                back = -back-1000;
                mCurrentIndex = (int)back;
                mCurrentAction = mActionList[mCurrentIndex];
                mCurrentAction.start();
                return 0;
            }
            else if (back == -2)
            {
                mCurrentIndex++;
                if (mCurrentIndex >= mActionList.Count)
                {
                    return -2;
                }
                else
                {
                    mCurrentAction = mActionList[mCurrentIndex];
                    return 0;
                }
            } else if (back == -3) {
                mCurrentIndex++;
                if (mCurrentIndex >= mActionList.Count)
                {
                    return -2;
                }
                else
                {
                    mCurrentAction = mActionList[mCurrentIndex];
                    mCurrentAction.start();
                    return 0;
                }
            }
            else if (back == 0) {
                return 0;
            }
            return back;
        }
        else {
            mCurrentIndex = 0;
            mCurrentAction = mActionList[0];
            long back = mCurrentAction.notificationDeal(eventID, eventValue);
//            Debug.Log("notificationDeal back = " + back);
            return mCurrentAction.notificationDeal(eventID, eventValue);
        }
    }
}