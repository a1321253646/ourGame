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
	public LocalManager mLocalManager;
    public PlayControl mPlayerControl;
    public List<NengliangkuaiControl> mNengLiangKuai = new List<NengliangkuaiControl>();
    public GuideManager mGuideManager;
    private bool isInit = false;
    float cardTop = 0;
    float yBase = 0;
    MapConfigBean mMapConfig = null;
    public void init()
    {
        updateIndex = 0;
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
        mLocalManager = new LocalManager();
        mFightManager.setLoaclManager(mLocalManager);
        mBackManager.init(BackgroupObject, JsonUtils.getIntance().getLevelData().map, cardTop);

        creaPlay(yBase);
      //  creatEnemyFactory(cardTop);
        SkillManage.getIntance().setSkillPrefer(skillObject);
        SkillManage.getIntance().setLoclaManager(mLocalManager);
        SkillManage.getIntance().setBackManagerManager(mBackManager);
        BackpackManager.getIntance().init(this);
        UiControlManager.getIntance().init();
        if ( SQLHelper.getIntance().mVersionCode < GameManager.mVersionCode) {

            GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().
                showUi("本次更新新卡牌以及转职系统，为了您的游戏正常进行，将强制轮回，本次轮回您将获得双倍轮回值共%D点。给您带来不便，我们表示抱歉。");
            Time.timeScale = 0;
            return;

        }
            


        GameObject.Find("jineng").GetComponent<CardManager>().init();
        nengLiangDian = 0;
        mNengLiangKuai.Clear();
        initNengliangkuai();
        BackpackManager.getIntance().updateZhuangbeiItem(true);
        mTime = 0;
        mGuideManager = GetComponent<GuideManager>();
        mGuideManager.init();
        isInit = true;
        if (GameManager.isTest) {
            Time.timeScale = 10;
        }
        else if (SQLHelper.getIntance().isLuiHui != -1 && BaseDateHelper.decodeLong(GameManager.getIntance().mCurrentLevel) <= SQLHelper.getIntance().isLuiHui)
        {
            Time.timeScale = 2;
            GameObject.Find("jiasu_tip").transform.localScale = new Vector2(1, 1);
        }
        else {
            Time.timeScale = 1;
            GameObject.Find("jiasu_tip").transform.localScale = new Vector2(0, 0);
        }
        GameManager.getIntance().isLuihuiIng = false;
        if (GameManager.getIntance().isHaveOutGet)
        {
            BigNumber outLineGet = new BigNumber();
            GameManager.getIntance().isHaveOutGet = false;
            long old2 = SQLHelper.getIntance().mOutTime;
            Debug.Log("========old = " + old2);
            if (old2 != -1)
            {
                old2 = TimeUtils.getTimeDistanceMin(old2);
                Debug.Log("========old = " + old2);
                BigNumber levelCryStal = JsonUtils.getIntance().getLevelData(BaseDateHelper.decodeLong(GameManager.getIntance().mCurrentLevel)).getOfflinereward();
                outLineGet = getOutGetEverySecond(levelCryStal);
                outLineGet = BigNumber.multiply(outLineGet, old2);
                outLineGet = BigNumber.multiply(outLineGet, GameManager.getIntance().getOutlineGet());

                if (old2 > 1)
                {
                    GameManager.getIntance().mCurrentCrystal = BigNumber.add(outLineGet, GameManager.getIntance().mCurrentCrystal);
                    GameManager.getIntance().updataGasAndCrystal();
                    SQLHelper.getIntance().updateHunJing(GameManager.getIntance().mCurrentCrystal);
                }

                if (old2 > JsonUtils.getIntance().getConfigValueForId(100032))
                {
                    long h = old2 / 60;
                    long min = old2 % 60;
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
                    BackpackManager.getIntance().showMessageTip(OutLineGetMessage.TYPPE_OUT_LINE, "欢迎回来，您在离线的" + str + "里", "" + outLineGet.toStringWithUnit());
                }
            }
        }
        SQLHelper.getIntance().updateOutTime();
    }
    void Start () {
        //init();
    }

    private void initNengliangkuai() {
        mNengLiangKuai.Clear();
        for (int i = 1; i <= 10; i++)
        {
            NengliangkuaiControl tmp1 = GameObject.Find("nengliangkuai_" + i).GetComponent<NengliangkuaiControl>();
            tmp1.init();
            tmp1.setCount(nengLiangDian);
            mNengLiangKuai.Add(tmp1);
        }
    }

    private int mPlayActionCount = 0;

    public void playerAction() {
        mPlayActionCount++;
        if (mPlayActionCount >= JsonUtils.getIntance().getConfigValueForId(100014)) {
            addNengliangDian(1);
            mPlayActionCount = 0;
        }
    }

    public void addNengliangDian(float nengliang) {
//        Debug.Log("addNengliangDian= " + nengliang);
        if (nengLiangDian >= 10) {
            nengLiangDian = 10;
            return;
        }
        if (mNengLiangKuai.Count < 10) {
            
            initNengliangkuai();
        }
        nengLiangDian += nengliang;
        if (nengLiangDian >= 10)
        {
            nengLiangDian = 10;
        }
        for (int i = 0; i < 10; i++)
        {
            mNengLiangKuai[i].setCount(nengLiangDian);
        }
    }
    public bool delectNengliangdian(float nengliang) {
        if (nengliang > nengLiangDian) {
            return false;
        }
        if (mNengLiangKuai.Count < 10)
        {
            initNengliangkuai();
        }
        nengLiangDian -= nengliang;
        for (int i = 0; i < 10; i++)
        {
            mNengLiangKuai[i].setCount(nengLiangDian);
        }
        return true;
    }

    private BigNumber getOutGetEverySecond(BigNumber level) {
        Debug.Log("getOutGetEverySecond");
        Debug.Log("GameManager.getIntance().mOutLineGet = "+ GameManager.getIntance().mOutLineGet.toStringWithUnit());
        Debug.Log("updateIndex * Time.deltaTime = "+ updateIndex * Time.deltaTime);
        if (SQLHelper.getIntance().mOutLineGet != null)
        {
            Debug.Log("SQLHelper.getIntance().mOutLineGet  = " + SQLHelper.getIntance().mOutLineGet.toStringWithUnit());
        }
        else {
            Debug.Log("SQLHelper.getIntance().mOutLineGet  = null" );
        }
        BigNumber back = null;
        if (SQLHelper.getIntance().mOutLineGet == null || SQLHelper.getIntance().mOutLineGet.isEmpty())
        {

            if (GameManager.getIntance().mOutLineGet.isEmpty() || updateIndex == 0)
            {
                back = level;
            }
            else
            {
                back = BigNumber.multiply(GameManager.getIntance().mOutLineGet,1f / (long)(updateIndex * Time.deltaTime));
                back = BigNumber.multiply(back, 60);
                back = BigNumber.multiply(back, JsonUtils.getIntance().getConfigValueForId(100048));
            }
            if (back.ieEquit(level) == -1) {
                back = level;
            }
            SQLHelper.getIntance().updateOutLineGet(back);
        }
        else
        {
            back = SQLHelper.getIntance().mOutLineGet;
            //Debug.Log("=============histroy back=" + back.toString());
            if (!GameManager.getIntance().mOutLineGet.isEmpty())
            {
                BigNumber tmpBig = BigNumber.multiply(GameManager.getIntance().mOutLineGet, 1f / (long)(updateIndex * Time.deltaTime));
                tmpBig = BigNumber.multiply(tmpBig,60);
                tmpBig = BigNumber.multiply(tmpBig, JsonUtils.getIntance().getConfigValueForId(100048));
                if (back.ieEquit(tmpBig) == -1)
                {
                    back = tmpBig;
                }

            }
            if (back.ieEquit(level) == -1)
            {
                back = level;

            }
            SQLHelper.getIntance().updateOutLineGet(back);
        }
        Debug.Log("back = " + back.toStringWithUnit());
        return back;
    }


	// Update is called once per frame
	bool starBoss = false;
    public float nengLiangDian = 0;
    public float mTime = 0;
    private long mOld = -1;
    private void OnApplicationPause(bool pause)
    {
//        Debug.Log("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++pause = "+ pause + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
        if (pause)
        {
            SQLHelper.getIntance().updateOutTime();
            mOld = TimeUtils.GetTimeStamp();
        }
        else {
            if (mOld != -1)
            {
                long outTime = TimeUtils.getTimeDistanceMin(mOld);
               
                BigNumber levelCryStal = JsonUtils.getIntance().getLevelData(BaseDateHelper.decodeLong(GameManager.getIntance().mCurrentLevel)).getOfflinereward();
              //  Debug.Log("=============levelCryStal=" + levelCryStal.toString());
              //  Debug.Log("=============outTime=" + outTime);
                BigNumber outLineGet = getOutGetEverySecond(levelCryStal);
                outLineGet = BigNumber.multiply(outLineGet, outTime);
                outLineGet = BigNumber.multiply(outLineGet, GameManager.getIntance().getOutlineGet());

              //  Debug.Log("=============outLineGet=" + outLineGet.toString());
              //  Debug.Log("=============GameManager.getIntance().getOutlineGet()=" + GameManager.getIntance().getOutlineGet());
                if (outTime > 1)
                {
                    GameManager.getIntance().mCurrentCrystal = BigNumber.add(outLineGet, GameManager.getIntance().mCurrentCrystal);
                    GameManager.getIntance().updateGasAndCrystal();
                    SQLHelper.getIntance().updateHunJing(GameManager.getIntance().mCurrentCrystal);
                }
                if (outTime > JsonUtils.getIntance().getConfigValueForId(100032))
                {
                    long h = outTime / 60;
                    long min = outTime % 60;
                    string str = "";
                    if (h > 9)
                    {
                        str += h+":";
                    }
                    else
                    {
                        str = str+"0" + h + ":";
                    }
                    if (min > 9)
                    {
                        str += min;
                    }
                    else
                    {
                        str = str+ "0" + min;
                    }
                    BackpackManager.getIntance().showMessageTip(OutLineGetMessage.TYPPE_OUT_LINE, "欢迎回来，您在离线的" + str + "里", "" + outLineGet.toStringWithUnit());
                }

                SQLHelper.getIntance().updateOutTime();
            }
        }
    }


    public long updateIndex = 0;

    void Update () {
        if (!isInit) {
            return;
        }
        updateIndex++;
        mTime += Time.deltaTime;
        if (mTime > 60000) {
            mTime = mTime - 60000;
            SQLHelper.getIntance().updateOutTime();
        }
        mLocalManager.upData ();
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
            NetServer.getIntance().getLocl();
            SQLManager.getIntance().saveLocal(NetServer.getIntance().getLocal());
            GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showUi("检测到您本地时间出现异常，已还原回服务器最新记录，请将时间修改为当前时间重新开始游戏", LuiHuiTips.TYPPE_ERROR_DATE);
            GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showSelf();
        }

    }
	public BackgroundManager getBackManager(){
		return mBackManager;
	}
	public FightManager getFightManager(){
		return mFightManager;
	}
    public LocalManager getLocalManager()
    {
        return mLocalManager;
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
        Hero hero = JsonUtils.getIntance().getHeroData();
        long vocation = 1;
        if (SQLHelper.getIntance().mPlayVocation == -1)
        {
            vocation = 1;
        }
        else {
            vocation = SQLHelper.getIntance().mPlayVocation;
        }

        Debug.Log(" SQLHelper.getIntance().mPlayVocation= " + vocation);
        Debug.Log(" JsonUtils.getIntance().getVocationById(SQLHelper.getIntance().mPlayVocation).resource= " + JsonUtils.getIntance().getVocationById(vocation).resource);
        ResourceBean bean = JsonUtils.getIntance().getEnemyResourceData(
           JsonUtils.getIntance().getVocationById(vocation).resource);
       

        GameObject newobj =  GameObject.Instantiate (Player, new Vector3 (-7, 
            cardTop-bean.idel_y,-1),
			Quaternion.Euler(0.0f,0.0f,0.0f));
		newobj.transform.localScale.Set (JsonUtils.getIntance().getConfigValueForId(100005), JsonUtils.getIntance().getConfigValueForId(100005), 1);
        mPlayerControl = newobj.GetComponent<PlayControl>();
        mPlayerControl.startGame();

    }
	public void creatEnemyFactory(float x,float y)
    {
		GameObject newobj =  GameObject.Instantiate (enemyFactory, new Vector2 (JsonUtils.getIntance().getConfigValueForId(100004),yBase),
			Quaternion.Euler(0.0f,0f,0.0f));
        mFightManager.mEnemyFactory = newobj.GetComponent<EnemyFactory>();
        mFightManager.mEnemyFactory.setMapConfig(mMapConfig, cardTop,x,y);
    }

}
