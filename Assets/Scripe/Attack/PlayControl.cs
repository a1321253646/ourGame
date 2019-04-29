using System.Collections;
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

            BackpackManager.getIntance().addGoods(4000001, count);
            BackpackManager.getIntance().addGoods(4000002, count);
            BackpackManager.getIntance().addGoods(4000003, count);

        }
        getOutLine();
        if (AdIntance.getIntance().getType() != -1)
        {
            long type = AdIntance.getIntance().getType();

            GameObject.Find("active_button_list").GetComponent<ActiveListControl>().showAd(type, true);
        }
    }

    private bool isFristStart = true;

    public void resetHero() {
        isBeAttacker = false;
        if(mSkillManager != null) {
            mSkillManager.removeAllSkill();
            mSkillManager.cardCardHurtPre.clear();
            mSkillManager.carHurtPre.clear();
            mSkillManager.removeAllVocationSkill();
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

        if (SQLHelper.getIntance().mVersionCode < GameManager.mVersionCode)
        {

            GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().
                showUi(JsonUtils.getIntance().getStringById(100038));
            UiControlManager.getIntance().show(UiControlManager.TYPE_LUIHUI);
            Time.timeScale = 0;
            return;

        }

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
       
        if (JsonUtils.getIntance().getConfigValueForId(100055) == 1)
        {
            GameObject.Find("attribute_show").GetComponent<AttributeShowManager>().showEnemy(this);
        }

        Vector3 tmp = PointUtils.screenTransToWorld(GameObject.Find("kapai_local_top").transform.position);
        Vector3 tmp2 = PointUtils.screenTransToWorld(GameObject.Find("kapai_local_bottom").transform.position);
        float cardTop = tmp.y - tmp2.y;
        MapConfigBean mMapConfig = JsonUtils.getIntance().getMapConfigByResource(
            JsonUtils.getIntance().getLevelData().map);
        float yBase = mMapConfig.y_base;
        yBase = cardTop + mMapConfig.y_base;
        transform.position = new Vector2(transform.position.x,yBase- resourceData.idel_y) ;
        GameObject.Find("hero").GetComponent<HeroRoleControl>().upDateUi();
    }


    private void getOutLine() {

        if (SQLHelper.getIntance().mVersionCode < GameManager.mVersionCode) {
            return;
        }


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

                if (old2 > JsonUtils.getIntance().getConfigValueForId(100032) && SQLHelper.getIntance().mVersionCode >= GameManager.mVersionCode)
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
        if (mAttackerTargets == null || mAttackerTargets.Count < 1) {
            setStatus(Attacker.PLAY_STATUS_RUN);
            mFightManager.mHeroStatus = Attacker.PLAY_STATUS_RUN;
        }
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
        mSkillManager.removeAllLunhuiSkill();
        GameManager.getIntance().mLunhuiOnlineGet.clear();
        GameManager.getIntance().mLunhuiOutlineGet.clear();
        GameManager.getIntance().mLunhuiLunhuiGet.clear();

        Dictionary<long, long>  samsaras= InventoryHalper.getIntance().getSamsaraLevelDate();
        Dictionary<long, long>.KeyCollection keys= samsaras.Keys;
        foreach (long key in keys) {
            long level = BaseDateHelper.decodeLong(samsaras[key]) ;
//            Debug.Log("level = " + level);
            if(level != 0) {
               long index = SkillIndexUtil.getIntance().getSamIndexBySamId(false, key);
              // mAllAttributePre.delete(index);
                
               SamsaraJsonBean sam=  JsonUtils.getIntance().getSamsaraInfoById(key);
               List<SamsaraValueBean> beanValue = sam.levelList[level];
                foreach (SamsaraValueBean date in beanValue) {
                    if (date.type > 500003 && mSkillManager.addLunhuiKill(date.type,this, index)) {
                        continue;
                    }
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
 //                       Debug.Log("mAllAttributePre aggressivity = " + mAllAttributePre.getAll().aggressivity);
                        mAllAttributePre.delete(index);
 //                       Debug.Log("mAllAttributePre aggressivity = " + mAllAttributePre.getAll().aggressivity);
                        mAllAttributePre.add(index, AttributePre.aggressivity, date.value);
 //                       Debug.Log("mAllAttributePre aggressivity = " + mAllAttributePre.getAll().aggressivity);

                    }
                    else if (date.type == 400003)
                    {
   //                     Debug.Log("mAllAttributePre maxBloodVolume = " + mAllAttributePre.getAll().maxBloodVolume);
                        mAllAttributePre.delete(index);
   //                     Debug.Log("mAllAttributePre maxBloodVolume = " + mAllAttributePre.getAll().maxBloodVolume);
                        mAllAttributePre.add(index, AttributePre.maxBloodVolume,date.value);
   //                     Debug.Log("mAllAttributePre maxBloodVolume = " + mAllAttributePre.getAll().maxBloodVolume);
                    }
                    else if (date.type == 400004)
                    {
                        //getAttribute(true);
     //                   Debug.Log("mAllAttributePre defense = " + mAllAttributePre.getAll().defense);
                        mAllAttributePre.delete(index);
       //                 Debug.Log("mAllAttributePre defense = " + mAllAttributePre.getAll().defense);
                        mAllAttributePre.add(index, AttributePre.defense, date.value);
         //               Debug.Log("mAllAttributePre defense = " + mAllAttributePre.getAll().defense);
                        //getAttribute(true);
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
     public override void getAttributeEnd() 
    {
        GameManager.getIntance().setBlood(mBloodVolume, mAttribute.maxBloodVolume);
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
        resourceData = null;
        setHeroData();
    }
    private void vocationDealSkill() {
        mSkillManager.removeAllVocationSkill();
        foreach (long index in SQLHelper.getIntance().mPlayVocation.Keys)
        {
//            Debug.Log("vocationDealSkill  index ==" + index + "  vocation= " + SQLHelper.getIntance().mPlayVocation[index]);
            VocationDecBean v = JsonUtils.getIntance().getVocationById(SQLHelper.getIntance().mPlayVocation[index]);
            if (v != null && v.skill != -1)
            {
                mSkillManager.addVocationSkill(v.skill, this, false, SkillIndexUtil.getIntance().getVocationIndexByVocationId(false, mVoication));
            }

        }
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
        Debug.Log("before hero mBaseAttribute= " + mBaseAttribute.toString());
        Hero mHero = JsonUtils.getIntance().getHeroData();
        if (resourceData == null) {
            mVoication = SQLHelper.getIntance().mCurrentVocation;
            if (mVoication == -1) {
                mVoication = 1;
            }


            //mVoication = 43003;

            resourceData = JsonUtils.getIntance().getEnemyResourceData(
                JsonUtils.getIntance().getVocationById(mVoication).resource);
            vocationDealSkill();
            initAnimalEvent();
            VocationDecBean bean = JsonUtils.getIntance().getVocationById(mVoication);
            mAttackLeng = bean.attack_range * resourceData.zoom;
            GameObject.Find("hero").GetComponent<HeroRoleControl>().vocation();
        }
        double mMaxTmp = mBaseAttribute.maxBloodVolume;
        if (mBloodVolume < 0) {
            mBloodVolume = 0;
//            Debug.Log("mBloodVolume = " + mBloodVolume);
        }
//        Debug.Log("mMaxTmp = " + mMaxTmp);
//        Debug.Log("mBloodVolume = " + mBloodVolume);
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
        Debug.Log("hero mBaseAttribute= " + mBaseAttribute.toString());
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
        if (getStatus() == Attacker.PLAY_STATUS_RUN && !mBackManager.isRun && !isStart) {
            mBackManager.move();
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
  //      Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>run<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
        if (isWinEnd || isStart) {
            transform.Translate(Vector2.right * (JsonUtils.getIntance().getConfigValueForId(100057) * Time.deltaTime));
        }
        
	}
	public override double BeAttack(HurtStatus status,Attacker hurter){
        status.blood = status.blood * hurter.mSkillManager.getHurtPre();
		return allHurt(status, hurter);
	}
    public override double allHurt(HurtStatus status, Attacker hurter)
    {
        if (GameManager.getIntance().isLunhuiWudiIng) {
            return status.blood;
        }
        mSkillManager.mEventAttackManager.allHurt(hurter, status);
        mBloodVolume = mBloodVolume - status.blood;
        GameManager.getIntance().setBlood(mBloodVolume, mAttribute.maxBloodVolume);
        if (mBloodVolume <= 0) {
            mSkillManager.mEventAttackManager.befroeDie();
        }
        if (mBloodVolume <= 0)
        {
            if (JsonUtils.getIntance().getConfigValueForId(100007) != 1)
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
//        Debug.Log("  play add blood =" + value);
        if (mBloodVolume + value > mAttribute.maxBloodVolume)
        {
            value = mAttribute.maxBloodVolume - mBloodVolume;
        }
        mBloodVolume = mBloodVolume + value;
        GameManager.getIntance().setBlood(mBloodVolume, mAttribute.maxBloodVolume);
        mState.add(value);
    }


}
