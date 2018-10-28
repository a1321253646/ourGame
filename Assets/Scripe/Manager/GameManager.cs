using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager
{
	public long mCurrentLevel = 1;
	public long mHeroLv = 1;
	public float mCurrentGas = 0;
	public float mCurrentCrystal = 0;
	public bool mStartBoss = false;
	public float startBossGas = 0;
	public float upLevelCrystal = 0;
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
        upLevelCrystal = hero.lvup_crystal;
    }
    private int uiLevel = 500;
    public int getUiCurrentLevel() {
        return uiLevel;
    }
    public int getUiLevel() {
        return ++uiLevel;
    }
	public void init(LevelManager levelmanage){
        if (!isInit)
        {
            isInit = true;
            mCurrentLevel = (long)JsonUtils.getIntance().getConfigValueForId(100010);
            if (mCurrentLevel == -1) {
                mCurrentLevel = SQLHelper.getIntance().mGameLevel;
                if (mCurrentLevel == -1) {
                    mCurrentLevel = 1;
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
            mCurrentCrystal = (long)JsonUtils.getIntance().getConfigValueForId(100012);
            if (mCurrentCrystal == -1)
            {
                mCurrentCrystal = SQLHelper.getIntance().mMojing;
                if (mCurrentCrystal == -1)
                {
                    mCurrentCrystal = 0;
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
        mCurrentCrystal = mCurrentCrystal - upLevelCrystal ;
        SQLHelper.getIntance().updateHunJing((long)mCurrentCrystal);
        getLevelData ();
		if (mCurrentCrystal >= upLevelCrystal) {
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
        if (enemy is EnemyBase) {
            EnemyBase tmp = (EnemyBase)enemy;
            List<FellObjectBean> list = tmp.mData.fell();
            if (list != null && list.Count > 0) {
                foreach (FellObjectBean bean in list) {
                    Debug.Log("怪物死亡掉落 " + bean.id);
                    BackpackManager.getIntance().addGoods(bean.id, (int)bean.count);
                }
            }
            
        }
		mCurrentGas += enemy.mDieGas*JsonUtils.getIntance().getConfigValueForId(100009);
		mCurrentCrystal += enemy.mDieCrysta * JsonUtils.getIntance().getConfigValueForId(100008);
        SQLHelper.getIntance().updateHunJing((long)mCurrentCrystal);
        uiManager.addGasAndCrystal ();
        BackpackManager.getIntance().upDataComposeControl();
	}
    public void updataGasAndCrystal() {
        uiManager.addGasAndCrystal();
    }
    private bool isAuto = false;
    public void setIsAutoBoss(bool auto) {
        isAuto = auto;
        SQLHelper.getIntance().updateAutoBoss(isAuto ? 2 : 1);
    }
    public bool gettIsAutoBoss()
    {
        return isAuto;
    }
}

