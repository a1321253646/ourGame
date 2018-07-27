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
			} 
		}
		Run ();
	}
    public float mEquipAggressivity = 0;
    public float mEquipDefense = 0;
    public float mEquipMaxBloodVolume = 0;

    public float mBaseAggressivity = 0;
    public float mBaseDefense = 0;
    public float mBaseMaxBloodVolume = 0;
    public void ChangeEquip() {
        Dictionary<long, PlayerBackpackBean> equips =  InventoryHalper.getIntance().getRoleUseList();
        ChangeEquip(equips);
    }
    public void ChangeEquip(Dictionary<long, PlayerBackpackBean> equips)
    {
        mEquipMaxBloodVolume = 0;
        mEquipDefense = 0;
        mEquipAggressivity = 0;
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
                }
            }
        }
        mAggressivity = mBaseAggressivity + mEquipAggressivity;
        mDefense = mBaseDefense + mEquipDefense;
        mMaxBloodVolume = mBaseMaxBloodVolume + mEquipMaxBloodVolume;
        mBloodVolume = (int)(mMaxBloodVolume * bili);
        GameManager.getIntance().setBlood(mBloodVolume, mMaxBloodVolume);
    }

    public void standyEvent(){
		if (status == Attacker.PLAY_STATUS_DIE) {
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
    }

    public void setHeroData(){
        Hero mHero = JsonUtils.getIntance().getHeroData(); 
        resourceData = JsonUtils.getIntance().getEnemyResourceData(mHero.resource);

        mBaseAggressivity = mHero.role_attack;
        mBaseDefense = mHero.role_defense;
        mBaseMaxBloodVolume = mHero.role_hp;

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
				mFightManager.attackerAction (id);
			}	
		}
	}
	void run(){
		transform.Translate (new Vector2 (1, 0)*(mRunSpeed-mBackManager.moveSpeed)*Time.deltaTime);
	}
	public override float BeAttack(float blood){
        if (JsonUtils.getIntance().getConfigValueForId(100007) != 1) {
            mBloodVolume = mBloodVolume - blood;
            GameManager.getIntance().setBlood(mBloodVolume, mMaxBloodVolume);
            if (mBloodVolume <= 0)
            {
                Die();
                mFightManager.unRegisterAttacker(this);
            }
        }
		
		mState.hurt (blood);
		return blood;
	}
}
