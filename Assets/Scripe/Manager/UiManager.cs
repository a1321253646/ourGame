using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class UiManager 
{	
	Text mHeroLvTv,mGameLevelTv,mCurrentCrystalTv,mLvUpCrystalTv,mHpTv,mGasTv,mBossHpTv;
	Slider mHpSl,mStartBossGasSl,mBossHpSl;
	Button mStartBossBt,mRoleUiShow,mPackUiShow,mHeChengUiShow,mSamsaraUiShow,mCardUiShow,mAutoBoss/*,mVoiceButton*/,mSettingButton,mRankingButton;
    public Button mLvUpBt;
    Image autoBack;
    Image mCardUiPoint,mRoleUiPoint,mPackUiPoint,mSamsaraUiPoint/*,mVoiceImage*/;
    //Sprite mAutoYes, mAutoNo,mVoiceOpen,mVoiceClose;

    float mGasFristX,mBossFristX;

    GameObject mBossUiRoot, mGasUiRoot;
    bool isChangeUi = false;
    bool isGasRuning = false;
    bool isBossRuning = false;
    bool isShowBoss = false;
    float mChangeUiXeach = 0;
    float mWidth = 0;
    float mxBili = 0;
    ActiveListControl mActiveListControl;

    bool isAuto = false;

	public void init(){
        
        mHeroLvTv = GameObject.Find ("lv_labe").GetComponent<Text> ();
		mGameLevelTv = GameObject.Find ("wellen_labe").GetComponent<Text> ();
		mCurrentCrystalTv = GameObject.Find ("god_labe").GetComponent<Text> ();
		mLvUpCrystalTv = GameObject.Find ("lvup_cost_labe").GetComponent<Text> ();
		mHpTv = GameObject.Find ("hp_show").GetComponent<Text> ();
		mGasTv = GameObject.Find ("moqi_show").GetComponent<Text> ();

		mStartBossBt = GameObject.Find ("Button_boss").GetComponent<Button> ();
		mLvUpBt = GameObject.Find ("Button_lvup").GetComponent<Button> ();

        mRoleUiShow = GameObject.Find("role_ui").GetComponent<Button>();
        mPackUiShow = GameObject.Find("pack_ui").GetComponent<Button>();
       // mHeChengUiShow = GameObject.Find("hecheng_ui").GetComponent<Button>();
        mSamsaraUiShow = GameObject.Find("lunhui_ui").GetComponent<Button>();
        mCardUiShow = GameObject.Find("skilcard_ui").GetComponent<Button>();
        mHpSl = GameObject.Find ("blood").GetComponent<Slider> ();
		mStartBossGasSl = GameObject.Find ("gas_sl").GetComponent<Slider> ();
        mRoleUiPoint = GameObject.Find ("role_ui_point").GetComponent<Image> ();
        mPackUiPoint = GameObject.Find ("pack_ui_point").GetComponent<Image> ();
        mSamsaraUiPoint = GameObject.Find ("lunhui_ui_point").GetComponent<Image> ();
        mCardUiPoint = GameObject.Find ("skilcard_ui_point").GetComponent<Image> ();
        mSettingButton = GameObject.Find("setting_button").GetComponent<Button>();
        mRankingButton = GameObject.Find("ranking_list_button").GetComponent<Button>();

        mBossUiRoot = GameObject.Find("boss_info");
        mGasUiRoot = GameObject.Find("moqi_root");
        mBossHpTv = GameObject.Find("boss_hp_show").GetComponent<Text>();
        mBossHpSl = GameObject.Find("boss_blood").GetComponent<Slider>();

        mGasFristX = mGasUiRoot.GetComponent<RectTransform>().position.x;
        mBossFristX = mBossUiRoot.GetComponent<RectTransform>().position.x;
        mWidth = mBossUiRoot.GetComponent<RectTransform>().rect.x;

        float x = GameObject.Find("Canvas").GetComponent<CanvasScaler>().referenceResolution.x;
        mxBili = Screen.width / x;

        mChangeUiXeach = (mBossUiRoot.transform.position.x - mGasUiRoot.transform.position.x) / 0.5f * Time.deltaTime;

        //        mVoiceImage = GameObject.Find ("voice_button").GetComponent<Image> ();
        //        mVoiceButton = GameObject.Find("voice_button").GetComponent<Button> ();

        //        mVoiceOpen = Resources.Load("ui_new/voice1", typeof(Sprite)) as Sprite;
        //        mVoiceClose = Resources.Load("ui_new/voice0", typeof(Sprite)) as Sprite;
        /*        long isVoice1 = SQLHelper.getIntance().isVoice;
                if (isVoice1 == -1 || isVoice1 == 1)
                {
                    mVoiceImage.sprite = mVoiceOpen;
                }
                else {
                    mVoiceImage.sprite = mVoiceClose;
                }

                mVoiceButton.onClick.AddListener(() => {
                    long isVoice =  SQLHelper.getIntance().isVoice;
                    AudioSource source = GameObject.Find("information").GetComponent<AudioSource>();
                    if (isVoice == -1 || isVoice == 1)
                    {
                        GameManager.getIntance().setVoice(source, false, true);
                        mVoiceImage.sprite = mVoiceClose;

                    }
                    else {
                        GameManager.getIntance().setVoice(source, true, true);
                        mVoiceImage.sprite = mVoiceOpen;
                    }
                });*/

        GameObject auto = GameObject.Find("zidongAuto");
        mAutoBoss = auto.GetComponent<Button>();
        autoBack = GameObject.Find("zidong").GetComponent<Image>();
//        mAutoYes = Resources.Load("ui_new/gouxuan_yes", typeof(Sprite)) as Sprite;
//        mAutoNo = Resources.Load("ui_new/gouxuan_no", typeof(Sprite)) as Sprite;
        mGameLevelTv.text =  JsonUtils.getIntance().getLevelData(BaseDateHelper.decodeLong(GameManager.getIntance().mCurrentLevel)).name;

      //  SQLHelper.getIntance().updateGameLevel(GameManager.getIntance().mCurrentLevel);
        mGasTv.text =
			GameManager.getIntance().mCurrentGas+ 
			"/"+
			GameManager.getIntance ().startBossGas;

		mStartBossGasSl.maxValue = GameManager.getIntance ().startBossGas;
		mStartBossGasSl.value = 0;

		mStartBossBt.interactable = false;


		mStartBossBt.onClick.AddListener (() => {
            GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_CLICK_BUTTON, GuideManager.BUTTON_START_BOSS);
            startBoss();
		});

        mSettingButton.onClick.AddListener(() =>
        {
            UiControlManager.getIntance().show(UiControlManager.TYPE_SETTING);
           
        });

        mRankingButton.onClick.AddListener(() =>
        {
            UiControlManager.getIntance().show(UiControlManager.TYPE_RANKING);
        });


        mRoleUiShow.onClick.AddListener(() => {
            UiControlManager.getIntance().show(UiControlManager.TYPE_HERO);
            setRolePointShow(2);
        });
        mPackUiShow.onClick.AddListener(() => {
            GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_CLICK_BUTTON, GuideManager.BUTTON_START_OPEN_BACK);
            UiControlManager.getIntance().show(UiControlManager.TYPE_IVERTORY);
            setPackPointShow(2);
        });
 //       mHeChengUiShow.onClick.AddListener(() => {
            //  BackpackManager.getIntance().composeUiShowClick();
  //          BackpackManager.getIntance().qianghuaClick();
   //     });
        mSamsaraUiShow.onClick.AddListener(() => {
            UiControlManager.getIntance().show(UiControlManager.TYPE_SAMSARA);
            setLunhuiPointShow(2);
        });

        mCardUiShow.onClick.AddListener(() => {
            GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_CLICK_BUTTON, GuideManager.BUTTON_CLICK_OPEN_CARD);
            UiControlManager.getIntance().show(UiControlManager.TYPE_CARD);
            setCardPointShow(2);
        });

        mAutoBoss.onClick.AddListener(() =>
        {

            //Time.timeScale = 0;
            isAuto = !isAuto;
            clickAuto();
            GameManager.getIntance().setIsAutoBoss(isAuto);
        });
        refreshData ();
        isAuto = GameManager.getIntance().gettIsAutoBoss();
        clickAuto();
        initPoint();

        mActiveListControl = GameObject.Find("active_button_list").GetComponent<ActiveListControl>();
        List<ActiveButtonBean> list = SQLHelper.getIntance().getActiveList();

        for (int i = 0; i < list.Count; i++) {
            if (list[i].buttonType == ActiveButtonControl.ACTIVE_BUTTON_TYPE_VOCATION) {
                mActiveListControl.showVocation(false);
            }
/*            else if (list[i].buttonType == ActiveButtonControl.ACTIVE_BUTTON_TYPE_AD)
            {
                if (!GameObject.Find("Manager").GetComponent<AdManager>().isReadyToShow())
                {
                    return;
                }
                mActiveListControl.showAd(list[i].adType, list[i].count, false);
            }*/
        }
        
        
    }

    public void reset() {
        mGameLevelTv.text = JsonUtils.getIntance().getLevelData(BaseDateHelper.decodeLong(GameManager.getIntance().mCurrentLevel)).name;

        //  SQLHelper.getIntance().updateGameLevel(GameManager.getIntance().mCurrentLevel);
        mGasTv.text =
            GameManager.getIntance().mCurrentGas +
            "/" +
            GameManager.getIntance().startBossGas;

        refreshData();

        mStartBossGasSl.maxValue = GameManager.getIntance().startBossGas;
        mStartBossGasSl.value = 0;

        mStartBossBt.interactable = false;
    }

    public void showVocation(bool isAddSql) {
        long level = BaseDateHelper.decodeLong(GameManager.getIntance().mHeroLv); 
        //Debug.Log("=================================level ==  " + level + " 100044==" + (level % (long)JsonUtils.getIntance().getConfigValueForId(100044) == 0));
        if (level % (long)JsonUtils.getIntance().getConfigValueForId(100044) == 0) {
            
            mActiveListControl.showVocation(isAddSql);
            GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_SHOW_VOCATION, 5);
        }
    }

    public void initPoint() {
        changePointShow(mRoleUiPoint, GameManager.getIntance().isShowPlayerPoint);
        changePointShow(mPackUiPoint, GameManager.getIntance().isShowBackpackPoint);
        changePointShow(mSamsaraUiPoint, GameManager.getIntance().isShowLuihuiPoint);
        changePointShow(mCardUiPoint, GameManager.getIntance().isShowCardPoint);
    }

    public void setRolePointShow(long value) {
        if (value == GameManager.getIntance().isShowPlayerPoint) {
            return;
        }
        GameManager.getIntance().isShowPlayerPoint = value;
        changePointShow(mRoleUiPoint, value);
        SQLHelper.getIntance().updatePointPlayer(value);
    }
    public void setPackPointShow(long value)
    {
        if (value == GameManager.getIntance().isShowBackpackPoint)
        {
            return;
        }
        GameManager.getIntance().isShowBackpackPoint = value;
        changePointShow(mPackUiPoint, value);
        SQLHelper.getIntance().updatePointBackpack(value);
    }
    public void setLunhuiPointShow(long value)
    {
        if (value == GameManager.getIntance().isShowLuihuiPoint)
        {
            return;
        }
        GameManager.getIntance().isShowLuihuiPoint = value;
        changePointShow(mSamsaraUiPoint, value);
        SQLHelper.getIntance().updatePointLunhui(value);
    }
    public void setCardPointShow(long value)
    {
        if (value == GameManager.getIntance().isShowCardPoint)
        {
            return;
        }
        GameManager.getIntance().isShowCardPoint = value;
        changePointShow(mCardUiPoint, value);
        SQLHelper.getIntance().updatePointCard(value);
    }
    private void changePointShow(Image image,long value) {
        if (value == 1)
        {
            image.color = new Color(0xff, 0xff, 0xff, 0xff);
        }
        else
        {
            image.color = new Color(0xff, 0xff, 0xff, 0);
        }
    }

    private void clickAuto() {    
        if (isAuto)
        {
            if ( GameManager.getIntance().mCurrentGas >= GameManager.getIntance().startBossGas)
            {
                startBoss();
            }
            autoBack.transform.localScale = new Vector2(1, 1);
//            autoBack.sprite = mAutoYes;
        }
        else
        {
            autoBack.transform.localScale = new Vector2(0, 0);
         //   autoBack.sprite = mAutoNo;
        }
    }

    public void refreshData(){
		mHeroLvTv.text = "" + BaseDateHelper.decodeLong(GameManager.getIntance().mHeroLv);
        mLvUpCrystalTv.text =  GameManager.getIntance ().upLevelCrystal.toStringWithUnit();
		mCurrentCrystalTv.text =   GameManager.getIntance ().mCurrentCrystal.toStringWithUnit();
        Hero l = JsonUtils.getIntance().getHeroData(BaseDateHelper.decodeLong(GameManager.getIntance().mHeroLv) + 1);
        if (l == null)
        {
            mLvUpBt.interactable = false;
            mLvUpCrystalTv.text = "已满级";
            return;
        }
        if (GameManager.getIntance ().mCurrentCrystal.ieEquit( GameManager.getIntance ().upLevelCrystal) != -1) {
			mLvUpBt.interactable = true;
		} else {
			mLvUpBt.interactable = false;
        }
	}

    public void changeBossBlod(double current, double max) {
        double bili = 1;
        if (current < 0)
        {
            mBossHpTv.text = 0 + "/" + StringUtils.doubleToStringShow(max);
        }
        else
        {
            mBossHpTv.text = StringUtils.doubleToStringShow(current) + "/" + StringUtils.doubleToStringShow(max);
        }
        if (max > float.MaxValue)
        {
            bili = max / float.MaxValue + 1;
        }

        mBossHpSl.maxValue = (float)(max / bili);
        mBossHpSl.value = (float)(current / bili);
    }


    public void changeHeroBlood(double current,double max){
        double bili = 1;

//        Debug.Log("============================current = " + current + "max =" + max);
		if (current < 0) {
			mHpTv.text =0 + "/" + StringUtils.doubleToStringShow(max);
		} else {
			mHpTv.text = StringUtils.doubleToStringShow(current) + "/" + StringUtils.doubleToStringShow(max);
		}
        if (max > float.MaxValue)
        {
            bili = max / float.MaxValue + 1;
        }

        mHpSl.maxValue = (float)(max / bili);
        mHpSl.value = (float)(current / bili);

    }

	public void addGasAndCrystal(){
		mCurrentCrystalTv.text = GameManager.getIntance ().mCurrentCrystal.toStringWithUnit();
        addGas();
        Hero l = JsonUtils.getIntance().getHeroData(BaseDateHelper.decodeLong(GameManager.getIntance().mHeroLv) + 1);
        if (l == null) {
            mLvUpBt.interactable = false;
            mLvUpCrystalTv.text = "已满级";
            return;
        }
        if (!mLvUpBt.IsInteractable() && GameManager.getIntance().mCurrentCrystal.ieEquit( GameManager.getIntance().upLevelCrystal) != -1)
        {
            mLvUpBt.interactable = true;
        
        }
        else if(mLvUpBt.IsInteractable() && GameManager.getIntance().mCurrentCrystal.ieEquit(GameManager.getIntance().upLevelCrystal) == -1)
        {
            mLvUpBt.interactable = false;
        }
 
	}

    bool isGuide = false;

    public void addGas() {
        mStartBossGasSl.maxValue = GameManager.getIntance().startBossGas;
        if (GameManager.getIntance().mCurrentGas > GameManager.getIntance().startBossGas)
        {
            mGasTv.text = GameManager.getIntance().startBossGas + "/" + GameManager.getIntance().startBossGas;
            mStartBossGasSl.value = GameManager.getIntance().startBossGas;
        }
        else
        {
            mGasTv.text = GameManager.getIntance().mCurrentGas + "/" + GameManager.getIntance().startBossGas;
            mStartBossGasSl.value = GameManager.getIntance().mCurrentGas;
        }
        if (!mStartBossBt.IsInteractable() && GameManager.getIntance().mCurrentGas >= GameManager.getIntance().startBossGas)
        {
            mStartBossBt.interactable = true;
            if (!isGuide)
            {
                isGuide = true;
                //   GameManager.getIntance().getGuideManager().ShowGuideNormalObject(mStartBossBt.gameObject);
            }
            if (isAuto)
            {
                startBoss();
            }
        }
    }

    public static int FLY_RIGHT = 1;
    public static int FLY_LEFT = 2;
    public static int FLY_UP = 3;

	public static void FlyTo(Graphic graphic,int fly)
	{
		RectTransform rt = graphic.rectTransform;
		Color c = graphic.color;
		//c.a = 0;
		graphic.color = c;
		Sequence mySequence = DOTween.Sequence ();
		Sequence mySequence2 = DOTween.Sequence ();

		Tweener move1 = rt.DOMoveY(rt.position.y + 150, 2f);

        Tweener move2;
        if (fly == FLY_RIGHT)
        {
            move2 = rt.DOMoveX(rt.position.x + 100, 2f);
            mySequence2.Append(move2);
        }
        else if (fly == FLY_LEFT)
        {
            move2 = rt.DOMoveX(rt.position.x - 100, 2f);
            mySequence2.Append(move2);
        }
        //		Tweener alpha1 = graphic.DOColor(new Color(c.r, c.g, c.b, 1), 0.5f);
        //		Tweener alpha2 = graphic.DOColor(new Color(c.r, c.g, c.b, 0), 0.5f);
    

        mySequence.Append(move1);
		
		//		mySequence.Join(alpha1);
	//	mySequence.AppendInterval(1);
		//		mySequence.Join(alpha2);
	}
	public void showStarBoss(){
		mStartBossBt.interactable = true;
	}
	public void showLevelUp(bool isEnable){
		mLvUpBt.interactable = true;
	}

	private void startBoss(){
		GameManager.getIntance ().mCurrentGas = 0;
        addGasAndCrystal();
        Debug.Log ("startBoss");
		GameManager.getIntance ().startBoss ();
		mStartBossBt.interactable = false;
	}
    Vector2 mHuanjingVect = new Vector2(0,0);
    Vector2 mGoodVect = new Vector2(0, 0);

    public Vector2 mgetTarget(int type) {
        if (type == DiaoluoDonghuaControl.GOOD_DIAOLUO_TYPE)
        {           
            if (mGoodVect.x == 0)
            {
                GameObject good = GameObject.Find("pack_ui");
                Debug.Log("good.x = " + good.transform.position.y + " good.y=" + good.transform.position.y);
                mGoodVect.x =good.transform.position.x;
                mGoodVect.y =good.transform.position.y;
            }
            return mGoodVect;
        }
        else if (type == DiaoluoDonghuaControl.SHUIJI_DIAOLUO_TYPE)
        {
            
            if (mHuanjingVect.x == 0)
            {
                GameObject hunjing = GameObject.Find("gas_sl_img");
                mHuanjingVect.x =hunjing.transform.position.x;
                mHuanjingVect.y = hunjing.transform.position.y;
            }
            return mHuanjingVect;
        }
        return new Vector2(0, 0);
    }


    public void setAutoStartBossShow(bool isShow) {
        setShow("zidongAuto", isShow);
    }
    public void setBackpackShow(bool isShow)
    {
        setShow("pack_ui", isShow);
    }
    public void setLunhuiShow(bool isShow)
    {
        setShow("lunhui_ui", isShow);
    }
    public void setCardShow(bool isShow)
    {
        setShow("skilcard_ui", isShow);
    }
    public void seHeroUpShow(bool isShow)
    {
        setShow("Button_lvup", isShow);
    }

    private void setShow(string obId, bool isShow) {
        Transform transfor = GameObject.Find(obId).transform;
        if (isShow)
        {
            transfor.localScale = new Vector3(1, 1, 1);
        }
        else {
            transfor.localScale = new Vector3(0, 0, 0);
        }
    }

    public void showBossUi() {
        isChangeUi = true;
        isShowBoss = true;
        isGasRuning = true;
        isBossRuning = false;
    }
    public void showGasUi() {
        isChangeUi = true;
        isShowBoss = false;
        isGasRuning = false;
        isBossRuning = true;
    }

    public void upDate() {
        if (!isChangeUi) {
            return;
        }
        if (isShowBoss)
        {


            if (isGasRuning)
            {
                if (mGasUiRoot.transform.position.x + mChangeUiXeach >= mBossFristX+ mWidth/ 2 *mxBili)
                {
                    mGasUiRoot.transform.Translate(Vector2.right * (mBossFristX + mWidth / 2*mxBili - mGasUiRoot.transform.position.x));
                    isGasRuning = false;
                    isBossRuning = true;
                }
                else
                {
                    mGasUiRoot.transform.Translate(Vector2.right * mChangeUiXeach);
                }
            }
            else if (isBossRuning)
            {
                if (mBossUiRoot.transform.position.x - mChangeUiXeach <= mGasFristX- mWidth/ 2*mxBili)
                {
                    mBossUiRoot.transform.Translate(Vector2.left * (mBossUiRoot.transform.position.x - mGasFristX+mWidth/ 2*mxBili));
                    BossCardManager card = GameObject.Find("boss_info").GetComponent<BossCardManager>();
                    card.init(GameManager.getIntance().mBoss);
                    card.show();
                    isBossRuning = false;
                }
                else
                {
                    mBossUiRoot.transform.Translate(Vector2.left * mChangeUiXeach);
                }
            }
            else {
                isChangeUi = false;
            }
        }
        else {
            if (isBossRuning)
            {
                if (mBossUiRoot.transform.position.x + mChangeUiXeach >= mBossFristX )
                {
                    mBossUiRoot.transform.Translate(Vector2.right * (mBossFristX - mBossUiRoot.transform.position.x));
                    isGasRuning = true;
                    isBossRuning = false;
                }
                else
                {
                    mBossUiRoot.transform.Translate(Vector2.right * mChangeUiXeach);
                }
            }
            else if (isGasRuning)
            {
                if (mGasUiRoot.transform.position.x - mChangeUiXeach <= mGasFristX )
                {
                    mGasUiRoot.transform.Translate(Vector2.left * (mGasUiRoot.transform.position.x - mGasFristX));
                    isBossRuning = false;
                }
                else
                {
                    mGasUiRoot.transform.Translate(Vector2.left * mChangeUiXeach);
                }
            }
            else
            {
                isChangeUi = false;
            }
        }
    }
}

