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
		mAttackSpeed = 3;
		mMaxBloodVolume = hero.getRoleHp ();
		mBloodVolume = GameManager.getIntance ().mCurrentBlood;
		toString ("Play");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	private void OnTriggerEnter2D(Collider2D collision){
		if (collision.gameObject.tag == "Enemy") {
			status = PLAY_STATUS_FIGHT;
			mBackManager.stop ();
			changeAnim ();
		}
	}
	void run(){
		transform.Translate (new Vector2 (1, 0)*(mRunSpeed-mBackManager.moveSpeed)*Time.deltaTime);
	}
	public override int BeAttack(int blood){
		return -1;
	}
}
