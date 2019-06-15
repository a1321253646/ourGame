using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour {
	private int mLevel;
	public GameObject game;
    public GameObject Effect;
    double timeCost = 0;
	public BackgroundManager mBackManager;
	public FightManager mFight;
	private Enemy data;
	private bool isCreat = true;
	private bool startBoss = false;
	private GameObject canvas;
    private MapConfigBean mMapConfig = null;
    private float mBottom = 0;
    float mHeroX = 0;
    float mHeroY = 0;
    private float mZPoint = 2;
    LevelManager mLevelMnager = null;
    // Use this for initialization

    private bool mIsDeal = false;

    public void destroyCreat() {
        mIsDeal = true;
        Destroy(gameObject);
    }

    void Start () {
        mLevelMnager = GameObject.Find("Manager").GetComponent<LevelManager>();
        mBackManager = mLevelMnager.getBackManager ();
		mFight = mLevelMnager.getFightManager ();
		canvas = GameObject.Find ("Canvas");
		mList = JsonUtils.getIntance ().getWellenEnemy ();
	}

    public void setMapConfig(MapConfigBean map,float bottom,float x,float y) {
        mMapConfig = map;
        mBottom = bottom;
        mHeroX = x;
        mHeroY = y;
    }

	// Update is called once per frame
	void Update () {
		if (startBoss || mIsDeal || GameManager.getIntance().isEnd) {
			return;
		}
		if (GameManager.getIntance ().mStartBoss) {
			GameManager.getIntance ().mStartBoss = false;
			startBoss = true;

			//int bossId = JsonUtils.getIntance ().getLevelData ().boss_DI;
			//Debug.Log ("start creat boss id ="+bossId);
			data = JsonUtils.getIntance ().getEnemyById (GameManager.getIntance().mBossId);
			creatEnemy (true);
			return;
		}


		if (!isCreat) {
			if (mFight.isEmptyEnemy ()) {
				isCreat = true;
				timeCost = 0;
				currentCount = 0;
			} else {
				return;
			}
		}

		timeCost += Time.deltaTime;

		if (timeCost*1000 > mList[currentCount].time) {
			long id = mList [currentCount].id;
		//	Debug.Log ("creat enemey id =" + id);
			data = JsonUtils.getIntance ().getEnemyById (id);
			creatEnemy (false);
			currentCount++;
			if (currentCount >= mList.Count) {
                mZPoint = 2;
				isCreat = false;
				mList = JsonUtils.getIntance ().getWellenEnemy ();
			}
		}

	}
	private int currentCount = 0;
	List<LevelEnemyWellen> mList;
	public void initFactory(int level){
		mLevel = level;
	}
	void creatEnemy(bool isBoss){	
//		Debug.Log ("creatEnemy id ="+data.id);
		ResourceBean bean = JsonUtils.getIntance ().getEnemyResourceData (data.resource);
		string res = bean.name;
        GameObject newobj = null;

            newobj = GameObject.Instantiate(
                game, new Vector3(transform.position.x, getYRamdom(isBoss) - (float)bean.idel_y, mZPoint), Quaternion.Euler(0.0f, 0f, 0.0f));
        
        mZPoint -= 0.1f;
        EnemyBase enmey = newobj.GetComponent<EnemyBase> ();
		
		if (isBoss) {
			enmey.mAttackType = Attacker.ATTACK_TYPE_BOSS;

        } else {
			enmey.mAttackType = Attacker.ATTACK_TYPE_ENEMY;
        }
        enmey.init(data, bean);
        if (JsonUtils.getIntance().getConfigValueForId(100055) == 1) {
            GameObject.Find("attribute_show").GetComponent<AttributeShowManager>().showEnemy(enmey);
        }
        enmey.mCampType = Attacker.CAMP_TYPE_MONSTER;
        enmey.setTarget(new Vector2(mHeroX, mHeroY));
        mLevelMnager.mPlayerControl.mSkillManager.mEventAttackManager.debuffMonster(enmey);
        if (isBoss)
        {
            enmey.mAttackType = Attacker.ATTACK_TYPE_BOSS;
            mLevelMnager.mPlayerControl.mSkillManager.mEventAttackManager.debuffBoss(enmey);
            GameManager.getIntance().mBoss = enmey;

        }
        else
        {
            enmey.mAttackType = Attacker.ATTACK_TYPE_ENEMY;
            mLevelMnager.mPlayerControl.mSkillManager.mEventAttackManager.debuffLitterMonster(enmey);
        }
        GameManager.getIntance().mAliveEnemy++;
    }
    private float getYRamdom(bool isBoss) {

        float y = 0;
        if (isBoss)
        {
            y = mLevelMnager.yBase;
        }
        else {
            y = Random.Range((float)mMapConfig.y_min + mBottom, (float)mMapConfig.y_max + mBottom);
        }
        
//        Debug.Log("==================getYRamdom  y = " + y+ " mBottom="+ mBottom);

        return y;
    }
}
