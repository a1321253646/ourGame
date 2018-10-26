using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SQLHelper
{
    List<SQLDate> mList = new List<SQLDate>();

    public long mGameLevel = -1;
    public long mHeroLevel = -1;
    public long isAutoBoss = -1;
    public long mLunhuiValue = - 1;
    public long mMojing = -1;

    private long GAME_ID_LEVEL = 1;
    private long GAME_ID_HERO = 2;
    private long GAME_ID_AUTO = 3;
    private long GAME_ID_LUNHUI = 4;
    private long GAME_ID_MOJING = 5;


    private long TYPE_GAME = 1;
    private long TYPE_GOOD = 2;
    private long TYPE_BOOK = 3;
    private long TYPE_CARD = 4;
    private long TYPE_ZHUANGBEI = 5;
    private long TYPE_LUNHUI = 6;

    List<long> mCard = new List<long>();
    List<long> mBook = new List<long>();
    List<PlayerBackpackBean> mUserZhuangbei = new List<PlayerBackpackBean>();
    List<PlayerBackpackBean> mALLGood = new List<PlayerBackpackBean>();
    Dictionary<long,long> mLunhuui = new Dictionary<long, long>();

   SQLManager mManager = null;
    private SQLHelper()
    {
        mManager = GameObject.Find("Manager").GetComponent<SQLManager>();
        mList = mManager.readAllTable();
        if (mList != null && mList.Count > 0) {
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
                        mMojing = long.Parse(date.extan);
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

    public void addGood(PlayerBackpackBean good) {

        string value = "count，" + good.count + "；" ;
        if (good.attributeList != null && good.attributeList.Count > 0) {
            foreach (PlayerAttributeBean b in good.attributeList) {
                value = value + b.type + "，" + b.value + "；";
            }
        }
        value = value + "sortID，" + good.sortID + "；";
        value = value + "tabId，" + good.tabId + "；";
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
        string value = "count," + good.count + ";";
        if (good.attributeList != null && good.attributeList.Count > 0)
        {
            foreach (PlayerAttributeBean b in good.attributeList)
            {
                value = value + b.type + "," + b.value + ";";
            }
        }
        mManager.InsertDataToSQL( new[] { "" + TYPE_ZHUANGBEI, "" + good.goodId, "'" + value + "'" });
    }
    public void addLunhui(long id)
    {
        mManager.InsertDataToSQL( new[] { "" + TYPE_LUNHUI, "" + id, "1" });
    }

    public void deleteGood(long goodId)
    {
        mManager.delete(TYPE_GOOD, goodId);
    }
    public void deleteBook(long book)
    {
        mManager.delete( TYPE_BOOK, book);
    }
    public void deleteUserCard(long card)
    {
        mManager.delete( TYPE_CARD, card);
    }
    public void deleteZuangbei(long goodId)
    {
        mManager.delete( TYPE_ZHUANGBEI, goodId);
    }


    public void ChangeGood(PlayerBackpackBean good) {
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
        mManager.UpdateInto( "'"+value+"'", TYPE_GOOD, good.goodId);
    }
    public void ChangeLuiHui(long id,long level)
    {
        string value = "" + level;
        mManager.UpdateInto( value, TYPE_LUNHUI, id);
    }

    public void updateGameLevel( long value)
    {
        if (mGameLevel == -1)
        {
            addGame(GAME_ID_LEVEL, value);
            mGameLevel = value;
        }
        else
        {
            updateGame(GAME_ID_LEVEL, value);
        }
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
    public void updateHunJing(long value)
    {
        if (mMojing == -1)
        {
            addGame(GAME_ID_MOJING, value);
            mMojing = value;
        }
        else
        {
            updateGame(GAME_ID_MOJING, value);
        }
       
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
