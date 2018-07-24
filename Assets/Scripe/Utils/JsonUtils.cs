using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class JsonUtils
{
    //	private  string levelFile =  "config/level";
    //	private  string heroFile = "config/hero";
    //	private  string enemyFile = "config/enemy";
    //private  string levelEnemyFile = "config/levelenemy";
    //private  string resourceFile = "config/resource";
    //private string configeFile = "config/config";
    //private string goodsFile = "config/goods";
    private string levelFile = "level.json";
    private string heroFile = "hero.json";
    private string enemyFile = "enemy.json";
    private string levelEnemyFile = "levelenemy.json";
    private string resourceFile = "resource.json";
    private string configeFile = "config.json";
    private string goodsFile = "item.json";
    private string attributeFile = "equip.json";
    private static JsonUtils mInance= new JsonUtils();

	List<Hero> heroData;
	List<Level> levelData;
	List<ResourceBean> resourceData;
    List<ConfigNote> mConfig;
    List<GoodJsonBean> mGoods;
    List<AccouterJsonBean> mAttribute;
    private JsonUtils(){
		readAllFile ();
	}

	public void readAllFile(){
        readConfig();
        readHeroData ();
		readLevelData ();
		readResource ();
        readGoodInfo();
        readAttributeInfo();
    }

	public static JsonUtils getIntance(){
		return mInance;
    }

    public string loadFile(string path, string fileName)
    {
        StreamReader sr = null;
        Debug.Log("file = " + path + "//" + fileName);
        try
        {
            sr = File.OpenText(path + "//" + fileName);
            Debug.Log("配置加载完成");
        }
        catch
        {
            Debug.Log("配置加载失败");
            return null;
        }
        string line;
        string str = sr.ReadToEnd();
        Debug.Log(fileName + ":\n" + str);
        sr.Close();
        sr.Dispose();
        return str;
}


    private string readFile(string fileName){
		//TextAsset jsonText = Resources.Load(fileName) as TextAsset;
        string str = loadFile(Application.dataPath + "/Resources", fileName);

        Debug.Log ("readFile :" + fileName + "\n " + str);

//		foreach (Hero hero in heros) {
//			Debug.Log ("hero:id=" + hero.role_lv + " hp=" + hero.role_hp + " attack=" + hero.role_attack + " defense=" + hero.role_defense + " crystal=" + hero.lvup_crystal);
//		}
		return str;
//		Hero jsonObj = JsonUtility.FromJson<Hero>(jsonText.text);
	}

    private void readAttributeInfo()
    {
        var arrdata = Newtonsoft.Json.Linq.JArray.Parse(readFile(attributeFile));
        mAttribute = arrdata.ToObject<List<AccouterJsonBean>>();
    }

    private void readGoodInfo() {
        var arrdata = Newtonsoft.Json.Linq.JArray.Parse(readFile(goodsFile));
        mGoods = arrdata.ToObject<List<GoodJsonBean>>();
    }

	private void readResource(){
		var arrdata = Newtonsoft.Json.Linq.JArray.Parse (readFile (resourceFile));
		resourceData = arrdata.ToObject<List<ResourceBean>> ();
	}
    private void readConfig()
    {
        var arrdata = Newtonsoft.Json.Linq.JArray.Parse(readFile(configeFile));
        mConfig = arrdata.ToObject<List<ConfigNote>>();
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

    
    public List<AccouterJsonBean> getAccouterInfoList()
    {
        return mAttribute;
    }
    public List<GoodJsonBean> getGoodInfoList() {
        return mGoods;
    }

    public float getConfigValueForId(long id) {
        foreach (ConfigNote note in mConfig) {
            if (note.id == id) {
                return note.value;
            }
        }
        return -1;
    }


	public  ResourceBean getEnemyResourceData(long resourceId){
		foreach (ResourceBean resource in resourceData) {
			if (resource.id == resourceId) {
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
	public  Level getLevelData(long id){
		foreach (Level level in levelData) {
			if (level.id == id) {
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
	public Hero getHeroData(long lv){
		foreach (Hero hero in heroData) {
			if (hero.role_lv == lv) {
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
	public long mCurrentLevel = 0;
	public List<long> mCurrentLevelWellent;
	Dictionary<long,List<LevelEnemyWellen>> mWellentList;
	Dictionary<long,Enemy> mEnemys;
	long bossId;
	float bossGas;
	public List<LevelEnemyWellen> getWellenEnemy(){
		
		if (mCurrentLevelWellent == null || mCurrentLevelWellent.Count == 0) {
			mCurrentLevelWellent = new List<long>();
			Level level = getLevelData ();
			bossId = level.boss_DI;
			bossGas = level.boss_gas;
			string[] wellenStr = level.wellen.Split ('#');
			foreach (string str in wellenStr) {
				mCurrentLevelWellent.Add ( int.Parse (str));
			}
		}
		if (mCurrentLevel >= mCurrentLevelWellent.Count) {
			mCurrentLevel = 0;
		}
		long wellent = mCurrentLevelWellent [(int)mCurrentLevel];
	//	Debug.Log ("getWellenEnemy :wellent="+wellent);
		if (mWellentList == null || mWellentList.Count == 0) {
			mWellentList = new Dictionary<long,List<LevelEnemyWellen>> ();
			mEnemys = new Dictionary<long,Enemy> ();
			List<Enemy> enemydata = readEnemyData ();
			List<LevelEnemy> levelList = readLevelEnemyData ();
			mWellentList = new Dictionary<long, List<LevelEnemyWellen>> ();


			foreach (Enemy tmp3 in  enemydata) {
			//	Debug.Log ("enemydata id="+tmp3.id +" bossId="+bossId );

				if (tmp3.id == bossId) {
					mEnemys.Add ( bossId, tmp3);
					break;
				}
			}


			foreach(long tmp in mCurrentLevelWellent){
				foreach(LevelEnemy wellen in levelList){
					if (wellen.wellen== tmp) {
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
									if (tmp3.id == enemy.id) {
										mEnemys.Add (enemy.id, tmp3);
										break;
									}
								}
							}
						}
                        Debug.Log("getWellenEnemy add :wellent=" + wellent);
                        mWellentList.Add (tmp, list);
						break;
					}
				}
			}
		}
       // Debug.Log("getWellenEnemy :wellent=" + wellent);
        List<LevelEnemyWellen> back = mWellentList [wellent];
		//Debug.Log ("getWellenEnemy :back size="+back.Count);
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

