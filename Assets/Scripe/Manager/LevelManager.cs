using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public GameObject BackgroupObject;
	public GameObject Player;
	public GameObject enemyFactory;
    public BackgroundManager mBackManager;
	public FightManager mFightManager;
	public LocalManager mLocalManager;
	void Start () {
		Debug.Log ("LevelManager Start");
		GameManager.getIntance ();
		JsonUtils.getIntance ().init ();
		GameManager.getIntance ().getLevelData ();
		GameManager.getIntance ().init ();
		GameManager.getIntance ().initUi ();
		mBackManager = new BackgroundManager ();
		mFightManager =new FightManager ();
		mLocalManager = new LocalManager ();
		mFightManager.setLoaclManager (mLocalManager);
		mBackManager.init (BackgroupObject,JsonUtils.getIntance().getLevelData().map);
		creaPlay ();
		creatEnemyFactory ();
	}
	
	// Update is called once per frame
	bool starBoss = false;
	void Update () {
		mLocalManager.upData ();
		if (!starBoss && GameManager.getIntance ().mStartBoss) {
			starBoss = true;
			mBackManager.setBackground (JsonUtils.getIntance ().getLevelData ().boss_bg);
		}
        if (Input.GetKeyDown(KeyCode.F6))
        {
            BackpackManager.getIntance().addGoods(10001,60);
            Debug.Log("Input.GetKeyDown(KeyCode.F6)");
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            BackpackManager.getIntance().deleteGoods(10001, 30);
            Debug.Log("Input.GetKeyDown(KeyCode.F5");
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            Debug.Log("Input.GetKeyDown(KeyCode.F4)");
            //   BackpackManager.getIntance().showBackpackUi();
            BackpackManager.getIntance().showTipUi(10001, 30);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            Debug.Log("Input.GetKeyDown(KeyCode.F3)");
            //  BackpackManager.getIntance().removeUi();
            BackpackManager.getIntance().removeTipUi();
        }

    }
	public BackgroundManager getBackManager(){
		return mBackManager;
	}
	public FightManager getFightManager(){
		return mFightManager;
	}


	private void creaPlay(){

        Hero mHero;
        ResourceBean mBean;
        Hero hero = JsonUtils.getIntance().getHeroData();
        ResourceBean bean = JsonUtils.getIntance().getEnemyResourceData(hero.resource);
        Debug.Log("hero.resource idel_y= " + bean.idel_y);

        GameObject newobj =  GameObject.Instantiate (Player, new Vector2 (JsonUtils.getIntance().getConfigValueForId(100003), JsonUtils.getIntance().getConfigValueForId(100002)-bean.idel_y),
			Quaternion.Euler(0.0f,0.0f,0.0f));
		newobj.transform.localScale.Set (JsonUtils.getIntance().getConfigValueForId(100005), JsonUtils.getIntance().getConfigValueForId(100005), 1);
	}
	private void creatEnemyFactory(){
		GameObject newobj =  GameObject.Instantiate (enemyFactory, new Vector2 (JsonUtils.getIntance().getConfigValueForId(100004), JsonUtils.getIntance().getConfigValueForId(100002)),
			Quaternion.Euler(0.0f,0f,0.0f));
        mFightManager.mEnemyFactory = newobj.GetComponent<EnemyFactory>();
    }

}
