using UnityEngine;
using System.Collections;

public class FightResource 
{
	private Attacker mAttacker;
	private ResourceBean mFightResource;
	long mTrajectoryId,mHitId;
	private GameObject mTrajectObj, mHitObj;
	private HurtStatus mHurtBlood;
    public ResourceBean mHitResource, mTrajectResource;
    EnemyFactory mFactory;
    public FightResource(Attacker attacker,EnemyFactory factory){
        mFactory = factory;
        mAttacker = attacker;
		mFightResource = mAttacker.resourceData;
        if (mAttacker is EnemyBase)
        {
            mTrajectoryId = ((EnemyBase)mAttacker).mData.trajectory_resource;
            mHitId = ((EnemyBase)mAttacker).mData.hit_resource;
            mHitResource = JsonUtils.getIntance().getEnemyResourceData(mHitId);
            mTrajectResource = JsonUtils.getIntance().getEnemyResourceData(mTrajectoryId);
        }
        else if(mAttacker is PlayControl)
        {
            mTrajectoryId = ((PlayControl)mAttacker).trajectory_resource;
            mHitId = ((PlayControl)mAttacker).hit_resource;
            mHitResource = JsonUtils.getIntance().getEnemyResourceData(mHitId);
            mTrajectResource = JsonUtils.getIntance().getEnemyResourceData(mTrajectoryId);
        }
	}
	public HurtStatus hurt(HurtStatus blood){
		mHurtBlood = blood;
		if (!creatTrajectObj ()) {
			if (!creatHit ()) {
				return mHurtBlood;
			}
			return null;
		}
		return null;
	}

	private bool creatTrajectObj(){
		if (mTrajectoryId != 0) {
			GameObject newobj =  GameObject.Instantiate (
				mFactory.Effect, new Vector2 (mAttacker.transform.position.x+mAttacker.resourceData.getFightOffset().x-mTrajectResource.getFightOffset().x
                , mAttacker.transform.position.y+ mAttacker.resourceData.getFightOffset().y - mTrajectResource.getFightOffset().y),Quaternion.Euler(0.0f,0f,0.0f));
            newobj.AddComponent<TrajectoryBase>();
            TrajectoryBase mbase = newobj.GetComponent<TrajectoryBase> ();
            mbase.setId(mTrajectoryId);
            mbase.startRun (mAttacker, mAttacker.mBackManager, this);
			return true;
		}
		return false;
	}

	private bool creatHit(){
		if (mHitId != 0 && mAttacker.mAttackerTargets.Count > 0) {
            Attacker attacker = mAttacker.mAttackerTargets[0];
            GameObject newobj =  GameObject.Instantiate (
                mFactory.Effect, new Vector2 (attacker.transform.position.x+attacker.resourceData.getHurtOffset().x- mHitResource.getHurtOffset().x,
                attacker.transform.position.y+attacker.resourceData.getHurtOffset().y - mHitResource.getHurtOffset().y
                ),Quaternion.Euler(0.0f,0f,0.0f));
            newobj.AddComponent<HitBase>();
            HitBase mbase = newobj.GetComponent<HitBase> ();
            mbase.setId(mHitId);
            mbase.startRun(this);
			return true;
		}
		return false;
	}

	public void trajectoryActionIsEnd(){
        if (!creatHit() && mAttacker.mAttackerTargets.Count > 0)
        {
            mAttacker.attackSync(mHurtBlood.blood);
            mAttacker.mAttackerTargets[0].BeAttack(mHurtBlood , mAttacker);
            
            mAttacker.mAttackerTargets[0].mSkillManager.mEventAttackManager.endBeHurt(mAttacker, mHurtBlood);
            if (mAttacker.mAttackerTargets.Count > 0) {
                mAttacker.mSkillManager.mEventAttackManager.endHurt(mHurtBlood, mAttacker.mAttackerTargets[0]);
            }
            
        }
	}
	public void hurt(){
		if (mAttacker.mAttackerTargets.Count > 0) {
			mAttacker.mAttackerTargets [0].BeAttack (mHurtBlood , mAttacker);
			mAttacker.attackSync (mHurtBlood.blood);
            
            if (mAttacker.mAttackerTargets.Count > 0) {
                mAttacker.mAttackerTargets[0].mSkillManager.mEventAttackManager.endBeHurt(mAttacker, mHurtBlood);
                mAttacker.mSkillManager.mEventAttackManager.endHurt(mHurtBlood, mAttacker.mAttackerTargets[0]);
            }
           
        }
	}
}

