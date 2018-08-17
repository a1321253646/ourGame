﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attacker : MonoBehaviour 
{

	public Animator _anim;
	// Use this for initialization

	public static  int PLAY_STATUS_STANDY = 1;
	public static  int PLAY_STATUS_FIGHT = 2;
	public static  int PLAY_STATUS_RUN = 3;
	public static  int PLAY_STATUS_DIE = 4;
	public static  int PLAY_STATUS_HURT= 5;
	public static  int PLAY_STATUS_WIN = 6;

	public static  int ATTACK_TYPE_DEFAULT = 1	;
	public static  int ATTACK_TYPE_HERO = 2;
	public static  int ATTACK_TYPE_ENEMY= 3;
	public static  int ATTACK_TYPE_BOSS = 4;

	public bool isBeAttacker = false;
	public int status = PLAY_STATUS_STANDY;
	public int id = -1;
	public float mBloodVolume = 0;
	public float mRunSpeed;
	public float mAttackLeng = 1;
	public float mDieGas = 0;
	public float mDieCrysta = 0;
	public int mAttackType =ATTACK_TYPE_DEFAULT;

    public Attribute mAttribute = new Attribute();
    
    public LocalBean mLocalBean;
	public List<Attacker> mAttackerTargets;
	public ResourceBean resourceData;

    public AttackSpeedBean mSpeedBean;
    public AnimationEvent mStandEvent;
    public AnimationEvent mFightEvent;
    public void upDataSpeed() {
        if (resourceData == null) {
            return;
        }
        mSpeedBean = AttackSpeedBean.GetAttackSpeed(resourceData, mAttribute.attackSpeed);
        if (mStandEvent != null) {
            Debug.Log(" mStandEvent.time 1=" + mStandEvent.time);
            mStandEvent.time = mSpeedBean.leng;
            Debug.Log(" mStandEvent.time 2=" + mStandEvent.time);
        }
        if (mFightEvent != null) {
            Debug.Log(" mFightEvent.time 1=" + mFightEvent.time);
            mFightEvent.time = mSpeedBean.leng * (resourceData.attack_framce / resourceData.attack_all_framce);
            Debug.Log(" mFightEvent.time 2=" + mFightEvent.time);
        }
        mWaitAttackTime = mSpeedBean.interval;

    }
    public abstract float BeAttack (HurtStatus status);

	public BackgroundManager mBackManager;
	public FightManager mFightManager;
	public void toString(string er){
		Debug.Log(er+":\n mAggressivity "+ mAttribute.aggressivity +
			" mDefense = "+ mAttribute.defense +
			" mBloodVolume = "+mBloodVolume+
			" mMaxBloodVolume = "+ mAttribute.maxBloodVolume +
			" mAttackSpeed = "+ mAttribute.attackSpeed +
			" mRunSpeed = "+mRunSpeed);
	}

	void Start () {
	}


	public void attack(){
		float attackeBlood = mFightManager.attackerAction (id);
	}
	public void attackSync(float blood){
		
	}
	public void changeAnim(){
		Debug.Log ("OnTriggerEnter2D enemyStatus= "+status);
		_anim.SetInteger ("status", status);
	}

	public void Standy(){
		status = PLAY_STATUS_STANDY;
		_anim.SetInteger ("status", status);
	}
	public void Fight(){
		status = PLAY_STATUS_FIGHT;
		_anim.SetInteger ("status", status);

	}
	public void Die(){
		status = PLAY_STATUS_DIE;
		_anim.SetInteger ("status", status);
	}
	public void Run(){
		status = PLAY_STATUS_RUN;
		_anim.SetInteger ("status", status);
	}
	public void Hurt(){
		status = PLAY_STATUS_HURT;
		_anim.SetInteger ("status", status);
	}
	public void Win(){
		status = PLAY_STATUS_WIN;
		_anim.SetInteger ("status", status);
	}
	public float mWaitAttackTime = 0;
}

