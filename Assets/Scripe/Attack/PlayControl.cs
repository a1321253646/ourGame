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


    public Attribute mEquipAttribute = new Attribute();
    public Attribute mBaseAttribute = new Attribute();

    public void ChangeEquip() {
        Dictionary<long, PlayerBackpackBean> equips =  InventoryHalper.getIntance().getRoleUseList();
        ChangeEquip(equips);
    }
    public void ChangeEquip(Dictionary<long, PlayerBackpackBean> equips)
    {
        
        float bili = 1;
        bili = mBloodVolume / (mBaseAttribute.maxBloodVolume+ mEquipAttribute.maxBloodVolume);
        mEquipAttribute.clear();
        Debug.Log("bili = " + bili);
        for (long i = 1; i < 7; i++) {
            if (equips != null && equips.ContainsKey(i)) {
                PlayerBackpackBean bean = equips[i];
                foreach (PlayerAttributeBean date in bean.attributeList) {
                    if (date.type == 100)
                    {
                        mEquipAttribute.aggressivity += date.value;
                    }
                    else if (date.type == 101)
                    {
                        mEquipAttribute.defense += date.value;
                    }
                    else if (date.type == 102)
                    {
                        mEquipAttribute.maxBloodVolume += date.value;
                    }
                    else if (date.type == 110)
                    {
                        mEquipAttribute.rate += date.value;
                    }
                    else if (date.type == 111)
                    {
                        mEquipAttribute.evd += date.value;                 
                    }
                    else if (date.type == 112)
                    {
                        mEquipAttribute.crt += date.value;
                    }
                    else if (date.type == 113)
                    {
                        mEquipAttribute.crtHurt += date.value;
                    }
                    else if (date.type == 115)
                    {
                        mEquipAttribute.readHurt += date.value;
                    }
                }
            }
        }
        mAttribute.clear();
        mAttribute.add(mBaseAttribute);
        mAttribute.add(mEquipAttribute);
        mBloodVolume = (int)(mAttribute.maxBloodVolume * bili);
        GameManager.getIntance().setBlood(mBloodVolume, mAttribute.maxBloodVolume);
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
        setHeroData();
    }

    public void setHeroData(){
        Hero mHero = JsonUtils.getIntance().getHeroData(); 
        resourceData = JsonUtils.getIntance().getEnemyResourceData(mHero.resource);
        float mMaxTmp = mBaseAttribute.maxBloodVolume;
        if (mBloodVolume < 0) {
            mBloodVolume = 0;
            Debug.Log("mBloodVolume = " + mBloodVolume);
        }
        Debug.Log("mMaxTmp = " + mMaxTmp);
        Debug.Log("mBloodVolume = " + mBloodVolume);
        mBaseAttribute.aggressivity = mHero.role_attack;
        mBaseAttribute.defense = mHero.role_defense;
        mBaseAttribute.maxBloodVolume = mHero.role_hp;
        mBaseAttribute.crt = mHero.cri;
        mBaseAttribute.crtHurt = mHero.cri_dam;
        mBaseAttribute.rate = mHero.hit;
        mBaseAttribute.readHurt = mHero.real_dam;
        mBaseAttribute.evd = mHero.dod;
        mBaseAttribute.attackSpeed = mHero.attack_speed;
        mBloodVolume = mBloodVolume + mBaseAttribute.maxBloodVolume - mMaxTmp;
        Debug.Log("mBloodVolume = " + mBloodVolume+ " mBaseAttribute.maxBloodVolume="+ mBaseAttribute.maxBloodVolume+ " mMaxTmp"+ mMaxTmp);
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
			if (mAttackTime >= mAttribute.attackSpeed) {
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
            GameManager.getIntance().setBlood(mBloodVolume, mAttribute.maxBloodVolume);
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
