using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : Attacker {	
	// Update is called once per frame
	private EnemyState mState;
	void Start () {
		_anim = gameObject.GetComponent<Animator> ();
		mBackManager = GameObject.Find ("Manager").GetComponent<LevelManager> ().getBackManager ();
		mFightManager = GameObject.Find ("Manager").GetComponent<LevelManager> ().getFightManager (); 
		mLocalBean = new LocalBean (transform.position.x, transform.position.y,mAttackLeng,false,this);
		mFightManager.registerAttacker (this);
		RuntimeAnimatorController rc = _anim.runtimeAnimatorController;
		AnimationClip[] cls = rc.animationClips;
		foreach(AnimationClip cl in cls){
			if (cl.name.Equals ("Dead")) {
				//isAddEvent = true;
				AnimationEvent event1 = new AnimationEvent ();
				event1.functionName = "deadEvent";
				event1.time = cl.length-0.1f;
				cl.AddEvent (event1);
			} else if (cl.name.Equals ("Attack")) {
                //isAddEvent = true;
                mStandEvent = new AnimationEvent();
                mStandEvent.functionName = "standyEvent";
                cl.AddEvent(mStandEvent);
                mFightEvent = new AnimationEvent();
                mFightEvent.functionName = "fightEvent";
                // Debug.Log("set fightEvent resourceData.attack_frame =" + resourceData.attack_framce);
                //Debug.Log("set fightEvent resourceData.attack_all_framce =" + resourceData.attack_all_framce);
                // Debug.Log("set fightEvent time =" + event2.time);
                cl.AddEvent(mFightEvent);
            } 
		}
		Run ();
	}
   // private float timeTest = 0;
    private bool isFighted = false;
    public void fightEvent() {
        
        if (status == Attacker.PLAY_STATUS_DIE || isFighted)
        {
            return;
        }
      //  Debug.Log("fightEvent time =" + timeTest);
        isFighted = true;
       // Debug.Log("fightEvent");
        mFightManager.attackerAction(id);
    }

	public void deadEvent(){
		Destroy (gameObject, 1);
	}
	public void standyEvent(){
      //  Debug.Log("standyEvent time="+ timeTest);
        if (status == Attacker.PLAY_STATUS_DIE) {
			return;
		}

        //Debug.Log ("standyEvent");
        //Debug.Log("end fight");
        mWaitAttackTime = 0;
        Standy ();
	}
	private bool isAddEvent = false;
	private bool isAddFightEvent = false;
	void Update () {
  //      timeTest += Time.deltaTime;

        run ();
        mWaitAttackTime += Time.deltaTime;
		if (status == Attacker.PLAY_STATUS_STANDY) {
			if (mWaitAttackTime >= mSpeedBean.interval) {
//                Debug.Log("Start fight");
    //            timeTest = 0;
                Fight ();
				mFightManager.attackerAction (id);
			}	
		}
	}
	private float xy = 0;
	public void run(){
		
		mLocalBean.mCurrentX = transform.position.x;
		mLocalBean.mCurrentY = transform.position.y;

		if ( mBackManager.isRun) {
			transform.Translate (Vector2.left * (mBackManager.moveSpeed * Time.deltaTime));
			mState.Update ();
		}
		if (status != Attacker.PLAY_STATUS_RUN && mLocalBean.mTargetX == -9999  && mLocalBean.mTargetY == -9999) {
			return;
		}
		float x;
		float y = 0;

		if(mLocalBean.mTargetX != -9999){
			if(mLocalBean.mCurrentX < mLocalBean.mTargetX){
				//x = mLocalBean.mCurrentX -  mLocalBean.mTargetX;
				y = mLocalBean.mTargetY - mLocalBean.mCurrentY;
				//transform.Translate (Vector2.left *(x+bgx));
				transform.Translate (Vector2.up * y);

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
		mState.Update ();
		//if (y != 0) {
			//transform.Translate (Vector2.down *y);
		//}
	}
	public Enemy mData;
	public void init(Enemy data,ResourceBean resource){
		resourceData= resource;
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
		mBloodVolume = mBloodVolume - status.blood;
		if (mBloodVolume <= 0) {
			Die ();
			mFightManager.unRegisterAttacker (this);
		}
		mState.hurt (status);
		return status.blood;
	}
}
