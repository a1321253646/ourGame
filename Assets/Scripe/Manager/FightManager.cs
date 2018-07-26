using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightManager{

	private int id = 0;
	private LocalManager mLocalManager;
    public EnemyFactory mEnemyFactory;
    public int mHeroStatus = Attacker.PLAY_STATUS_RUN;

	Dictionary<int,Attacker> mAliveActtackers = new Dictionary<int,Attacker>();
	public bool isEmptyEnemy(){
		//Debug.Log ("mAliveActtackers.Count =" + mAliveActtackers.Count);
		if (mAliveActtackers.Count < 3) {
			return true;
		} else {
			return false;
		}
	}
	public void setLoaclManager(LocalManager local){
		mLocalManager = local;
	}

	public void registerAttacker(Attacker attcker){
		if (attcker.id != -1 || mAliveActtackers.ContainsKey(attcker.id)) {
			Debug.Log ("registerAttacker:this attcker is register");
			return;
		}
		attcker.id = id;
		id++;
		mAliveActtackers.Add (attcker.id, attcker);
		if (attcker.mLocalBean.mIsHero) {
			mLocalManager.setHeroLoacl (attcker.mLocalBean);
		} else {
			mLocalManager.addAttack (attcker.mLocalBean);
		}
	}
	public void unRegisterAttacker(Attacker attcker){
		if (attcker.mAttackType == Attacker.ATTACK_TYPE_BOSS ) {
			GameManager.getIntance ().mHeroIsAlive = true;
			GameManager.getIntance ().mCurrentLevel += 1;
            GameManager.getIntance().enemyDeal(attcker);
            //    SceneManager.UnloadSceneAsync (0);    
            SceneManager.LoadScene(1);
            
		}else if(attcker.mAttackType == Attacker.ATTACK_TYPE_HERO){
			GameManager.getIntance ().mHeroIsAlive = false;
			//SceneManager.UnloadSceneAsync (0);
			SceneManager.LoadScene(1);
		}
		if (attcker.id == -1 || mAliveActtackers.Count < 1) {
			Debug.Log ("unRegisterAttacker:this attcker is not register");
			return;
		}
        
        foreach (Attacker tmp in mAliveActtackers.Values)
        {
            if (tmp.mAttackerTargets != null && tmp.mAttackerTargets.Count > 0)
            {
                int index = tmp.mAttackerTargets.IndexOf(attcker);
                if (index >= 0)
                {
                    tmp.mAttackerTargets.RemoveAt(index);
                }
            }
        }
        mLocalManager.EnemyDeal(attcker);
        mAliveActtackers.Remove(attcker.id);
      
		if (attcker is EnemyBase) {
			GameManager.getIntance ().enemyDeal (attcker);
		}

	}

/*	public void addAttacker(int myId,int beAttackerid){
		
		if (!mAliveActtackers.ContainsKey (myId) || !mAliveActtackers.ContainsKey (beAttackerid)) {
			return ;
		}

		Attacker my = mAliveActtackers [myId];
		Attacker beAttacker = mAliveActtackers [beAttackerid];
		if (my.mAttackerTargets == null) {
			my.mAttackerTargets = new List<Attacker> ();
		}
		my.mAttackerTargets.Add (beAttacker);
	}*/
	public float attackerAction(int id){
		float hurtBloodAll = 0;
		float hurtBlood = 0;
		Attacker attacker = getAttackerById (id);
		if (attacker.mAttackerTargets != null && attacker.mAttackerTargets.Count > 0) {
			if (attacker is EnemyBase) {//enemy only one target
				FightResource resouce = new FightResource (attacker, mEnemyFactory);
                hurtBlood = attackBllod(attacker, attacker.mAttackerTargets[0]);
                hurtBlood = resouce.hurt(hurtBlood);
                if (hurtBlood != 0) {
                    attacker.mAttackerTargets[0].BeAttack(hurtBlood);
                }
                return hurtBlood;
            }
			for (int i = 0; i < attacker.mAttackerTargets.Count; i++) {
				Attacker tager = attacker.mAttackerTargets [i];
				hurtBlood = attackBllod (attacker, tager);
				tager.BeAttack (hurtBlood);
				hurtBloodAll += hurtBlood;
			}
			return hurtBloodAll;
		}

		return 0;
	}
	private Attacker getAttackerById(int id){
		return mAliveActtackers [id];
	}
	private float attackBllod(Attacker attacker,Attacker beAttacker){
		//Debug.Log (" attacker.mAggressivity  =" + attacker.mAggressivity + " beAttacker.mDefense=" + beAttacker.mDefense);
		float hurt= attacker.mAggressivity - beAttacker.mDefense;
		if (hurt <= attacker.mAggressivity/10) {
            int tmp = attacker.mAggressivity % 10 == 0 ? 0 : 1;
            hurt = ((int)attacker.mAggressivity )/ 10+ tmp;
		}
		return hurt;
	}
}
