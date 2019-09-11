using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UiControlManager 
{
    public static long TYPE_TIP = 1;
    public static long TYPE_SETTING = 2;
    public static long TYPE_SAMSARA = 3;
    public static long TYPE_RANKING = 4;
    public static long TYPE_OUTGET = 5;
    public static long TYPE_LUIHUI = 6;
    public static long TYPE_IVERTORY = 7;
    public static long TYPE_CARD = 8;
    public static long TYPE_VOCATION = 9;
    public static long TYPE_PET = 10;
    public static long TYPE_HERO = 11;
    public static long TYPE_AD = 12;
    public static long TYPE_DEC_TIP= 13;
    public static long TYPE_SHOP = 14;
    public static long TYPE_VIP = 15;

    public static long SHOW_TYPE_TIP = 1;
    public static long SHOW_TYPE_UI = 2;

    public List<UiControlBean> mShowList = new List<UiControlBean>();
    public Dictionary<long, UiControlBean> mAllUi = new Dictionary<long, UiControlBean>();

    public void show(long type) {//先不考虑英雄宠物界面

        Debug.Log("UiControlManager show type=" + type);
        foreach (int key in mAllUi.Keys) {
            Debug.Log("mAllUi have type=" + key);
        }


        removeAllTip();
        UiControlBean bean = mAllUi[type];
        if (bean.isShow)
        {
            if (mShowList[0] == bean)
            {
                bean.isShow = false;
                bean.mControl.remove();
                mShowList.Remove(bean);
                return;
            }
            else
            {
                mShowList.Remove(bean);
                int level = GameManager.getIntance().getUiLevel();
                bean.mControl.gameObject.transform.SetSiblingIndex(level);
                bean.mControl.showUiControl();
                mShowList.Insert(0, bean);
            }
        }
        else {
            int level = GameManager.getIntance().getUiLevel();
            bean.mControl.gameObject.transform.SetSiblingIndex(level);
            bean.isShow = true;
            bean.mControl.showUiControl();
            mShowList.Insert(0, bean);
        }
    }

    public void remove(long type) {
        UiControlBean bean;
        for (int i = 0; i < mShowList.Count;)
        {
            bean = mShowList[i];
            if (bean.mControl.mControlType == type)
            {
                bean.isShow = false;
                bean.mControl.remove();
                mShowList.Remove(bean);
                return;
            }
            else
            {
                i++;
            }
        }
    }

    private void removeAllTip() {
        UiControlBean bean;
        for (int i = 0; i < mShowList.Count;){
            bean = mShowList[i];
            if (bean.mType == SHOW_TYPE_TIP)
            {
                bean.isShow = false;
                bean.mControl.remove();
                mShowList.Remove(bean);
            }
            else {
                i++;
            }
        }
    }
    public void removeAll() {
        UiControlBean bean;
        while (mShowList.Count > 0) {
            bean = mShowList[0];
            bean.isShow = false;
            bean.mControl.remove();
            mShowList.Remove(bean);
        }
    }

    public void init()
    {
        mAllUi.Clear();
        mShowList.Clear();
        UiControlBase iver = GameObject.Find("Backpack").GetComponentInChildren<IvertoryControl>();
        addAllUi(iver, 2, TYPE_IVERTORY);
        iver = GameObject.Find("tip").GetComponentInChildren<TipControl>();
        addAllUi(iver, 1, TYPE_TIP);
        iver = GameObject.Find("lunhui_tips").GetComponentInChildren<LuiHuiTips>();
        addAllUi(iver, 1, TYPE_LUIHUI);
        iver = GameObject.Find("message_tip").GetComponentInChildren<OutLineGetMessage>();
        addAllUi(iver, 1, TYPE_OUTGET);
        iver = GameObject.Find("game_dec_tip").GetComponentInChildren<GameDecTip>();
        addAllUi(iver, 1, TYPE_DEC_TIP);

        iver = GameObject.Find("ranking_list").GetComponentInChildren<RankingListControl>();
        addAllUi(iver, 2, TYPE_RANKING);
        iver = GameObject.Find("hero").GetComponentInChildren<HeroRoleControl>();
        addAllUi(iver, 2, TYPE_HERO);
        iver = GameObject.Find("setting_ui").GetComponentInChildren<SettingUiControl>();
        addAllUi(iver, 2, TYPE_SETTING);
        iver = GameObject.Find("lunhui").GetComponentInChildren<SamsaraManage>();
        addAllUi(iver, 2, TYPE_SAMSARA);
        iver = GameObject.Find("card_view_new_control").GetComponentInChildren<CardViewContolRoot>();
        addAllUi(iver, 2, TYPE_CARD);
        iver = GameObject.Find("pet").GetComponentInChildren<PetControl>();
        addAllUi(iver, 2, TYPE_PET);
        iver = GameObject.Find("vocation").GetComponentInChildren<VocationControl>();
        addAllUi(iver, 2, TYPE_VOCATION);
        iver = GameObject.Find("advert").GetComponentInChildren<AdUiControl>();
        addAllUi(iver, 2, TYPE_AD);
        iver = GameObject.Find("shop_root").GetComponentInChildren<ShopViewControl>();
        addAllUi(iver, 2, TYPE_SHOP);
        iver = GameObject.Find("vip_show_view").GetComponentInChildren<VipViewControl>();
        addAllUi(iver, 2, TYPE_VIP);


    }
    private void addAllUi(UiControlBase control, long type,long controlType) {
        control.init();
        UiControlBean bean  = new UiControlBean();
        bean.mControl = control;
        bean.mType = type;
        mAllUi.Add(controlType, bean);
    }


    private static UiControlManager mIntance = new UiControlManager();
    public static UiControlManager getIntance()
    {
        return mIntance;
    }
    private UiControlManager()
    {

    }
}
