using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UiManager 
{	
	Text mHeroLvTv,mGameLevelTv,mCurrentCrystalTv,mLvUpCrystalTv,mHpTv,mGasTv;
	Slider mHpSl,mStartBossGasSl;
	Button mStartBossBt,mLvUpBt;
	public void init(){
		mHeroLvTv = GameObject.Find ("lv_labe").GetComponent<Text> ();
		mGameLevelTv = GameObject.Find ("wellen_labe").GetComponent<Text> ();
		mCurrentCrystalTv = GameObject.Find ("god_labe").GetComponent<Text> ();
		mLvUpCrystalTv = GameObject.Find ("cost_labe").GetComponent<Text> ();
		mHpTv = GameObject.Find ("hp_show").GetComponent<Text> ();
		mGasTv = GameObject.Find ("moqi_show").GetComponent<Text> ();

		mStartBossBt = GameObject.Find ("Button_boss").GetComponent<Button> ();
		mLvUpBt = GameObject.Find ("Button_lvup").GetComponent<Button> ();

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
		refreshData ();
	}
	public void refreshData(){
		mHeroLvTv.text = "英雄等级:" + GameManager.getIntance ().mCurrentLevel+"级";
		mLvUpCrystalTv.text = "升级消耗：魔晶" + GameManager.getIntance ().upLevelCrystal;
		mCurrentCrystalTv.text = "拥有魔晶：" + GameManager.getIntance ().mCurrentCrystal;
		mHpTv.text = GameManager.getIntance ().mCurrentBlood + "/" + GameManager.getIntance ().maxBlood;
		mHpSl.maxValue = GameManager.getIntance ().maxBlood;
		mHpSl.value = GameManager.getIntance ().mCurrentBlood;
		if (GameManager.getIntance ().mCurrentCrystal >= GameManager.getIntance ().upLevelCrystal) {
			mLvUpBt.interactable = true;
		} else {
			mLvUpBt.interactable = false;
		}
	}

	public void changeHeroBlood(){
		mHpSl.value = GameManager.getIntance ().mCurrentBlood;
		mHpTv.text = GameManager.getIntance ().mCurrentBlood + "/" + GameManager.getIntance ().maxBlood;
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


	public void showStarBoss(){
		mStartBossBt.interactable = true;
	}
	public void showLevelUp(){
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
		mLvUpBt.interactable = false;
		Debug.Log ("levelUp");
	}

}

