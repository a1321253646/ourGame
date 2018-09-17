using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
public class UiManager 
{	
	Text mHeroLvTv,mGameLevelTv,mCurrentCrystalTv,mLvUpCrystalTv,mHpTv,mGasTv;
	Slider mHpSl,mStartBossGasSl;
	Button mStartBossBt,mLvUpBt,mRoleUiShow,mPackUiShow,mHeChengUiShow,mSamsaraUiShow,mCardUiShow;
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
        mHeChengUiShow = GameObject.Find("hecheng_ui").GetComponent<Button>();
        mSamsaraUiShow = GameObject.Find("lunhui_ui").GetComponent<Button>();
        mCardUiShow = GameObject.Find("skilcard_ui").GetComponent<Button>();
        mHpSl = GameObject.Find ("blood").GetComponent<Slider> ();
		mStartBossGasSl = GameObject.Find ("gas_sl").GetComponent<Slider> ();

		mGameLevelTv.text = "当前关卡:第" + GameManager.getIntance ().mCurrentLevel+"关";
		mGasTv.text =
			GameManager.getIntance().mCurrentGas+ 
			"/"+
			GameManager.getIntance ().startBossGas;

		mStartBossGasSl.maxValue = GameManager.getIntance ().startBossGas;
		mStartBossGasSl.value = 0;

		mStartBossBt.interactable = false;


		mStartBossBt.onClick.AddListener (() => {
			startBoss();
		});
		mLvUpBt.onClick.AddListener (() => {
			levelUp();
		});

        mRoleUiShow.onClick.AddListener(() => {
            BackpackManager.getIntance().heroUiShowClick();
        });
        mPackUiShow.onClick.AddListener(() => {
            BackpackManager.getIntance().packUiShowClick();
        });
        mHeChengUiShow.onClick.AddListener(() => {
            BackpackManager.getIntance().composeUiShowClick();
        });
        mSamsaraUiShow.onClick.AddListener(() => {
            BackpackManager.getIntance().samsaraShowClick();
        });

        mCardUiShow.onClick.AddListener(() => {
            BackpackManager.getIntance().cardUiShowClick();
        });

        refreshData ();
	}

	public void refreshData(){
		mHeroLvTv.text = "英雄等级:" + GameManager.getIntance ().mHeroLv +"级";
		mLvUpCrystalTv.text = "升级消耗：魔晶" + GameManager.getIntance ().upLevelCrystal;
		mCurrentCrystalTv.text = "拥有魔晶：" + GameManager.getIntance ().mCurrentCrystal;
		if (GameManager.getIntance ().mCurrentCrystal >= GameManager.getIntance ().upLevelCrystal) {
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
		mCurrentCrystalTv.text = "拥有魔晶：" + GameManager.getIntance ().mCurrentCrystal;

		if (GameManager.getIntance ().mCurrentGas > GameManager.getIntance ().startBossGas) {
			mGasTv.text =GameManager.getIntance ().startBossGas+ "/"+GameManager.getIntance ().startBossGas;
			mStartBossGasSl.value = GameManager.getIntance ().startBossGas;
		} else {
			mGasTv.text =GameManager.getIntance().mCurrentGas+ "/"+GameManager.getIntance ().startBossGas;
			mStartBossGasSl.value = GameManager.getIntance().mCurrentGas;
		}
		if (!mLvUpBt.IsInteractable() && GameManager.getIntance ().mCurrentCrystal >= GameManager.getIntance ().upLevelCrystal) {
			mLvUpBt.interactable = true;
		}
		if (!mStartBossBt.IsInteractable() && GameManager.getIntance ().mCurrentGas >= GameManager.getIntance ().startBossGas) {
			mStartBossBt.interactable = true;
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
		Debug.Log ("startBoss");
		GameManager.getIntance ().startBoss ();
		mStartBossBt.interactable = false;
	}
	private void levelUp(){
		GameManager.getIntance ().heroUp ();
	//	mLvUpBt.interactable = false;
		Debug.Log ("levelUp");
	}
}

