﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LuiHuiTips : MonoBehaviour {


    public static int TYPPE_LUIHUI = 1;
    public static int TYPPE_TIP = 2;


    Text mDes;
    Text mButtonDec;
    Button mSure, mClose;
    private Vector2 mFri;
    private int mType;

    // Use this for initialization
    void Start()
    {
        mDes = GameObject.Find("lunhui_tips_des").GetComponent<Text>();
        mSure = GameObject.Find("lunhui_tips_sure").GetComponent<Button>();
        mClose = GameObject.Find("lunhui_tips_close").GetComponent<Button>();
        mButtonDec = GameObject.Find("lunhui_sure_ButtonTx").GetComponent<Text>();
        mFri = gameObject.transform.localPosition;
        mSure.onClick.AddListener(() =>
        {
            if (mType == TYPPE_LUIHUI)
            {
                sure();
            }
            else if (mType == TYPPE_TIP)
            {
                removeUi();
            }
            
        });
        mClose.onClick.AddListener(() =>
        {
            removeUi();
        });
    }
    public void showUi(float dec) {
        mType = TYPPE_TIP;
        mButtonDec.text = "确定";
        mDes.text = "离线获得魂晶奖励："+dec;
        gameObject.transform.localPosition = new Vector2(0, 0);
        int level = GameManager.getIntance().getUiLevel();
        gameObject.transform.SetSiblingIndex(level);
    }

    public void showUi()
    {
        mType = TYPPE_LUIHUI;
        gameObject.transform.localPosition = new Vector2(0, 0);
        int level = GameManager.getIntance().getUiLevel();
        gameObject.transform.SetSiblingIndex(level);
        string dec = mDes.text;
        Level level2 = JsonUtils.getIntance().getLevelData();

        dec =dec.Replace("%D", level2.reincarnation + "");
        mButtonDec.text = "轮回";
        mDes.text = dec;
    }
    public void removeUi()
    {
        gameObject.transform.localPosition = mFri;
        
    }
    private void sure() {
        Level level = JsonUtils.getIntance().getLevelData();
        GameManager.getIntance().mReincarnation +=((long) (level.reincarnation * GameManager.getIntance().getLunhuiGet()));
        SQLHelper.getIntance().updateLunhuiValue(GameManager.getIntance().mReincarnation);
        GameManager.getIntance().isAddGoodForTest = false; 
        BackpackManager.getIntance().removeAll();
        InventoryHalper.getIntance().dealClear();
        GameManager.getIntance().mCurrentCrystal = new BigNumber();
        SQLHelper.getIntance().updateHunJing(GameManager.getIntance().mCurrentCrystal);
        GameManager.getIntance().mHeroLv = 1;
        SQLHelper.getIntance().updateHeroLevel(1);
        GameManager.getIntance().mCurrentLevel = 1;
        SQLHelper.getIntance().updateGameLevel(1);
        GameObject.Find("qiehuanchangjing").GetComponent<QieHuangChangJing>().run(3);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
