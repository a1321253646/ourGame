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
				AnimationEvent event1 = new AnimationEvent ();
				event1.functionName = "standyEvent";
				event1.time = cl.length-0.1f;
				cl.AddEvent (event1);
			} 
		}
		Run ();
	}
	public void deadEvent(){
		Destroy (gameObject, 1);
	}
	public void standyEvent(){
		if (status == Attacker.PLAY_STATUS_DIE) {
			return;
		}

	//	Debug.Log ("standyEvent");
		Standy ();
	}
	private bool isAddEvent = false;
	private bool isAddFightEvent = false;
	void Update () {
		run ();
		mAttackTime += Time.deltaTime;
		if (status == Attacker.PLAY_STATUS_STANDY) {
			if (mAttackTime >= mAttackSpeed) {
				Debug.Log("hurt");
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
		if (y != 0) {
			//transform.Translate (Vector2.down *y);
		}
	}
	public void init(Enemy data){
		this.mAggressivity = data.getMonsterAttack();
		this.mDefense = data.getMonsterDefense();
		this.mBloodVolume = data.getMonsterHp();
		mMaxBloodVolume = mBloodVolume;
		this.mRunSpeed = data.getMonsterSpeed();
		this.mAttackSpeed = data.getAttackSpeed();
		mAttackLeng = data.getAttackRange();
		mDieGas = data.getDieGas ();
		mDieCrysta = 100;//data.getDieCrystal ();
		toString ("enemy");
		mState = new EnemyState (this);
	}

	public int dieGas = 0;
	public int dieCrystal = 0;

	public override int BeAttack(int blood){
		mBloodVolume = mBloodVolume - blood;
		if (mBloodVolume <= 0) {
			Die ();
			mFightManager.unRegisterAttacker (this);
		}
		mState.hurt (blood);
		return blood;
	}
}
