using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayControl : Attacker 
{

    // Use this for initialization

    private HeroState mState;
    void Start () {
		mAttackType = Attacker.ATTACK_TYPE_HERO;
        mCampType = CAMP_TYPE_PLAYER;
        mBackManager = GameObject.Find ("Manager").GetComponent<LevelManager> ().getBackManager ();
		mFightManager = GameObject.Find ("Manager").GetComponent<LevelManager> ().getFightManager ();
		//mBackManager.setBackground ("map/map_03");
		toString ("Play");
		mFightManager.mHeroStatus = Attacker.PLAY_STATUS_RUN;       
        setHeroData ();
    mFightManager.registerAttacker (this);
	}
    private void initAnimalEvent() {
        mSpriteRender = gameObject.GetComponent<SpriteRenderer>();
        mAnimalControl = new AnimalControlBase(resourceData, mSpriteRender);
        mAnimalControl.setStatueDelayStatue(ActionFrameBean.ACTION_ATTACK, ActionFrameBean.ACTION_STANDY);
        mAnimalControl.addIndexCallBack(ActionFrameBean.ACTION_ATTACK,(int)resourceData.attack_framce, new AnimalStatu.animalIndexCallback(fightEcent));
        mAnimalControl.start();
        Run();
    }

    void fightEcent(int status) {
        if (status == ActionFrameBean.ACTION_ATTACK) {
            mFightManager.attackerAction(id);
        }
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
                    else if (date.type == 114) {
                        mEquipAttribute.attackSpeed += date.value;
                    }
                }
            }
        }
        mAttribute.clear();
        mAttribute.add(mBaseAttribute);
        mAttribute.add(mEquipAttribute);
        mBloodVolume = (int)(mAttribute.maxBloodVolume * bili);
        GameManager.getIntance().setBlood(mBloodVolume, mAttribute.maxBloodVolume);
        upDataSpeed();
    }
    public void heroUp() {
        setHeroData();
    }

    public void setHeroData(){
        Hero mHero = JsonUtils.getIntance().getHeroData();
        if (resourceData == null) {           
            resourceData = JsonUtils.getIntance().getEnemyResourceData(mHero.resource);
            initAnimalEvent();
        }
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
        mTime += Time.deltaTime;
        if (getStatus() != mFightManager.mHeroStatus && getStatus() != Attacker.PLAY_STATUS_STANDY) {
			setStatus(mFightManager.mHeroStatus);
			if (getStatus() == Attacker.PLAY_STATUS_FIGHT) {
				Fight ();
                Debug.Log(" Fight () ");             
                mBackManager.stop ();
			}
			if (getStatus() == Attacker.PLAY_STATUS_RUN ) {
				Run ();
				mBackManager.move ();
			}
		}
        mAnimalControl.update();
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
