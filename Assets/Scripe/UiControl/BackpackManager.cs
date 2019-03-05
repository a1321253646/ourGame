﻿using UnityEngine;
//using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
public class BackpackManager
{
    public static long ALL_TYPE = 10;
    public static long CAILIAO_TYPE = 1;
    public static long ZHUANGBEI_TYPE = 2;
    public static long KAPAI_TYPE = 3;

    private List<PlayerBackpackBean> mInventoryList;
    private List<PlayerBackpackBean> mHeroEquip;
    private List<GoodJsonBean> mGoodInfoList;
    private List<AccouterJsonBean> mAccouterInfoList;
    private List<CardJsonBean> mCardInfoList;


    private RectTransform mTipTranform;
    private RectTransform mBackpack;
    private RectTransform mHeroTranform;
    //    private RectTransform mComposeTranform;
    private RectTransform mSamsaraTranform;
    private RectTransform mLunHuiTips;
    private RectTransform mMessageTip;
    private RectTransform mRankingTip;
    //    private RectTransform mQiangHua;

    public IvertoryControl mInvertoryControl;
    private TipControl mTipControl;
    private HeroRoleControl mHeroControl;
    private CardShowControl mCardControl;
    private LuiHuiTips mLunhuiControl;
    private OutLineGetMessage mMessageTipControl;
    private SettingUiControl mSettingControl;
    private RankingListControl mRankingTipControl;
    //    private QiangHuaManager mQianghuaControl;

    //    private ComposeControl mComposeControl;
    private SamsaraManage mSamsaraControl;

    private ZhuangbeiUpdateControl mZhuangbeiShowControl;
    LevelManager mLevel;
    private BackpackManager()
    {

    }

    public void init(LevelManager manager) {
        mLevel = manager;
        mInventoryList = InventoryHalper.getIntance().getInventorys();
        mHeroEquip = InventoryHalper.getIntance().getRoleUseList();
        mGoodInfoList = JsonUtils.getIntance().getGoodInfoList();
        mAccouterInfoList = JsonUtils.getIntance().getAccouterInfoList();
        mCardInfoList = JsonUtils.getIntance().getCardInfos();
        mBackpack = GameObject.Find("Backpack").GetComponent<RectTransform>();
        mInvertoryControl = mBackpack.GetComponentInChildren<IvertoryControl>();

        mTipTranform = GameObject.Find("tip").GetComponent<RectTransform>();
        mTipControl = mTipTranform.GetComponent<TipControl>();

        mLunHuiTips = GameObject.Find("lunhui_tips").GetComponent<RectTransform>();
        mLunhuiControl = GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>();

        mMessageTip = GameObject.Find("message_tip").GetComponent<RectTransform>();
        mMessageTipControl = GameObject.Find("message_tip").GetComponent<OutLineGetMessage>();

        mRankingTip = GameObject.Find("ranking_list").GetComponent<RectTransform>();
        mRankingTipControl = GameObject.Find("ranking_list").GetComponent<RankingListControl>();

        mHeroTranform = GameObject.Find("hero").GetComponent<RectTransform>();
        mHeroControl = mHeroTranform.GetComponent<HeroRoleControl>();

        mSettingControl = GameObject.Find("setting_ui").GetComponent<SettingUiControl>();

        mSamsaraTranform = GameObject.Find("lunhui").GetComponent<RectTransform>();
        mSamsaraControl = GameObject.Find("lunhui").GetComponent<SamsaraManage>();
        mCardControl = GameObject.Find("Card2").GetComponent<CardShowControl>();

        mZhuangbeiShowControl = GameObject.Find("jineng").GetComponent<ZhuangbeiUpdateControl>();
    }

    public GoodJsonBean getGoodInfoById(long id) {
        foreach (GoodJsonBean bean in mGoodInfoList) {
            if (bean.id == id) {
                return bean;
            }
        }
        return null;
    }

    public AccouterJsonBean getAccouterInfoById(long id)
    {
        foreach (AccouterJsonBean bean in mAccouterInfoList)
        {
            if (bean.id == id)
            {
                return bean;
            }
        }
        return null;
    }

    public CardJsonBean getCardInfoById(long id) {
        foreach (CardJsonBean bean in mCardInfoList)
        {
            if (bean.id == id)
            {
                return bean;
            }
        }
        return null;
    }

    public PlayControl getHero() {
        return mLevel.mPlayerControl;
    }
    public List<PlayerBackpackBean> getHeroEquipInfo() {
        return mHeroEquip;
    }
    public void saveEquipDate(PlayerBackpackBean bean) {
        SQLHelper.getIntance().updateZHUANGBEI(bean);
        SQLHelper.getIntance().updateHunJing(GameManager.getIntance().mCurrentCrystal);
    }

