using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        BackpackManager.getIntance().init(this);        
        nengLiangDian = 0;
        mNengLiangKuai.Clear();
        initNengliangkuai();
        BackpackManager.getIntance().updateZhuangbeiItem(true);
        mTime = 0;
        mGuideManager = GetComponent<GuideManager>();
        mGuideManager.init();
        isInit = true;
        if (SQLHelper.getIntance().isLuiHui != -1 && GameManager.getIntance().mCurrentLevel <= SQLHelper.getIntance().isLuiHui)
        {
            Time.timeScale = 2;
        }
        else {
            Time.timeScale = 1;
        }
       
    }
    void Start () {
        //init();
    }

    private void initNengliangkuai() {
        mNengLiangKuai.Clear();
        for (int i = 1; i <= 10; i++)
        {
            NengliangkuaiControl tmp1 = GameObject.Find("nengliangkuai_" + i).GetComponent<NengliangkuaiControl>();
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
                BigNumber levelCryStal = JsonUtils.getIntance().getLevelData(GameManager.getIntance().mCurrentLevel).getOfflinereward();
                BigNumber  outLineGet = BigNumber.multiply(levelCryStal, outTime);
                outLineGet = BigNumber.multiply(outLineGet, GameManager.getIntance().getOutlineGet());
                GameManager.getIntance().mCurrentCrystal = BigNumber.add(outLineGet, GameManager.getIntance().mCurrentCrystal);
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
                    BackpackManager.getIntance().showMessageTip(MessageTips.TYPPE_OUT_LINE, "欢迎回来，您在离线的" + str + "里", "" + outLineGet.toStringWithUnit());
                }
                GameManager.getIntance().updateGasAndCrystal();
                SQLHelper.getIntance().updateOutTime();
            }
        }
    }


    void Update () {
        if (!isInit) {
            return;
        }
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


    private void creaPlay(float cardTop)
    {
        Hero hero = JsonUtils.getIntance().getHeroData();
        ResourceBean bean = JsonUtils.getIntance().getEnemyResourceData(hero.resource);
        Debug.Log("hero.resource idel_y= " + bean.idel_y);

        GameObject newobj =  GameObject.Instantiate (Player, new Vector2 (-7, 
            cardTop-bean.idel_y),
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
