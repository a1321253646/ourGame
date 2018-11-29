using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SQLHelper
{
    List<SQLDate> mList = new List<SQLDate>();

    public long mGameLevel = -9999L;
    public long mHeroLevel = -1;
    public long isAutoBoss = -1;
    public long mLunhuiValue = - 1;
    public BigNumber mMojing = new BigNumber();
    public long mOutTime = -1;

    private long GAME_ID_LEVEL = 1;
    private long GAME_ID_HERO = 2;
    private long GAME_ID_AUTO = 3;
    private long GAME_ID_LUNHUI = 4;
    private long GAME_ID_MOJING = 5;
    private long GAME_ID_TIME = 6;
    private long GAME_ID_GUIDE = 7;


    private long TYPE_GAME = 1;
    private long TYPE_GOOD = 2;
    private long TYPE_BOOK = 3;
    private long TYPE_CARD = 4;
    private long TYPE_ZHUANGBEI = 5;
    private long TYPE_LUNHUI = 6;

    List<long> mGuide = new List<long>();
    List<long> mCard = new List<long>();
    List<long> mBook = new List<long>();
    List<PlayerBackpackBean> mUserZhuangbei = new List<PlayerBackpackBean>();
    List<PlayerBackpackBean> mALLGood = new List<PlayerBackpackBean>();
    Dictionary<long,long> mLunhuui = new Dictionary<long, long>();

   SQLManager mManager = null;
    private SQLHelper()
    {
        mManager = GameObject.Find("Manager").GetComponent<SQLManager>();
    }

    public void init() {
        mList =  mManager.readAllTable();
        mLunhuui.Clear();
        mALLGood.Clear();
        mUserZhuangbei.Clear();
        mCard.Clear();
        mBook.Clear();
        if (mList != null && mList.Count > 0)
        {
            foreach (SQLDate date in mList)
            {
                Debug.Log("读取数据库 " + date.toString());
                if (date.type == TYPE_LUNHUI)
                {
                    mLunhuui.Add(date.id, long.Parse(date.extan));
                }
                else if (date.type == TYPE_GOOD)
                {
                    PlayerBackpackBean bean = getBeanFromStr(date.extan);
                    bean.goodId = date.id;
                    Debug.Log("读取数据库  id= " +bean.goodId+" count ="+bean.count);
                    mALLGood.Add(bean);
                }
                else if (date.type == TYPE_ZHUANGBEI)
                {
                    PlayerBackpackBean bean = getBeanFromStr(date.extan);
                    bean.goodId = date.id;
                    mUserZhuangbei.Add(bean);
                }
                else if (date.type == TYPE_CARD)
                {
                    mCard.Add(date.id);
                }
                else if (date.type == TYPE_BOOK)
                {
                    mBook.Add(date.id);
                }
                else if (date.type == TYPE_GAME)
                {
                    if (date.id == GAME_ID_LEVEL)
                    {
                        mGameLevel = long.Parse(date.extan);
                    }
                    else if (date.id == GAME_ID_HERO)
                    {
                        mHeroLevel = long.Parse(date.extan);
                    }
                    else if (date.id == GAME_ID_AUTO)
                    {
                        isAutoBoss = long.Parse(date.extan);
                    }
                    else if (date.id == GAME_ID_LUNHUI)
                    {
                        mLunhuiValue = long.Parse(date.extan);

                    }
                    else if (date.id == GAME_ID_MOJING)
                    {
                        Debug.Log("读取数据库 mMojing" + date.extan);
                        mMojing = BigNumber.getBigNumForString(date.extan);
                       
                    }
                    else if (date.id == GAME_ID_TIME) {
                        mOutTime = long.Parse(date.extan);
                        Debug.Log("读取数据库 上次离线时间" + mOutTime);
                    }else if(date.id == GAME_ID_GUIDE)
                    {
                        long id = long.Parse(date.extan);
                        mGuide.Add(id);
                        Debug.Log("读取数据库 已经引导 " + id);
                    }
                }
            }
            Debug.Log("读取数据库 物品数量" + mALLGood.Count);
        }
    }

    private PlayerBackpackBean getBeanFromStr(string str) {
        PlayerBackpackBean bean = new PlayerBackpackBean();
        string[] strs = str.Split('；');
        if (str != null && str.Length > 0) {
            foreach (string s in strs) {
                string[] ss = s.Split('，');
                if (ss != null && ss.Length == 2) {
                    if (ss[0].Equals("count"))
                    {
                        bean.count = int.Parse(ss[1]);
                    }
                    else if (ss[0].Equals("sortID"))
                    {
                        bean.sortID = int.Parse(ss[1]);
                    }
                    else if (ss[0].Equals("tabId"))
                    {
                        bean.tabId = int.Parse(ss[1]);
                    }
                    else {
                        PlayerAttributeBean att = new PlayerAttributeBean();
                        att.type = long.Parse(ss[0]);
                        att.value = long.Parse(ss[1]);
                        if (bean.attributeList == null) {
                            bean.attributeList = new List<PlayerAttributeBean>();                        
                        }
                        bean.attributeList.Add(att);
                    }
                }
            }
        }
        return bean;
    }

    private static SQLHelper mIntance = new SQLHelper();
    public static SQLHelper getIntance()
    {
        return mIntance;
    }


    public long getLunhuiLevelById(long id) {
        if (mLunhuui.ContainsKey(id))
        {
            return mLunhuui[id];
        }
        else {
            return 0;
        }
        
    }
    public List<long> getBook()
    {
        return mBook;
    }
    public List<long> getGuide()
    {
        return mGuide;
    }
    public List<long> getUserCard() {
        return mCard;
    }
    public List<PlayerBackpackBean> getPlayUserZhuangbei() {
        return mUserZhuangbei;
    }
    public List<PlayerBackpackBean> getAllGood()
    {
        return mALLGood;
    }
    public Dictionary<long, long> getLunHui()
    {
        return mLunhuui;
    }
    public void addGood(PlayerBackpackBean good) {
        string value = getGoodExtra(good);
        mManager.InsertDataToSQL(new[] { "" + TYPE_GOOD, "" + good.goodId, "'" + value + "'" });

    }
    public void addBook(long book)
    {
                mManager.InsertDataToSQL( new[] { "" + TYPE_BOOK, "" + book, "1" });
    }
    public void addUserCard(long card)
    {
               mManager.InsertDataToSQL( new[] { "" + TYPE_CARD, "" + card, "1" });
    }
    public void addZHUANGBEI(PlayerBackpackBean good)
    {
        string value = getGoodExtra(good);
        mManager.InsertDataToSQL( new[] { "" + TYPE_ZHUANGBEI, "" + good.goodId, "'" + value + "'" });
    }
    public void addLunhui(long id)
    {
                mManager.InsertDataToSQL( new[] { "" + TYPE_LUNHUI, "" + id, "1" });
    }

    public void deleteGood(PlayerBackpackBean good)
    {
        string value = getGoodExtra(good);
        mManager.delete(TYPE_GOOD, good.goodId,"'"+ value+"'");
    }
    public void deleteAllGood()
    {
               mManager.delete(TYPE_GOOD, -1);
    }
    public void deleteBook(long book)
    {
                mManager.delete( TYPE_BOOK, book);
    }
    public void deleteAllBook()
    {
                       mManager.delete(TYPE_BOOK, -1);
    }
    public void deleteUserCard(long card)
    {
                      mManager.delete( TYPE_CARD, card);
    }
    public void deleteAllUserCard()
    {
                        mManager.delete(TYPE_CARD, -1);
    }
    public void deleteZuangbei(long goodId)
    {
                  mManager.delete( TYPE_ZHUANGBEI, goodId);
    }
    public void deleteAllZuangbei()
    {
                mManager.delete(TYPE_ZHUANGBEI, -1);
    }

    public string getGoodExtra(PlayerBackpackBean good) {
        string value = "count，" + good.count + "；";
        if (good.attributeList != null && good.attributeList.Count > 0)
        {
            foreach (PlayerAttributeBean b in good.attributeList)
            {
                value = value + b.type + "，" + b.value + "；";
            }
        }
        value = value + "sortID，" + good.sortID + "；";
        value = value + "tabId，" + good.tabId + "；";
        return value;
    }

    public void updateZHUANGBEI(PlayerBackpackBean good, string extra)
    {
        string value = getGoodExtra(good);
        mManager.UpdateZhuangbeiInto("'" + extra + "'", TYPE_ZHUANGBEI, good.goodId, "'" + value + "'");
    }
    public void ChangeGood(PlayerBackpackBean good) {
        string value = getGoodExtra(good);
        mManager.UpdateInto( "'"+value+"'", TYPE_GOOD, good.goodId);
    }
    public void ChangeLuiHui(long id,long level)
    {
        string value = "" + level;
        mManager.UpdateInto( value, TYPE_LUNHUI, id);
    }
    public void updateGameLevel( long value)
    {
        if (mGameLevel == -9999)
        {
            addGame(GAME_ID_LEVEL, value);
            mGameLevel = value;
        }
        else
        {
            updateGame(GAME_ID_LEVEL, value);
        }
    }
    public void updateOutTime()
    {
        long value = TimeUtils.GetTimeStamp(); 
        if (mOutTime == -1)
        {
            addGame(GAME_ID_TIME, value);
            mOutTime = value;
        }
        else
        {
            updateGame(GAME_ID_TIME, value);
        }
    }

    public void addGuide(long value) {
        Debug.Log("addGuide  =" + value);
        addGame(GAME_ID_GUIDE, value);
    }

    public void updateHeroLevel(long value)
    {
        if (mHeroLevel == -1)
        {
            addGame(GAME_ID_HERO, value);
            mHeroLevel = value;
        }
        else
        {
            updateGame(GAME_ID_HERO, value);
        }  
    }
    public void updateAutoBoss(long value)
    {
        if (isAutoBoss == -1)
        {
            addGame(GAME_ID_AUTO, value);
            isAutoBoss = value;
        }
        else
        {
            updateGame(GAME_ID_AUTO, value);
        }
       
    }
    public void updateHunJing(BigNumber value)
    {
        if (mMojing.isEmpty())
        {
            addGame(GAME_ID_MOJING, value.toString());
            mMojing = value;
        }
        else
        {
            updateGame(GAME_ID_MOJING, value.toString());
        }
       
    }

    private void updateGame(long id, string value)
    {
        Debug.Log("=================================updateGame value== " + value);
        mManager.UpdateInto("'"+value+ "'", TYPE_GAME, id);
    }
    private void addGame(long id, string value)
    {
        Debug.Log("==================================addGame value== " + value);
        mManager.InsertDataToSQL(new[] { "" + TYPE_GAME, "" + id, "'"+value+ "'" });
    }

    public void updateLunhuiValue(long value)
    {
        if (mLunhuiValue == -1)
        {
            addGame(GAME_ID_LUNHUI, value);
            mLunhuiValue = value;
        }
        else
        {
            updateGame(GAME_ID_LUNHUI, value);
            mLunhuiValue = value;
        }        
    }

    private void updateGame(long id, long value) {
        string value1 = "" + value;
        mManager.UpdateInto(value1, TYPE_GAME, id);
    }



    private void addGame(long id, long value)
    {
        string value1 = "" + value;
                   mManager.InsertDataToSQL(new[] { "" + TYPE_GAME, "" + id, value1 });
    }
}
