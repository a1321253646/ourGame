using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FightManager{

	private int id = 0;
	private LocalManager mLocalManager;
    public EnemyFactory mEnemyFactory;
    public int mHeroStatus = Attacker.PLAY_STATUS_RUN;

	public Dictionary<int,Attacker> mAliveActtackers = new Dictionary<int,Attacker>();

    public void reset() {
        id = 0;
        mHeroStatus = Attacker.PLAY_STATUS_RUN;
        foreach (Attacker a in mAliveActtackers.Values)
        {
            if (a is PlayControl)
            {
                continue;
            }
            ((EnemyBase)a).endDieNoshow();
        }
        mAliveActtackers.Clear();


    }

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
            GameObject.Find("boss_info").GetComponent<BossCardManager>().disShow();
            GameManager.getIntance ().mHeroIsAlive = false;
            GameManager.getIntance().isEnd = true;
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
              if (attcker.mAttackerTargets != null) {
                attcker.mAttackerTargets.Clear();
              }
            if (SQLHelper.getIntance().isLuiHui != -1 && BaseDateHelper.decodeLong(GameManager.getIntance().mCurrentLevel) <= SQLHelper.getIntance().isLuiHui) {
                long deal = SQLHelper.getIntance().isLuiHuiDeal;
                if (deal == -1)
                {
                    SQLHelper.getIntance().updateIsLunhuiDealValue(1);
                }
                else
                {
                    SQLHelper.getIntance().updateIsLunhuiDealValue(deal + 1);
                }
                deal = SQLHelper.getIntance().isLuiHuiDeal;
                if (JsonUtils.getIntance().getConfigValueForId(100033) <= deal)
                {
                    SQLHelper.getIntance().updateIsLunhuiValue(0);
                }
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
            GameObject.Find("boss_info").GetComponent<BossCardManager>().disShow();
            if (BaseDateHelper.decodeLong(GameManager.getIntance().mCurrentLevel) == 0)
            {
                GameManager.getIntance().mCurrentLevel = BaseDateHelper.encodeLong((long)JsonUtils.getIntance().getConfigValueForId(100019) + 1) ;
                SQLHelper.getIntance().updateGameLevel(GameManager.getIntance().mCurrentLevel);
            }
            else
            {
                GameManager.getIntance().mCurrentLevel =BaseDateHelper.encodeLong(BaseDateHelper.decodeLong(GameManager.getIntance().mCurrentLevel)+ 1) ;

                if (BaseDateHelper.decodeLong(GameManager.getIntance().mCurrentLevel) > (long)JsonUtils.getIntance().getConfigValueForId(100042))
                {
                    GameManager.getIntance().mCurrentLevel = BaseDateHelper.encodeLong((long)JsonUtils.getIntance().getConfigValueForId(100042));
                }
                else {
                    if (JsonUtils.getIntance().isNeedReReadAboutLevel(BaseDateHelper.decodeLong(GameManager.getIntance().mCurrentLevel))) {
                        GameManager.getIntance().mIsNeedToReReadAboutLevel = true;
                    }
                }
                SQLHelper.getIntance().updateGameLevel(GameManager.getIntance().mCurrentLevel);
            }
            if (BaseDateHelper.decodeLong(GameManager.getIntance().mCurrentLevel) == JsonUtils.getIntance().getConfigValueForId(100017)) {
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
    public BigNumber attckerOutLine(Attacker hero, long time, float outGet) {
         string str = "";
        Level l = JsonUtils.getIntance().getLevelData(BaseDateHelper.decodeLong(GameManager.getIntance().mCurrentLevel));
        long firstId = JsonUtils.getIntance().getLevelFirstEnemey(l);
           str += "\n=================attckerOutLine  firstId=" + firstId;
        // Debug.Log("=================attckerOutLine  firstId=" + firstId);
        // JsonUtils.getIntance().getWellenEnemy();
        Enemy enemy = JsonUtils.getIntance().getEnemyById(firstId);
        double hurt1 = hero.mAttribute.aggressivity * hero.mAttribute.aggressivity / (hero.mAttribute.aggressivity + enemy.monster_defense);
         double hurt2 = hurt1 * 2 + hero.mAttribute.crtHurt + hero.mAttribute.readHurt;
         hurt1 = hurt1 + hero.mAttribute.readHurt;
         double crtBili = hero.mAttribute.crt / 10000;
         double hurt = hurt2 * crtBili + hurt1*(1- crtBili);
        hurt = hurt * hero.mSkillManager.getHurtPre();
        //double hurt = hurt1 * hero.mSkillManager.getHurtPre();
        int a = (int)(enemy.monster_hp / hurt);
        if (a == 0)
        {
            a = 1;
        }
        else if (a * hurt < enemy.monster_hp) {
            a = a + 1;
        }
          str += "\n=================attckerOutLine  英雄攻击力=" + hero.mAttribute.aggressivity;
        // Debug.Log("=================attckerOutLine  英雄攻击力=" + hero.mAttribute.aggressivity);
         str += "\n=================attckerOutLine  多少刀杀死=" + a;
        // Debug.Log("=================attckerOutLine  多少刀杀死=" + a);
        time = time / 1000;
         str += "\n=================attckerOutLine  时间=" + time;
        //  Debug.Log("=================attckerOutLine  时间=" + time);
        // float speed = JsonUtils.getIntance().getFrequencyByValue(hero.mAttribute.attackSpeed);
        float speed = JsonUtils.getIntance().getFrequencyByValue(hero.mAttribute.attackSpeed);
          str += "\n================attckerOutLine  每秒刀=" + speed;
        //   Debug.Log("=================attckerOutLine  每秒刀=" + speed);
        time = (long)(speed * time);
           str += "\n================att;ckerOutLine 总刀数=" + time;
        //  Debug.Log("=================attckerOutLine 总刀数=" + time);
        // str += "\n=================attckerOutLine  a=" + a;
        //  Debug.Log("=================attckerOutLine  a=" + a);
        long die =(time / a);
         str += "\n=================attckerOutLine  总杀死=" + die;
         str += "\n=================attckerOutLine  每个=" + enemy.getDieCrystal().toString();
        //  Debug.Log("=================attckerOutLine  总杀死=" + die);
        //  Debug.Log("=================attckerOutLine  每个=" + enemy.getDieCrystal().toString());
        BigNumber outlineGet =  BigNumber.multiply(enemy.getDieCrystal(), die);
          str += "\n================attckerOutLine  杀怪总魂晶=" + outlineGet.toString();
        //  Debug.Log("=================attckerOutLine  杀怪总魂晶=" + outlineGet.toString());
         outlineGet = BigNumber.multiply(outlineGet, outGet);
         str += "\n=================attckerOutLine  轮回倍增=" + outGet;
         str += "\n================attckerOutLine  计算轮回后=" + outlineGet.toString();
        //  Debug.Log("=================attckerOutLine  轮回倍增=" + outGet);
        //  Debug.Log("=================attckerOutLine  计算轮回后=" + outlineGet.toString());
        outlineGet = BigNumber.multiply(outlineGet, JsonUtils.getIntance().getConfigValueForId(100048));
        if(Time.timeScale > 1) {
            outlineGet = BigNumber.multiply(outlineGet, Time.timeScale);
        }
        
         str += "\n================attckerOutLine  离线衰减=" + JsonUtils.getIntance().getConfigValueForId(100048);
         str += "\n================attckerOutLine  计算衰减后=" + outlineGet.toString();
        //  Debug.Log("=================attckerOutLine  离线衰减=" + JsonUtils.getIntance().getConfigValueForId(100048));
        //  Debug.Log("=================attckerOutLine  计算衰减后=" + outlineGet.toString());
        //  if (outlineGet == null || outlineGet.toStringWithUnit().Equals("0K") || outlineGet.toStringWithUnit().Equals("0k") ) {
         Text text = GameObject.Find("uid_test").GetComponent<Text>();
         text.text = str;
        //   GameObject ob = GameObject.Find("game_error_messge");
        //  ob.transform.localPosition = new Vector2(0, 0);
        //  ob.transform.SetSiblingIndex(GameManager.getIntance().getUiLevel());
        //  }
        return outlineGet;
    }

	public double attackerAction(int id){
		double hurtBloodAll = 0;
		HurtStatus hurtBlood = null;
        float hurt = 0;
		Attacker attacker = getAttackerById (id);
		if (attacker!= null && attacker.mAttackerTargets != null && attacker.mAttackerTargets.Count > 0) {
			if (attacker is EnemyBase) {//enemy only one target
				FightResource resouce = new FightResource (attacker, mEnemyFactory);
//                Debug.Log("attackBllod");
                hurtBlood = attackBllod(attacker, attacker.mAttackerTargets[0]);
//                Debug.Log("attackBllod");
                attacker.mSkillManager.mEventAttackManager.beforeHurt(hurtBlood);
                hurtBlood = resouce.hurt(hurtBlood);
                if (hurtBlood != null && attacker.mAttackerTargets.Count >0) {
                    attacker.mAttackerTargets[0].mSkillManager.mEventAttackManager.beforeBeHurt(attacker,hurtBlood);
                    attacker.mAttackerTargets[0].BeAttack(hurtBlood, attacker);
                    if ( attacker.mAttackerTargets.Count > 0 && attacker.mAttackerTargets[0].getStatus() != ActionFrameBean.ACTION_DIE)
                    {
                        attacker.mAttackerTargets[0].mSkillManager.mEventAttackManager.endBeHurt(attacker,hurtBlood);
                        attacker.mSkillManager.mEventAttackManager.endHurt(hurtBlood);
                    }
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
                attacker.mSkillManager.mEventAttackManager.beforeHurt(hurtBlood);
                tager.mSkillManager.mEventAttackManager.beforeBeHurt(attacker,hurtBlood);
                
                tager.BeAttack (hurtBlood, attacker);
                tager.mSkillManager.mEventAttackManager.endBeHurt(attacker,hurtBlood);
                attacker.mSkillManager.mEventAttackManager.endHurt(hurtBlood);
                if (tager.getStatus() == ActionFrameBean.ACTION_DIE) {
                    attacker.mSkillManager.mEventAttackManager.killEnemy();
                }
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

        List<EquipKeyAndValue>  list = attacker.mSkillManager.mEventAttackManager.beforeActtack();
       
        if (!isHurt(attacker, beAttacker)) {
            return new HurtStatus(0, HurtStatus.TYPE_RATE);
        }
        attacker.mSkillManager.mEventAttackManager.Acttacking();

        bool isIgnoeDefen = false;
        if (list != null&& list.Count > 0) {
            foreach (EquipKeyAndValue key in list) {
                if (key.key == 111) {
                    isIgnoeDefen = true;
                    break;
                }
            }
        }
        double hurt = 0;
        if (isIgnoeDefen)
        {
            hurt = attacker.mAttribute.aggressivity;
        }
        else {
            hurt = attacker.mAttribute.aggressivity * attacker.mAttribute.aggressivity / (attacker.mAttribute.aggressivity + beAttacker.mAttribute.defense);
        }
        
        bool crt = isCrt(attacker);
        long type = HurtStatus.TYPE_DEFAULT;
        if (crt)
        {
            hurt = hurt * 2 + attacker.mAttribute.crtHurt + attacker.mAttribute.readHurt;
            type = HurtStatus.TYPE_CRT;
        }
        else {
            hurt = hurt + attacker.mAttribute.readHurt;
        }
        
        return new HurtStatus(hurt, type);
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
