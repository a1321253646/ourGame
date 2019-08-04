using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;

public class LuiHuiTips : UiControlBase
{


    public static int TYPPE_LUIHUI = 1;
    public static int TYPPE_TIP = 2;
    public static int TYPPE_RETURN_START = 3;
    public static int TYPPE_UPDATE_LINE = 4;
    public static int TYPPE_ERROR_DATE = 5;
    public static int TYPPE_VOCATION = 6;
   // public static int TYPPE_SELF = 7;
    public static int TYPPE_LUIHUI_NEED = 8;
    public static int TYPPE_UPDATE = 9;
    public static int TYPPE_QIANGZHI_LUNHUI = 10;
    public static int TYPPE_EMPTY_TOKEN = 11;
    public static int TYPPE_ERROR_TOKEN = 12;
    public static int TYPPE_BUY_ZUANSHI = 13;

    Text mDes;
    Text mButtonDec,mLeftDec,mRightDec;
    Button mSure, mClose,mLeft,mRight;
    private int mType;
    GameObject buttonList;



    public void isUpdate(bool isNeed)
    {
        if (isNeed)
        {
            Thread th1 = new Thread(() =>
            {
                string local = NetServer.getIntance().getLocal();
                Debug.Log("local == " + local);
                SQLManager.getIntance().saveLocal(local);
                Debug.Log("local save end ");
                GameManager.getIntance().mInitStatus = 8;
            });
            th1.Start();
        }
        else
        {
            Thread th1 = new Thread(() =>
            {
                SQLHelper.getIntance().updateIsCleanNet();
                GameManager.getIntance().mInitStatus = 8;
            });
            th1.Start();
        }
    }

    public void showUi(float dec) {
        mControlType = TYPPE_TIP;
        mButtonDec.text = "确定";
        mDes.text = "离线获得魂晶奖励："+dec;
        gameObject.transform.localPosition = new Vector2(0, 0);
        int level = GameManager.getIntance().getUiLevel();
        gameObject.transform.SetSiblingIndex(level);
        mDes.alignment = TextAnchor.UpperLeft;
    }

    public void showUi(string dec) {

        GameObject.Find("lunhui_tips_back").transform.localScale = new Vector2(1, 1);

        mSure.transform.localScale = new Vector2(1, 1);
        buttonList.transform.localScale = new Vector2(0, 0);
        mType = TYPPE_LUIHUI_NEED;
        gameObject.transform.localPosition = new Vector2(0, 0);
        int level = GameManager.getIntance().getUiLevel();
        gameObject.transform.SetSiblingIndex(level);
        //string dec = "轮回将使您失去等级、装备和卡牌，并回到初始关卡。\n您将获得 %D点轮回点作为奖励，轮回点购买的属性将永久保留。";
        Level level2 = JsonUtils.getIntance().getLevelData();




        mLuiHui = BigNumber.multiply(level2.getReincarnation(), GameManager.getIntance().getLunhuiGet());
        mLuiHui = BigNumber.multiply(mLuiHui, 2);
        Debug.Log("============================轮回点获得 GameManager.getIntance().getLunhuiGet()= " + GameManager.getIntance().getLunhuiGet());
        Debug.Log("============================轮回点获得= " + mLuiHui.toString());
        dec = dec.Replace("%D", mLuiHui.toStringWithUnit() + "");
        mButtonDec.text = "确定";
        mDes.text = dec;
        mDes.alignment = TextAnchor.UpperLeft;
    }
    private BigNumber mLuiHui;
    public void showUi()
    {
        mSure.transform.localScale = new Vector2(1, 1);
        buttonList.transform.localScale = new Vector2(0, 0);
        mType = TYPPE_LUIHUI;
        gameObject.transform.localPosition = new Vector2(0, 0);
        int level = GameManager.getIntance().getUiLevel();
        gameObject.transform.SetSiblingIndex(level);
        string dec = "轮回将使您失去等级、装备和卡牌，并回到初始关卡。\n您将获得 %D点轮回点作为奖励，轮回点购买的属性将永久保留。";
        Level level2 = JsonUtils.getIntance().getLevelData();
       

        BigNumber tmp = BigNumber.multiply(level2.getReincarnation(), GameManager.getIntance().getLunhuiGet());
        Debug.Log("============================轮回点获得 GameManager.getIntance().getLunhuiGet()= " + GameManager.getIntance().getLunhuiGet());
        Debug.Log("============================轮回点获得= " + tmp.toString());
        dec =dec.Replace("%D", tmp.toStringWithUnit() + "");
        mButtonDec.text = "轮回";
        mDes.text = dec;
        mDes.alignment = TextAnchor.UpperLeft;
    }

