using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayControl : Attacker 
{

    // Use this for initialization

    private HeroState mState;
	void Start () {
		mAttackType = Attacker.ATTACK_TYPE_HERO;
		mBackManager = GameObject.Find ("Manager").GetComponent<LevelManager> ().getBackManager ();
		mFightManager = GameObject.Find ("Manager").GetComponent<LevelManager> ().getFightManager (); 
		_anim = gameObject.GetComponent<Animator> ();
		//mBackManager.setBackground ("map/map_03");
		toString ("Play");
		mFightManager.mHeroStatus = Attacker.PLAY_STATUS_RUN;       
        setHeroData ();
		mFightManager.registerAttacker (this);
		RuntimeAnimatorController rc = _anim.runtimeAnimatorController;
		AnimationClip[] cls = rc.animationClips;
		foreach(AnimationClip cl in cls){
			if (cl.name.Equals ("Attack")) {
				//isAddEvent = true;
				AnimationEvent event1 = new AnimationEvent ();
				event1.functionName = "standyEvent";
				event1.time = cl.length-0.1f;
				cl.AddEvent (event1);
                AnimationEvent event2 = new AnimationEvent();
                event2.functionName = "fightEvent";
                event2.time = (cl.length - 0.1f) * (resourceData.attack_framce / resourceData.attack_all_framce);
                // Debug.Log("set fightEvent resourceData.attack_frame =" + resourceData.attack_framce);
                //Debug.Log("set fightEvent resourceData.attack_all_framce =" + resourceData.attack_all_framce);
                // Debug.Log("set fightEvent time =" + event2.time);
                cl.AddEvent(event2);
            } 
		}
		Run ();
	}

    public float mEquipAggressivity = 0;
    public float mEquipDefense = 0;
    public float mEquipMaxBloodVolume = 0;
    public float mEquipRate = 100;
    public float mEquipEvd = 0;
    public float mEquipCrt = 0;
    public float mEquipReadHurt = 0;
    public float mEquipCrtHurt = 0;


    public float mBaseAggressivity = 0;
    public float mBaseDefense = 0;
    public float mBaseMaxBloodVolume = 0;
    public float mBaseRate = 100;
    public float mBaseEvd = 0;
    public float mBaseCrt = 0;
    public float mBaseReadHurt = 0;
    public float mBaseCrtHurt = 0;



    public void ChangeEquip() {
        Dictionary<long, PlayerBackpackBean> equips =  InventoryHalper.getIntance().getRoleUseList();
        ChangeEquip(equips);
    }
    public void ChangeEquip(Dictionary<long, PlayerBackpackBean> equips)
    {
        mEquipMaxBloodVolume = 0;
        mEquipDefense = 0;
        mEquipAggressivity = 0;
        mEquipRate = 0;
        mEquipEvd = 0;
        mEquipCrt = 0;
        mEquipCrtHurt = 0;
        mEquipReadHurt = 0;
        float bili = mBloodVolume / mMaxBloodVolume;
        for (long i = 1; i < 7; i++) {
            if (equips != null && equips.ContainsKey(i)) {
                PlayerBackpackBean bean = equips[i];
                foreach (PlayerAttributeBean date in bean.attributeList) {
                    if (date.type == 100)
                    {
                        mEquipAggressivity += date.value;
                    }
                    else if (date.type == 101)
                    {
                        mEquipDefense += date.value;
                    }
                    else if (date.type == 102)
                    {
                        mEquipMaxBloodVolume += date.value;
                    }
                    else if (date.type == 110)
                    {
                        mEquipRate += date.value;
                    }
                    else if (date.type == 111)
                    {
                        mEquipEvd += date.value;                 
                    }
                    else if (date.type == 112)
                    {
                        mEquipCrt += date.value;
                    }
                    else if (date.type == 113)
                    {
                        mEquipCrtHurt += date.value;
                    }
                    else if (date.type == 115)
                    {
                        mEquipReadHurt += date.value;
                    }
                }
            }
        }
        mAggressivity = mBaseAggressivity + mEquipAggressivity;
        mDefense = mBaseDefense + mEquipDefense;
        mMaxBloodVolume = mBaseMaxBloodVolume + mEquipMaxBloodVolume;
        mBloodVolume = (int)(mMaxBloodVolume * bili);
        mRate = mBaseRate + mEquipRate;
        mEvd = mBaseEvd + mEquipEvd;
        mCrt = mBaseCrt + mEquipCrt;
        mCrtHurt = mBaseCrtHurt + mEquipCrtHurt;
        mReadHurt = mBaseReadHurt + mEquipReadHurt;
        GameManager.getIntance().setBlood(mBloodVolume, mMaxBloodVolume);
    }
    private bool isFighted = false;
    public void fightEvent()
    {

        if (status == Attacker.PLAY_STATUS_DIE || isFighted)
        {
            return;
        }
        //  Debug.Log("fightEvent time =" + timeTest);
        isFighted = true;
        // Debug.Log("fightEvent");
        mFightManager.attackerAction(id);
    }
    public void standyEvent(){
		if (status == Attacker.PLAY_STATUS_DIE || status != Attacker.PLAY_STATUS_FIGHT) {
			return;
		}
		Standy ();
	}
    public void heroUp() {
        Hero mHero = JsonUtils.getIntance().getHeroData();
        mBaseAggressivity = mHero.role_attack;
        mBaseDefense = mHero.role_defense;
        float mMaxTmp = mBaseMaxBloodVolume;
        mBaseMaxBloodVolume = mHero.role_hp;
        mAggressivity = mBaseAggressivity + mEquipAggressivity;
        mDefense = mBaseDefense + mEquipDefense;
        mMaxBloodVolume = mBaseMaxBloodVolume + mEquipMaxBloodVolume;
        mBloodVolume = mBloodVolume + mBaseMaxBloodVolume - mMaxTmp;
        GameManager.getIntance().setBlood(mBloodVolume, mMaxBloodVolume);
        setHeroData();
    }

    public void setHeroData(){
        Hero mHero = JsonUtils.getIntance().getHeroData(); 
        resourceData = JsonUtils.getIntance().getEnemyResourceData(mHero.resource);

        mBaseAggressivity = mHero.role_attack;
        mBaseDefense = mHero.role_defense;
        mBaseMaxBloodVolume = mHero.role_hp;
        mBaseCrt = mHero.cri;
        mBaseCrtHurt = mHero.cri_dam;
        mBaseRate = mHero.hit;
        mBaseReadHurt = mHero.real_dam;
        mBaseEvd = mHero.dod;


        mAggressivity = mBaseAggressivity ;
        mMaxBloodVolume = mBaseMaxBloodVolume;
        mDefense = mBaseDefense;

        mAttackSpeed = mHero.attack_speed;
		mAttackLeng = mHero.attack_range;
		mBloodVolume = mMaxBloodVolume;     
		mLocalBean = new LocalBean (transform.position.x, transform.position.y,mAttackLeng,true,this);
		mState = new HeroState (this);
        ChangeEquip();
    }


	// Update is called once per frame
	private float mTime = 0; 
	void Update () {

		if (status != mFightManager.mHeroStatus && status != Attacker.PLAY_STATUS_STANDY) {
			status = mFightManager.mHeroStatus;
			if (status == Attacker.PLAY_STATUS_FIGHT) {
				Fight ();
                isFighted = false;
                mBackManager.stop ();
			}
			if (status == Attacker.PLAY_STATUS_RUN ) {
				Run ();
				mBackManager.move ();
			}
		}
		mAttackTime += Time.deltaTime;
		if (status == Attacker.PLAY_STATUS_STANDY) {
			if (mAttackTime >= mAttackSpeed) {
		//		Debug.Log("hurt");
				Fight ();
                isFighted = false;
			}	
		}
	}
	void run(){
		transform.Translate (new Vector2 (1, 0)*(mRunSpeed-mBackManager.moveSpeed)*Time.deltaTime);
	}
	public override float BeAttack(HurtStatus status){
//        Debug.Log("hero BeAttack :blood=" + status.blood + " isCrt=" + status.isCrt + " isRate=" + status.isRate);
        if (JsonUtils.getIntance().getConfigValueForId(100007) != 1) {
            mBloodVolume = mBloodVolume - status.blood;
            GameManager.getIntance().setBlood(mBloodVolume, mMaxBloodVolume);
            if (mBloodVolume <= 0)
            {
                Die();
                mFightManager.unRegisterAttacker(this);
            }
        }
		
		mState.hurt (status);
		return status.blood;
	}
}
