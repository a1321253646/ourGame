using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayControl : Attacker 
{

	// Use this for initialization
	void Start () {
		mBackManager = GameObject.Find ("Manager").GetComponent<LevelManager> ().getBackManager ();
		mFightManager = GameObject.Find ("Manager").GetComponent<LevelManager> ().getFightManager (); 
		_anim = gameObject.GetComponent<Animator> ();
		mBackManager.setBackground ("map/map_03");
		Hero hero = JsonUtils.getIntance ().getHeroData ();
		mAggressivity = hero.getRoleAttack ();
		mDefense = hero.getRoleDefense ();
		mAttackSpeed =1;
		mMaxBloodVolume = hero.getRoleHp ();
		mBloodVolume = GameManager.getIntance ().mCurrentBlood;
		toString ("Play");
		//mAttackLeng = 1;
		mLocalBean = new LocalBean (transform.position.x, transform.position.y,mAttackLeng,true,this);
		mFightManager.registerAttacker (this);
		mFightManager.mHeroStatus = Attacker.PLAY_STATUS_RUN;

		Run ();
	}
	
	// Update is called once per frame
	private float mTime = 0; 
	void Update () {
		if (status != mFightManager.mHeroStatus) {
			status = mFightManager.mHeroStatus;
			if (status == Attacker.PLAY_STATUS_FIGHT) {
				Fight ();
				mBackManager.stop ();
			}
			if (status == Attacker.PLAY_STATUS_RUN ) {
				Run ();
				mBackManager.move ();
			}
		}
		if (status == Attacker.PLAY_STATUS_FIGHT) {
			mTime += Time.deltaTime;
			if (mTime >= mAttackSpeed) {
				Debug.Log("hurt");
				mTime = 0;
				mFightManager.attackerAction (id);
			}		
		}
	}
	void run(){
		transform.Translate (new Vector2 (1, 0)*(mRunSpeed-mBackManager.moveSpeed)*Time.deltaTime);
	}
	public override int BeAttack(int blood){
		return -1;
	}
}