    public void showUi(string str,int type) {
        mType = type;
        mDes.text = str;
        if (type == TYPPE_RETURN_START  || type == TYPPE_EMPTY_TOKEN)
        {
            mSure.transform.localScale = new Vector2(1, 1);
            buttonList.transform.localScale = new Vector2(0, 0);
            mButtonDec.text = "确定";
        }
        else if (type == TYPPE_UPDATE_LINE)
        {
            mSure.transform.localScale = new Vector2(0, 0);
            buttonList.transform.localScale = new Vector2(1, 1);
            mLeftDec.text = "不需要";
            mRightDec.text = "需要";
        }
        else if (type == TYPPE_UPDATE)
        {
            if (GameManager.getIntance().mIsMust == 1)
            {
                mSure.transform.localScale = new Vector2(1, 1);
                buttonList.transform.localScale = new Vector2(0, 0);
                mButtonDec.text = "升级";
                mTimeScale = Time.timeScale;
                Time.timeScale = 0;
            }
            else
            {
                mSure.transform.localScale = new Vector2(0, 0);
                buttonList.transform.localScale = new Vector2(1, 1);
                mLeftDec.text = "暂不升级";
                mRightDec.text = "升级";
                mTimeScale = Time.timeScale;
                Time.timeScale = 0;
            }

        }
        else if (type == TYPPE_ERROR_TOKEN)
        {

            mSure.transform.localScale = new Vector2(0, 0);
            buttonList.transform.localScale = new Vector2(1, 1);
            mLeftDec.text = "重新登陆";
            mRightDec.text = "创建新号";
            mTimeScale = Time.timeScale;
            Time.timeScale = 0;

        }
        else if ( type == TYPPE_ERROR_DATE) {
            Time.timeScale = 0;
            GameManager.getIntance().isQuiteGame = true;
            mSure.transform.localScale = new Vector2(1, 1);
            buttonList.transform.localScale = new Vector2(0, 0);
            mButtonDec.text = "确定";
        }
        else if (type == TYPPE_BUY_ZUANSHI)
        {

            mSure.transform.localScale = new Vector2(0, 0);
            buttonList.transform.localScale = new Vector2(1, 1);
            mLeftDec.text = "取消";
            mRightDec.text = "确认";

        }

        mDes.alignment = TextAnchor.UpperLeft;
    }
    VocationDecBean mVocationBean = null;
    public void showUi(long  vocation)
    {
        toShowUi();
        mType = TYPPE_VOCATION;
        mSure.transform.localScale = new Vector2(0, 0);
        buttonList.transform.localScale = new Vector2(1, 1);
        mLeftDec.text = "确定";
        mRightDec.text = "取消";
        mVocationBean = JsonUtils.getIntance().getVocationById(vocation);
        mDes.text = mVocationBean.tip_dec;
        mDes.alignment = TextAnchor.MiddleCenter;
    }
    public void showUiQiangzhi()
    {
        toShowUi();
        mType = TYPPE_QIANGZHI_LUNHUI;
        mSure.transform.localScale = new Vector2(0, 0);
        buttonList.transform.localScale = new Vector2(1, 1);
        mLeftDec.text = "确定";
        mRightDec.text = "取消";
        mDes.text = JsonUtils.getIntance().getStringById(100037);
        mDes.alignment = TextAnchor.UpperLeft;
    }

    /*  public void showUi(string str, int type)
      {
          mType = TYPPE_RETURN_START;
          mDes.text = str;
          mButtonDec.text = "确定";
          gameObject.transform.localPosition = new Vector2(0, 0);
          int level = GameManager.getIntance().getUiLevel();
          gameObject.transform.SetSiblingIndex(level);

      }*/
    private void sure() {
        UiControlManager.getIntance().remove(UiControlManager.TYPE_SAMSARA);
        Level level = JsonUtils.getIntance().getLevelData();
        BigNumber tmp = BigNumber.multiply(level.getReincarnation(), GameManager.getIntance().getLunhuiGet());
        sure(tmp);
        toremoveUi();
       

    }
    private void sure(BigNumber tmp) {
        sure(tmp, true);
    }

