using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : Attacker {	
	// Update is called once per frame

	void Start () {
		_anim = gameObject.GetComponent<Animator> ();
		mBackManager = GameObject.Find ("Manager").GetComponent<LevelManager> ().getBackManager ();
		mFightManager = GameObject.Find ("Manager").GetComponent<LevelManager> ().getFightManager (); 
		mLocalBean = new LocalBean (transform.position.x, transform.position.y,mAttackLeng,false,this);
		mFightManager.registerAttacker (this);
		Run ();
	}
	public void deadEvent(){
		Destroy (gameObject, 1);
	}

	private bool isAddEvent = false;
	void Update () {
		run ();
		if (!isAddEvent && status == Attacker.PLAY_STATUS_DIE) {
			AnimatorClipInfo[] infos = _anim.GetCurrentAnimatorClipInfo (0);
			foreach (AnimatorClipInfo info in infos) {
				if (info.clip.name.Equals ("Dead")) {
					isAddEvent = true;
					AnimationEvent event1 = new AnimationEvent();
					event1.functionName = "deadEvent";
					event1.time = info.clip.length;
					info.clip.AddEvent (event1);
				}
			}

		}
	}
	private float xy = 0;
	public void run(){
		if (status != Attacker.PLAY_STATUS_RUN && mLocalBean.mTargetX == -9999  && mLocalBean.mTargetY == -9999) {
			return;
		}
		float x;
		float y = 0;
		float bgx= 0;
		mLocalBean.mCurrentX = transform.position.x;
		mLocalBean.mCurrentY = transform.position.y;

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

		if (mBackManager.isRun) {
			bgx = mBackManager.moveSpeed * Time.deltaTime;
			//transform.Translate (Vector2.left *);
		}
		x = mRunSpeed * Time.deltaTime;
		if (xy != 0) {
			y = x * xy ;
		}



		transform.Translate (Vector2.left *(x+bgx));
		if (y != 0) {
			//transform.Translate (Vector2.down *y);
		}
	}
	public void init(Enemy data){
		this.mAggressivity = data.getMonsterAttack();
		this.mDefense = data.getMonsterDefense();
		this.mBloodVolume = data.getMonsterHp();
		this.mRunSpeed = data.getMonsterSpeed();
		this.mAttackSpeed = 3;
		toString ("enemy");
	}

	public int dieGas = 0;
	public int dieCrystal = 0;

	public override int BeAttack(int blood){
		Debug.Log("Behurt");
		mBloodVolume = mBloodVolume - blood;
		if (mBloodVolume <= 0) {
			Die ();
			mFightManager.unRegisterAttacker (this);
		}
		return 0;
	}
}
