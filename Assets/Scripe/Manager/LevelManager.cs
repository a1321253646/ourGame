using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	public GameObject BackgroupObject;
	public GameObject Player;
	public GameObject enemyFactory;
    public GameObject skillObject;

    public BackgroundManager mBackManager;
	public FightManager mFightManager;
    public PlayControl mPlayerControl;
    
    public GuideManager mGuideManager;
    private bool isInit = false;
    float cardTop = 0;
    public float yBase = 0;
    MapConfigBean mMapConfig = null;
    

    public void init()
    {
        Debug.Log("LevelManager Start");
        GameManager.getIntance().getLevelData();
        long old = GameManager.getIntance().init(this);
        GameManager.getIntance().initUi();

        Vector3 tmp = PointUtils.screenTransToWorld(GameObject.Find("kapai_local_top").transform.position);
        Vector3 tmp2 = PointUtils.screenTransToWorld(GameObject.Find("kapai_local_bottom").transform.position);
        cardTop = tmp.y - tmp2.y;
        mMapConfig = JsonUtils.getIntance().getMapConfigByResource(
            JsonUtils.getIntance().getLevelData().map);
        yBase = mMapConfig.y_base;
        yBase = cardTop + mMapConfig.y_base;

        mBackManager = new BackgroundManager();
        mFightManager = new FightManager();

        mBackManager.init(BackgroupObject, JsonUtils.getIntance().getLevelData().map, cardTop);
        if (JsonUtils.getIntance().getConfigValueForId(100055) == 1) {
            GameObject.Find("attribute_show").transform.localScale = new Vector2(1, 1);
        }
        if (JsonUtils.getIntance().getConfigValueForId(100047) == 1)
        {
            Time.timeScale = JsonUtils.getIntance().getConfigValueForId(100054);
        }
        else if (SQLHelper.getIntance().isLuiHui != -1 && BaseDateHelper.decodeLong(GameManager.getIntance().mCurrentLevel) <= SQLHelper.getIntance().isLuiHui)
        {
            Time.timeScale = 2;
            GameObject.Find("jiasu_tip").transform.localScale = new Vector2(1, 1);
        }
        else
        {
            Time.timeScale = 1;
            GameObject.Find("jiasu_tip").transform.localScale = new Vector2(0, 0);
        }

        GameObject.Find("speed_setting_tx").GetComponent<Text>().text = "X" + Time.timeScale;
        //        GameManager.getIntance().mTestSpeed = (long)Time.timeScale;

        creaPlay(yBase);
        
        //  creatEnemyFactory(cardTop);
        SkillManage.getIntance().setSkillPrefer(skillObject);
        SkillManage.getIntance().setBackManagerManager(mBackManager);
        BackpackManager.getIntance().init(this);
        UiControlManager.getIntance().init();

        mPlayerControl.mCardManager.init(mPlayerControl);
        BackpackManager.getIntance().updateZhuangbeiItem(true);
        mTime = 0;
        mGuideManager = GetComponent<GuideManager>();
        mGuideManager.init();
        isInit = true;

        GameManager.getIntance().isLuihuiIng = false;
       
   //     GameObject.Find("uid_test").GetComponent<Text>().text = SystemInfo.deviceUniqueIdentifier+"token = "+SQLHelper.getIntance().mToken+"\n"+
     //       " Application.internetReachability == NetworkReachability.NotReachable"+ (Application.internetReachability == NetworkReachability.NotReachable)+"\n"
       //     + "System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()="+ System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
       //  SQLHelper.getIntance().updateOutTime();


    }

    public void reset()
    {
        ActiveListControl a = GameObject.Find("active_button_list").GetComponent<ActiveListControl>();
      //  a.removeVocation(false);
        GameManager.getIntance().reStart();
        starBoss = false;
        if (mFightManager.mEnemyFactory != null)
        {
            mFightManager.mEnemyFactory.destroyCreat();
        }
        JsonUtils.getIntance().init();
        // mBackManager.init(BackgroupObject, JsonUtils.getIntance().getLevelData().map, cardTop);
        mFightManager.reset();
        //GameObject.Find("Role").GetComponent<PlayControl>().resetHero();

        mPlayerControl.resetHero();

        mPlayerControl.mCardManager.reset();
        BackpackManager.getIntance().updateZhuangbeiItem(true);
        GameManager.getIntance().isEnd = false;
        //GameObject.Find("")
        mBackManager.init(BackgroupObject, JsonUtils.getIntance().getLevelData().map, cardTop);
        if (JsonUtils.getIntance().getConfigValueForId(100047) == 1)
        {
            Time.timeScale = JsonUtils.getIntance().getConfigValueForId(100054);
        }
        else if (SQLHelper.getIntance().isLuiHui != -1 && BaseDateHelper.decodeLong(GameManager.getIntance().mCurrentLevel) <= SQLHelper.getIntance().isLuiHui)
        {
            Time.timeScale = 2;
            GameObject.Find("jiasu_tip").transform.localScale = new Vector2(1, 1);
        }
        else
        {
            Time.timeScale = 1;
            GameObject.Find("jiasu_tip").transform.localScale = new Vector2(0, 0);
        }
 /*           if (GameManager.getIntance().mTestSpeed != -1)
            {
                Time.timeScale = GameManager.getIntance().mTestSpeed;
            }*/
        Debug.Log("=================================JsonUtils.getIntance().getConfigValueForId(100044) ==  " + JsonUtils.getIntance().getConfigValueForId(100044) +
        " level==" + BaseDateHelper.decodeLong(GameManager.getIntance().mHeroLv) + "SQLHelper.getIntance().mVocationCount = " + SQLHelper.getIntance().mVocationCount +
        "  level / (long)JsonUtils.getIntance().getConfigValueForId(100044) > SQLHelper.getIntance().mVocationCount=" +
        (BaseDateHelper.decodeLong(GameManager.getIntance().mHeroLv) / (long)JsonUtils.getIntance().getConfigValueForId(100044) > SQLHelper.getIntance().mVocationCount));
        SkillManage.getIntance().reset();
        if (BaseDateHelper.decodeLong(GameManager.getIntance().mHeroLv) / (long)JsonUtils.getIntance().getConfigValueForId(100044) > SQLHelper.getIntance().mVocationCount)
        {
            a.showVocation(false);
        }
        /*  List<ActiveButtonBean> list = SQLHelper.getIntance().getActiveList();

          for (int i = 0; i < list.Count; i++)
          {
              if (list[i].buttonType == ActiveButtonControl.ACTIVE_BUTTON_TYPE_VOCATION)
              {
                  a.showVocation(false);
              }
          }*/
        isWudingTime = 0;
        GameObject.Find("advert").GetComponent<AdUiControl>().update();
    }
    private float isWudingTime = 0;
    void Start () {
        //init();
    }

	// Update is called once per frame
	bool starBoss = false;
    public float mTime = 0;
    private long mOld = -1;
    private void OnApplicationPause(bool pause)
    {
        GameObject.Find("kapai_click").GetComponent<CardDetailShowControl>().remove();
        if (GameManager.getIntance().isHaveOutGet) {
            return;
        }

        Debug.Log("============ 大年30修bug  ====================OnApplicationPause pause = " + pause + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
 //       Debug.Log("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++pause = "+ pause + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
        if (pause)
        {
            SQLHelper.getIntance().updateOutTime();
            mOld = TimeUtils.GetTimeStamp();
        }
        else if(GameManager.getIntance().isHaveOutGet)
        {
            if (mOld != -1)
            {
                GameManager.getIntance().isHaveOutGet = false;
                long old = TimeUtils.GetTimeStamp() - mOld;
                long outTime = TimeUtils.getTimeDistanceMin(mOld);

                BigNumber outGet = mFightManager.attckerOutLine(mPlayerControl, old, GameManager.getIntance().getOutlineGet());

                if (outTime > 1)
                {
                    GameManager.getIntance().mCurrentCrystal = BigNumber.add(outGet, GameManager.getIntance().mCurrentCrystal);
                    GameManager.getIntance().updataGasAndCrystal();
                    SQLHelper.getIntance().updateHunJing(GameManager.getIntance().mCurrentCrystal);
                }

                if (outTime > JsonUtils.getIntance().getConfigValueForId(100032) && SQLHelper.getIntance().mVersionCode >= GameManager.mVersionCode)
                {
                    BackpackManager.getIntance().showMessageTip(OutLineGetMessage.TYPPE_OUT_LINE, "", "" + outGet.toStringWithUnit());
                }
                else
                {
                    GameManager.getIntance().isHaveOutGet = true;
                }
                Debug.Log("============ 大年30修bug  ====================OnApplicationPause 返回屏幕 " );

                SQLHelper.getIntance().updateOutTime();
            }
        }
    }

    void Update () {
        if (!isInit) {
            return;
        }
        mTime += Time.deltaTime;
        if (mTime > 60) {
            Debug.Log("============ 大年30修bug  ====================mTime > 60000  ");
            mTime = mTime - 60;
            SQLHelper.getIntance().updateOutTime();
        }
        if (GameManager.getIntance().isLunhuiWudiIng) {
            isWudingTime += Time.deltaTime;
            if (isWudingTime >= JsonUtils.getIntance().getConfigValueForId(100058)) {
                GameManager.getIntance().isLunhuiWudiIng = false;
            }
        }
		if (!starBoss && GameManager.getIntance ().mStartBoss) {
			starBoss = true;
			mBackManager.setBackground (JsonUtils.getIntance ().getLevelData ().boss_bg);
		}
       /* if (Input.GetKeyDown(KeyCode.F6))
        {
            BackpackManager.getIntance().addGoods(10001,60);
            Debug.Log("Input.GetKeyDown(KeyCode.F6)");
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            BackpackManager.getIntance().deleteGoods(10001, 30);
            Debug.Log("Input.GetKeyDown(KeyCode.F5");
        }*/
        SkillManage.getIntance().update();

        if (GameManager.getIntance().mInitStatus == 12) {
            Time.timeScale = 0;
            string message = GameManager.getIntance().mGameErrorString;
            Text text = GameObject.Find("game_error_des").GetComponent<Text>();
            text.text = message;
            GameObject ob =  GameObject.Find("game_error_messge");
            ob.transform.localPosition = new Vector2(0, 0);
            ob.transform.SetSiblingIndex(GameManager.getIntance().getUiLevel());
        }

        if (GameManager.getIntance().isError) {
            Time.timeScale = 0;
            NetServer.getIntance().getLocl(SQLHelper.getIntance().mToken, false,false);
            SQLManager.getIntance().saveLocal(NetServer.getIntance().getLocal());
            GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showUi("检测到您本地时间出现异常，已还原回服务器最新记录，请将时间修改为当前时间重新开始游戏", LuiHuiTips.TYPPE_ERROR_DATE);
            GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showSelf();
        }
        if (!GameManager.getIntance().isHaveNoteUpdate && GameManager.mAPKVersionCode < GameManager.getIntance().mNewAPKVersionCode) {
            NetServer.getIntance().getUpdateInfoRun();
        }
        if (GameManager.getIntance().isHaveNoteUpdate && GameManager.getIntance().mUpdateStr != null) {
            string dec = GameManager.getIntance().mUpdateStr;
            GameManager.getIntance().mUpdateStr = null;
            GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showUi(dec,LuiHuiTips.TYPPE_UPDATE);
            GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showSelf();
        }
        GameManager.getIntance().getUiManager().upDate();
    }
	public BackgroundManager getBackManager(){
		return mBackManager;
	}
	public FightManager getFightManager(){
		return mFightManager;
	}


    public void ChangeEquip( PlayerBackpackBean equips, bool isAdd) {
        Debug.Log(" ChangeEquip");
        mPlayerControl.ChangeEquip(equips,isAdd);
    }

    public void upLunhui()
    {
        mPlayerControl.upLunhui();
    }

    public void heroUp() {
        mPlayerControl.heroUp();
    }
    public void heroVocation() {
        mPlayerControl.vocation();
    }

    private void creaPlay(float cardTop)
    {

        for (int i = 0; i < 5; i++) {
            float dely = -(-2L + i % 5L) * 0.5f;
            float delx = (i / 5L) * 0.5f;
            creatPlayIndex(-7 - delx, cardTop + dely,i);            
        }
   

    }
    private void creatPlayIndex(float x, float y,int index) {
        Debug.Log("creatPlayIndex= x" + x+"  y="+ y);
        Hero hero = JsonUtils.getIntance().getHeroData();
        long vocation = 1;
        if (SQLHelper.getIntance().mCurrentVocation == -1)
        {
            vocation = 1;
        }
        else
        {
            vocation = SQLHelper.getIntance().mCurrentVocation;
        }

        Debug.Log(" SQLHelper.getIntance().mPlayVocation= " + vocation);
        Debug.Log(" JsonUtils.getIntance().getVocationById(SQLHelper.getIntance().mPlayVocation).resource= " + JsonUtils.getIntance().getVocationById(vocation).resource);
        ResourceBean bean = JsonUtils.getIntance().getEnemyResourceData(
           JsonUtils.getIntance().getVocationById(vocation).resource);


        GameObject newobj = GameObject.Instantiate(Player, new Vector3(x,
            y - bean.idel_y, -1),
            Quaternion.Euler(0.0f, 0.0f, 0.0f));

        newobj.transform.localScale.Set(JsonUtils.getIntance().getConfigValueForId(100005), JsonUtils.getIntance().getConfigValueForId(100005), 1);
        PlayControl play = newobj.GetComponent<PlayControl>();
        if (index == 0)
        {
            mPlayerControl = play;
            mPlayerControl.isMain = true;
        }
        play.startGame();

    }

	public void creatEnemyFactory(float x,float y)
    {

		GameObject newobj =  GameObject.Instantiate (enemyFactory, new Vector2 (JsonUtils.getIntance().getConfigValueForId(100004),yBase),
			Quaternion.Euler(0.0f,0f,0.0f));
        mFightManager.mEnemyFactory = newobj.GetComponent<EnemyFactory>();
        mFightManager.mEnemyFactory.setMapConfig(mMapConfig, cardTop,x,y);
    }

    

}
