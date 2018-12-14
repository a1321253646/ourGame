﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayControl : Attacker 
{

    // Use this for initialization

    public GameObject mLevelAnimal;

    private HeroState mState;
    private LevelManager mLevelManager;
    HeroLevelUpAnimal mLevelAnimalControl;

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
        int count = 3;
        mFightManager.registerAttacker (this);
        mLevelAnimalControl = new HeroLevelUpAnimal(mLevelAnimal, JsonUtils.getIntance().getEnemyResourceData(40002),this);
                if (GameManager.isAdd)
                  //  &&InventoryHalper.getIntance().getInventorys().Count == 0 
                  //   && InventoryHalper.getIntance().getUsercard().Count == 0
                  //   && !GameManager.getIntance().isAddGoodForTest)
                 {
                    GameManager.getIntance().isAddGoodForTest = true;
                    BackpackManager.getIntance().addGoods(3000001, count);
                    BackpackManager.getIntance().addGoods(3000002, count);
                    BackpackManager.getIntance().addGoods(3000003, count);
                    BackpackManager.getIntance().addGoods(3000004, count);
                    BackpackManager.getIntance().addGoods(3000005, count);
                    BackpackManager.getIntance().addGoods(3000006, count);
                    BackpackManager.getIntance().addGoods(3000007, count);
                    BackpackManager.getIntance().addGoods(3000008, count);
                    BackpackManager.getIntance().addGoods(3000009, count);
                    BackpackManager.getIntance().addGoods(3000010, count);
                    BackpackManager.getIntance().addGoods(3000011, count);
                    BackpackManager.getIntance().addGoods(3000012, count);
                    BackpackManager.getIntance().addGoods(3000013, count);
                    BackpackManager.getIntance().addGoods(3000014, count);
                    BackpackManager.getIntance().addGoods(3000015, count);
        }
        if (GameManager.getIntance().isHaveOutGet)
        {
            BigNumber outLineGet = new BigNumber();
            GameManager.getIntance().isHaveOutGet = false;
            long old = SQLHelper.getIntance().mOutTime;
            Debug.Log("========old = " + old);
            if (old != -1)
            {
                old = TimeUtils.getTimeDistanceMin(old);
                Debug.Log("========old = " + old);
                BigNumber levelCryStal = JsonUtils.getIntance().getLevelData(GameManager.getIntance().mCurrentLevel).getOfflinereward();
                outLineGet = BigNumber.multiply(levelCryStal, old);
                outLineGet = BigNumber.multiply(outLineGet, GameManager.getIntance().getOutlineGet());
                GameManager.getIntance().mCurrentCrystal = BigNumber.add(outLineGet, GameManager.getIntance().mCurrentCrystal);
                GameManager.getIntance().updataGasAndCrystal();
                SQLHelper.getIntance().updateHunJing(GameManager.getIntance().mCurrentCrystal);
                if (old > JsonUtils.getIntance().getConfigValueForId(100032))
                {
                    Level level = JsonUtils.getIntance().getLevelData(GameManager.getIntance().mCurrentLevel);
                    BigNumber outLine = BigNumber.multiply(level.getOfflinereward(), old);
                    outLine = BigNumber.multiply(outLine, GameManager.getIntance().getOutlineGet());
                    long h = old / 60;
                    long min = old % 60;
                    string str = "";
                    if (h > 9)
                    {
                        str += h + ":";
                    }
                    else
                    {
                        str = str + "0" + h + ":";
                    }
                    if (min > 9)
                    {
                        str += min;
                    }
                    else
                    {
                        str = str + "0" + min;
                    }
                    BackpackManager.getIntance().showMessageTip(MessageTips.TYPPE_OUT_LINE, "欢迎回来，您在离线的" + str + "里", "" + outLine.toStringWithUnit());
                }
            }
        }
        SQLHelper.getIntance().updateOutTime();
    }
    private void initAnimalEvent() {
        mSpriteRender = gameObject.GetComponent<SpriteRenderer>();
        mAnimalControl = new AnimalControlBase(resourceData, mSpriteRender);
        mAnimalControl.setStatueDelayStatue(ActionFrameBean.ACTION_ATTACK, ActionFrameBean.ACTION_STANDY);
        mAnimalControl.addIndexCallBack(ActionFrameBean.ACTION_ATTACK,(int)resourceData.attack_framce, new AnimalStatu.animalIndexCallback(fightEcent));
        mAnimalControl.setEndCallBack(ActionFrameBean.ACTION_WIN, new AnimalStatu.animalEnd(winEnd));
        mAnimalControl.setEndCallBack(ActionFrameBean.ACTION_DIE, new AnimalStatu.animalEnd(faileEnd));
        mAnimalControl.start();
        Run();
    }
    void faileEnd(int status)
    {
        mFightManager.dieOrWin(false, false);
    }
    void winEnd(int status) {
        isWinEnd = true;
        setStatus(Attacker.PLAY_STATUS_RUN);
        Run();
        if (GameManager.getIntance().isGuide)
        {
            mBackManager.move();
        }
        else {
            mBackManager.stop();
        }
       
        //mFightManager.dieOrWin(true, false);
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
        float bili = mBloodVolume/mAttribute.maxBloodVolume;
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
            if (isAddSkill)
            {
                mSkillManager.addSkill(bean, this);
            }
            else {
                mSkillManager.updateSkill(bean, this);
            }
           
        }
        getAttribute();
        mBloodVolume = (int)(mAttribute.maxBloodVolume* bili);
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
        mLunhuiAttributePre.clear();
        mSkillManager.lunhuiDownCardCost = 0;
        mSkillManager.lunhuiCardHurtPre = 0;
        mSkillManager.lunhuiHurtPre = 0;

        GameManager.getIntance().mLunhuiOnlineGet = 0;
        GameManager.getIntance().mLunhuiOutlineGet = 0;
        GameManager.getIntance().mLunhuiLunhuiGet = 0;

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
                    else if (date.type == 400001) {
                        mSkillManager.lunhuiHurtPre += ((float)date.value / 10000);
                        
                    }
                    else if (date.type == 400002)
                    {
                        mLunhuiAttributePre.aggressivity += ((float)date.value / 10000);
                    }
                    else if (date.type == 400003)
                    {
                        mLunhuiAttributePre.maxBloodVolume += date.value;
                    }
                    else if (date.type == 400004)
                    {
                        mLunhuiAttributePre.defense += date.value;
                    }
                    else if (date.type == 400005)
                    {
                        mLunhuiAttribute.crt += date.value;
                    }
                    else if (date.type == 400006)
                    {
                        mLunhuiAttribute.evd += date.value;
                    }
                    else if (date.type == 400007)
                    {
                        mSkillManager.lunhuiCardHurtPre += ((float)date.value/10000);
                    }
                    else if (date.type == 400008)
                    {
                        
                        mSkillManager.lunhuiDownCardCost += date.value;
                    }
                    else if (date.type == 400012)
                    {
                        mLunhuiAttributePre.attackSpeed += date.value;
                    }
                    else if (date.type == 500001)
                    {
                        GameManager.getIntance().mLunhuiOnlineGet += ((float)date.value/10000);
                    }
                    else if (date.type == 500002)
                    {
                        GameManager.getIntance().mLunhuiOutlineGet += ((float)date.value / 10000);
                    }
                    else if (date.type == 500003)
                    {
                        GameManager.getIntance().mLunhuiLunhuiGet += ((float)date.value / 10000);
                    }
                   
                }  
            }
        }
        Debug.Log("GameManager.getIntance().mLunhuiOnlineGet =" + GameManager.getIntance().mLunhuiOnlineGet);
        Debug.Log("GameManager.getIntance().mLunhuiOutlineGet =" + GameManager.getIntance().mLunhuiOutlineGet);
        Debug.Log("GameManager.getIntance().mLunhuiLunhuiGet =" + GameManager.getIntance().mLunhuiLunhuiGet);
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
//        Debug.Log("===============mAttribute.evd = " + mAllAttribute.evd);
        mAllAttribute.add(mEquipAttribute);
