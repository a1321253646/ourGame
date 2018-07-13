using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager
{
	public int mCurrentLevel = 1;
	public int mHeroLv = 1;
	public int mCurrentGas = 0;
	public int mCurrentCrystal = 0;
	public int maxBlood = 0;
	public int mCurrentBlood = 0;
	public bool mStartBoss = false;
	public int startBossGas = 0;
	public int upLevelCrystal = 0;
	public int mBossId;
	public bool mHeroIsAlive;
	public UiManager uiManager;
	public bool isLvUp = false;

	private GameManager(){
	}
	private static GameManager mIntance = new GameManager();
	public static GameManager getIntance(){
		return mIntance;
	}
		

	public void getLevelData(){
		Hero hero = JsonUtils.getIntance ().getHeroData ();
		mHeroLv = hero.getRoleLv ();
		Debug.Log ("hero.getRoleHp () = " + hero.getRoleHp () + "  maxBlood=" + maxBlood + " mCurrentBlood =" + mCurrentBlood);
		mCurrentBlood = hero.getRoleHp () - maxBlood +mCurrentBlood;
		maxBlood = hero.getRoleHp ();
		upLevelCrystal = hero.getLvupCrystal ();

	}
	public void init(){
		Level level = JsonUtils.getIntance ().getLevelData ();
		startBossGas = int.Parse (level.boss_gas);
		mBossId = int.Parse (level.boss_DI);
		mCurrentGas = 0;
		mCurrentBlood = maxBlood;
	}

	public void initUi(){
		uiManager = new UiManager ();
		uiManager.init ();
	}

	public void setBlood(int blood){
		mCurrentBlood = blood;
		uiManager.changeHeroBlood ();
	}
	public void heroUp(){
		mHeroLv += 1;
		mCurrentCrystal = mCurrentCrystal - upLevelCrystal ;
		getLevelData ();
		if (mCurrentCrystal >= upLevelCrystal) {
			uiManager.showLevelUp (true);
		} else {
			uiManager.showLevelUp (false);
		}
		uiManager.refreshData ();
		//修改数据到英雄
		isLvUp = true;
	}
	public void startBoss(){
		mStartBoss = true;
	}

	public void enemyDeal(Attacker enemy){
		mCurrentGas += enemy.mDieGas;
		mCurrentCrystal += enemy.mDieCrysta;
		uiManager.addGasAndCrystal ();
	}
}

