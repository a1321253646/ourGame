﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TipControl : MonoBehaviour {

    private long id;
    private long count;
    private GoodControl mGoodControl;
    private Text mName;
    private Text nButtonTx;
    private Text nDepictTx;
    //private Image mTipImage;
    //private Text mTipCount;
    private Text mTipName;
    PlayerBackpackBean mBean;
    private DepictTextControl mDepoct;
    AccouterJsonBean mAccouter = null;
    GoodJsonBean mGoodJson = null;
    CardJsonBean mCardJson = null;
    private Button mClose, mActionClick;
    private Text mClickText;
    public static int COMPOSE_TYPE = 1;
    public static int USE_TYPE = 2;
    public static int UNUSE_TYPE = 3;
    public static int BOOK_TYPE = 4;
    public static int USE_CARD_TYPE = 5;
    public static int UNUSE_CARD_TYPE = 6;
    public static int SHOW_COMPOSE_TYPE = 7;

    public long mCardId = -1;
    LevelManager mLevelManager;
    private int mCurrentType = 1;
    private Vector2 mFri;
    void Start()
    {
        mLevelManager = GameObject.Find("Manager").GetComponent<LevelManager>();
        mActionClick = GameObject.Find("tip_Button").GetComponent<Button>();
        mClose = GameObject.Find("tip_close").GetComponent<Button>();
        mClickText = GameObject.Find("tipButtonTx").GetComponent<Text>();
        mTipName = GameObject.Find("tipName").GetComponent<Text>();
        mActionClick.onClick.AddListener(() =>
        {
            use();
        });
        mClose.onClick.AddListener(() =>
        {
            removeUi();
        });
        mFri = gameObject.transform.localPosition;
    }

    private void use()
    {
        if (mCurrentType != SHOW_COMPOSE_TYPE) {
            BackpackManager.getIntance().use(mBean, count, mCurrentType);
        }
        
        removeUi();
    }

    public void setShowData(PlayerBackpackBean bean,long count,int type) {
        mCurrentType = type;
        if (mCurrentType == COMPOSE_TYPE)
        {
            mClickText.text = "合成";
        }
        else if (mCurrentType == USE_TYPE)
        {
            mClickText.text = "穿戴";
        }
        else if (mCurrentType == UNUSE_TYPE)
        {
            mClickText.text = "脱下";
        }
        else if (mCurrentType == BOOK_TYPE)
        {
            mClickText.text = "使用";
        }
        else if (mCurrentType == USE_CARD_TYPE)
        {
            mClickText.text = "装备";
        }
        else if (mCurrentType == UNUSE_CARD_TYPE) {
            mClickText.text = "卸下";
        }
        else if (mCurrentType == SHOW_COMPOSE_TYPE) {
            mClickText.text = "关闭";
        }
        mBean = bean;
        this.id = bean.goodId;
        this.count = count;
        showUi();
        updataTip();
        updataUi();
    }


    private void updataTip()
    {
/*        if (mTipImage == null) {
            mTipImage = GameObject.Find("box_icon_tip").GetComponent<Image>();
        }
        if (mTipCount == null) {
            mTipCount = GameObject.Find("box_text_tip").GetComponent<Text>();
        }*/
        if (mTipName == null)
        {
            mTipName = GameObject.Find("tipName").GetComponent<Text>();
        }
        string img = null;
        string name = null;
        long tabID = mBean.tabId;
        if (mBean.tabId == GoodControl.TABID_COMPOSE_TYPE)
        {
            Debug.Log("mBean.goodId = " + mBean.goodId);
            if (mBean.goodId > InventoryHalper.TABID_1_START_ID && mBean.goodId < InventoryHalper.TABID_2_START_ID)
            {
                mBean.tabId = GoodControl.TABID_ITEM_TYPE;
            }
            else if (mBean.goodId > InventoryHalper.TABID_2_START_ID && mBean.goodId < InventoryHalper.TABID_3_START_ID)
            {
                mBean.tabId = GoodControl.TABID_EQUIP_TYPY;
            }
            else if (mBean.goodId > InventoryHalper.TABID_3_START_ID)
            {
                mBean.tabId = GoodControl.TABID_CARD_TYPE;
            }
        }
        if (mBean.tabId == GoodControl.TABID_EQUIP_TYPY)
        {
            mAccouter = JsonUtils.getIntance().getAccouterInfoById(id);
            img = mAccouter.icon;
            name = mAccouter.name;
        }
        
        else if (mBean.tabId == GoodControl.TABID_ITEM_TYPE)
        {
            mGoodJson = JsonUtils.getIntance().getGoodInfoById(id);
            img = mGoodJson.icon;
            name = mGoodJson.name;
        }
        else if (mBean.tabId == GoodControl.TABID_CARD_TYPE)
        {
            mCardJson = JsonUtils.getIntance().getCardInfoById(id);
            img = mCardJson.icon;
            name = mCardJson.name;
        }

        GoodControl g = GetComponentInChildren<GoodControl>();
        g.updateUi(mBean.goodId, count, mBean);
        //mTipCount.text = "" + count;
        mTipName.text = name;
        //mTipImage.sprite = Resources.
        //         Load("backpackIcon/" + img, typeof(Sprite)) as Sprite;
        //mTipImage.color = Color.white;
        mBean.tabId = tabID;
    }

    private void updataUi() {               
        creatDepictText();
       
    }
    private void creatDepictText() {
        string str = "";
        if (mGoodJson != null) {
            str = str + mGoodJson.describe + "\n";
        }
        if (mAccouter != null && mBean.attributeList != null && mBean.attributeList.Count > 0) {
            str = "";
            foreach (PlayerAttributeBean b in mBean.attributeList) {
                if (b.getTypeStr() != null) {
                    str = str + b.getTypeStr() + ":" + b.value + "\n";
                }              
            }
        }
        if (mCardJson != null) {
            SkillJsonBean sj = JsonUtils.getIntance().getSkillInfoById(mCardJson.skill_id);
            str = "";
            str += sj.skill_describe;
            Debug.Log(" SkillJsonBean.describe = " + sj.skill_describe);
            if (str.Contains("&n")) {
                Debug.Log(" str.Contains" );
                CalculatorUtil calcuator = new CalculatorUtil(sj.calculator,sj.effects_parameter);
                float value = calcuator.getValue(mLevelManager.mPlayerControl, null);
                str = str.Replace("&n", "" + value);
            }
        }
        if (nDepictTx == null)
        {
            mDepoct = GameObject.Find("depict_text").GetComponent<DepictTextControl>();
        }
        mDepoct.setText(str);
    }

    private void showUi()
    {
       
        gameObject.transform.localPosition = new Vector2(0, 0);
        int level = GameManager.getIntance().getUiLevel();
        gameObject.transform.SetSiblingIndex(level);
    }
    public void removeUi()
    {
        gameObject.transform.localPosition = mFri;
    }
}