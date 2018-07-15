using UnityEngine;
using System.Collections;

public class FightResource 
{
	private Attacker mAttacker;
	private ResourceBean mFightResource;
	long mTrajectoryId,mHitId;
	private GameObject mTrajectObj, mHitObj;
	private float mHurtBlood;
    public ResourceBean mHitResource, mTrajectResource;
    EnemyFactory mFactory;
    public FightResource(Attacker attacker,EnemyFactory factory){
        mFactory = factory;
        mAttacker = attacker;
		mFightResource = mAttacker.resourceData;
		if (mAttacker is EnemyBase) {
			mTrajectoryId = ((EnemyBase)mAttacker).mData.trajectory_resource;
			mHitId = ((EnemyBase)mAttacker).mData.hit_resource;
            mHitResource = JsonUtils.getIntance().getEnemyResourceData(mHitId);
            mTrajectResource = JsonUtils.getIntance().getEnemyResourceData(mTrajectoryId);

        }
	}
	public float hurt(float blood){
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
		if (mTrajectoryId != 0) {
			long id = mTrajectoryId- 30001;
			GameObject newobj =  GameObject.Instantiate (
				mFactory.Effect[id], new Vector2 (mAttacker.transform.position.x+mAttacker.resourceData.getFightOffset().x+mTrajectResource.getFightOffset().x
                , mAttacker.transform.position.y+ mAttacker.resourceData.getFightOffset().y + mTrajectResource.getFightOffset().y),Quaternion.Euler(0.0f,0f,0.0f));
			TrajectoryBase mbase = newobj.GetComponent<TrajectoryBase> ();
			mbase.startRun (mAttacker, mAttacker.mBackManager, this);
			return true;
		}
		return false;
	}

	private bool creatHit(){
		if (mHitId != 0 && mAttacker.mAttackerTargets.Count > 0) {
			long id = mHitId - 30001;
            Attacker attacker = mAttacker.mAttackerTargets[0];

            GameObject newobj =  GameObject.Instantiate (
                mFactory.Effect[id], new Vector2 (attacker.transform.position.x+attacker.resourceData.getHurtOffset().x+ mHitResource.getHurtOffset().x,
                attacker.transform.position.y+attacker.resourceData.getHurtOffset().y + mHitResource.getHurtOffset().y
                ),Quaternion.Euler(0.0f,0f,0.0f));
			HitBase mbase = newobj.GetComponent<HitBase> ();
			mbase.startRun (this);
			return true;
		}
		return false;
	}

	public void trajectoryActionIsEnd(){
        if (!creatHit())
        {
            mAttacker.attackSync(mHurtBlood);
            mAttacker.mAttackerTargets[0].BeAttack(mHurtBlood);
        }
	}
	public void hurt(){
		if (mAttacker.mAttackerTargets.Count > 0) {
			mAttacker.mAttackerTargets [0].BeAttack (mHurtBlood);
			mAttacker.attackSync (mHurtBlood);
		}
	}
}

