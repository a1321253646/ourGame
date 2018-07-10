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
	void Update () {
		mLocalManager.upData ();
		if (GameManager.getIntance ().mStartBoss) {
			mBackManager.setBackground (JsonUtils.getIntance ().getLevelData ().boss_bg);
		}
	}
	public BackgroundManager getBackManager(){
		return mBackManager;
	}
	public FightManager getFightManager(){
		return mFightManager;
	}


	private void creaPlay(){
		GameObject newobj =  GameObject.Instantiate (Player, new Vector2 (-5.23f,-1.738f),
			Quaternion.Euler(0.0f,0.0f,0.0f));
		newobj.transform.localScale.Set (0.3703687f, 0.3703687f, 1);
	}
	private void creatEnemyFactory(){
		GameObject newobj =  GameObject.Instantiate (enemyFactory, new Vector2 (7.44f,-1.738f),
			Quaternion.Euler(0.0f,0f,0.0f));
	}

}