    private void sure(BigNumber tmp,bool isUpdataLuihunLevel) {
        //Time.timeScale = 1;
        GameManager.getIntance().isLuihuiIng = true;
        GameManager.getIntance().isEnd = true;
        GameManager.getIntance().isLunhuiWudiIng = true;
        GameManager.getIntance().uiManager.setLunhuiPointShow(1);
        

        
        Debug.Log("============================轮回点获得= " + tmp.toString());
        GameManager.getIntance().mReincarnation = BigNumber.add(GameManager.getIntance().mReincarnation, tmp);

        SQLHelper.getIntance().updateLunhuiValue(GameManager.getIntance().mReincarnation);
        GameManager.getIntance().isAddGoodForTest = false;
   //     UiControlManager.getIntance().removeAll();

        Level level = JsonUtils.getIntance().getLevelData();
        float speedLevel = level.levelspeedup;
        Debug.Log("===========1================= BaseDateHelper.decodeLong(SQLHelper.getIntance().mTargetSpeed)= " + BaseDateHelper.decodeLong(SQLHelper.getIntance().mTargetSpeed));
        if (isUpdataLuihunLevel) {
            SQLHelper.getIntance().updateTarget(BaseDateHelper.encodeLong((long)speedLevel));
        }
        Debug.Log("===========2================= BaseDateHelper.decodeLong(SQLHelper.getIntance().mTargetSpeed)= " + BaseDateHelper.decodeLong(SQLHelper.getIntance().mTargetSpeed));
        if (SQLHelper.getIntance().mTargetSpeed != BaseDateHelper.encodeLong(-1)) {
            speedLevel = BaseDateHelper.decodeLong(SQLHelper.getIntance().mTargetSpeed);
        }

        SQLHelper.getIntance().updateIsLunhuiValue((long)speedLevel);

       
        long oldLevel =BaseDateHelper.decodeLong( GameManager.getIntance().mCurrentLevel);
        InventoryHalper.getIntance().dealClear();

        // GameManager.getIntance().mCurrentCrystal = new BigNumber();
        //SQLHelper.getIntance().updateHunJing(GameManager.getIntance().mCurrentCrystal);
        long newLevel = 0;
        long luihuiLevel = InventoryHalper.getIntance().getSamsaraLevelById(13);
        long value = 0;
 
        if (luihuiLevel != BaseDateHelper.encodeLong(0) && SQLHelper.getIntance().isCloseYueqiang != 1) 
        {
            List<SamsaraValueBean> list = JsonUtils.getIntance().getSamsaraVulueInfoByIdAndLevel(13, BaseDateHelper.decodeLong(luihuiLevel));
            foreach (SamsaraValueBean bean in list)
            {
                if (bean.type == 500004)
                {
                    value = (long)bean.value;
                    break;
                }
            }
            if (value != 0)
            {
                double bili = value / 10000f;

                newLevel = (long)(oldLevel * bili);
                
            }
        }
        luihuiLevel = InventoryHalper.getIntance().getSamsaraLevelById(14);

        if (luihuiLevel != BaseDateHelper.encodeLong(0) && SQLHelper.getIntance().isCloseChuangye != 1)
        {
            List<SamsaraValueBean> list = JsonUtils.getIntance().getSamsaraVulueInfoByIdAndLevel(14, BaseDateHelper.decodeLong(luihuiLevel));
            foreach (SamsaraValueBean bean in list)
            {
                if (bean.type == 500011)
                {
                    value = (long)bean.value;
                    break;
                }
            }
            if (value != 0)
            {
                if(newLevel == 0) {
                    newLevel = 1;    
                }
                newLevel = newLevel+((long)value);

            }
        }

        if (oldLevel <= 0)
        {
            newLevel = -(long)JsonUtils.getIntance().getConfigValueForId(100019) + 1;
        }
        else if (newLevel >= oldLevel)
        {
            newLevel = oldLevel;
        }
        if(newLevel == 0) {
            newLevel = 1;
        }
       
        GameManager.getIntance().isOpenStop = true;
        Debug.Log("newLevel = " + newLevel);
       // if (SQLHelper.getIntance().isCanLunhui != BaseDateHelper.encodeLong(-1) || BaseDateHelper.decodeLong(SQLHelper.getIntance().isCanLunhui) <= newLevel)
       // {
            SQLHelper.getIntance().UpdateCanLunhui(BaseDateHelper.encodeLong(newLevel + (long)JsonUtils.getIntance().getConfigValueForId(100017)));
       // }

        SQLHelper.getIntance().mGameLevel = BaseDateHelper.encodeLong(-9999L);
        SQLHelper.getIntance().updateGameLevel(BaseDateHelper.encodeLong(newLevel));
        GameObject.Find("qiehuanchangjing").GetComponent<QieHuangChangJing>().run(3);
        SQLHelper.getIntance().addHadLunhui();


        ActiveListControl a = GameObject.Find("active_button_list").GetComponent<ActiveListControl>();
        a.removeVocation(false);
        SQLHelper.getIntance().getActiveList().Clear();
        SQLHelper.getIntance().mCurrentVocation = -1;
        SQLHelper.getIntance().mPlayVocation.Clear();
        SQLHelper.getIntance().mVocationCount = 0;
        JsonUtils.getIntance().reReadAboutLevelFile(BaseDateHelper.decodeLong(SQLHelper.getIntance().mGameLevel));
    }

