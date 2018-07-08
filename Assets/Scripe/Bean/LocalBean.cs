using UnityEngine;
using System.Collections;

public class LocalBean
{

	public static float MIX_LENG = 2;
	public static float MID_LENG = 4;
	public static float MAX_LENG = 6;

	public static int MIX_TYPE = 1;
	public static int MID_TYPE = 2;
	public static int MAX_TYPE = 3;
	public float mCurrentX;
	public float mCurrentY;
	public float mTargetX = -9999;
	public float mTargetY = -9999;

	public float mAttackLeng;
	public int mAttackType;
	public bool mIsHero = false;
	public Attacker mAttacker;

	public LocalBean next;
	public LocalBean previous;

	public LocalBean(float x,float y,float leng,bool hero,Attacker attack){
		mCurrentX = x;
		mCurrentY = y;
		mIsHero = hero;
		mAttacker = attack;
		mAttackLeng = leng;
		if (leng < MID_LENG) {
			mAttackType = MIX_TYPE;
		} else if (leng > MID_LENG) {
			mAttackType = MAX_TYPE;
		} else {
			mAttackType = MID_TYPE;
		}
	}
	public LocalBean(){
	
	}
}

