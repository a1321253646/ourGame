using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour {
	private int mLevel;
	public GameObject[] _game;
	public GameObject blood;
	double timeCost = 0;
	public BackgroundManager mBackManager;
	public FightManager mFight;
	private Enemy data;
	private bool isCreat = true;
	private bool startBoss = false;
	private GameObject canvas;
	// Use this for initialization
	void Start () {
		mBackManager = GameObject.Find ("Manager").GetComponent<LevelManager> ().getBackManager ();
		mFight = GameObject.Find ("Manager").GetComponent<LevelManager> ().getFightManager ();
		canvas = GameObject.Find ("Canvas");
		mList = JsonUtils.getIntance ().getWellenEnemy ();
	}
	
	// Update is called once per frame
	void Update () {
		if (startBoss) {
			return;
		}
		if (GameManager.getIntance ().mStartBoss) {
			GameManager.getIntance ().mStartBoss = false;
			startBoss = true;

			string bossId = JsonUtils.getIntance ().getLevelData ().boss_DI;
			Debug.Log ("start creat boss id ="+bossId);
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

		if (timeCost > mList[currentCount].time) {
			long id = mList [currentCount].id;
			Debug.Log ("creat enemey id =" + id);
			data = JsonUtils.getIntance ().getEnemyById (id);
			creatEnemy (false);
			currentCount++;
			if (currentCount >= mList.Count) {
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
		Debug.Log ("creatEnemy id ="+data.id);
		string res = JsonUtils.getIntance ().getEnemyResourceData (data.getResource ()).role;
		getEnemyPrefab(res);
		GameObject newobj =  GameObject.Instantiate (getEnemyPrefab(res), new Vector2 (transform.position.x, transform.position.y),Quaternion.Euler(0.0f,0f,0.0f));
		EnemyBase enmey = newobj.GetComponent<EnemyBase> ();
		enmey.init (data);
		if (isBoss) {
			enmey.mAttackType = Attacker.ATTACK_TYPE_BOSS;
		} else {
			enmey.mAttackType = Attacker.ATTACK_TYPE_ENEMY;
		}
		GameObject bewBlood = GameObject.Instantiate (blood, new Vector2 (transform.position.x, transform.position.y), Quaternion.Euler (0.0f, 0f, 0.0f));
		bewBlood.transform.SetParent (canvas.transform,false);
		enmey.setBloodObject (bewBlood);
//		enmey.dieCrystal = enmey.g
		//newobj.transform.rotation.y
	}
	private GameObject getEnemyPrefab(string name){
		string[] strs = name.Split ('_');
		if (strs.Length == 2) {
			int id = int.Parse (strs[1]);
			return _game [id - 1];
		}

		return null;
	}
}