    // Update is called once per frame
    float mTimeScale = 1;
    public override void init()
    {
        mControlType = UiControlManager.TYPE_LUIHUI;
        mDes = GameObject.Find("lunhui_tips_des").GetComponent<Text>();
        mSure = GameObject.Find("lunhui_tips_sure").GetComponent<Button>();
        mClose = GameObject.Find("lunhui_tips_close").GetComponent<Button>();
        mButtonDec = GameObject.Find("lunhui_sure_ButtonTx").GetComponent<Text>();

        buttonList = GameObject.Find("lunhui_tips_button_list");
        mLeft = GameObject.Find("lunhui_tips_button_list_left").GetComponent<Button>();
        mRight = GameObject.Find("lunhui_tips_button_list_right").GetComponent<Button>();
        mLeftDec = GameObject.Find("lunhui_tips_button_list_left_tx").GetComponent<Text>();
        mRightDec = GameObject.Find("lunhui_tips_button_list_right_tx").GetComponent<Text>();

        mLeft.onClick.AddListener(() =>
        {
            if (mType == TYPPE_UPDATE_LINE)
            {
                isUpdate(false);
            }
            else if (mType == TYPPE_VOCATION)
            {
                vocation();
            }
            else if (mType == TYPPE_UPDATE)
            {
                Time.timeScale = mTimeScale;
            }
            else if (mType == TYPPE_QIANGZHI_LUNHUI)
            {
                UiControlManager.getIntance().remove(UiControlManager.TYPE_SAMSARA);
                sure(new BigNumber(), false);

            }
            else if (mType == TYPPE_ERROR_TOKEN)
            {
                NetServer.getIntance().getLocl(SQLHelper.getIntance().mToken,true,true);

                Time.timeScale = mTimeScale;
            }

            if (isShowSelf)
            {
                transform.localPosition = mFri;
            }
            else
            {
                toremoveUi();
            }

        });
        mRight.onClick.AddListener(() =>
        {
            if (mType == TYPPE_UPDATE_LINE)
            {
                isUpdate(true);
            }
            else if (mType == TYPPE_UPDATE) {
                
                showUpdate();
                Time.timeScale = mTimeScale;
            }
            else if (mType == TYPPE_ERROR_TOKEN)
            {
                SQLHelper.getIntance().mToken = "";
                GameObject.Find("reload").GetComponent<ReloadControl>().reload(true);
                Time.timeScale = mTimeScale;
            }
            else if (mType == TYPPE_BUY_ZUANSHI)
            {
                GameObject.Find("shop_root").GetComponentInChildren<ShopViewControl>().showView(ShopViewControl.SHOW_ZUANSHI);

            }


            if (isShowSelf)
            {
                transform.localPosition = mFri;
            }
            else {
                toremoveUi();
            }
        });

        mSure.onClick.AddListener(() =>
        {
            if (mType == TYPPE_LUIHUI)
            {

                sure();
            }
            else if (mType == TYPPE_LUIHUI_NEED)
            {
                Time.timeScale = 1;
                toremoveUi();
                GameObject.Find("lunhui_tips_back").transform.localScale = new Vector2(0, 0);
                if (SQLHelper.getIntance().mVersionCode < GameManager.mVersionCode)
                {

                    SQLHelper.getIntance().updateVersionCode(GameManager.mVersionCode);
                    sure(mLuiHui, false);


                }
            }
            else if (mType == TYPPE_TIP || mType == TYPPE_EMPTY_TOKEN)
            {
                if (isShowSelf)
                {
                    transform.localPosition = mFri;
                }
                else
                {
                    toremoveUi();
                }
            }
            else if (mType == TYPPE_RETURN_START)
            {
                GameManager.getIntance().mInitStatus = 6;
                toremoveUi();
            }
            else if (mType == TYPPE_ERROR_DATE)
            {
                Application.Quit();
            }
            else if (mType == TYPPE_UPDATE)
            {
                mTimeScale = Time.timeScale;
                showUpdate();
            }
        });
        mClose.onClick.AddListener(() =>
        {
            if (mType == TYPPE_RETURN_START)
            {
                GameManager.getIntance().mInitStatus = 6;
            }
            else if (mType == TYPPE_ERROR_DATE)
            {
                Application.Quit();
            }
            else if (isShowSelf)
            {
                transform.localPosition = mFri;
                if (mType == TYPPE_UPDATE && GameManager.getIntance().mIsMust == 1)
                {
                    Application.Quit();
                }
                else if (mType == TYPPE_UPDATE)
                {
                    Time.timeScale = mTimeScale;
                }
            }
            else if (mType == TYPPE_LUIHUI_NEED)
            {
                Time.timeScale = 1;
                GameObject.Find("lunhui_tips_back").transform.localScale = new Vector2(0, 0);
                toremoveUi();
                if (SQLHelper.getIntance().mVersionCode < GameManager.mVersionCode)
                {
                    sure(mLuiHui, false);
                    SQLHelper.getIntance().updateVersionCode(GameManager.mVersionCode);
                 
                }             
            }
            else if (mType == TYPPE_UPDATE && GameManager.getIntance().mIsMust == 1)
            {
                Application.Quit();
            } else if (mType == TYPPE_UPDATE) {
                Time.timeScale = mTimeScale;
            }
            else
            {
                toremoveUi();
            }
        });
    }