//        Debug.Log("===============mAttribute.evd = " + mAllAttribute.evd);
        mAllAttribute.add(mLunhuiAttribute);
//        Debug.Log("===============mAttribute.evd = " + mAllAttribute.evd);
        mAllAttribute.add(mSkillAttribute);
//        Debug.Log("===============mAttribute.evd = " + mAllAttribute.evd);


        mAllAttributePre.add(mEquipAttributePre);
        mAllAttributePre.add(mLunhuiAttributePre);
        mAllAttributePre.add(mSkillAttributePre);



        mAttribute.add(mAllAttribute);
        mAttribute.chen(mAllAttributePre);
        Debug.Log("===============mAttribute.evd = " + mAttribute.evd);
        GameManager.getIntance().setBlood(mBloodVolume, mAttribute.maxBloodVolume);
    }

    public void ChangeEquip(PlayerBackpackBean bean,bool isAdd)
    {        
        float bili = 1;
        bili =mBloodVolume / mAttribute.maxBloodVolume; ;
        mEquipAttribute.clear();
        List<PlayerBackpackBean> list = InventoryHalper.getIntance().getRoleUseList();
        Debug.Log("bili = " + bili);
        long fuhao = isAdd ? 1 : -1;       
        foreach (PlayerBackpackBean bean2 in list)
        {
            foreach (PlayerAttributeBean date in bean2.attributeList)
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
            Debug.Log("addSkill ChangeEquip");          
        }
        if (isAdd)
        {
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
        mLevelAnimalControl.levelUp();
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
//        Debug.Log("===============英雄mHero.dod = " + mHero.dod);
        mBaseAttribute.attackSpeed = mHero.attack_speed;
//        Debug.Log("===============英雄攻速 = " + mBaseAttribute.attackSpeed);
        mAttackLeng = mHero.attack_range;
         mBloodVolume = mBloodVolume + mBaseAttribute.maxBloodVolume - mMaxTmp;
//        Debug.Log("mBloodVolume = " + mBloodVolume+ " mBaseAttribute.maxBloodVolume="+ mBaseAttribute.maxBloodVolume+ " mMaxTmp"+ mMaxTmp);
        mLocalBean = new LocalBean (transform.position.x, transform.position.y,mAttackLeng,true,this);
		mState = new HeroState (this);
        getAttribute();
    }


	// Update is called once per frame
	private float mTime = 0;
    private bool isShowGuoChang = false;
	void Update () {
        mTime += Time.deltaTime;
        //        Debug.Log(" isWin  ="+ isWin);
        if (isWin)
        {
            if (GameManager.getIntance().isGuide) {
                return;
            }
            winrun();
            if (!isShowGuoChang && transform.position.x > 0)
            {
                isShowGuoChang = true;
                mFightManager.dieOrWin(true,true);
            }
            // return;
        }
        else if (isStart)
        {
            winrun();
            if (transform.position.x > JsonUtils.getIntance().getConfigValueForId(100003))
            {
                isStart = false;
                mBackManager.move();
                mLocalBean = new LocalBean(transform.position.x, transform.position.y, mAttackLeng, true, this);
                mFightManager.registerAttacker(this);
                mLevelManager.creatEnemyFactory(transform.position.x, transform.position.y+resourceData.idel_y);
            }
        }
        else if (getStatus() != mFightManager.mHeroStatus && getStatus() != Attacker.PLAY_STATUS_STANDY)
        {
            setStatus(mFightManager.mHeroStatus);
            if (getStatus() == Attacker.PLAY_STATUS_FIGHT)
            {
                Fight();
                //              Debug.Log(" Fight () ");             
                mBackManager.stop();
            }
            if (getStatus() == Attacker.PLAY_STATUS_RUN)
            {
                Run();
                mBackManager.move();
            }
        }

        mSkillManager.upDate();
        mAnimalControl.update();
        mLevelAnimalControl.updateAnimal();
    }
    private bool isWin = false;
    private bool isWinEnd = false;
    public void win()
    {
        isWin = true;
        mBackManager.stop();
        setStatus(Attacker.PLAY_STATUS_WIN);
    }
    private bool isStart = false;
    public void startGame() {
        isStart = true;
        mLevelManager = GameObject.Find("Manager").GetComponent<LevelManager>();
        mBackManager = mLevelManager.getBackManager();
        mBackManager.stop();
    }

    void winrun(){
        //      Debug.Log(" run ");
        if (isWinEnd || isStart) {
            transform.Translate(Vector2.right * (2 * Time.deltaTime));
        }
        
	}
	public override float BeAttack(HurtStatus status,Attacker hurter){
        //        Debug.Log("hero BeAttack :blood=" + status.blood + " isCrt=" + status.isCrt + " isRate=" + status.isRate);
        mSkillManager.beforeBeHurt(status);
        status.blood = status.blood * hurter.mSkillManager.getHurtPre();
        int tmp = status.blood % 1 == 0 ? 0 : 1;
        status.blood = ((int)status.blood) / 1 + tmp;
        if (JsonUtils.getIntance().getConfigValueForId(100007) != 1) {
            mBloodVolume = mBloodVolume - status.blood;
            GameManager.getIntance().setBlood(mBloodVolume, mAttribute.maxBloodVolume);
            if (mBloodVolume <= 0)
            {
                mFightManager.unRegisterAttacker(this);
                Die();
               
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



            if (hurt != null && hurt.mAttackType == ATTACK_TYPE_HERO && status.blood >= mBloodVolume)
            {
                if (mBloodVolume > 1)
                {
                    status.blood = mBloodVolume - 1;
                }
                else if (mBloodVolume == 1)
                {
                    status.blood = 1;
                }
            }
            else if (hurt != null)
            {
                status.blood = status.blood * hurt.mSkillManager.getCardHurtPre();
                int tmp = status.blood % 1 == 0 ? 0 : 1;
                status.blood = ((int)status.blood) / 1 + tmp;
            }
            else {
                int tmp = status.blood % 1 == 0 ? 0 : 1;
                status.blood = ((int)status.blood) / 1 + tmp;
            }


            if (JsonUtils.getIntance().getConfigValueForId(100007) != 1)
            {
                mBloodVolume = mBloodVolume - status.blood;
                GameManager.getIntance().setBlood(mBloodVolume, mAttribute.maxBloodVolume);
                if (mBloodVolume <= 0)
                {
                    mFightManager.unRegisterAttacker(this);
                    Die();
                    
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
