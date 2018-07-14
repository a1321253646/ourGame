using UnityEngine;
using System.Collections;

public class TrajectoryBase : MonoBehaviour
{
	public FightManager mFightManager;
	public BackgroundManager mBackManager;
	FightResource mFightResource;
	public float speed = 2.0f;
	public bool isRUn = false;
	Attacker mAttacker;


	// Use this for initialization

	public void startRun(Attacker attacker,BackgroundManager back,FightResource fight){
		mBackManager = back;
		mAttacker = attacker;
		mFightResource = fight;
		isRUn = true;
	}

	// Update is called once per frame
	void Update ()
	{
		if (isRUn)
			return;
		if (mBackManager.isRun) {
			transform.Translate (Vector2.left * (mBackManager.moveSpeed + speed) * Time.deltaTime);
		} else {
			transform.Translate (Vector2.left *  speed * Time.deltaTime);
		}
		isReach ();
	}
	private void isReach(){
		if (transform.position.x <= mAttacker.transform.position.x) {
			mFightResource.trajectoryActionIsEnd ();
			Destroy (this);
		} 
	}
}