    private void showUpdate() {
#if UNITY_ANDROID
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        string[] mObject = new string[1];
        jo.Call<string>("showTaptap", mObject);
#endif
#if UNITY_IOS

#endif
    }
    private void vocation() {
        SQLHelper.getIntance().updateVocation(mVocationBean.id, mVocationBean.skill != -1);
        JsonUtils.getIntance().reReadHero();
        GameObject.Find("Manager").GetComponent<LevelManager>().heroVocation();
        
        UiControlManager.getIntance().remove(UiControlManager.TYPE_VOCATION);
        SQLHelper.getIntance().updateVocationCount(SQLHelper.getIntance().mVocationCount+1);

        Debug.Log("=================================JsonUtils.getIntance().getConfigValueForId(100044) ==  " + JsonUtils.getIntance().getConfigValueForId(100044) +
            " level==" + BaseDateHelper.decodeLong(GameManager.getIntance().mHeroLv) + "SQLHelper.getIntance().mVocationCount = " + SQLHelper.getIntance().mVocationCount +
            "   BaseDateHelper.decodeLong(GameManager.getIntance().mHeroLv) /(long) JsonUtils.getIntance().getConfigValueForId(100044) <= SQLHelper.getIntance().mVocationCount=" +
            (BaseDateHelper.decodeLong(GameManager.getIntance().mHeroLv) / (long)JsonUtils.getIntance().getConfigValueForId(100044) <= SQLHelper.getIntance().mVocationCount) +
            "mType = "+mType);

        if (mType == TYPPE_VOCATION && BaseDateHelper.decodeLong(GameManager.getIntance().mHeroLv) /(long) JsonUtils.getIntance().getConfigValueForId(100044) <= SQLHelper.getIntance().mVocationCount)
        {
            GameObject.Find("active_button_list").GetComponent<ActiveListControl>().removeVocation();
        }
        

    }

    public override void show()
    {
        isShowSelf = false;
        gameObject.transform.localPosition = new Vector2(0, 0);
    }
    private bool isShowSelf = false;
    public void showSelf()
    {
        isShowSelf = true;
//        mType = TYPPE_SELF;
        int level = GameManager.getIntance().getUiLevel();
        gameObject.transform.SetSiblingIndex(level);

        gameObject.transform.localPosition = new Vector2(0, 0);
    }
}
