﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
public class UiManager 
{	
	Text mHeroLvTv,mGameLevelTv,mCurrentCrystalTv,mLvUpCrystalTv,mHpTv,mGasTv;
	Slider mHpSl,mStartBossGasSl;
	Button mStartBossBt,mLvUpBt,mRoleUiShow,mPackUiShow,mHeChengUiShow,mSamsaraUiShow,mCardUiShow,mAutoBoss;
    Image autoBack;
    Image mCardUiPoint,mRoleUiPoint,mPackUiPoint,mSamsaraUiPoint;
    Sprite mAutoYes, mAutoNo;

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

        GameObject auto = GameObject.Find("zidong");
        mAutoBoss = auto.GetComponent<Button>();
        autoBack = auto.GetComponent<Image>();
        mAutoYes = Resources.Load("ui_new/gouxuan_yes", typeof(Sprite)) as Sprite;
        mAutoNo = Resources.Load("ui_new/gouxuan_no", typeof(Sprite)) as Sprite;
        mGameLevelTv.text = "当前关卡:" + JsonUtils.getIntance().getLevelData(GameManager.getIntance().mCurrentLevel).name;       
        SQLHelper.getIntance().updateGameLevel(GameManager.getIntance().mCurrentLevel);
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
		mLvUpBt.onClick.AddListener (() => {
            Debug.Log("levelUp");
            GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_CLICK_BUTTON, GuideManager.BUTTON_START_HERO_UP);
            levelUp();
		});

        mRoleUiShow.onClick.AddListener(() => {
            BackpackManager.getIntance().heroUiShowClick();
            setRolePointShow(2);
        });
        mPackUiShow.onClick.AddListener(() => {
            GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_CLICK_BUTTON, GuideManager.BUTTON_START_OPEN_BACK);
            BackpackManager.getIntance().packUiShowClick();
            setPackPointShow(2);
        });
 //       mHeChengUiShow.onClick.AddListener(() => {
            //  BackpackManager.getIntance().composeUiShowClick();
  //          BackpackManager.getIntance().qianghuaClick();
   //     });
        mSamsaraUiShow.onClick.AddListener(() => {
            
            BackpackManager.getIntance().samsaraShowClick();
            setLunhuiPointShow(2);
        });

        mCardUiShow.onClick.AddListener(() => {
            GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_CLICK_BUTTON, GuideManager.BUTTON_CLICK_OPEN_CARD);
            BackpackManager.getIntance().cardUiShowClick();
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
            autoBack.sprite = mAutoYes;
        }
        else
        {
            autoBack.sprite = mAutoNo;
        }
    }

	public void refreshData(){
		mHeroLvTv.text = "英雄等级:" + GameManager.getIntance ().mHeroLv +"级";
        SQLHelper.getIntance().updateHeroLevel(GameManager.getIntance().mHeroLv);
        SQLHelper.getIntance().updateHeroLevel(GameManager.getIntance().mHeroLv);
        mLvUpCrystalTv.text =  GameManager.getIntance ().upLevelCrystal.toStringWithUnit();
		mCurrentCrystalTv.text =   GameManager.getIntance ().mCurrentCrystal.toStringWithUnit();
        Hero l = JsonUtils.getIntance().getHeroData(GameManager.getIntance().mHeroLv + 1);
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

	public void changeHeroBlood(float current,float max){
        mHpSl.maxValue = max;
        mHpSl.value = current;
		if (current < 0) {
			mHpTv.text =0 + "/" + max;
		} else {
			mHpTv.text = current + "/" + max;
		}

	}

	public void addGasAndCrystal(){
		mCurrentCrystalTv.text = GameManager.getIntance ().mCurrentCrystal.toStringWithUnit();
        addGas();
        Hero l = JsonUtils.getIntance().getHeroData(GameManager.getIntance().mHeroLv + 1);
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

	public static void FlyTo(Graphic graphic)
	{
		RectTransform rt = graphic.rectTransform;
		Color c = graphic.color;
		//c.a = 0;
		graphic.color = c;
		Sequence mySequence = DOTween.Sequence ();

		Tweener move1 = rt.DOMoveY(rt.position.y + 150, 2f);
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
	private void levelUp(){
		GameManager.getIntance ().heroUp ();
	//	mLvUpBt.interactable = false;
		
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
}

