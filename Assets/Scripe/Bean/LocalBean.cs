using UnityEngine;
using System.Collections;

public class LocalBean
{

	public float mCurrentX;
	public float mCurrentY;
	public float mTargetX = -9999;
	public float mTargetY = -9999;

	public float mAttackLeng;
	public bool mIsHero = false;
	public Attacker mAttacker;
	public bool isInMultiple = false;

	public LocalBean(float x,float y,float leng,bool hero,Attacker attack){
		mCurrentX = x;
		mCurrentY = y;
		mIsHero = hero;
		mAttacker = attack;
		mAttackLeng = leng;
	}
	public LocalBean(){
	
	}
}

