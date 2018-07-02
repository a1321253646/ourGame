using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : Attacker {	
	// Update is called once per frame

	void Start () {
		_anim = gameObject.GetComponent<Animator> ();
		mBackManager = GameObject.Find ("Manager").GetComponent<LevelManager> ().getBackManager ();
		mFightManager = GameObject.Find ("Manager").GetComponent<LevelManager> ().getFightManager (); 
	}

	void Update () {
		run ();
	}
	public void run(){
		if (mBackManager.isRun) {
			transform.Translate (Vector2.right *( mBackManager.moveSpeed+mRunSpeed) * Time.deltaTime);
		}
	}
	private void OnTriggerEnter2D(Collider2D collision){
		if (collision.gameObject.tag == "Hero") {
			Debug.Log ("OnTriggerEnter2D");
			status = PLAY_STATUS_FIGHT;
			changeAnim ();
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
		return 0;
	}
}
