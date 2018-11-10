using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayControl : Attacker 
{

    // Use this for initialization

    private HeroState mState;
    private LevelManager mLevelManager;
    void Start () {
        startComment();
        mAttackType = Attacker.ATTACK_TYPE_HERO;
        mCampType = CAMP_TYPE_PLAYER;
        mLevelManager = GameObject.Find("Manager").GetComponent<LevelManager>();
        mBackManager = mLevelManager.getBackManager ();
		mFightManager = mLevelManager.getFightManager ();
		//mBackManager.setBackground ("map/map_03");
		toString ("Play");
		mFightManager.mHeroStatus = Attacker.PLAY_STATUS_RUN;
        setHeroData ();
        upLunhui();
        initEquip();
        mFightManager.registerAttacker (this);
        if (InventoryHalper.getIntance().getInventorys().Count == 0 
            && InventoryHalper.getIntance().getUsercard().Count == 0
            && !GameManager.getIntance().isAddGoodForTest)
        {
            GameManager.getIntance().isAddGoodForTest = true;
            BackpackManager.getIntance().addGoods(3000001, 1);
            BackpackManager.getIntance().addGoods(3000002, 1);
            BackpackManager.getIntance().addGoods(3000003, 1);
            BackpackManager.getIntance().addGoods(3000004, 1);
            BackpackManager.getIntance().addGoods(3000005, 1);
            BackpackManager.getIntance().addGoods(3000006, 1);
            BackpackManager.getIntance().addGoods(3000007, 1);
            BackpackManager.getIntance().addGoods(3000008, 1);
            BackpackManager.getIntance().addGoods(3000009, 1);
            BackpackManager.getIntance().addGoods(3000010, 1);
            BackpackManager.getIntance().addGoods(3000011, 1);
            BackpackManager.getIntance().addGoods(3000012, 1);
            BackpackManager.getIntance().addGoods(3000013, 1);
            BackpackManager.getIntance().addGoods(3000014, 1);
            BackpackManager.getIntance().addGoods(3000015, 1);
            BackpackManager.getIntance().addGoods(2110001, 1);
            BackpackManager.getIntance().addGoods(2110001, 1);
            BackpackManager.getIntance().addGoods(2110001, 1);
            BackpackManager.getIntance().addGoods(2110001, 1);
            BackpackManager.getIntance().addGoods(2110001, 1);
            BackpackManager.getIntance().addGoods(2110001, 1);
        }

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
            mLevelManager.playerAction();
            mFightManager.attackerAction(id);
            mSkillManager.inFight();
        }
    }

    public Attribute mBaseAttribute = new Attribute();

    public Attribute mEquipAttribute = new Attribute();
    public Attribute mEquipAttributePre = new Attribute();

    public Attribute mLunhuiAttribute = new Attribute();
    public Attribute mLunhuiAttributePre = new Attribute();

    public Attribute mSkillAttribute = new Attribute();
    public Attribute mSkillAttributePre = new Attribute();

    public Attribute mAllAttribute = new Attribute();
    public Attribute mAllAttributePre = new Attribute().setToPre();

    public void initEquip(bool isAddSkill) {
        List<PlayerBackpackBean> list = InventoryHalper.getIntance().getRoleUseList();
        mEquipAttribute.clear();
        foreach (PlayerBackpackBean bean in list)
        {
            foreach (PlayerAttributeBean date in bean.attributeList)
            {
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
                else if (date.type == 114)
                {
                    mEquipAttribute.attackSpeed += date.value;
                }
            }
            Debug.Log("addSkill initEquip");
            if (isAddSkill) {
                mSkillManager.addSkill(bean, this);
            }
           
        }
        getAttribute();
        mBloodVolume = (int)mAttribute.maxBloodVolume;
        GameManager.getIntance().setBlood(mBloodVolume, mAttribute.maxBloodVolume);
        upDataSpeed();
    }

    public void initEquip() {
        initEquip(true);
    }
    public void upLunhui()
    {
        float mMaxTmp = mLunhuiAttribute.maxBloodVolume;
        mLunhuiAttribute.clear();
        Dictionary<long, long>  samsaras= InventoryHalper.getIntance().getSamsaraLevelDate();
        Dictionary<long, long>.KeyCollection keys= samsaras.Keys;
        foreach (long key in keys) {
            long level = samsaras[key];
            if(level != 0) {
               SamsaraJsonBean sam=  JsonUtils.getIntance().getSamsaraInfoById(key);
               List<SamsaraValueBean> beanValue = sam.levelList[level];
                foreach (SamsaraValueBean date in beanValue) {
                    if (date.type == 100)
                    {
                        mLunhuiAttribute.aggressivity += date.value;
                    }
                    else if (date.type == 101)
                    {
                        mLunhuiAttribute.defense += date.value;
                    }
                    else if (date.type == 102)
                    {
                        mLunhuiAttribute.maxBloodVolume += date.value;                    
                    }
                    else if (date.type == 110)
                    {
                        mLunhuiAttribute.rate += date.value;
                    }
                    else if (date.type == 111)
                    {
                        mLunhuiAttribute.evd += date.value;
                    }
                    else if (date.type == 112)
                    {
                        mLunhuiAttribute.crt += date.value;
                    }
                    else if (date.type == 113)
                    {
                        mLunhuiAttribute.crtHurt += date.value;
                    }
                    else if (date.type == 115)
                    {
                        mLunhuiAttribute.readHurt += date.value;
                    }
                    else if (date.type == 114)
                    {
                        mLunhuiAttribute.attackSpeed += date.value;
                    }
                }  
            }
        }
        mBloodVolume = mBloodVolume + mLunhuiAttribute.maxBloodVolume - mMaxTmp;
        getAttribute();
        GameManager.getIntance().setBlood(mBloodVolume, mAttribute.maxBloodVolume);
        upDataSpeed();
    }

    public override  void getAttribute() {
        mAttribute.clear();
        mAllAttribute.clear();
        mAllAttributePre.setToPre();

        mAllAttribute.add(mBaseAttribute);
        mAllAttribute.add(mEquipAttribute);
        mAllAttribute.add(mLunhuiAttribute);
        mAllAttribute.add(mSkillAttribute);

        mAllAttributePre.add(mEquipAttributePre);
        mAllAttributePre.add(mLunhuiAttributePre);
        mAllAttributePre.add(mSkillAttributePre);

        mAttribute.add(mAllAttribute);
        mAttribute.chen(mAllAttributePre);
    }

    public void ChangeEquip(PlayerBackpackBean bean,bool isAdd)
    {        
        float bili = 1;
        bili = mBloodVolume / (mBaseAttribute.maxBloodVolume+ mEquipAttribute.maxBloodVolume);
        mEquipAttribute.clear();
        Debug.Log("bili = " + bili);
        long fuhao = isAdd ? 1 : -1;
        foreach (PlayerAttributeBean date in bean.attributeList) {
            if (date.type == 100)
            {
                mEquipAttribute.aggressivity += date.value* fuhao;
            }
            else if (date.type == 101)
            {
                mEquipAttribute.defense += date.value * fuhao;
            }
            else if (date.type == 102)
            {
                mEquipAttribute.maxBloodVolume += date.value * fuhao;
            }
            else if (date.type == 110)
            {
                mEquipAttribute.rate += date.value * fuhao;
            }
            else if (date.type == 111)
            {
                mEquipAttribute.evd += date.value * fuhao;
            }
            else if (date.type == 112)
            {
                mEquipAttribute.crt += date.value * fuhao;
            }
            else if (date.type == 113)
            {
                mEquipAttribute.crtHurt += date.value * fuhao;
            }
            else if (date.type == 115)
            {
                mEquipAttribute.readHurt += date.value * fuhao;
            }
            else if (date.type == 114) {
                mEquipAttribute.attackSpeed += date.value * fuhao;
            }
        }
        if (isAdd)
        {
            Debug.Log("addSkill ChangeEquip");
            mSkillManager.addSkill(bean, this);
        }
        else
        {
            mSkillManager.removeSkill(bean, this);
        }
        getAttribute();
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
        mAttackLeng = mHero.attack_range;
         mBloodVolume = mBloodVolume + mBaseAttribute.maxBloodVolume - mMaxTmp;
        Debug.Log("mBloodVolume = " + mBloodVolume+ " mBaseAttribute.maxBloodVolume="+ mBaseAttribute.maxBloodVolume+ " mMaxTmp"+ mMaxTmp);
        mLocalBean = new LocalBean (transform.position.x, transform.position.y,mAttackLeng,true,this);
		mState = new HeroState (this);
        getAttribute();
    }


	// Update is called once per frame
	private float mTime = 0;
	void Update () {
        mTime += Time.deltaTime;
        if (getStatus() != mFightManager.mHeroStatus && getStatus() != Attacker.PLAY_STATUS_STANDY) {
			setStatus(mFightManager.mHeroStatus);
			if (getStatus() == Attacker.PLAY_STATUS_FIGHT) {
				Fight ();
  //              Debug.Log(" Fight () ");             
                mBackManager.stop ();
			}
			if (getStatus() == Attacker.PLAY_STATUS_RUN ) {
				Run ();
				mBackManager.move ();
			}
		}
        mSkillManager.upDate();
        mAnimalControl.update();
    }
	void run(){
		transform.Translate (new Vector2 (1, 0)*(mRunSpeed-mBackManager.moveSpeed)*Time.deltaTime);
	}
	public override float BeAttack(HurtStatus status,Attacker hurter){
        //        Debug.Log("hero BeAttack :blood=" + status.blood + " isCrt=" + status.isCrt + " isRate=" + status.isRate);
        mSkillManager.beforeBeHurt(status);
        status.blood = status.blood * hurter.mSkillManager.hurtPre;
        int tmp = status.blood % 1 == 0 ? 0 : 1;
        status.blood = ((int)status.blood) / 1 + tmp;
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
    public override float BeKillAttack(long effect, float value,Attacker hurt)
    {
        if (effect == 1 || effect == 6 || effect == 4 || effect == 30001)
        {
            HurtStatus status = new HurtStatus(value, false, true);
            if (hurt != null)
            {
                status.blood = status.blood * hurt.mSkillManager.cardHurtPre;
            }
            if (JsonUtils.getIntance().getConfigValueForId(100007) != 1)
            {
                mBloodVolume = mBloodVolume - status.blood;
                GameManager.getIntance().setBlood(mBloodVolume, mAttribute.maxBloodVolume);
                if (mBloodVolume <= 0)
                {
                    Die();
                    mFightManager.unRegisterAttacker(this);
                }
            }
            mState.hurt(status);
            return status.blood;
        }
        else if (effect == 2 || effect == 10001) {
            int tmp = value % 1 == 0 ? 0 : 1;
            value = ((int)value) / 1 + tmp;
            AddBlood(value);
        }
        return value;
    }
    public override void AddBlood(float value) {
        if (mBloodVolume + value > mAttribute.maxBloodVolume)
        {
            value = mAttribute.maxBloodVolume - mBloodVolume;
        }
        mBloodVolume = mBloodVolume + value;
        GameManager.getIntance().setBlood(mBloodVolume, mAttribute.maxBloodVolume);
        mState.add(value);
    }
}
