using UnityEngine;
using System.Collections;

public class FightResource 
{
	private Attacker mAttacker;
	private ResourceBean mFightResource;
	int mTrajectoryId,mHitId;
	private GameObject mTrajectObj, mHitObj;
	public GameObject[] mTrajectList, mHitList;
	private int mHurtBlood;
	public FightResource(Attacker attacker){
		mAttacker = attacker;
		mFightResource = mAttacker.resourceData;
		if (mAttacker is EnemyBase) {
			mTrajectoryId = ((EnemyBase)mAttacker).mData.getTrajectoryResource ();
			mHitId = ((EnemyBase)mAttacker).mData.getHitResource();
		}
	}
	public int hurt(int blood){
		mHurtBlood = blood;
		if (!creatTrajectObj ()) {
			if (!creatHit ()) {
				return mHurtBlood;
			}
			return 0;
		}
		return 0;
	}

	private bool creatTrajectObj(){
		if (mTrajectoryId != -1) {
			int id = mTrajectoryId;
			GameObject newobj =  GameObject.Instantiate (
				mTrajectList[id], new Vector2 (mAttacker.transform.position.x, mAttacker.transform.position.y),Quaternion.Euler(0.0f,0f,0.0f));
			TrajectoryBase mbase = newobj.GetComponent<TrajectoryBase> ();
			mbase.startRun (mAttacker, mAttacker.mBackManager, this);
			return true;
		}
		return false;
	}

	private bool creatHit(){
		if (mHitId != -1 && mAttacker.mAttackerTargets.Count > 0) {
			int id = mHitId;
			GameObject newobj =  GameObject.Instantiate (
				mTrajectList[id], new Vector2 (mAttacker.mAttackerTargets[0].transform.position.x, mAttacker.mAttackerTargets[0].transform.position.y),Quaternion.Euler(0.0f,0f,0.0f));
			HitBase mbase = newobj.GetComponent<HitBase> ();
			mbase.startRun (this);
			return true;
		}
		return false;
	}

	public void trajectoryActionIsEnd(){
		if (!creatHit ()) {
			mAttacker.attackSync (mHurtBlood);
		}
	}
	public void hurt(){
		if (mAttacker.mAttackerTargets.Count > 0) {
			mAttacker.mAttackerTargets [0].BeAttack (mHurtBlood);
			mAttacker.attackSync (mHurtBlood);
		}
	}
}

