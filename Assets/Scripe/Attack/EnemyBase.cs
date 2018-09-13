﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : Attacker {	
	// Update is called once per frame
	private EnemyState mState;
    private float mDestroyTime = 0.5f;
	void Start () {
        startComment();
        mBackManager = GameObject.Find ("Manager").GetComponent<LevelManager> ().getBackManager ();
		mFightManager = GameObject.Find ("Manager").GetComponent<LevelManager> ().getFightManager (); 
		mLocalBean = new LocalBean (transform.position.x, transform.position.y,mAttackLeng,false,this);
		mFightManager.registerAttacker (this);
        Run ();
	}
    // private float timeTest = 0;
    private void initAnimalEvent()
    {
        mSpriteRender = gameObject.GetComponent<SpriteRenderer>();
        mAnimalControl = new AnimalControlBase(resourceData, mSpriteRender);
        mAnimalControl.setStatueDelayStatue(ActionFrameBean.ACTION_ATTACK, ActionFrameBean.ACTION_STANDY);
        mAnimalControl.addIndexCallBack(ActionFrameBean.ACTION_ATTACK, (int)resourceData.attack_framce, new AnimalStatu.animalIndexCallback(fightEcent));
        mAnimalControl.addIndexCallBack(ActionFrameBean.ACTION_DIE, (int)resourceData.attack_framce, new AnimalStatu.animalIndexCallback(dieEcent));
        mAnimalControl.start();
    }
    void fightEcent(int status)
    {
        if (status == ActionFrameBean.ACTION_ATTACK)
        {
            mFightManager.attackerAction(id);
        }
    }
    void dieEcent(int status)
    {
        if (status == ActionFrameBean.ACTION_DIE)
        {
            Destroy(gameObject, 1);
        }
    }

    private bool isFighted = false;
	private bool isAddEvent = false;
	private bool isAddFightEvent = false;
    private float mTime = 0;
	void Update () {
        //      timeTest += Time.deltaTime;
        run ();
        mAnimalControl.update();
        mSkillManager.upDate();
    }
	private float xy = 0;
	public void run(){
		
		mLocalBean.mCurrentX = transform.position.x;
		mLocalBean.mCurrentY = transform.position.y;

		if ( mBackManager.isRun) {
			transform.Translate (Vector2.left * (mBackManager.moveSpeed * Time.deltaTime));
            mSkillManager.upDateLocal(mBackManager.moveSpeed * Time.deltaTime, 0);
            mState.Update ();
		}
		if (getStatus() != Attacker.PLAY_STATUS_RUN && mLocalBean.mTargetX == -9999  && mLocalBean.mTargetY == -9999) {
			return;
		}
		float x;
		float y = 0;

		if(mLocalBean.mTargetX != -9999){
			if(mLocalBean.mCurrentX < mLocalBean.mTargetX){
				y = mLocalBean.mTargetY - mLocalBean.mCurrentY;
				transform.Translate (Vector2.up * y);
                mSkillManager.upDateLocal(0, y);
                xy = 0;
				mLocalBean.mTargetX = -9999;
				mLocalBean.mTargetY = -9999;
				Fight ();
                isFighted = false;
            }
		}


		if (mLocalBean.mTargetX != -9999 && mLocalBean.mTargetY != -9999 && xy == 0) {
			xy = (mLocalBean.mCurrentX - mLocalBean.mTargetX) / (mLocalBean.mCurrentY - mLocalBean.mTargetY);
		}


		x = mRunSpeed * Time.deltaTime;
		if (xy != 0) {
			y = x * xy ;
		}



		transform.Translate (Vector2.left *(x));
        mSkillManager.upDateLocal(x, 0);
        mState.Update ();
		//if (y != 0) {
			//transform.Translate (Vector2.down *y);
		//}
	}
	public Enemy mData;
	public void init(Enemy data,ResourceBean resource){
		resourceData= resource;
        initAnimalEvent();
        mData = data;
        mAttribute.aggressivity = data.monster_attack;
        mAttribute.defense = data.monster_defense;
        mAttribute.maxBloodVolume = data.monster_hp;
        mAttribute.attackSpeed = data.attack_speed;
        mAttribute.rate = data.hit;
        mAttribute.evd = data.dod;
        mAttribute.crt = data.cri;
        mAttribute.crtHurt = data.cri_dam;
        mAttribute.readHurt = data.real_dam;
        mBloodVolume = mAttribute.maxBloodVolume;
        mRunSpeed = data.monster_speed;
        mAttackLeng = data.attack_range;
        mDieGas = data.die_gas;
        mDieCrysta = data.die_crystal;
        
		//toString ("enemy");
		mState = new EnemyState (this);
        upDataSpeed();
	}

	public int dieGas = 0;
	public int dieCrystal = 0;

	public override float BeAttack(HurtStatus status){
        mSkillManager.beforeBeHurt(status);
        mBloodVolume = mBloodVolume - status.blood;
        if (mBloodVolume <= 0) {
			Die ();
			mFightManager.unRegisterAttacker (this);
		}
		mState.hurt (status);
		return status.blood;
	}
    public override float BeKillAttack(long effect, float value)
    {
        if (effect == 1 || effect == 6)
        {
            HurtStatus status = new HurtStatus(value, false, true);
            mBloodVolume = mBloodVolume - status.blood;
            if (mBloodVolume <= 0)
            {
                Die();
                mFightManager.unRegisterAttacker(this);
            }
            mState.hurt(status);
            return status.blood;
        }
        else if (effect == 2 || effect == 10001)
        {
            int tmp = value % 1 == 0 ? 0 : 1;
            value = ((int)value) / 1 + tmp;
            
            if (mBloodVolume + value > mAttribute.maxBloodVolume)
            {
                value = mAttribute.maxBloodVolume - mBloodVolume;
            }
            mBloodVolume = mBloodVolume + value;
            mState.add(value);
        }
        return value;
    }
}
