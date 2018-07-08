using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour {
	private int mLevel;
	public GameObject _game;
	double timeCost = 0;
	public BackgroundManager mBackManager;
	private Enemy data;
	// Use this for initialization
	void Start () {
		mBackManager = GameObject.Find ("Manager").GetComponent<LevelManager> ().getBackManager ();
		mList = JsonUtils.getIntance ().getWellenEnemy ();
	}
	
	// Update is called once per frame
	void Update () {
		if (mBackManager.isRun) {
			timeCost += Time.deltaTime;
		}
		if (timeCost > mList[currentCount].time) {
			long id = mList [currentCount].id;
			data = JsonUtils.getIntance ().getEnemyById (id);
			creatEnemy ();
			currentCount++;
			if (currentCount >= mList.Count) {
				currentCount = 0;
				timeCost = 0;
				mList = JsonUtils.getIntance ().getWellenEnemy ();
			}
		}

	}
	private int currentCount = 0;
	List<LevelEnemyWellen> mList;
	public void initFactory(int level){
		mLevel = level;
	}
	void creatEnemy(){	
		GameObject newobj =  GameObject.Instantiate (_game, new Vector2 (transform.position.x, transform.position.y),Quaternion.Euler(0.0f,0f,0.0f));
		EnemyBase enmey = newobj.GetComponent<EnemyBase> ();
		enmey.init (data);
//		enmey.dieCrystal = enmey.g
		//newobj.transform.rotation.y
	}
}
