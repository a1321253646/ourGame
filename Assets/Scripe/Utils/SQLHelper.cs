using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SQLHelper
{
    List<SQLDate> mList = new List<SQLDate>();

    public long mGameLevel = -9999L;
    public long mHeroLevel = -1;
    public long isAutoBoss = -1;
    public long isLuiHui = -1;
    public long isLuiHuiDeal = -1;
    public BigNumber mLunhuiValue = new BigNumber();
    public BigNumber mMojing = new BigNumber();
    public long mOutTime = -1;

    public long isShowCardPoint = -1;
    public long isShowBackpackPoint = -1;
    public long isShowLuihuiPoint = -1;
    public long isShowPlayerPoint = -1;
    public long isFristStartGame = -1;
    public long isVoice = -1;
    public long mMaxGoodId = -1;


    public static long GAME_ID_LEVEL = 1;
    public static long GAME_ID_HERO = 2;
    public static long GAME_ID_AUTO = 3;
    public static long GAME_ID_LUNHUI = 4;
    public static long GAME_ID_MOJING = 5;
    public static long GAME_ID_TIME = 6;
//    public static long GAME_ID_GUIDE = 7;
    public static long GAME_ID_POINT_PLAYER = 8;
    public static long GAME_ID_POINT_BACKPACK = 9;
    public static long GAME_ID_POINT_LUNHUI = 10;
    public static long GAME_ID_POINT_CARD = 11;
    public static long GAME_ID_FRIST_START = 12;
    public static long GAME_ID_NO_LUNHUI = 13;
    public static long GAME_ID_IS_VOICE = 14;
    public static long GAME_ID_LUNHUI_DEAL = 15;
    public static long GAME_ID_GOOD_MAXID = 16;
    public static long GAME_ID_IS_NET = 17;


    public static long TYPE_GAME = 1;
    public static long TYPE_GOOD = 2;
    public static long TYPE_BOOK = 3;
    public static long TYPE_CARD = 4;
    public static long TYPE_ZHUANGBEI = 5;
    public static long TYPE_LUNHUI = 6;
    public static long TYPE_DROP = 7;
    public static long TYPE_GUIDE = 8;

    List<long> mGuide = new List<long>();
    List<long> mBook = new List<long>();
    List<PlayerBackpackBean> mALLGood = new List<PlayerBackpackBean>();
    Dictionary<long,long> mLunhuui = new Dictionary<long, long>();
    Dictionary<long,long> mDropDeviceCount = new Dictionary<long, long>();
    NetHelper mNetHelp = new NetHelper();

    SQLManager mManager = null;
    private SQLHelper()
    {
        mManager = GameObject.Find("Manager").GetComponent<SQLManager>();
    }

    public void init() {
        mList = mManager.readAllTable();
        if (mList != null) {
            Debug.Log("SQLHelper init mlist coutn =  " + mList.Count);
        }
        
        mLunhuui.Clear();
        mALLGood.Clear();
        mBook.Clear();
        mDropDeviceCount.Clear();
        if (mList != null && mList.Count > 0)
        {
            foreach (SQLDate date in mList)
            {
                Debug.Log("读取数据库 " + date.toString());
                if (date.type == TYPE_LUNHUI)
                {
                    mLunhuui.Add(date.id, long.Parse(date.extan));
                }
                else if (date.type == TYPE_DROP) {
                    if (mDropDeviceCount.ContainsKey(date.id))
                    {
                        mDropDeviceCount[date.id] = long.Parse(date.extan);
                    }
                    else {
                        mDropDeviceCount.Add(date.id, long.Parse(date.extan));
                    }
                    Debug.Log("读取数据库  掉落器 id= " + date.id + " count =" + mDropDeviceCount[date.id]);
                }
                else if (date.type == TYPE_GOOD)
                {
                    PlayerBackpackBean bean = getBeanFromStr(date.extan);
                    bean.goodId = date.id;
                    bean.sqlGoodId = date.goodId;
                    bean.goodType = date.goodType;
                    Debug.Log("读取数据库  id= " + bean.goodId + " count =" + bean.count);
                    mALLGood.Add(bean);
                }
                else if (date.type == TYPE_BOOK)
                {
                    mBook.Add(date.id);
                }
                else if (date.type == TYPE_GUIDE)
                {
                  //  long id = long.Parse(date.extan);
                    mGuide.Add(date.id);
                    Debug.Log("读取数据库 已经引导 " + date.id);
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
                        mLunhuiValue = BigNumber.getBigNumForString(date.extan); 

                    }
                    else if (date.id == GAME_ID_MOJING)
                    {
                        Debug.Log("读取数据库 mMojing" + date.extan);
                        mMojing = BigNumber.getBigNumForString(date.extan);

                    }
                    else if (date.id == GAME_ID_TIME) {
                        mOutTime = long.Parse(date.extan);
                        Debug.Log("读取数据库 上次离线时间" + mOutTime);
                    }
                 /*   else if (date.id == GAME_ID_GUIDE)
                    {
                        long id = long.Parse(date.extan);
                        mGuide.Add(id);
                        Debug.Log("读取数据库 已经引导 " + id);
                    }*/
                    else if (date.id == GAME_ID_POINT_PLAYER)
                    {
                        isShowPlayerPoint = long.Parse(date.extan);
                        Debug.Log("读取数据库 上次离线时间" + mOutTime);
                    }
                    else if (date.id == GAME_ID_POINT_BACKPACK)
                    {
                        isShowBackpackPoint = long.Parse(date.extan);
                        Debug.Log("读取数据库 上次离线时间" + mOutTime);
                    }
                    else if (date.id == GAME_ID_POINT_LUNHUI)
                    {
                        isShowLuihuiPoint = long.Parse(date.extan);
                        Debug.Log("读取数据库 上次离线时间" + mOutTime);
                    }
                    else if (date.id == GAME_ID_POINT_CARD)
                    {
                        isShowCardPoint = long.Parse(date.extan);
                        Debug.Log("读取数据库 上次离线时间" + mOutTime);
                    }
                    else if (date.id == GAME_ID_FRIST_START)
                    {
                        isFristStartGame = long.Parse(date.extan);
                        Debug.Log("读取数据库 上次离线时间" + mOutTime);
                    }
                    else if (date.id == GAME_ID_NO_LUNHUI)
                    {
                        isLuiHui = long.Parse(date.extan);
                        Debug.Log("读取数据库 是否轮回 " + isLuiHui);
                    }
                    else if (date.id == GAME_ID_IS_VOICE)
                    {
                        isVoice = long.Parse(date.extan);
                        Debug.Log("读取数据库 是否开启音量 " + isVoice);
                    }
                    else if (date.id == GAME_ID_LUNHUI_DEAL)
                    {
                        isLuiHuiDeal = long.Parse(date.extan);
                        Debug.Log("读取数据库 轮回后死亡 " + isLuiHuiDeal);
                    }
                    else if (date.id == GAME_ID_GOOD_MAXID)
                    {
                        mMaxGoodId = long.Parse(date.extan);
                        Debug.Log("读取数据库 最大物品id " + mMaxGoodId);
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
                    else if (ss[0].Equals("showPoint")) {
                        bean.isShowPoint = int.Parse(ss[1]);
                    }
                    else
                    {
                        PlayerAttributeBean att = new PlayerAttributeBean();
                        att.type = long.Parse(ss[0]);
                        att.value = long.Parse(ss[1]);
                        if (bean.attributeList == null)
                        {
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


    public long getCurrentGoodId() {
        if (mMaxGoodId == -1)
        {
            mMaxGoodId = 1;
            addGame(GAME_ID_GOOD_MAXID, mMaxGoodId);
        }
        else {
            mMaxGoodId++;
            updateGame(GAME_ID_GOOD_MAXID, mMaxGoodId);
        }
        return mMaxGoodId;
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
    public List<PlayerBackpackBean> getAllGood()
    {
        return mALLGood;
    }
    public Dictionary<long, long> getLunHui()
    {
        return mLunhuui;
    }
    public Dictionary<long, long> getDropDevice()
    {
        return mDropDeviceCount;
    }

    public void addGood(PlayerBackpackBean good) {
        SQLDate date = new SQLDate();
        date.extan = getGoodExtra(good);
        date.type = TYPE_GOOD;
        date.id = good.goodId;
        date.goodId = good.sqlGoodId;
        date.isClean = SQLDate.CLEAR;
        date.goodType = SQLDate.GOOD_TYPE_BACKPACK;
        mManager.InsertDataToSQL(date);
        

    }
    public void addBook(long book)
    {
            //    mManager.InsertDataToSQL( new[] { "" + TYPE_BOOK, "" + book, "1", "" + SQLDate.DEFAULT_GOOD_ID, "" + SQLDate.GOOD_TYPE_NOGOOD, isclean });
    }
    public void addLunhui(long id)
    {
        SQLDate date = new SQLDate();
        date.extan = "1";
        date.type = TYPE_LUNHUI;
        date.id = id;
        date.goodType = SQLDate.GOOD_TYPE_NOGOOD;
        date.goodId = SQLDate.DEFAULT_GOOD_ID;
        date.getClean();
        mManager.InsertDataToSQL(date);
    }

    public void deleteGood(PlayerBackpackBean good)
    {
        SQLDate date = new SQLDate();
        date.goodId = good.sqlGoodId;
        date.type = TYPE_GOOD;
        date.getClean();
        mManager.deleteGood(date);
    }

    public void deleteLuihui() {
        mDropDeviceCount.Clear();
        mManager.deleteLuiHui();
    }

    public void changeGoodTyppe(PlayerBackpackBean good) {
        SQLDate date = new SQLDate();
        date.type = TYPE_GOOD;
        date.id = good.goodId;
        date.goodId = good.sqlGoodId;
        date.goodType = good.goodType;
        date.extan = getGoodExtra(good);
        date.getClean();
        mManager.changeGoodType(date);

    }

    public static string getGoodExtra(PlayerBackpackBean good) {
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
        value = value + "showPoint，" + good.isShowPoint + "；";
        return value;
    }

    public void updateZHUANGBEI(PlayerBackpackBean good)
    {       
        SQLDate date = new SQLDate();
        date.type = TYPE_GOOD;
        date.id = good.goodId;
        date.extan = getGoodExtra(good);
        date.goodId = good.sqlGoodId;
        date.goodType = good.goodType;
        date.getClean();
        mManager.UpdateZhuangbeiInto(date);
    }
    public void ChangeGoodExtra(PlayerBackpackBean good) {
        SQLDate date = new SQLDate();
        date.type = TYPE_GOOD;
        date.id = good.goodId;
        date.extan = getGoodExtra(good);
        date.goodId = good.sqlGoodId;
        date.goodType = good.goodType;
        date.getClean();
        mManager.UpdateZhuangbeiInto(date);
    }
    public void ChangeLuiHui(long id,long level)
    {
        SQLDate date = new SQLDate();
        date.extan = "" + level;
        date.type = TYPE_LUNHUI;
        date.id = id;
        date.getClean();
        mManager.UpdateInto(date);
    }
    public void updateGameLevel( long value)
    {
        if (mGameLevel == -9999)
        {
            addGame(GAME_ID_LEVEL, value);
           
        }
        else
        {
            updateGame(GAME_ID_LEVEL, value);
        }
        mGameLevel = value;
    }
    public void updateOutTime()
    {
        long value = TimeUtils.GetTimeStamp(); 
        if (mOutTime == -1)
        {
            addGame(GAME_ID_TIME, value);
         
        }
        else
        {
            updateGame(GAME_ID_TIME, value);
        }
        mOutTime = value;
        mManager.updateToNet();
    }

    public void addGuide(long value) {
        Debug.Log("addGuide  =" + value);
        SQLDate date = new SQLDate();
        date.extan = "-1";
        date.type = TYPE_GUIDE;
        date.id = value;
        date.getClean();
        mManager.InsertDataToSQL(date);
       // addGame(GAME_ID_GUIDE, value);
    }

    public void updateHeroLevel(long value)
    {
        if (mHeroLevel == -1)
        {
            addGame(GAME_ID_HERO, value);
           
        }
        else
        {
            updateGame(GAME_ID_HERO, value);
        }
        mHeroLevel = value;
    }
    public void updateFristStart(long value)
    {
        if (isFristStartGame == -1)
        {
            addGame(GAME_ID_FRIST_START, value);           
        }
        else
        {
            updateGame(GAME_ID_FRIST_START, value);
        }
        isFristStartGame = value;
    }
    public void updateIsVoice(long value)
    {
        if (isVoice == -1)
        {
            addGame(GAME_ID_IS_VOICE, value);
        }
        else
        {
            updateGame(GAME_ID_IS_VOICE, value);
        }
        isVoice = value;
    }


    public void updatePointPlayer(long value)
    {
        if (isShowCardPoint == value)
        {
            return;
        }
        if (isShowCardPoint == -1)
        {
            addGame(GAME_ID_POINT_PLAYER, value);         
        }
        else
        {
            updateGame(GAME_ID_POINT_PLAYER, value);
        }
        isShowCardPoint = value;
    }
    public void updatePointBackpack(long value)
    {
        if (isShowBackpackPoint == value)
        {
            return;
        }
        if (isShowBackpackPoint == -1)
        {
            addGame(GAME_ID_POINT_BACKPACK, value);           
        }
        else
        {
            updateGame(GAME_ID_POINT_BACKPACK, value);
        }
        isShowBackpackPoint = value;
    }
    public void updatePointLunhui(long value)
    {
        if (isShowLuihuiPoint == value)
        {
            return;
        }
        if (isShowLuihuiPoint == -1)
        {
            addGame(GAME_ID_POINT_LUNHUI, value);
            
        }
        else
        {
            updateGame(GAME_ID_POINT_LUNHUI, value);
        }
        isShowLuihuiPoint = value;
    }
    public void updatePointCard(long value)
    {
        if (isShowCardPoint == value) {
            return;
        }
        if (isShowCardPoint == -1)
        {
            addGame(GAME_ID_POINT_CARD, value);
         
        }
        else
        {
            updateGame(GAME_ID_POINT_CARD, value);
        }
        isShowCardPoint = value;
    }

    public void updateAutoBoss(long value)
    {
        if (isAutoBoss == -1)
        {
            addGame(GAME_ID_AUTO, value);
           
        }
        else
        {
            updateGame(GAME_ID_AUTO, value);
        }
        isAutoBoss = value;

    }

    public void updateIsLunhuiValue(long value)
    {

        if (isLuiHui == -1)
        {
            addGame(GAME_ID_NO_LUNHUI, value);

        }
        else
        {
            updateGame(GAME_ID_NO_LUNHUI, value);
        }
        isLuiHui = value;
    }
    public void updateIsLunhuiDealValue(long value)
    {

        if (isLuiHuiDeal == -1)
        {
            addGame(GAME_ID_LUNHUI_DEAL, value);

        }
        else
        {
            updateGame(GAME_ID_LUNHUI_DEAL, value);
        }
        isLuiHuiDeal = value;
    }


    public void updateHunJing(BigNumber value)
    {
        if (mMojing.isEmpty())
        {
            addGame(GAME_ID_MOJING, value.toString());
         
        }
        else
        {
            updateGame(GAME_ID_MOJING, value.toString());
        }
        mMojing = value;
    }

    private void updateGame(long id, string value)
    {
        //        Debug.Log("=================================updateGame value== " + value+ "id="+id);
        SQLDate date = new SQLDate();
        date.extan = value;
        date.type = TYPE_GAME;
        date.id = id;
        date.getClean();
        mManager.UpdateInto(date);

    }
    private void addGame(long id, string value)
    {
        //        Debug.Log("==================================addGame value== " + value);
        SQLDate date = new SQLDate();
        date.extan =  value ;
        date.type = TYPE_GAME;
        date.id = id;
        date.getClean();
        mManager.InsertDataToSQL(date);

    }

    public void updateLunhuiValue(BigNumber value)
    {

        if (mLunhuiValue.isEmpty())
        {
            addGame(GAME_ID_LUNHUI, value.toString());

        }
        else
        {
            updateGame(GAME_ID_LUNHUI, value.toString());
        }
        mLunhuiValue = value;     
    }

    public void updateDropValue(long drop, long value)
    {
        if (mDropDeviceCount.ContainsKey(drop))
        {
          //  mDropDeviceCount[drop] = value;
            addDrop( drop, value);
            
        }
        else
        {
            //mDropDeviceCount.Add(drop, value);
            updateDrop( drop, value);
        }
    }


    private void updateDrop(long id, long value) {
        string value1 = ""+ value;
        SQLDate date = new SQLDate();
        date.extan = "" + value;
        date.type = TYPE_DROP;
        date.id = id;
        date.getClean();
        mManager.UpdateInto(date);
    }

    private void addDrop(long id, long value)
    {
        SQLDate date = new SQLDate();
        date.extan = "" + value;
        date.type = TYPE_DROP;
        date.id = id;
        date.getClean();
        mManager.InsertDataToSQL(date);
    }

    private void updateGame(long id, long value)
    {
        SQLDate date = new SQLDate();
        date.extan = "" + value;
        date.type = TYPE_GAME;
        date.id = id;
        date.getClean();
        mManager.UpdateInto(date);
    }
    private void addGame(long id, long value)
    {
/*        string value1 = "" + value;
        string isclean = "";
        if (id == SQLHelper.GAME_ID_AUTO ||
           id == SQLHelper.GAME_ID_LUNHUI ||
           id == SQLHelper.GAME_ID_TIME ||
           id == SQLHelper.GAME_ID_GUIDE ||
           id == SQLHelper.GAME_ID_POINT_LUNHUI ||
           id == SQLHelper.GAME_ID_NO_LUNHUI ||
           id == SQLHelper.GAME_ID_IS_VOICE)
        {
            isclean = isclean+ SQLDate.CLEAR_NO;
        }
        else
        {
            isclean = isclean + SQLDate.CLEAR;
        }*/
        SQLDate date = new SQLDate();
        date.extan = "" + value;
        date.type = TYPE_GAME;
        date.id = id;
        date.getClean();
        mManager.InsertDataToSQL(date);
    }
}
