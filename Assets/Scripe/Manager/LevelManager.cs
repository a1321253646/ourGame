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

    private bool isInit = false;
    public void init()
    {
        SQLHelper.getIntance().init();
        Debug.Log("LevelManager Start");
        GameManager.getIntance();
        JsonUtils.getIntance().init();
        GameManager.getIntance().getLevelData();
        GameManager.getIntance().init(this);
        GameManager.getIntance().initUi();

        Vector3 tmp = PointUtils.screenTransToWorld(GameObject.Find("kapai_local_top").transform.position);
        Vector3 tmp2 = PointUtils.screenTransToWorld(GameObject.Find("kapai_local_bottom").transform.position);
        float cardTop = tmp.y - tmp2.y;
        mBackManager = new BackgroundManager();
        mFightManager = new FightManager();
        mLocalManager = new LocalManager();
        mFightManager.setLoaclManager(mLocalManager);
        mBackManager.init(BackgroupObject, JsonUtils.getIntance().getLevelData().map, cardTop);

        creaPlay(cardTop);
        creatEnemyFactory(cardTop);
        SkillManage.getIntance().setSkillPrefer(skillObject);
        SkillManage.getIntance().setLoclaManager(mLocalManager);
        BackpackManager.getIntance().init(this);
        nengLiangDian = 0;
        mNengLiangKuai.Clear();
        initNengliangkuai();
        BackpackManager.getIntance().updateZhuangbeiItem();
        isInit = true;
       
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

	void Update () {
        if (!isInit) {
            return;
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

        GameObject newobj =  GameObject.Instantiate (Player, new Vector2 (JsonUtils.getIntance().getConfigValueForId(100003), cardTop+ JsonUtils.getIntance().getConfigValueForId(100002)-bean.idel_y),
			Quaternion.Euler(0.0f,0.0f,0.0f));
		newobj.transform.localScale.Set (JsonUtils.getIntance().getConfigValueForId(100005), JsonUtils.getIntance().getConfigValueForId(100005), 1);
        mPlayerControl = newobj.GetComponent<PlayControl>();

    }
	private void creatEnemyFactory(float cardTop)
    {
		GameObject newobj =  GameObject.Instantiate (enemyFactory, new Vector2 (JsonUtils.getIntance().getConfigValueForId(100004), cardTop +JsonUtils.getIntance().getConfigValueForId(100002)),
			Quaternion.Euler(0.0f,0f,0.0f));
        mFightManager.mEnemyFactory = newobj.GetComponent<EnemyFactory>();
    }

}
