using UnityEngine;
using System.Collections;

public class GameManager
{
	public int mCurrentLevel = 1;
	public int mHeroLv = 1;
	public int mCurrentGas = 0;
	public int mCurrentCrystal = 0;
	public int maxBlood = 0;
	public int mCurrentBlood = 0;

	private GameManager(){
		//getLevelData ();
	}
	private static GameManager mIntance = new GameManager();
	public static GameManager getIntance(){
		return mIntance;
	}
	public void getLevelData(){
		JsonUtils.getIntance ().init ();
		Hero hero = JsonUtils.getIntance ().getHeroData ();
		mHeroLv = hero.getRoleLv ();
		mCurrentBlood = hero.getRoleLv () - maxBlood +mCurrentBlood;
		maxBlood = hero.getRoleHp ();
	}
	public void setBlood(int blood){
		mCurrentBlood = blood;
		float scale = mCurrentBlood/maxBlood;
	}
}

