using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public GameObject BackgroupObject;
	public GameObject Player;
	public GameObject enemyFactory;

	public BackgroundManager mBackManager;
	public FightManager mFightManager;
	void Start () {
		GameManager.getIntance ().getLevelData();
		JsonUtils.getIntance ().init ();
		mBackManager = new BackgroundManager ();
		mFightManager =new FightManager ();
		mBackManager.init (BackgroupObject);
		creaPlay ();
		creatEnemyFactory ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public BackgroundManager getBackManager(){
		return mBackManager;
	}
	public FightManager getFightManager(){
		return mFightManager;
	}


	private void creaPlay(){
		GameObject newobj =  GameObject.Instantiate (Player, new Vector2 (-6.04f,-2.44f),
			Quaternion.Euler(0.0f,0.0f,0.0f));
		newobj.transform.localScale.Set (0.3703687f, 0.3703687f, 1);
	}
	private void creatEnemyFactory(){
		GameObject newobj =  GameObject.Instantiate (enemyFactory, new Vector2 (7.44f,-2.41f),
			Quaternion.Euler(0.0f,0f,0.0f));
	}

}
