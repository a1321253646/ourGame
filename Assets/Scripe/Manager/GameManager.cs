﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager
{
	public long mCurrentLevel = 8;
	public long mHeroLv = 18;
	public float mCurrentGas = 0;
	public float mCurrentCrystal = 0;
	public float maxBlood = 0;
	public float mCurrentBlood = 0;
	public bool mStartBoss = false;
	public float startBossGas = 0;
	public float upLevelCrystal = 0;
	public long mBossId;
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
		mHeroLv = hero.role_lv;
		Debug.Log ("hero.getRoleHp () = " + hero.role_hp + "  maxBlood=" + maxBlood + " mCurrentBlood =" + mCurrentBlood);
		mCurrentBlood = hero.role_hp - maxBlood +mCurrentBlood;
		maxBlood = hero.role_hp;
		upLevelCrystal = hero.lvup_crystal;

	}
	public void init(){
		Level level = JsonUtils.getIntance ().getLevelData ();
		startBossGas = level.boss_gas;
		mBossId = level.boss_DI;
		mCurrentGas = 0;
		mCurrentBlood = maxBlood;
	}

	public void initUi(){
		uiManager = new UiManager ();
		uiManager.init ();
	}

	public void setBlood(float blood){
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
		mCurrentGas += enemy.mDieGas*JsonUtils.getIntance().getConfigValueForId(100009);
		mCurrentCrystal += enemy.mDieCrysta * JsonUtils.getIntance().getConfigValueForId(100008);
        uiManager.addGasAndCrystal ();
	}
}

