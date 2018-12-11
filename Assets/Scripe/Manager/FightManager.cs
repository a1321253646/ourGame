using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightManager{

	private int id = 0;
	private LocalManager mLocalManager;
    public EnemyFactory mEnemyFactory;
    public int mHeroStatus = Attacker.PLAY_STATUS_RUN;

	public Dictionary<int,Attacker> mAliveActtackers = new Dictionary<int,Attacker>();
	public bool isEmptyEnemy(){
		//Debug.Log ("mAliveActtackers.Count =" + mAliveActtackers.Count);
		if (mAliveActtackers.Count < 2) {
			return true;
		} else {
			return false;
		}
	}
	public void setLoaclManager(LocalManager local){
		mLocalManager = local;
	}

	public void registerAttacker(Attacker attcker){
        if (mAliveActtackers.ContainsKey(attcker.id)) {
            if (attcker.mLocalBean.mIsHero)
            {
                mLocalManager.setHeroLoacl(attcker.mLocalBean);
                return;
            }
        }
        else if (attcker.id != -1) {
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

        if(attcker.mAttackType == Attacker.ATTACK_TYPE_HERO){
			GameManager.getIntance ().mHeroIsAlive = false;
            //SceneManager.UnloadSceneAsync (0);
            GameObject.Find("Manager").GetComponent<LevelManager>().getBackManager().stop();
            foreach (Attacker a in mAliveActtackers.Values)
            {
                if (a is PlayControl)
                {
                    continue;
                }
                ((EnemyBase)a).Standy();
            }
         
            //return;
        }
        if (attcker.id == -1 || mAliveActtackers.Count < 1)
        {
            Debug.Log("unRegisterAttacker:this attcker is not register");
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
        mAliveActtackers.Remove(attcker.id);
        if (attcker is EnemyBase) {
			GameManager.getIntance ().enemyDeal (attcker);
            mLocalManager.EnemyDeal(attcker);
        }
        if (attcker.mAttackType == Attacker.ATTACK_TYPE_BOSS)
        {
            if (GameManager.getIntance().mCurrentLevel == 0)
            {
                GameManager.getIntance().mCurrentLevel = (long)JsonUtils.getIntance().getConfigValueForId(100019) + 1;
                SQLHelper.getIntance().updateGameLevel(GameManager.getIntance().mCurrentLevel);
            }
            else
            {
                GameManager.getIntance().mCurrentLevel += 1;
                SQLHelper.getIntance().updateGameLevel(GameManager.getIntance().mCurrentLevel);
            }
            if (GameManager.getIntance().mCurrentLevel == JsonUtils.getIntance().getConfigValueForId(100017)) {
                GameManager.getIntance().uiManager.setLunhuiPointShow(1);
            }
            LevelManager level = GameObject.Find("Manager").GetComponent<LevelManager>();
            GameManager.getIntance().isEnd = true;
            GameObject.Find("Manager").GetComponent<LevelManager>().getBackManager().stop();
            foreach (Attacker a in mAliveActtackers.Values)
            {
                if (a is PlayControl)
                {
                    continue;
                }
                ((EnemyBase)a).endDie();
            }
            level.mPlayerControl.win();
            GameManager.getIntance().mHeroIsAlive = true;

           
            // return;
        }

    }
    public void dieOrWin(bool isWin,bool isChange)
    {
        if (isWin)
        {
            GameObject.Find("qiehuanchangjing").GetComponent<QieHuangChangJing>().run(isWin ? 1 : 2, isChange);
        }
        else {
            GameObject.Find("qiehuanchangjing").GetComponent<QieHuangChangJing>().run(isWin ? 1 : 2);
        }
        

    }
    public void dieOrWin(bool isWin) {
        dieOrWin(isWin, true);
    }
    public void changeColor() {

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
		HurtStatus hurtBlood = null;
        float hurt = 0;
		Attacker attacker = getAttackerById (id);
		if (attacker!= null && attacker.mAttackerTargets != null && attacker.mAttackerTargets.Count > 0) {
			if (attacker is EnemyBase) {//enemy only one target
				FightResource resouce = new FightResource (attacker, mEnemyFactory);
                hurtBlood = attackBllod(attacker, attacker.mAttackerTargets[0]);
                hurtBlood = resouce.hurt(hurtBlood);
                if (hurtBlood != null) {
                    attacker.mAttackerTargets[0].BeAttack(hurtBlood, attacker);
                    return hurtBlood.blood;

                }
                else {
                    return 0;
                }
                
            }
			for (int i = 0; i < attacker.mAttackerTargets.Count; ) {
                int size = attacker.mAttackerTargets.Count;
                Attacker tager = attacker.mAttackerTargets [i];
				hurtBlood = attackBllod (attacker, tager);
				tager.BeAttack (hurtBlood, attacker);
				hurtBloodAll += hurtBlood.blood;
                i++;
             //   if (size == attacker.mAttackerTargets.Count) {
                    
             //   }
			}
			return hurtBloodAll;
		}

		return 0;
	}

    private Attacker getAttackerById(int id){
        if (mAliveActtackers.ContainsKey(id))
        {
            return mAliveActtackers[id];
        }
        else return null;
		
	}
	private HurtStatus attackBllod(Attacker attacker,Attacker beAttacker){
        if (!isHurt(attacker, beAttacker)) {
            return new HurtStatus(0, false, false);
        }
//        Debug.Log("attackBllod attacker.mAttribute.aggressivity = " + attacker.mAttribute.aggressivity);
//        Debug.Log(" beAttacker.mAttribute.defense = " + beAttacker.mAttribute.defense);
		float hurt= attacker.mAttribute.aggressivity - beAttacker.mAttribute.defense;
		if (hurt <= attacker.mAttribute.aggressivity /10) {
            int tmp = attacker.mAttribute.aggressivity % 10 == 0 ? 0 : 1;
            hurt = ((int)attacker.mAttribute.aggressivity )/ 10+ tmp;
		}
//        Debug.Log(" hurt = " + hurt);
        bool crt = isCrt(attacker);
//        Debug.Log(" crt = " + crt);
        if (crt)
        {
            //        Debug.Log("hurt =" + hurt + " attacker.mCrtHurt=" + attacker.mCrtHurt + " attacker.mReadHurt=" + attacker.mReadHurt);
//            Debug.Log(" attacker.mAttribute.crtHurt = " + attacker.mAttribute.crtHurt);
//            Debug.Log(" attacker.mAttribute.readHurt = " + attacker.mAttribute.readHurt);
//            Debug.Log(" hurt * 2 = " + hurt * 2);
            hurt = hurt * 2 + attacker.mAttribute.crtHurt + attacker.mAttribute.readHurt;
//            Debug.Log(" hurt  = " + hurt);
        }
        else {
//            Debug.Log(" attacker.mAttribute.readHurt = " + attacker.mAttribute.readHurt);
//            Debug.Log(" hurt  = " + hurt);
            hurt = hurt + attacker.mAttribute.readHurt;
//            Debug.Log(" hurt  = " + hurt);
        }
		return new HurtStatus(hurt, crt,true);
	}
    private bool isCrt(Attacker attacker) {
        return randomResult(10000,(int) attacker.mAttribute.crt,false);
    }
    private bool isHurt(Attacker attacker, Attacker beAttacker) {
       // int max =(int) (attacker.mAttribute.rate + beAttacker.mAttribute.evd);
        return !randomResult(10000,(int)beAttacker.mAttribute.evd, false);
    }
    private bool randomResult(int max ,int value,bool isPri) {
        int rangeRadomNum = Random.Range(0, max);
        if (isPri) {
            Debug.Log("fight rangeRadomNum=" + rangeRadomNum + " value=" + value);
        }
        return rangeRadomNum <= value;
    }

}
