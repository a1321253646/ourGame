using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager
{
	public long mCurrentLevel = BaseDateHelper.encodeLong( 1);
	public long mHeroLv = BaseDateHelper.encodeLong(1);
	public float mCurrentGas = 0;
	public BigNumber mCurrentCrystal = new BigNumber();
	public bool mStartBoss = false;
	public float startBossGas = 0;
	public BigNumber upLevelCrystal;
	public long mBossId;
	public bool mHeroIsAlive;
	public bool isEnd = false;
	public UiManager uiManager;
	public bool isLvUp = false;
    public bool isInit = false;
    public LevelManager mLevelManage= null;
    public BigNumber mReincarnation = new BigNumber();
    public bool isShowQieHuang = false;
    public bool isWinQirHuang = false;
    public bool isLuihuiIng = false;
    public bool isError = false;
    public static long ERROR_TIME_MIN = 86400000l;

    public List<RankingListDateBean> mRankingList = null;
    public bool mRankingListUpdate = false;


    public int mInitStatus = -1;//0为开始初始化 1 为开始读取json文件 2为json文件读取完成 3为开始处理本地数据库 4 为本地数据库处理完成 5为开始同步网络数据库 
                                //6为网络数据库同步完成  7 将网络数据同步到本地 8 完成网络数据本地化 9为存档读取 10读取存档完成
    public string mInitDec = "";
    public float mLunhuiOnlineGet = 0;
    public float mLunhuiOutlineGet = 0;
    public float mLunhuiLunhuiGet = 0;

    public float mCardOnlineGet = 0;
    public float mCardOutlineGet = 0;
    public float mCardLunhuiGet = 0;

    public long isShowCardPoint = 2;
    public long isShowBackpackPoint = 2;
    public long isShowLuihuiPoint = 2;
    public long isShowPlayerPoint = 2;

    public static bool isAdd = false;
    public static bool isTest = false;


    public static long mVersionCode = 25;
    public static long mAPKVersionCode = 33;
    public  long mNewAPKVersionCode = -1;
    public  long mIsMust = -1;//1为必须，0为提醒
    public string mUpdateStr = null;
    public bool isHaveNoteUpdate = false;


    public  bool mAllSaleGreed = false;
    public  bool mAllSaleBlue = false;
    public  bool mAllSalePurple = false;
    public  bool mAllSaleOrange = false;

    public  bool mIsNeedToReReadAboutLevel = false;


    public string mGameErrorString = "";

    public float getOnlineGet() {
        return 1+ mLunhuiOnlineGet+ mCardOnlineGet;
    }
    public float getOutlineGet()
    {
        return 1+ mLunhuiOutlineGet+ mCardOutlineGet;
    }
    public float getLunhuiGet()
    {
        return 1+mLunhuiLunhuiGet+ mCardLunhuiGet;
    }
    private GameManager(){
    }
	private static GameManager mIntance = new GameManager();
	public static GameManager getIntance(){
		return mIntance;
	}

    public bool isAddGoodForTest = false;

    public void reStart() {
        isEnd = false;
        isLuihuiIng = false;
        getLevelData();
        init(null);
        uiManager.reset();
    }

	public void getLevelData(){
        Hero hero = JsonUtils.getIntance ().getHeroData ();
        upLevelCrystal = hero.getLvupCrystal();
    }
    private int uiLevel = 500;
    public int getUiCurrentLevel() {
        return uiLevel;
    }
    public int getUiLevel() {
        return ++uiLevel;
    }
    public bool isHaveOutGet = true;

    public GuideManager getGuideManager() {
        return mLevelManage.mGuideManager;    
    }
	public long init(LevelManager levelmanage){
        long old = 0;
        if (!isInit)
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            isInit = true;
            mCurrentLevel = BaseDateHelper.encodeLong((long)JsonUtils.getIntance().getConfigValueForId(100010)) ;
            if (mCurrentLevel == BaseDateHelper.encodeLong(-1)) {
                mCurrentLevel = SQLHelper.getIntance().mGameLevel;
                if (mCurrentLevel == BaseDateHelper.encodeLong(-9999)) {
                    mCurrentLevel = BaseDateHelper.encodeLong(-(long)JsonUtils.getIntance().getConfigValueForId(100019)+1) ;
                }
                else if (BaseDateHelper.decodeLong(mCurrentLevel) > (long)JsonUtils.getIntance().getConfigValueForId(100042))
                {
                    mCurrentLevel = BaseDateHelper.encodeLong( (long)JsonUtils.getIntance().getConfigValueForId(100042));
                    SQLHelper.getIntance().updateGameLevel(GameManager.getIntance().mCurrentLevel);
                }
            }
            mHeroLv = BaseDateHelper.encodeLong((long)JsonUtils.getIntance().getConfigValueForId(100011)) ;
            if (mHeroLv == BaseDateHelper.encodeLong(-1))
            {
                mHeroLv = SQLHelper.getIntance().mHeroLevel;
                if (mHeroLv == BaseDateHelper.encodeLong(-1))
                {
                    mHeroLv = BaseDateHelper.encodeLong(1);
                }
            }
            if (isTest)
            {
                mCurrentCrystal = BigNumber.getBigNumForString("24E+22");
                
            }
            else {
                mCurrentCrystal = SQLHelper.getIntance().mMojing;
                //mCurrentCrystal = BigNumber.getBigNumForString("0");
            }
            long auto = SQLHelper.getIntance().isAutoBoss;
            if (auto == -1 || auto == 1) {
                isAuto = false;
            }
            else {
                isAuto = true;  
            }
            if (isTest)
            {
                mReincarnation = BigNumber.getBigNumForString("2.1E+40");
            }
            else
            {
                mReincarnation = SQLHelper.getIntance().mLunhuiValue;
              //  mReincarnation = BigNumber.getBigNumForString("2.1E+40");
            }
        //    mReincarnation = SQLHelper.getIntance().mLunhuiValue;

            isShowPlayerPoint = SQLHelper.getIntance().isShowPlayerPoint;
            isShowBackpackPoint = SQLHelper.getIntance().isShowBackpackPoint;
            isShowLuihuiPoint = SQLHelper.getIntance().isShowLuihuiPoint;
            isShowCardPoint = SQLHelper.getIntance().isShowCardPoint;
        }
       // mCurrentLevel = 1;

        Level level = JsonUtils.getIntance ().getLevelData ();
		startBossGas = level.boss_gas;
		mBossId = level.boss_DI;
		mCurrentGas = 0;
        if (levelmanage != null) {
            mLevelManage = levelmanage;
            uiLevel = 99;
        }

        getLevelData();
        
        return 0;
    }

    private long mMosterDealHuijingBili = 0;
    private long mMosterDealLunhuiBili = 0;
    private long mMosterDealAllBili = 0;


    public void initUi(){
        mMosterDealHuijingBili = (long)JsonUtils.getIntance().getConfigValueForId(100045);
        mMosterDealLunhuiBili = (long)JsonUtils.getIntance().getConfigValueForId(100046);
        mMosterDealAllBili = mMosterDealHuijingBili + mMosterDealLunhuiBili;
        isEnd = false;
		uiManager = new UiManager ();
		uiManager.init ();
	}

    private void playAd() {
        if (!GameObject.Find("Manager").GetComponent<AdManager>().isReadyToShow()) {

            return;
        }
        int range = Random.Range(0, 1000);
        Debug.Log("==========================playAd range= " + range);
        long type = -1;
        string value = null;
        if (range < mMosterDealHuijingBili)
        {
            type = ActiveButtonControl.TYPE_AD_HUIJING;
            LevelManager level = GameObject.Find("Manager").GetComponent<LevelManager>();
            long time =(long) JsonUtils.getIntance().getConfigValueForId(100049);
            value = level.mFightManager.attckerOutLine(level.mPlayerControl, time * 60*1000, GameManager.getIntance().getOnlineGet()).toString();

        }
        else if (range < mMosterDealAllBili) {
            type = ActiveButtonControl.TYPE_AD_LUNHUI;
            
            value = JsonUtils.getIntance().getLevelData(BaseDateHelper.decodeLong(GameManager.getIntance().mCurrentLevel)).
                lunhui;
            BigNumber bigValue = BigNumber.getBigNumForString(value);
            bigValue = BigNumber.multiply(bigValue, GameManager.getIntance().getLunhuiGet());
            value = bigValue.toString();

        }
        if (type != -1) {
            Debug.Log("==========================playAd type= " + type + " value=" + value);
            GameObject.Find("active_button_list").GetComponent<ActiveListControl>().showAd(type, value,true);
        }

    }

	public void setBlood(double blood,double max){
        uiManager.changeHeroBlood (blood,max);
	}
	public void heroUp(){
        GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_CLICK_BUTTON, GuideManager.BUTTON_START_HERO_UP);
        mHeroLv  = BaseDateHelper.encodeLong(BaseDateHelper.decodeLong(mHeroLv) + 1);
        
        mCurrentCrystal =BigNumber.minus(mCurrentCrystal, upLevelCrystal) ;
       
        getLevelData ();
		if (mCurrentCrystal.ieEquit(upLevelCrystal) != -1  ) {
			uiManager.showLevelUp (true);
		} else {
			uiManager.showLevelUp (false);
		}
		uiManager.refreshData ();
        //修改数据到英雄
        mLevelManage.heroUp();
        BackpackManager.getIntance().updateHeroControl();
        uiManager.showVocation(true);

    }
	public void startBoss(){
		mStartBoss = true;
	}

	public void enemyDeal(Attacker enemy){
        if (isLuihuiIng) {
            return;
        }
        mCurrentGas += enemy.mDieGas;
        //Debug.Log("=============enemy.mDieCrysta=" + enemy.mDieCrysta.toString());
        //Debug.Log("=============mCurrentCrystal=" + mCurrentCrystal.toString());
        //Debug.Log("============= getOnlineGet()=" + getOnlineGet());
        BigNumber dealHunjin = mLevelManage.mPlayerControl.mSkillManager.mEventAttackManager.getDieHuijing(enemy.mDieCrysta);
        if (getOnlineGet() != 1)
        {
            mCurrentCrystal = BigNumber.add(mCurrentCrystal, BigNumber.multiply(dealHunjin, getOnlineGet()));
        }
        else {
            mCurrentCrystal = BigNumber.add(mCurrentCrystal, dealHunjin);
        }
       // Debug.Log("=============mCurrentCrystal=" + mCurrentCrystal.toString());

        uiManager.addGas();
        if (enemy is EnemyBase) {


            EnemyBase tmp = (EnemyBase)enemy;
            int count = 1;
            if (mLevelManage.mPlayerControl.mSkillManager.mEventAttackManager.endGetDrop()) {
                count = 2;
            }
            for (int i = 0; i < count; i++) {
                List<FellObjectBean> list = tmp.mData.fell();
                if (list != null && list.Count > 0)
                {
                    foreach (FellObjectBean bean in list)
                    {
                        Debug.Log("怪物死亡掉落 " + bean.id);
                        BackpackManager.getIntance().addGoods(bean.id, (int)bean.count);
                        string path = InventoryHalper.getIntance().getIcon(bean.id);
                        showDIaoLuo((EnemyBase)enemy, DiaoluoDonghuaControl.GOOD_DIAOLUO_TYPE, path, 0, bean.id);
                    }
                }
                getGuideManager().eventNotification(GuideManager.EVENT_ENEMY_DEAL, tmp.mData.id);
            }
            if (enemy.mAttackType == Attacker.ATTACK_TYPE_BOSS)
            {
                if (JsonUtils.getIntance().isHavePet())
                {
                    List<PetJsonBean> jsons = JsonUtils.getIntance().getPet();
                    foreach (PetJsonBean j in jsons)
                    {
                        if (j.activateLevel == BaseDateHelper.decodeLong(mCurrentLevel))
                        {
                            List<PlayerBackpackBean> list = InventoryHalper.getIntance().getPet();
                            bool isHave = false;
                            foreach (PlayerBackpackBean p in list)
                            {
                                if (p.goodId == j.id)
                                {
                                    isHave = true;
                                    break;
                                }
                            }
                            if (!isHave)
                            {
                                InventoryHalper.getIntance().addInventory(j.id, 1);
                                GameObject.Find("hero").GetComponent<HeroRoleControl>().showPetTable();
                                GameObject.Find("Manager").GetComponent<PetManager>().addPet(j.id);
                                uiManager.setRolePointShow(1);
                            }
                            break;
                        }
                    }
                }
            }
            else {
                playAd();
            }
        }
        showDIaoLuo((EnemyBase)enemy, DiaoluoDonghuaControl.SHUIJI_DIAOLUO_TYPE, "", mCurrentGas);
        
	}
    public void updateGasAndCrystal() {
        SQLHelper.getIntance().updateHunJing(mCurrentCrystal);
        if (uiManager != null) {
            uiManager.addGasAndCrystal();
        }
        
        BackpackManager.getIntance().upDataComposeControl();
    }
    private void showDIaoLuo(EnemyBase e, int type, string path, float count,long id)
    {
        GameObject obj = Object.Instantiate(Resources.Load("prefab/diaoluodonghua")) as GameObject;//Resources.Load<GameObject>();
        obj.transform.SetParent(GameObject.Find("enemyStatePlane").transform);
        obj.transform.localScale = new Vector3(1, 1, 1);
        DiaoluoDonghuaControl control = obj.GetComponent<DiaoluoDonghuaControl>();
        control.init(e, type, path, count,id);
    }

    public void showDIaoLuo(Vector2 e, int type, string path, float count, long id) {
        showDIaoLuo(e,type, path, count, id, false);
    }

    public void showDIaoLuo(Vector2 e, int type, string path, float count, long id,bool isTop)
    {
        GameObject obj = Object.Instantiate(Resources.Load("prefab/diaoluodonghua")) as GameObject;//Resources.Load<GameObject>();
        
        
        if (isTop)
        {
            obj.transform.SetParent(GameObject.Find("Canvas").transform);
        }
        else {
            obj.transform.SetParent(GameObject.Find("enemyStatePlane").transform);
        }
        obj.transform.localScale = new Vector3(1, 1, 1);
        DiaoluoDonghuaControl control = obj.GetComponent<DiaoluoDonghuaControl>();
        control.init(e, type, path, count, id);
    }

    private void  showDIaoLuo(EnemyBase e,int type,string path,float count) {
        showDIaoLuo(e, type, path, count,-1);
    }

    public void updataGasAndCrystal() {
        uiManager.addGasAndCrystal();
    }
    private bool isAuto = false;
    public bool isGuide = false;

    public void setIsAutoBoss(bool auto) {
        isAuto = auto;
        SQLHelper.getIntance().updateAutoBoss(isAuto ? 2 : 1);
    }
    public bool gettIsAutoBoss()
    {
        return isAuto;
    }
  //  private bool isPlayBgm = false;
    public void playBgm(AudioSource source,string bgm) {
/*        if(isPlayBgm)
        {
            return;
        }*/
//        isPlayBgm = false;
        AudioClip clip = Resources.Load(bgm) as AudioClip;
        source.clip = clip;
        source.loop = true;
        source.Play();
        long isVoice = SQLHelper.getIntance().isVoice;
        if (isVoice == -1 || isVoice == 1)
        {
            setVoice(source, true, false);
        }
        else {
            setVoice(source, false, false);
        }
    }
    public void setVoice(AudioSource source,bool isVoice,bool isSave) {
        Debug.Log("setVoice");
        if (isVoice)
        {
            source.volume = 1;
        }
        else {
            source.volume = 0;
        }
        if (isSave) {
            Debug.Log("setVoice save isVoice= "+ isVoice);
            SQLHelper.getIntance().updateIsVoice(isVoice ? 1 : 2);
        }
    }
}

