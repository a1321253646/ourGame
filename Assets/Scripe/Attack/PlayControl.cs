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

    private Vector2 mFirVetor;

    void Start() {
        mFirVetor = transform.position;
        
        mCampType = CAMP_TYPE_PLAYER;
        mLevelManager = GameObject.Find("Manager").GetComponent<LevelManager>();
        mBackManager = mLevelManager.getBackManager();
        mFightManager = mLevelManager.getFightManager();
        
        toString("Play");
        int count = 1;
        GameObject.Find("Manager").GetComponent<PetManager>().init();
        resetHero();
        
        mLevelAnimalControl = new HeroLevelUpAnimal(mLevelAnimal, JsonUtils.getIntance().getEnemyResourceData(40002), this);
        if (GameManager.isAdd)
        {
            GameManager.getIntance().isAddGoodForTest = true;
           /* BackpackManager.getIntance().addGoods(3000001, count);
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
            BackpackManager.getIntance().addGoods(3000016, count);
            BackpackManager.getIntance().addGoods(3000017, count);
            BackpackManager.getIntance().addGoods(3000018, count);
            BackpackManager.getIntance().addGoods(3000019, count);
            BackpackManager.getIntance().addGoods(3000020, count);
            BackpackManager.getIntance().addGoods(3000021, count);
            BackpackManager.getIntance().addGoods(3000022, count);
            BackpackManager.getIntance().addGoods(3000023, count);
            BackpackManager.getIntance().addGoods(3000024, count);
            BackpackManager.getIntance().addGoods(3000025, count);
            BackpackManager.getIntance().addGoods(3000026, count);
            BackpackManager.getIntance().addGoods(3000027, count);
            BackpackManager.getIntance().addGoods(3000028, count);
            //  BackpackManager.getIntance().addGoods(4000001, count);
            //  BackpackManager.getIntance().addGoods(4000002, count);
            // BackpackManager.getIntance().addGoods(4000003, count);
            //  BackpackManager.getIntance().addGoods(4000004, count);
            //  BackpackManager.getIntance().addGoods(4000005, count);
            // BackpackManager.getIntance().addGoods(4000006, count);
            //  BackpackManager.getIntance().addGoods(4000007, count);
            //  BackpackManager.getIntance().addGoods(4000008, count);

            BackpackManager.getIntance().addGoods(4000009, count);
            BackpackManager.getIntance().addGoods(4000010, count);
            BackpackManager.getIntance().addGoods(4000011, count);
            BackpackManager.getIntance().addGoods(4000012, count);
            BackpackManager.getIntance().addGoods(4000013, count);
            BackpackManager.getIntance().addGoods(22054024, count);
            BackpackManager.getIntance().addGoods(22054024, count);
            BackpackManager.getIntance().addGoods(22053023, count);*/
            BackpackManager.getIntance().addGoods(4000005, count);

        }
        getOutLine();
        if (AdIntance.getIntance().getType() != -1)
        {
            long type = AdIntance.getIntance().getType();
            string value = "";
            if (type == ActiveButtonControl.TYPE_AD_LUNHUI)
            {
                value = JsonUtils.getIntance().getLevelData(BaseDateHelper.decodeLong(GameManager.getIntance().mCurrentLevel)).
                    lunhui;
                BigNumber bigValue = BigNumber.getBigNumForString(value);
                bigValue = BigNumber.multiply(bigValue, GameManager.getIntance().getLunhuiGet());
                value = bigValue.toString();
            }
            else
            {
                LevelManager level = GameObject.Find("Manager").GetComponent<LevelManager>();
                long time = (long)JsonUtils.getIntance().getConfigValueForId(100049);
                value = level.mFightManager.attckerOutLine(level.mPlayerControl, time * 60 * 1000, GameManager.getIntance().getOutlineGet()).toString();
            }

            GameObject.Find("active_button_list").GetComponent<ActiveListControl>().showAd(type, value, true);
        }
    }

    private bool isFristStart = true;

    public void resetHero() {
        isBeAttacker = false;
        if(mSkillManager != null) {
            mSkillManager.removeAllSkill();
            mSkillManager.cardCardHurtPre.clear();
            mSkillManager.carHurtPre.clear();
        }
        if (mVoication != -1) {
            resourceData = null;
        }
        if (mSkillAttribute != null) {
            mSkillAttribute.clear();
        }
        mPetAttribute.clear();
        //   status = PLAY_STATUS_RUN;
        id = -1;
        mBloodVolume = 0;
        mAttackLeng = 1;
        mDieGas = 0;
        bloodBili = -1;
        bloodDistance = -1;
        mTime = 0;
        isShowGuoChang = false;
        isWin = false;
        isWinEnd = false;
        isStop = false;
        oldStatus = PLAY_STATUS_RUN;
        isStart = false;

        mVoication = -1;
        if (mAttackerTargets != null) {
            mAttackerTargets.Clear();
        }

        // mAttackerTargets = null;
        if (mAllAttributePre != null)
        {
            mAllAttributePre.clear();
        }
        transform.position = mFirVetor;

        setHeroData();

        upLunhui();
        initEquip();
        GameObject.Find("Manager").GetComponent<PetManager>().reInit();
        if (!isFristStart)
        {
            restart();
        }
        isFristStart = false;
        oldStatus = PLAY_STATUS_RUN;
        if (mLocalBean != null) {
            mLocalBean.next = null;
            mLocalBean.mTargetX = -999;
            mLocalBean.mTargetY = -999;
        }
        mFightManager.mHeroStatus = Attacker.PLAY_STATUS_RUN;
        mFightManager.registerAttacker(this);
        startGame();
    }


    private void getOutLine() {
        Debug.Log("============ 大年30修bug  ====================getOutLine（）GameManager.getIntance().isHaveOutGet=" + GameManager.getIntance().isHaveOutGet);
        if (GameManager.getIntance().isHaveOutGet)
        {
            GameManager.getIntance().isHaveOutGet = false;
            long old2 = SQLHelper.getIntance().mOutTime;
            Debug.Log("============ 大年30修bug  ====================getOutLine（）old2=" + old2);
            if (old2 != -1)
            {
                long old = TimeUtils.GetTimeStamp() - old2;
                old2 = TimeUtils.getTimeDistanceMin(old2);
                Debug.Log("============ 大年30修bug  ====================离线时间 old2=" + old2);
                BigNumber outGet = mFightManager.attckerOutLine(this, old, GameManager.getIntance().getOutlineGet());

                if (old2 > 1)
                {
                    GameManager.getIntance().mCurrentCrystal = BigNumber.add(outGet, GameManager.getIntance().mCurrentCrystal);
                    GameManager.getIntance().updataGasAndCrystal();
                    SQLHelper.getIntance().updateHunJing(GameManager.getIntance().mCurrentCrystal);
                }

                if (old2 > JsonUtils.getIntance().getConfigValueForId(100032))
                {
                    BackpackManager.getIntance().showMessageTip(OutLineGetMessage.TYPPE_OUT_LINE, "", "" + outGet.toStringWithUnit());
                }
            }
            Debug.Log("============ 大年30修bug  ==================== updateOutTime getOutLine");

            SQLHelper.getIntance().updateOutTime();
        }
    }

    private void initAnimalEvent() {
        mSpriteRender = gameObject.GetComponent<SpriteRenderer>();
        mAnimalControl = new AnimalControlBase(resourceData, mSpriteRender);
        mAnimalControl.setStatueDelayStatue(ActionFrameBean.ACTION_ATTACK, ActionFrameBean.ACTION_STANDY);
        mAnimalControl.addIndexCallBack(ActionFrameBean.ACTION_ATTACK, (int)resourceData.attack_framce, new AnimalStatu.animalIndexCallback(fightEcent));
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
            mCardManager.playerAction();
            mFightManager.attackerAction(id);
        }
    }


    public void addPetAttribute(PlayerAttributeBean date) {
        if (date.type == 100)
        {
            mPetAttribute.aggressivity += date.value;
        }
        else if (date.type == 101)
        {
            mPetAttribute.defense += date.value;
        }
        else if (date.type == 102)
        {
            mPetAttribute.maxBloodVolume += date.value;
        }
        else if (date.type == 110)
        {
            mPetAttribute.rate += (float)date.value;
        }
        else if (date.type == 111)
        {
            mPetAttribute.evd += (float)date.value;
        }
        else if (date.type == 112)
        {
            mPetAttribute.crt += (float)date.value;
        }
        else if (date.type == 113)
        {
            mPetAttribute.crtHurt += date.value;
        }
        else if (date.type == 115)
        {
            mPetAttribute.readHurt += date.value;
        }
        else if (date.type == 114)
        {
            mPetAttribute.attackSpeed += (float)date.value;
        }
    }

    private bool isInit = false;
    public void initEquip(bool isAddSkill)
    {
        initEquip(isAddSkill, true);
    }
    public void initEquip(bool isAddSkill,bool isIniting) {
        //if (isInit && isIniting) {
        //    return;
       // }
        isInit = true;
        List<PlayerBackpackBean> list = InventoryHalper.getIntance().getRoleUseList();
        bloodBili = mBloodVolume/mAttribute.maxBloodVolume;
        bloodDistance = -1;
        mEquipAttribute.clear();
        
        foreach (PlayerBackpackBean bean in list)
        {
            Debug.Log(" bean = " + bean.goodId);
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
                    mEquipAttribute.rate +=(float) date.value;
                }
                else if (date.type == 111)
                {
                    mEquipAttribute.evd += (float)date.value;
                }
                else if (date.type == 112)
                {
                    mEquipAttribute.crt += (float)date.value;
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
                    mEquipAttribute.attackSpeed += (float)date.value;                 
                }
            }
            Debug.Log("addSkill initEquip");
            
            mSkillManager.addSkill(bean, this,SkillIndexUtil.getIntance().getEquitIndexByGoodId(false,bean.sqlGoodId));           
        }
        Debug.Log("initEquip");
        getAttribute(false);
        upDataSpeed();
    }
    private double bloodBili = -1;
    private double bloodDistance = -1;


    public void initEquip() {
        initEquip(true);
    }
    public void upLunhui()
    {
        bloodBili = mBloodVolume / mAttribute.maxBloodVolume;
        bloodDistance = -1;
        mLunhuiAttribute.clear();
        mSkillManager.lunhuiDownCardCost = 0;
        mSkillManager.lunhuiCardHurtPre.clear();
        mSkillManager.lunhuiHurtPre.clear();

        GameManager.getIntance().mLunhuiOnlineGet.clear();
        GameManager.getIntance().mLunhuiOutlineGet.clear();
        GameManager.getIntance().mLunhuiLunhuiGet.clear();

        Dictionary<long, long>  samsaras= InventoryHalper.getIntance().getSamsaraLevelDate();
        Dictionary<long, long>.KeyCollection keys= samsaras.Keys;
        foreach (long key in keys) {
            long level = BaseDateHelper.decodeLong(samsaras[key]) ;
            Debug.Log("level = " + level);
            if(level != 0) {
               long index = SkillIndexUtil.getIntance().getSamIndexBySamId(false, key);
              // mAllAttributePre.delete(index);
                
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
                        mSkillManager.lunhuiHurtPre.AddFloat(index,1 + (float)date.value / 10000);


                    }
                    else if (date.type == 400002)
                    {
                        mAllAttributePre.add(index, AttributePre.aggressivity, date.value);
                    }
                    else if (date.type == 400003)
                    {
                        mAllAttributePre.add(index, AttributePre.maxBloodVolume, date.value);
                    }
                    else if (date.type == 400004)
                    {
                        getAttribute();
                        mAllAttributePre.add(index, AttributePre.defense, date.value);
                        getAttribute();
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
                        mSkillManager.lunhuiCardHurtPre.AddFloat(index, 1 + (float)date.value / 10000);
                    }
                    else if (date.type == 400008)
                    {
                        
                        mSkillManager.lunhuiDownCardCost += date.value;
                    }
                    else if (date.type == 400012)
                    {
                        mAllAttributePre.add(index, AttributePre.attackSpeed, date.value);
                    }
                    else if (date.type == 500001)
                    {
                        GameManager.getIntance().mLunhuiOnlineGet.AddFloat(index,1 + ((float)date.value / 10000));
   
                    }
                    else if (date.type == 500002)
                    {
                        GameManager.getIntance().mLunhuiOutlineGet.AddFloat(index, 1 + ((float)date.value / 10000));
                    }
                    else if (date.type == 500003)
                    {
                        GameManager.getIntance().mLunhuiLunhuiGet.AddFloat(index, 1 + ((float)date.value / 10000));
                    }
                   
                }  
            }
        }
   //     Debug.Log("GameManager.getIntance().mLunhuiOnlineGet =" + GameManager.getIntance().mLunhuiOnlineGet);
    //    Debug.Log("GameManager.getIntance().mLunhuiOutlineGet =" + GameManager.getIntance().mLunhuiOutlineGet);
     //   Debug.Log("GameManager.getIntance().mLunhuiLunhuiGet =" + GameManager.getIntance().mLunhuiLunhuiGet);
      //  Debug.Log("upLunhui");
        getAttribute(false);
        upDataSpeed();
    }
     void getAttribute(bool isGetBili) 
    {
        if (isGetBili) {
            bloodDistance = -1;
            bloodBili = mBloodVolume / mAttribute.maxBloodVolume;
        }
        //        Debug.Log("===============getAttribute. mBloodVolume = "+ mBloodVolume );
        //      Debug.Log("===============getAttribute. mAttribute.maxBloodVolume = " + mAttribute.maxBloodVolume);
        //    Debug.Log("===============getAttribute. bloodBili = " + bloodBili);
        //  Debug.Log("===============getAttribute. bloodDistance = " + bloodDistance);
        mAttribute.clear();
        mAllAttribute.clear();

        mAllAttribute.add(mBaseAttribute);
        Debug.Log("===============mBaseAttribute = " + mBaseAttribute.toString());
        Debug.Log("===============mAttribute.mAllAttribute = " + mAllAttribute.toString());
        mAllAttribute.add(mEquipAttribute);
        Debug.Log("===============mAttribute.mEquipAttribute = " + mEquipAttribute.toString());
        Debug.Log("===============mAttribute.mAllAttribute = " + mAllAttribute.toString());
        mAllAttribute.add(mLunhuiAttribute);
        Debug.Log("===============mAttribute.mLunhuiAttribute = " + mLunhuiAttribute.toString());
        Debug.Log("===============mAttribute.mAllAttribute = " + mAllAttribute.toString());
        mAllAttribute.add(mSkillAttribute);
        Debug.Log("===============mAttribute.mSkillAttribute = " + mSkillAttribute.toString());
        Debug.Log("===============mAttribute.mAllAttribute = " + mAllAttribute.toString());
        mAllAttribute.add(mPetAttribute);
        Debug.Log("===============mAttribute.mSkillAttribute = " + mPetAttribute.toString());
        Debug.Log("===============mAttribute.mAllAttribute = " + mAllAttribute.toString());

        //Debug.Log("===============mAttribute.mEquipAttributePre = " + mEquipAttributePre.toString());
        //Debug.Log("===============mAttribute.mLunhuiAttributePre = " + mLunhuiAttributePre.toString());
        //Debug.Log("===============mAttribute.mSkillAttributePre = " + mSkillAttributePre.toString());
        //Debug.Log("===============mAttribute.mAllAttributePre = " + mAllAttributePre.toString());


        mAttribute.add(mAllAttribute);
        //Debug.Log("===============mAttribute.mAttribute = " + mAllAttribute);
        Debug.Log("===============mAttribute.mAllAttributePre = " + mAttribute.toString());
        mAttribute.chen(mAllAttributePre.getAll());
        Debug.Log("===============mAttribute.mAllAttributePre = " + mAllAttributePre.getAll().toString());
        //Debug.Log("===============mAttribute.mAttribute = " + mAttribute);
        if (bloodDistance != -1)
        {
            mBloodVolume = mAttribute.maxBloodVolume - bloodDistance;
        }
        else
        {
            mBloodVolume = mAttribute.maxBloodVolume * bloodBili;
        }

        GameManager.getIntance().setBlood(mBloodVolume, mAttribute.maxBloodVolume);
    }


    public override  void getAttribute() {
        getAttribute(true);
    }
    public void changePetStatu(PlayerBackpackBean bean, long  type) {
        if (type == SQLDate.GOOD_TYPE_USER_PET)
        {
            mSkillManager.addSkill(bean, this, SkillIndexUtil.getIntance().getEquitIndexByGoodId(false,bean.sqlGoodId));
        }
        else
        {
            mSkillManager.removeSkill(bean);
            SkillIndexUtil.getIntance().deleteEquitIndexByGoodId(false, bean.sqlGoodId);
        }
    }


    public void ChangeEquip(PlayerBackpackBean bean,bool isAdd)
    {
        bloodBili = mBloodVolume / mAttribute.maxBloodVolume;
        bloodDistance = -1;
        mEquipAttribute.clear();
        List<PlayerBackpackBean> list = InventoryHalper.getIntance().getRoleUseList();
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
                    mEquipAttribute.rate += (float)date.value;
                }
                else if (date.type == 111)
                {
                    mEquipAttribute.evd += (float)date.value;
                }
                else if (date.type == 112)
                {
                    mEquipAttribute.crt += (float)date.value;
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
                    mEquipAttribute.attackSpeed += (float)date.value;
                }
            }
            Debug.Log("addSkill ChangeEquip");          
        }
        if (isAdd)
        {
            mSkillManager.addSkill(bean, this, SkillIndexUtil.getIntance().getEquitIndexByGoodId(false, bean.sqlGoodId));
        }
        else
        {
            mSkillManager.removeSkill(bean);
            SkillIndexUtil.getIntance().deleteEquitIndexByGoodId(false, bean.sqlGoodId);
        }
        Debug.Log("ChangeEquip");
        getAttribute(false);
        upDataSpeed();
    }
    public void heroUp() {
        mLevelAnimalControl.levelUp();
        setHeroData();
    }


    private long mVoication = -1;
    private PlayerBackpackBean mVocationBean = new PlayerBackpackBean();
    public void vocation() {
        if ( SQLHelper.getIntance().mPlayVocation != mVoication) {
            long skill = JsonUtils.getIntance().getVocationById(mVoication).skill;
            if (mSkillManager!= null && skill != -1) {
                mSkillManager.removeSkill(skill);
            }
        }

        resourceData = null;
        setHeroData();

    }

    public void setHeroData()
    {
        if (mBloodVolume > 0)
        {
            bloodBili = -1;
            bloodDistance = mAttribute.maxBloodVolume - mBloodVolume;
        }
        else {
            bloodBili = -1;
            bloodDistance = 0;
        }
        
        Hero mHero = JsonUtils.getIntance().getHeroData();
        if (resourceData == null) {
            mVoication = SQLHelper.getIntance().mPlayVocation;
            if (mVoication == -1) {
                mVoication = 1;
            }
            resourceData = JsonUtils.getIntance().getEnemyResourceData(
                JsonUtils.getIntance().getVocationById(mVoication).resource);
            initAnimalEvent();
            VocationDecBean bean = JsonUtils.getIntance().getVocationById(mVoication);
            long skill = bean.skill;
            if (skill != -1)
            {
                mSkillManager.addSkill(skill,this,false,SkillIndexUtil.getIntance().getVocationIndexByVocationId(false,mVoication));
            }
            mAttackLeng = bean.attack_range * resourceData.zoom;
            GameObject.Find("hero").GetComponent<HeroRoleControl>().vocation();
        }
        double mMaxTmp = mBaseAttribute.maxBloodVolume;
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

        mLocalBean = new LocalBean (transform.position.x, transform.position.y,mAttackLeng,true,this);
		mState = new HeroState (this);
        Debug.Log("setHeroData");
        getAttribute(false);
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
                getOutLine();
                GameObject.Find("Manager").GetComponent<AdManager>().initAd();
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
//        Debug.Log("================== getStatus()  =" +getStatus()+ " mFightManager.mHeroStatus="+ mFightManager.mHeroStatus);
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
        if(mAllAttributePre == null) {
            mAllAttributePre = new AttributePre(this);
        }    
        mAttackType = Attacker.ATTACK_TYPE_HERO;
        startComment();
        isStart = true;
        mLevelManager = GameObject.Find("Manager").GetComponent<LevelManager>();
        mBackManager = mLevelManager.getBackManager();
        mCardManager = GameObject.Find("jineng").GetComponent<HeroCardManager>();
        mBackManager.stop();
    }

    void winrun(){
        //      Debug.Log(" run ");
        if (isWinEnd || isStart) {
            transform.Translate(Vector2.right * (2 * Time.deltaTime));
        }
        
	}
	public override double BeAttack(HurtStatus status,Attacker hurter){
        status.blood = status.blood * hurter.mSkillManager.getHurtPre();
		return allHurt(status, hurter);
	}
    public double allHurt(HurtStatus status, Attacker hurter)
    {
        if (JsonUtils.getIntance().getConfigValueForId(100007) != 1)
        {
            mSkillManager.mEventAttackManager.allHurt(hurter, status);
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
    public override double BeKillAttack(double value,Attacker hurt)
    {
        HurtStatus status = new HurtStatus(value, HurtStatus.TYPE_DEFAULT);
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
        else if (hurt != null && hurt.mAttackType != ATTACK_TYPE_HERO)
        {
            status.blood = status.blood * hurt.mSkillManager.getCardHurtPre();
        }
        return allHurt(status, hurt);
    }
    public override void AddBlood(double value) {
        Debug.Log("  play add blood =" + value);
        if (mBloodVolume + value > mAttribute.maxBloodVolume)
        {
            value = mAttribute.maxBloodVolume - mBloodVolume;
        }
        mBloodVolume = mBloodVolume + value;
        GameManager.getIntance().setBlood(mBloodVolume, mAttribute.maxBloodVolume);
        mState.add(value);
    }
}
