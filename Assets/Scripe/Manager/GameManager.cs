using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager
{
	public long mCurrentLevel = 1;
	public long mHeroLv = 1;
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
    public long mReincarnation = 0;
    public bool isShowQieHuang = false;
    public bool isWinQirHuang = false;
    private GameManager(){
    }
	private static GameManager mIntance = new GameManager();
	public static GameManager getIntance(){
		return mIntance;
	}

    public bool isAddGoodForTest = false;

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
    private bool isHaveOutGet = true;

    public GuideManager getGuideManager() {
        return mLevelManage.mGuideManager;    
    }

	public BigNumber init(LevelManager levelmanage){
        BigNumber outLineGet = new BigNumber();
        if (!isInit)
        {
            isInit = true;
            mCurrentLevel = (long)JsonUtils.getIntance().getConfigValueForId(100010);
            if (mCurrentLevel == -1) {
                mCurrentLevel = SQLHelper.getIntance().mGameLevel;
                if (mCurrentLevel == -9999) {
                    mCurrentLevel = -(long)JsonUtils.getIntance().getConfigValueForId(100019)+1;
                }
            }
            mHeroLv = (long)JsonUtils.getIntance().getConfigValueForId(100011);
            if (mHeroLv == -1)
            {
                mHeroLv = SQLHelper.getIntance().mHeroLevel;
                if (mHeroLv == -1)
                {
                    mHeroLv = 1;
                }
            }

            mCurrentCrystal = SQLHelper.getIntance().mMojing;
               
            if (isHaveOutGet) {
                isHaveOutGet = false;
                long old = SQLHelper.getIntance().mOutTime;
                if (old != -1) {
                    old = TimeUtils.getTimeDistanceMin(old);
                    BigNumber levelCryStal = JsonUtils.getIntance().getLevelData(mCurrentLevel).getOfflinereward();
                    outLineGet = BigNumber.multiply(levelCryStal, old);
                    mCurrentCrystal = BigNumber.add(outLineGet,mCurrentCrystal);
                    if(old <  5) {
                        outLineGet = new BigNumber();
                    }
                    SQLHelper.getIntance().updateHunJing(mCurrentCrystal);
                }
            }
            
            long auto = SQLHelper.getIntance().isAutoBoss;
            if (auto == -1 || auto == 1) {
                isAuto = false;
            }
            else {
                isAuto = true;  
            }
            mReincarnation = SQLHelper.getIntance().mLunhuiValue;
            if (mReincarnation == -1) {
                mReincarnation = 0;
            }
        }
       // mCurrentLevel = 1;
        Level level = JsonUtils.getIntance ().getLevelData ();
		startBossGas = level.boss_gas;
		mBossId = level.boss_DI;
		mCurrentGas = 0;
        mLevelManage = levelmanage;
        uiLevel = 99;
        getLevelData();
        return outLineGet;
    }

	public void initUi(){
        isEnd = false;
		uiManager = new UiManager ();
		uiManager.init ();
	}

	public void setBlood(float blood,float max){
		uiManager.changeHeroBlood (blood,max);
	}
	public void heroUp(){
		mHeroLv += 1;
        mCurrentCrystal =BigNumber.minus(mCurrentCrystal, upLevelCrystal) ;
        SQLHelper.getIntance().updateHunJing(mCurrentCrystal);
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

    }
	public void startBoss(){
		mStartBoss = true;
	}

	public void enemyDeal(Attacker enemy){
        mCurrentGas += enemy.mDieGas;
        mCurrentCrystal = BigNumber.add(mCurrentCrystal, enemy.mDieCrysta); 
        uiManager.addGas();
        if (enemy is EnemyBase) {
            EnemyBase tmp = (EnemyBase)enemy;
            List<FellObjectBean> list = tmp.mData.fell();
            if (list != null && list.Count > 0) {
                foreach (FellObjectBean bean in list) {                
                    Debug.Log("怪物死亡掉落 " + bean.id);
                    BackpackManager.getIntance().addGoods(bean.id, (int)bean.count);
                    string path = InventoryHalper.getIntance().getIcon(bean.id);
                    showDIaoLuo((EnemyBase)enemy, DiaoluoDonghuaControl.GOOD_DIAOLUO_TYPE, path,0, bean.id);
                }
            }
            getGuideManager().eventNotification(GuideManager.EVENT_ENEMY_DEAL, tmp.mData.id);
        }
        showDIaoLuo((EnemyBase)enemy, DiaoluoDonghuaControl.SHUIJI_DIAOLUO_TYPE, "", mCurrentGas);
        
	}
    public void updateGasAndCrystal() {
        SQLHelper.getIntance().updateHunJing(mCurrentCrystal);
        uiManager.addGasAndCrystal();
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
}

