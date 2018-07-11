using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JsonUtils
{
	private  string levelFile =  "level";
	private  string heroFile = "hero";
	private  string enemyFile = "enemy";
	private  string levelEnemyFile = "levelenemy";
	private  string resourceFile = "resource";
	private static JsonUtils mInance= new JsonUtils();

	List<Hero> heroData;
	List<Level> levelData;
	List<ResourceBean> resourceData;
	private JsonUtils(){
		readAllFile ();
	}

	public void readAllFile(){
		readHeroData ();
		readLevelData ();
		readResource ();
	}

	public static JsonUtils getIntance(){
		return mInance;
	}
	private string readFile(string fileName){
		TextAsset jsonText = Resources.Load(fileName) as TextAsset;

		Debug.Log ("readFile :" + fileName + "\n " + jsonText.text);

//		foreach (Hero hero in heros) {
//			Debug.Log ("hero:id=" + hero.role_lv + " hp=" + hero.role_hp + " attack=" + hero.role_attack + " defense=" + hero.role_defense + " crystal=" + hero.lvup_crystal);
//		}
		return jsonText.text;
//		Hero jsonObj = JsonUtility.FromJson<Hero>(jsonText.text);
	}

	private void readResource(){
		var arrdata = Newtonsoft.Json.Linq.JArray.Parse (readFile (resourceFile));
		resourceData = arrdata.ToObject<List<ResourceBean>> ();
	}

	private void readHeroData(){
		var arrdata = Newtonsoft.Json.Linq.JArray.Parse (readFile (heroFile));
		heroData = arrdata.ToObject<List<Hero>> ();
/*		Debug.Log ("readHeroData:");
		foreach (Hero hero in heroData) {
			Debug.Log ("hero getRoleLv="+hero.getRoleLv()+
				" hero.getRoleHp ="+hero.getRoleHp()+
				" hero.getLvupCrystal ="+hero.getLvupCrystal()+
				" hero.getRoleAttack ="+hero.getRoleAttack()+
				" hero.getRoleDefense ="+hero.getRoleDefense());
		}*/
	}
	private void readLevelData(){
		var arrdata = Newtonsoft.Json.Linq.JArray.Parse (readFile (levelFile));
		levelData = arrdata.ToObject<List<Level>> ();
/*		Debug.Log ("readLevelData:");
		foreach (Level level in levelData) {
			Debug.Log ("hero id="+level.id+
				" level.boss_DI ="+level.boss_DI+
				" level.boss_gas ="+level.boss_gas+
				" level.name ="+level.name+
				" level.wellen ="+level.wellen);
		}*/
	}
	private List<LevelEnemy> readLevelEnemyData(){
		var arrdata = Newtonsoft.Json.Linq.JArray.Parse (readFile (levelEnemyFile));
		return arrdata.ToObject<List<LevelEnemy>> ();
	}
	private List<Enemy> readEnemyData(){
		var arrdata = Newtonsoft.Json.Linq.JArray.Parse (readFile (enemyFile));
		return arrdata.ToObject<List<Enemy>> ();
	}

	public  ResourceBean getEnemyResourceData(int resourceId){
		foreach (ResourceBean resource in resourceData) {
			if (resource.getId() == resourceId) {
				return resource;
			}
		}
		return null;
	}

	public  Level getLevelData(){
		return getLevelData (
			GameManager.
			getIntance ().mCurrentLevel);
	}
	public  Level getLevelData(int id){
		foreach (Level level in levelData) {
			if (int.Parse(level.id) == id) {
				return level;
			}
		}
		return null;
	}

	public Hero getHeroData(){
		 return getHeroData (GameManager.
			getIntance ().
			mHeroLv);
	}
	public Hero getHeroData(int lv){
		foreach (Hero hero in heroData) {
			if (hero.getRoleLv() == lv) {
				return hero;
			}
		}
		return null;
	}
	public void init (){
		Debug.Log ("JsonUtils init");
		mCurrentLevelWellent = null;
		mWellentList = null;
		mEnemys = null;
		mCurrentLevel = 0;
		bossId = 0;
		bossGas = 0;
	}
	public int mCurrentLevel = 0;
	public List<int> mCurrentLevelWellent;
	Dictionary<int,List<LevelEnemyWellen>> mWellentList;
	Dictionary<long,Enemy> mEnemys;
	long bossId;
	int bossGas;
	public List<LevelEnemyWellen> getWellenEnemy(){
		
		if (mCurrentLevelWellent == null || mCurrentLevelWellent.Count == 0) {
			mCurrentLevelWellent = new List<int>();
			Level level = getLevelData ();
			bossId = long.Parse(level.boss_DI);
			bossGas = int.Parse(level.boss_gas);
			string[] wellenStr = level.wellen.Split ('#');
			foreach (string str in wellenStr) {
				mCurrentLevelWellent.Add ( int.Parse (str));
			}
		}
		if (mCurrentLevel >= mCurrentLevelWellent.Count) {
			mCurrentLevel = 0;
		}
		int wellent = mCurrentLevelWellent [mCurrentLevel];
		Debug.Log ("getWellenEnemy :wellent="+wellent);
		if (mWellentList == null || mWellentList.Count == 0) {
			mWellentList = new Dictionary<int,List<LevelEnemyWellen>> ();
			mEnemys = new Dictionary<long,Enemy> ();
			List<Enemy> enemydata = readEnemyData ();
			List<LevelEnemy> levelList = readLevelEnemyData ();
			mWellentList = new Dictionary<int, List<LevelEnemyWellen>> ();


			foreach (Enemy tmp3 in  enemydata) {
				Debug.Log ("enemydata id="+tmp3.getId() +" bossId="+bossId );

				if (tmp3.getId() == bossId) {
					mEnemys.Add ( bossId, tmp3);
					break;
				}
			}


			foreach(int tmp in mCurrentLevelWellent){
				foreach(LevelEnemy wellen in levelList){
					if (wellen.wellen.Equals ("" + tmp)) {
						List<LevelEnemyWellen> list = new List<LevelEnemyWellen> ();
						string str = wellen.collocation;
						string[] array = str.Split('}');
						foreach(string str2 in array){
							if (str2 == null || str2.Length < 1) {
								continue;
							}
							string str3 = str2.Replace ("{", "");
							if (str3 == null || str3.Length < 1) {
								continue;
							}
							string[] array2 = str3.Split (',');
							LevelEnemyWellen enemy = new LevelEnemyWellen ();
							enemy.id =  long.Parse (array2 [0]);
							enemy.time =  int.Parse (array2 [1]);
							list.Add (enemy);
							if (!mEnemys.ContainsKey (enemy.id)) {
								foreach (Enemy tmp3 in  enemydata) {
									if (tmp3.id.Equals (array2 [0])) {
										mEnemys.Add (enemy.id, tmp3);
										break;
									}
								}
							}
						}
						mWellentList.Add (tmp, list);
						break;
					}
				}
			}
		}
		List<LevelEnemyWellen> back = mWellentList [wellent];
		Debug.Log ("getWellenEnemy :back size="+back.Count);
		mCurrentLevel++;
		if (mCurrentLevel > mCurrentLevelWellent.Count) {
			mCurrentLevel = 0;
		}
		return back;
	}	

	public Enemy getEnemyById(long id){
		return mEnemys [id];
	}

}

