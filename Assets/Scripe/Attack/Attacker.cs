using System.Collections;
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
	public int mAggressivity;
	public int mDefense;
	public int mBloodVolume;
	public int mMaxBloodVolume;
	public float mAttackSpeed;
	public float mRunSpeed;
	public float mAttackLeng = 1;
	public int mDieGas = 0;
	public int mDieCrysta = 0;
	public int mAttackType =ATTACK_TYPE_DEFAULT;

	public LocalBean mLocalBean;
	public List<Attacker> mAttackerTargets;
	public ResourceBean resourceData;

	public abstract int BeAttack (int blood);

	public BackgroundManager mBackManager;
	public FightManager mFightManager;
	public void toString(string er){
		Debug.Log(er+":\n mAggressivity "+mAggressivity+
			" mDefense = "+mDefense+
			" mBloodVolume = "+mBloodVolume+
			" mMaxBloodVolume = "+mMaxBloodVolume+
			" mAttackSpeed = "+mAttackSpeed+
			" mAttackSpeed = "+mAttackSpeed+
			" mRunSpeed = "+mRunSpeed);
	}

	void Start () {
	}


	public void attack(){
		int attackeBlood = mFightManager.attackerAction (id);
	}
	public void attackSync(int blood){
		
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
		mAttackTime = 0;
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
	public float mAttackTime = 0;
}

