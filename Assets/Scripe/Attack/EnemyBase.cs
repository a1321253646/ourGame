using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : Attacker {	
	// Update is called once per frame
	private EnemyState mState;
    private float mDestroyTime = 0.5f;

    private  Vector2 mTarger = new Vector2(-9999,-9999) ;

    public void setTarget(Vector2 v)
    {
        mTarger = v;
        mTarger.x = mTarger.x + mAttackLeng;
        mTarger.y = mTarger.y - resourceData.idel_y;
//        Debug.Log("mTarger x = " + mTarger.x + "  mTarger y =" + mTarger.y);
    }

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

    public void endDie()
    {
        setStatus(ActionFrameBean.ACTION_DIE);
        mState.delectBlood();
    }

    private float xy = 0;
	public void run(){
		
		mLocalBean.mCurrentX = transform.position.x;
		mLocalBean.mCurrentY = transform.position.y;
        float speedX = 0;
		if ( mBackManager.isRun) {
            speedX = mBackManager.moveSpeed;
         //   transform.Translate (Vector2.left * (mBackManager.moveSpeed * Time.deltaTime));
         //   mSkillManager.upDateLocal(mBackManager.moveSpeed * Time.deltaTime, 0);
         //   mState.Update ();
		}
		if ((getStatus() != Attacker.PLAY_STATUS_RUN && mLocalBean.mTargetX == -9999  && mLocalBean.mTargetY == -9999)||
            (getStatus() != Attacker.PLAY_STATUS_RUN && mTarger.x == -9999 && mTarger.y == -9999)) {
               transform.Translate (Vector2.left * (speedX * Time.deltaTime));
               mSkillManager.mEventAttackManager.upDateLocal(speedX * Time.deltaTime, 0);
               mState.Update ();
            return;
		}
		float x = 0;
		float y = 0;

        if (mLocalBean.mTargetX != -9999)
        {
            mTarger.x = -9999;
            mTarger.y = -9999;
            if (mLocalBean.mCurrentX < mLocalBean.mTargetX)
            {
                y = mLocalBean.mTargetY - mLocalBean.mCurrentY;
                transform.Translate(Vector2.up * y);
                mSkillManager.mEventAttackManager.upDateLocal(0, y);
                xy = 0;
                mLocalBean.mTargetX = -9999;
                mLocalBean.mTargetY = -9999;
                Fight();
                isFighted = false;
            }
        }
        else if ((mTarger.x != -9999)) {
            if (mLocalBean.mCurrentX < mTarger.x)
            {
                y = mTarger.y - mLocalBean.mCurrentY;
                transform.Translate(Vector2.up * y);
                mSkillManager.mEventAttackManager.upDateLocal(0, y);
                xy = 0;
                mTarger.x = -9999;
                mTarger.y = -9999;
            //    Fight();
                isFighted = false;
            }
        }


        
        if (mLocalBean.mTargetX != -9999 && mLocalBean.mTargetY != -9999)
        {
            if (mLocalBean.mCurrentX - mLocalBean.mTargetX == 0)
            {
                xy = 0;
            }
            else {
                xy = (mLocalBean.mCurrentY - mLocalBean.mTargetY) / (mLocalBean.mCurrentX - mLocalBean.mTargetX);
            }
                         
        }
        else if (mTarger.x != -9999 && mTarger.y != -9999 )
        {
            if (mLocalBean.mCurrentX - mTarger.x == 0)
            {
                xy = 0;
            }
            else {
                xy = (mLocalBean.mCurrentY - mTarger.y) / (mLocalBean.mCurrentX - mTarger.x);
            }
            
        }


		x = (mRunSpeed+ speedX) * Time.deltaTime;
		if (xy != 0) {
			y = x * xy ;
		}



		transform.Translate (Vector2.left *(x));
        if (y != 0)
        {
            transform.Translate(Vector2.down * y);
        }
        mSkillManager.mEventAttackManager.upDateLocal(x, 0);
        mState.Update ();

	}
	public Enemy mData;
	public void init(Enemy data,ResourceBean resource){
		resourceData= resource;
        initAnimalEvent();
        mData = data;
        mBaseAttribute.aggressivity = data.monster_attack;
        mBaseAttribute.defense = data.monster_defense;
        mBaseAttribute.maxBloodVolume = data.monster_hp;
        mBaseAttribute.attackSpeed = data.attack_speed;
        mBaseAttribute.rate = data.hit;
        mBaseAttribute.evd = data.dod;
        mBaseAttribute.crt = data.cri;
        mBaseAttribute.crtHurt = data.cri_dam;
        mBaseAttribute.readHurt = data.real_dam;

        mAttribute.clear();
        mAttribute.add(mBaseAttribute);

        mBloodVolume = mAttribute.maxBloodVolume;
        mRunSpeed = data.monster_speed;
        mAttackLeng = data.attack_range;
        mDieGas = data.die_gas;
        mDieCrysta = data.getDieCrystal();
        
		//toString ("enemy");
		mState = new EnemyState (this);
        upDataSpeed();
	}

	public int dieGas = 0;
	public int dieCrystal = 0;

	public override double BeAttack(HurtStatus status,Attacker hurter)
    {
//        Debug.Log("===============EnemyBase hurt =" + status.blood);
   //     mSkillManager.mEventAttackManager.beforeBeHurt(status);
//        Debug.Log(" status.blood == " + status.blood);
        status.blood = status.blood * hurter.mSkillManager.getHurtPre();
//        Debug.Log("===============EnemyBase hurt =" + status.blood);
        //        Debug.Log(" status.blood == " + status.blood);
        //int tmp = status.blood % 1 == 0 ? 0 : 1;
        //status.blood = status.blood / 1 + tmp;
//        Debug.Log("===============EnemyBase hurt =" + status.blood);
        //if (mBloodVolume == mAttribute.maxBloodVolume) {
        mBloodVolume = mBloodVolume - status.blood;
        //}
        
        if (mBloodVolume <= 0) {
			Die ();
			mFightManager.unRegisterAttacker (this);
		}
		mState.hurt (status);
		return status.blood;
	}
    public override double BeKillAttack( double value,Attacker hurt)
    {
//        if (effect == 1 || effect == 6 || effect == 30001 || effect == 8 || effect == 3)
//        {
            HurtStatus status = new HurtStatus(value, false, true);
            if (hurt != null) {
                Debug.Log(" status.blood == " + status.blood);
                status.blood = status.blood * hurt.mSkillManager.getCardHurtPre();
                Debug.Log("  hurt.mSkillManager.getCardHurtPre() == " + hurt.mSkillManager.getCardHurtPre());
//                int tmp = status.blood % 1 == 0 ? 0 : 1;
//                status.blood = ((int)status.blood) / 1 + tmp;
                Debug.Log(" status.blood == " + status.blood);
            }
            mBloodVolume = mBloodVolume - status.blood;
            if (mBloodVolume <= 0)
            {
                Die();
                mFightManager.unRegisterAttacker(this);
            }
            mState.hurt(status);
            return status.blood;
 //       }
  //      return value;
    }
    public override void AddBlood(double value)
    {
        if (mBloodVolume + value > mAttribute.maxBloodVolume)
        {
            value = mAttribute.maxBloodVolume - mBloodVolume;
        }
        mBloodVolume = mBloodVolume + value;
        mState.add(value); ;
    }

    public override void getAttribute()
    {
        double bloodBili = mBloodVolume / mAttribute.maxBloodVolume;

        mAttribute.clear();
        mAllAttribute.clear();
        mAllAttributePre.setToPre();

        mAllAttribute.add(mBaseAttribute);
        mAllAttribute.add(mEquipAttribute);
        mAllAttribute.add(mLunhuiAttribute);
        mAllAttribute.add(mSkillAttribute);

        mAllAttributePre.add(mEquipAttributePre);
        mAllAttributePre.add(mLunhuiAttributePre);
        mAllAttributePre.add(mSkillAttributePre);


        mAttribute.add(mAllAttribute);
        //Debug.Log("===============mAttribute.mAttribute = " + mAllAttribute);
        mAttribute.chen(mAllAttributePre);
        mBloodVolume = mAttribute.maxBloodVolume * bloodBili;
        upDataSpeed();
        mState.resetHp();
    }
}
