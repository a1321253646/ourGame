﻿using UnityEngine;
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
        private string composeFile = "compose.json";
        private string dropDeviceDetailFile = "dropdevicedetail.json";
        private string dropDeviceFile = "dropdevice.json";
        private string speedValueFile = "speedvalue.json";
        private string skillFile = "skill.json";
        private string cardFile = "card.json";
        private string affixFile = "afffix.json";
    private string samsaraFile = "samsara.json";
    private string mapConfigFile = "mapconfig.json";
    /*private string levelFile = "level";
        private string heroFile = "hero";
        private string enemyFile = "enemy";
        private string levelEnemyFile = "levelenemy";
        private string resourceFile = "resource";
        private string configeFile = "config";
        private string goodsFile = "item";
        private string attributeFile = "equip";
        private string composeFile = "compose";
        private string dropDeviceDetailFile = "dropdevicedetail";
        private string dropDeviceFile = "dropdevice";
        private string speedValueFile = "speedvalue";
        private string skillFile = "skill";
        private string cardFile = "card";
        private string affixFile = "afffix";
        private string samsaraFile = "samsara";
        private string mapConfigFile = "mapconfig";*/
    private static JsonUtils mInance = new JsonUtils();

    List<Hero> heroData;
    List<Level> levelData;
    List<ResourceBean> resourceData;
    List<ConfigNote> mConfig;
    List<GoodJsonBean> mGoods;
    List<AccouterJsonBean> mAttribute;
    List<ComposeJsonBen> mComposeData;
    List<DropDeviceDetail> mDropDeviceDetailData;
    List<DropDevice> mDropDevoce;
    List<SpeedValueJsonBean> mSpeedValue;
    List<SkillJsonBean> mSkillDate;
    List<CardJsonBean> mCardDate;
    List<AffixJsonBean> mAffixDate;
    Dictionary<long, SamsaraJsonBean> mSamsaraDate = new Dictionary<long, SamsaraJsonBean>();
    private JsonUtils() {
        readAllFile();
    }

    public void readAllFile() {
        readResource();
        readConfig();
        readHeroData();
        readLevelData();        
        readGoodInfo();
        readAffixInfo();
        readAttributeInfo();
        readComposeInfo();
        readdropDeviceInfo();
        readdropDeviceDetailInfo();
        readSpeedValueInfo();
        readSkillInfo();
        readCardInfo();
        readSamsaraInfo();
    }

    public static JsonUtils getIntance() {
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


    private string readFile(string fileName) {
        //TextAsset jsonText = Resources.Load(fileName) as TextAsset;
        string str = loadFile(Application.dataPath + "/Resources", fileName);
        //string str = Resources.Load<TextAsset>(  fileName).text;
        Debug.Log("readFile :" + fileName + "\n " + str);

        return str;
    }
    private void readSkillInfo()
    {
        var arrdata = Newtonsoft.Json.Linq.JArray.Parse(readFile(skillFile));
        mSkillDate = arrdata.ToObject<List<SkillJsonBean>>();
    }
    private void readCardInfo()
    {
        var arrdata = Newtonsoft.Json.Linq.JArray.Parse(readFile(cardFile));
        mCardDate = arrdata.ToObject<List<CardJsonBean>>();
    }
    private void readSamsaraInfo() {
        var arrdata = Newtonsoft.Json.Linq.JArray.Parse(readFile(samsaraFile));
        List<SamsaraJsonBean> list = arrdata.ToObject<List<SamsaraJsonBean>>();
        foreach (SamsaraJsonBean bean in list) {
            SamsaraJsonBean samsaraId;
            List<SamsaraValueBean> tmpList;
            if (mSamsaraDate.ContainsKey(bean.id))
            {
                samsaraId = mSamsaraDate[bean.id];
            }
            else {
                samsaraId = new SamsaraJsonBean();
                samsaraId.id = bean.id;
                samsaraId.name = bean.name;
                samsaraId.sort = bean.sort;
                samsaraId.icon = bean.icon;
                mSamsaraDate.Add(bean.id, samsaraId);
            }
            if (samsaraId.costList == null) {
                samsaraId.costList = new Dictionary<long, long>();
                samsaraId.costList.Add(bean.level, bean.coast);
            }
            else{
                if (samsaraId.costList.ContainsKey(bean.level))
                {
                   samsaraId.costList[bean.level]= bean.coast;
                }
                else
                {
                    samsaraId.costList.Add(bean.level, bean.coast);
                }
            }
            if (samsaraId.levelList == null)
            {
                samsaraId.levelList = new Dictionary<long, List<SamsaraValueBean>>();
                tmpList = new List<SamsaraValueBean>();
                samsaraId.levelList.Add(bean.level, tmpList);
            }
            else {
                if (samsaraId.levelList.ContainsKey(bean.level))
                {
                    tmpList = samsaraId.levelList[bean.level];
                }
                else
                {
                    tmpList = new List<SamsaraValueBean>();
                    samsaraId.levelList.Add(bean.level, tmpList);
                }
            }
            bean.getKeyAndValueList(tmpList);
//            Debug.Log("readSamsaraInfo ud= "+ bean.id + " bean.level= " + bean.level+" count = "+ mSamsaraDate[bean.id].levelList[bean.level].Count);
        }
    }
    public Dictionary<long, SamsaraJsonBean> getSamsaraInfo(){
        return mSamsaraDate;
    }

    public SamsaraJsonBean getSamsaraInfoById(long id) {
        if (mSamsaraDate.ContainsKey(id))
        {
            return mSamsaraDate[id];
        }
        else {
            return null;
        }
    }
    public long getSamsaraCostByIdAndLevel(long id, long level)
    {
        if (mSamsaraDate.ContainsKey(id))
        {
            SamsaraJsonBean tmp = mSamsaraDate[id];
            if (tmp.costList != null && tmp.costList.ContainsKey(level))
            {
                return tmp.costList[level];
            }
        }
        return -1;
    }

    public List<SamsaraValueBean> getSamsaraVulueInfoByIdAndLevel(long id, long level) {
        if (mSamsaraDate.ContainsKey(id)) {
            SamsaraJsonBean tmp = mSamsaraDate[id];
            if (tmp.levelList != null && tmp.levelList.ContainsKey(level)) {
                return tmp.levelList[level];
            } 
        }
        return null;
    }
    private void readAttributeInfo()
    {
        var arrdata = Newtonsoft.Json.Linq.JArray.Parse(readFile(attributeFile));
        mAttribute = arrdata.ToObject<List<AccouterJsonBean>>();
    }
    private void readAffixInfo()
    {
        var arrdata = Newtonsoft.Json.Linq.JArray.Parse(readFile(affixFile));
        mAffixDate = arrdata.ToObject<List<AffixJsonBean>>();
    }

    private void readComposeInfo()
    {
        var arrdata = Newtonsoft.Json.Linq.JArray.Parse(readFile(composeFile)); 
         mComposeData = arrdata.ToObject<List<ComposeJsonBen>>();
    }
    private void readdropDeviceInfo()
    {
        var arrdata = Newtonsoft.Json.Linq.JArray.Parse(readFile(dropDeviceFile));
        mDropDevoce = arrdata.ToObject<List<DropDevice>>();
    }
    private void readSpeedValueInfo()
    {
        var arrdata = Newtonsoft.Json.Linq.JArray.Parse(readFile(speedValueFile));
        mSpeedValue = arrdata.ToObject<List<SpeedValueJsonBean>>();
    }
    public DropDevice getDropDevoiceByID(long id) {
        foreach (DropDevice detail in mDropDevoce)
        {
            if (id == detail.id)
            {
                return detail;
            }
        }
        return null;
    }
    private void readdropDeviceDetailInfo()
    {
        var arrdata = Newtonsoft.Json.Linq.JArray.Parse(readFile(dropDeviceDetailFile));
        List < DropDeviceDetailJson > detailJson = arrdata.ToObject<List<DropDeviceDetailJson>>();
        DropDeviceDetail dropDeviceDetail;
        mDropDeviceDetailData = new List<DropDeviceDetail>();
        foreach (DropDeviceDetailJson json in detailJson) {
            dropDeviceDetail = null;
            foreach (DropDeviceDetail detail in mDropDeviceDetailData) {
                if (detail.id == json.id) {
                    dropDeviceDetail = detail;
                    break;
                }
            }
            if (dropDeviceDetail == null)
            {
                dropDeviceDetail = new DropDeviceDetail();
                dropDeviceDetail.id = json.id;
                dropDeviceDetail.dropCount = json.dropCount;
                
                mDropDeviceDetailData.Add(dropDeviceDetail);
            }
            dropDeviceDetail.addItem(json);
        }
    }
    public float getFrequencyByValue(float value) {
        if (value <= mSpeedValue[0].value)
        {
            return mSpeedValue[0].frequency;
        }
        else if(value >= mSpeedValue[mSpeedValue.Count-1].value)
        {
            return mSpeedValue[mSpeedValue.Count - 1].frequency;

        }
        for (int i = 0; i < mSpeedValue.Count; i++) {
            if (value < mSpeedValue[i].value) {
                return mSpeedValue[i - 1].frequency;
            }
        }
        return 1;
    }

    public DropDeviceDetail getDropDeviceDetailById(long id) {
        foreach (DropDeviceDetail detail in mDropDeviceDetailData) {
            if (id == detail.id) {
                return detail;
            }
        }
        return null;
    }

    private void readGoodInfo() {
        var arrdata = Newtonsoft.Json.Linq.JArray.Parse(readFile(goodsFile));
        mGoods = arrdata.ToObject<List<GoodJsonBean>>();
    }

	private void readResource(){
		var arrdata = Newtonsoft.Json.Linq.JArray.Parse (readFile (resourceFile));
		resourceData = arrdata.ToObject<List<ResourceBean>> ();
        foreach (ResourceBean bean in resourceData) {
            bean.getBloodOffset().x = bean.zoom * bean.getBloodOffset().x;
            bean.getBloodOffset().y = bean.zoom * bean.getBloodOffset().y;
            bean.getHurtOffset().x = bean.zoom * bean.getHurtOffset().x;
            bean.getHurtOffset().y = bean.zoom * bean.getHurtOffset().y;
            bean.getFightOffset().x = bean.zoom * bean.getFightOffset().x;
            bean.getFightOffset().y = bean.zoom * bean.getFightOffset().y;
            bean.idel_y = bean.zoom * bean.idel_y;
            List<float> tar = bean.getTargetBorder();
            if (tar != null && tar.Count > 0) {
                for (int i = 0; i < tar.Count; i++)
                {
                    tar[i] = tar[i] * bean.zoom;
                }
            }

        }
	}
    private void readConfig()
    {
        var arrdata = Newtonsoft.Json.Linq.JArray.Parse(readFile(configeFile));
        mConfig = arrdata.ToObject<List<ConfigNote>>();
    }

    private void readHeroData(){
		var arrdata = Newtonsoft.Json.Linq.JArray.Parse (readFile (heroFile));
		heroData = arrdata.ToObject<List<Hero>> ();
        
        foreach (Hero hero in heroData) {
            ResourceBean res = getEnemyResourceData(hero.resource);
            hero.attack_range = hero.attack_range * (hero.range_type == 1? res.zoom :1);
        }
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

        List<Enemy> list =  arrdata.ToObject<List<Enemy>> ();
        foreach (Enemy e in list) {
            ResourceBean res = getEnemyResourceData(e.resource);
            e.attack_range = e.attack_range *(e.range_type == 1? res.zoom : 1) ;
        }
        return list;
  
	}

    
    public List<AccouterJsonBean> getAccouterInfoList()
    {
        return mAttribute;
    }
    public List<AffixJsonBean> getAffixInfoList()
    {
        return mAffixDate;
    }

    public AffixJsonBean getAffixInfoById(long id)
    {
        foreach (AffixJsonBean note in mAffixDate)
        {
            if (note.id == id)
            {
                return note;
            }
        }
        return null;
    }

    public AccouterJsonBean getAccouterInfoById(long id)
    {
        foreach (AccouterJsonBean note in mAttribute)
        {
            if (note.id == id)
            {
                return note;
            }
        }
        return null;
    }
    public SkillJsonBean getSkillInfoById(long id)
    {
        foreach (SkillJsonBean note in mSkillDate)
        {
            if (note.id == id)
            {
                return note;
            }
        }
        return null;
    }

    public List<CardJsonBean> getCardInfos() {
        return mCardDate;
    }

    public CardJsonBean getCardInfoById(long id)
    {
        foreach (CardJsonBean note in mCardDate)
        {
            if (note.id == id)
            {
                return note;
            }
        }
        return null;
    }

    public GoodJsonBean getGoodInfoById(long id)
    {
        foreach (GoodJsonBean note in mGoods)
        {
            if (note.id == id)
            {
                return note;
            }
        }
        return null;
    }
    public List<GoodJsonBean> getGoodInfoList() {
        return mGoods;
    }
    public List<ComposeJsonBen> getComposeInfo() {
        return mComposeData;
    }
    public ComposeJsonBen gettComposeInfoForId(long id)
    {
        foreach (ComposeJsonBen note in mComposeData)
        {
            if (note.id == id)
            {
                return note;
            }
        }
        return null;
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