    public void UpdateZhuangBei(PlayerBackpackBean bean, BigNumber cost, long level)
    {
        if (GameManager.getIntance().getGuideManager().guideIsHappen(4)) {
            GameManager.getIntance().getGuideManager().removeGuide(4);
        }
        GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_EQUITE_UP_CLICK, bean.goodId);
        InventoryHalper.getIntance().updateZhuangbei(bean, level,false);
        GameManager.getIntance().mCurrentCrystal = BigNumber.minus(GameManager.getIntance().mCurrentCrystal, cost);
        GameManager.getIntance().updataGasAndCrystal();
        SQLHelper.getIntance().updateHunJing(GameManager.getIntance().mCurrentCrystal);
        mInvertoryControl.update(false);
        mHeroControl.upDateUi(false);
        updateZhuangbeiItem(false);
        mLevel.mPlayerControl.initEquip(false,false);
    }
    public bool use(PlayerBackpackBean bean, long count, int type) {
        if (type == TipControl.USE_TYPE)
        {
            bool isUsed = InventoryHalper.getIntance().use(bean);
            if (!isUsed)
            {
                return false;
            }
            mInvertoryControl.update();
            Debug.Log("TipControl.USE_TYPE");
            mLevel.ChangeEquip(bean, true);
            mHeroControl.upDateUi();
            updateZhuangbeiItem(true);
        }
        else if (type == TipControl.SALE_TYPE  || type == TipControl.SALE_ALL_TYPE) {
            AccouterJsonBean aj =  JsonUtils.getIntance().getAccouterInfoById(bean.goodId);
            List<PlayerAttributeBean> list = bean.attributeList;
            long level = 0;
            foreach (PlayerAttributeBean p in list) {
                if (p.type == 10001) {
                    level = (long)p.value;
                    break;
                }
            }
            BigNumber b1 = aj.getSale();
            BigNumber b2 = aj.getSaleLevel();

            Debug.Log("=====================SALE_TYPE=========================");
            Debug.Log("getSale="+ b1.toString());
            Debug.Log("getSaleLevel="+b2.toString());
            Debug.Log("level= "+ level);
            if (!b2.isEmpty()) {
                b2 = BigNumber.multiply(b2, level);
                b1 = BigNumber.add(b1, b2);
            }
            Debug.Log("sale= " + b1.toString());
            Debug.Log("=====================SALE_TYPE=========================End");
            if (!b1.isEmpty()) {
                GameManager.getIntance().mCurrentCrystal = BigNumber.add(GameManager.getIntance().mCurrentCrystal, b1);
                GameManager.getIntance().updataGasAndCrystal();
                
            }
            
            InventoryHalper.getIntance().deleteIventory(bean, (int)1);
            if (type == TipControl.SALE_TYPE) {
                mInvertoryControl.update();
                mHeroControl.upDateUi();
                updateZhuangbeiItem(true);
                SQLHelper.getIntance().updateHunJing(GameManager.getIntance().mCurrentCrystal);
            }
            
        }
        else if (type == TipControl.UNUSE_TYPE)
        {
            InventoryHalper.getIntance().unUse(bean);
            mInvertoryControl.update();
            mLevel.ChangeEquip(bean, false);
            mHeroControl.upDateUi();
            updateZhuangbeiItem(true);
        }
        else if (type == TipControl.COMPOSE_TYPE)
        {
            //     composeUiShowClick();
        }
        else if (type == TipControl.BOOK_TYPE)
        {
            long id = InventoryHalper.getIntance().useBook(bean, count);
            bool isSuccess = InventoryHalper.getIntance().getIsAddSuccess();
            mInvertoryControl.update();
            if (!isSuccess)
            {
                ComposeJsonBen book = JsonUtils.getIntance().gettComposeInfoForId(id);
                // GameManager.getIntance().mCurrentCrystal += book.compensate;
                GameManager.getIntance().updataGasAndCrystal();
            }
            else
            {
                ///     mComposeControl.updateListPart();
            }
        }
        else if (type == TipControl.USE_CARD_TYPE)
        {
            int userCardCount = InventoryHalper.getIntance().getUsercard().Count;
            float maxCount = JsonUtils.getIntance().getConfigValueForId(100016);
            Debug.Log("userCardCount== " + userCardCount + " maxCount==" + maxCount);

            if (userCardCount >= maxCount)
            {
                return false;
            }
            InventoryHalper.getIntance().useCard(bean);
            mInvertoryControl.update();
            mCardControl.upDateUi();
        }
        else if (type == TipControl.UNUSE_CARD_TYPE)
        {
            InventoryHalper.getIntance().removeUserCard(bean);
            mInvertoryControl.update();
            mCardControl.upDateUi();
        }
        return true;
    }

    public void upLunhui() {
        mLevel.upLunhui();
        mSamsaraControl.updata();
    }

    public void addGoods(long id, int count) {
        Debug.Log("addGoods id=" + id + " count=" + count);
        bool isAddNiew = InventoryHalper.getIntance().addInventory(id, count);
        Debug.Log("isAddNiew =" + isAddNiew);
        mInvertoryControl.update();
        upDataComposeControl();
    }

    public void updateZhuangbeiItem(bool isDelete) {
        mZhuangbeiShowControl.upDate(isDelete);
    }

    public void upDataComposeControl() {
        //  mComposeControl.updataUi();
    }

    public void updateHeroControl() {
        mHeroControl.upDateUi();
    }
    private static BackpackManager mIntance = new BackpackManager();
    public static BackpackManager getIntance()
    {
        return mIntance;
    }


    public void showTipUi(PlayerBackpackBean bean, long count, int type)
    {
        mTipControl.setShowData(bean, count, type);
    }


    public void showMessageTip(int type, string str1, string str2)
    {
        mMessageTipControl.showUI(type, str1, str2);
    }
    public void updateCardBackShow() {
        mCardControl.updateBack();
    }
}