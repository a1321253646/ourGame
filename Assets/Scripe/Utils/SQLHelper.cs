using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SQLHelper
{
    List<SQLDate> mList = new List<SQLDate>();

    public long mGameLevel = BaseDateHelper.encodeLong(-9999L) ;
    public long mHeroLevel = BaseDateHelper.encodeLong(-1);
    public long isAutoBoss = -1;
    public long isLuiHui = -1;
    public long isLuiHuiDeal = -1;
    public BigNumber mLunhuiValue = new BigNumber();
    public BigNumber mMojing = new BigNumber();
    public BigNumber mOutLineEachjing = new BigNumber();
    public long mOutTime = -1;
    public long mMaxOutTime = -1;
    public long mMaxLevel = BaseDateHelper.encodeLong(-1);
    public long mNetLevel = BaseDateHelper.encodeLong(-1);

    public long isShowCardPoint = -1;
    public long isShowBackpackPoint = -1;
    public long isShowLuihuiPoint = -1;
    public long isShowPlayerPoint = -1;
    public long isShowPetTablePoint = -1;
    public long isFristStartGame = -1;
    public long isVoice = -1;
    public long mMaxGoodId = -1;
    public long isUpdate = -1;
    public string mPlayName =null;
    public string mToken ="";
    
    public long mVersionCode=-1;
    public bool isCleanNet=false;

    public long mCardLevel = -1;
    public long mCardListId = -1;

    public long isCloseYueqiang = -1;
    public long isCloseChuangye = -1;
    public long isHadLunhui = -1;
    public long mVocationCount = 0;
    public long mTargetSpeed = BaseDateHelper.encodeLong(-1);
    public long isCanLunhui = BaseDateHelper.encodeLong(-1);

    public BigNumber mOutLineGet = null;

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
    public static long GAME_ID_IS_UPDATE = 18;
    public static long GAME_ID_POINT_PETTABLE = 19;
    public static long GAME_ID_MAX_TIME = 20;
    public static long GAME_ID_PLAYER_NAME = 21;
    public static long GAME_ID_PLAYER_VOCATION = 22;
    public static long GAME_ID_OUTLINE_VALUE = 24;
    public static long GAME_ID_VERSION_CODE = 25;
    public static long GAME_ID_LEVEL_CARD = 26;
    public static long GAME_ID_PLAYER_VOCATION_1 = 27;
    public static long GAME_ID_PLAYER_VOCATION_2 = 28;
    public static long GAME_ID_PLAYER_VOCATION_3 = 29;
    public static long GAME_ID_PLAYER_VOCATION_4 = 30;
    public static long GAME_ID_PLAYER_VOCATION_5 = 31;
    public static long[] GAME_ID_PLAYER_VOCATION_LIST = new long[] { GAME_ID_PLAYER_VOCATION_1, GAME_ID_PLAYER_VOCATION_2, GAME_ID_PLAYER_VOCATION_3, GAME_ID_PLAYER_VOCATION_4, GAME_ID_PLAYER_VOCATION_5 };
    public static long GAME_ID_PLAYER_MAX_LEVEL = 32;
    public static long GAME_ID_SETTING_CLOSED_CHUANGYE = 33;
    public static long GAME_ID_SETTING_CLOSED_YUEQIANG = 34;
    public static long GAME_ID_HAD_LUNHUI = 35;
    public static long GAME_ID_NET_LEVEL = 36;

    public static long GAME_ID_TARGET_SPEED = 37;
    public static long GAME_ID_OUTLINE_MAX = 38;
    public static long GAME_ID_VOCATION_COUNT_MAX = 39;
    
    public static long GAME_ID_CAN_LUNHUI = 42;
    public static long GAME_ID_TOKEN = 43;

    public static long ACTIVITY_BUTTON_VOCATION = 1;
    public static long GAME_ID_PLAYER_AD = 2;

    public static long TYPE_GAME = 1;
    public static long TYPE_GOOD = 2;
    public static long TYPE_BOOK = 3;
    public static long TYPE_CARD = 4;
    public static long TYPE_ZHUANGBEI = 5;
    public static long TYPE_LUNHUI = 6;
    public static long TYPE_DROP = 7;
    public static long TYPE_GUIDE = 8;
    public static long TYPE_ACTIVITY_BUTTON = 9;
    public static long TYPE_ENCODE_VERSION = 10;
    public static long TYPE_CLEAN_NET = 11;
    public long mCurrentVocation = -1;
    public Dictionary<long, long>  mPlayVocation = new Dictionary<long, long>();

    List<long> mGuide = new List<long>();
    List<long> mBook = new List<long>();
    List<PlayerBackpackBean> mALLGood = new List<PlayerBackpackBean>();
    List<ActiveButtonBean> mActiveList = new List<ActiveButtonBean>();
    List<PlayerBackpackBean> mUser = new List<PlayerBackpackBean>();
    List<PlayerBackpackBean> mCard = new List<PlayerBackpackBean>();
    List<PlayerBackpackBean> mPet = new List<PlayerBackpackBean>();
    Dictionary<long,long> mLunhuui = new Dictionary<long, long>();
    Dictionary<long,long> mDropDeviceCount = new Dictionary<long, long>();
 //   NetHelper mNetHelp = new NetHelper();

    

  //  SQLManager SQLManager.getIntance() = null;
    private SQLHelper()
    {
    //    SQLManager.getIntance() = GameObject.Find("Manager").GetComponent<SQLManager>();
    }

    public void init() {
//        Debug.Log("readAllTable");
        mList = SQLManager.getIntance().readAllTable();
        if (mList != null) {
//            Debug.Log("SQLHelper init mlist coutn =  " + mList.Count);
        }
        
        mLunhuui.Clear();
        mALLGood.Clear();
        mUser.Clear();
        mCard.Clear();
        mBook.Clear();
        mPet.Clear();
        mDropDeviceCount.Clear();
        mGuide.Clear();
        mActiveList.Clear();
        mGameLevel = BaseDateHelper.encodeLong(-9999L);
        mHeroLevel = BaseDateHelper.encodeLong(-1);
        isAutoBoss = -1;
        isLuiHui = -1;
        mMaxLevel = BaseDateHelper.encodeLong(-1);
        mNetLevel = BaseDateHelper.encodeLong(-1);
        isLuiHuiDeal = -1;
        mLunhuiValue = new BigNumber();
        mMojing = new BigNumber();
        mOutLineEachjing =new BigNumber();
        mOutTime = -1;
      //  mVocationCount = 0;
        mOutLineGet = null;
        isCanLunhui = BaseDateHelper.encodeLong(-1);

        isShowCardPoint = -1;
        isShowBackpackPoint = -1;
        isShowLuihuiPoint = -1;
        isShowPlayerPoint = -1;
        isShowPetTablePoint = -1;
        isFristStartGame = -1;
        mTargetSpeed = BaseDateHelper.encodeLong(-1);
        mPlayVocation.Clear();
        mCurrentVocation = -1;
        isCloseYueqiang = -1;
        isCloseChuangye = -1;
        isHadLunhui = -1;
        isVoice = -1;
     //   mMaxGoodId = -1;
    //    isUpdate = -1;
        mPlayName = null;
        mToken = "";
        int goodListIndex = 0;
        long maxGoodIdTmp = -1;

        Dictionary<long, bool> goodIdMap = new Dictionary<long, bool>();

        if (mList != null && mList.Count > 0)
        {
            foreach (SQLDate date in mList)
            {
                if (date.isDelete == 2) {
                    continue;
                }
                Debug.Log("读取数据库 " + date.toString());
                if (date.type == TYPE_LUNHUI)
                {
                    long tmp = long.Parse(date.extan);
                //    if (tmp > 40) {
                //        tmp = 40;
                //    }
                    long count =BaseDateHelper.encodeLong(tmp) ;
                    if (mLunhuui.ContainsKey(date.id) )
                    {
                        if (BaseDateHelper.decodeLong(mLunhuui[date.id])  < BaseDateHelper.decodeLong(count)) {
                            mLunhuui[date.id] = count;
                        }
                        
                    }
                    else
                    {
                        mLunhuui.Add(date.id, count);
                    }
                    
                }
                else if (date.type == TYPE_DROP) {
                    long count = long.Parse(date.extan);
                    if (mDropDeviceCount.ContainsKey(date.id) )
                    {
                        if (mDropDeviceCount[date.id] < count) {
                            mDropDeviceCount[date.id] = long.Parse(date.extan);
                        }
                        
                    }
                    else {
                        mDropDeviceCount.Add(date.id, long.Parse(date.extan));
                    }
//                    Debug.Log("读取数据库  掉落器 id= " + date.id + " count =" + mDropDeviceCount[date.id]);
                }
                else if (date.type == TYPE_GOOD)
                {
                    PlayerBackpackBean bean = getBeanFromStr(date.extan);
                    bean.goodId = date.id;
                    bean.sqlGoodId = date.goodId;
                    
                    if (bean.sqlGoodId > maxGoodIdTmp)
                    {
                        maxGoodIdTmp = bean.sqlGoodId;
                    }
                    if(bean.count < 1) {
                        deleteGood(bean);
                        continue;
                    }
                    bean.goodType = date.goodType;
//                    Debug.Log("读取数据库  id= " + bean.goodId + " count =" + bean.count);
                    if (mALLGood.Count == 0) {
                        mALLGood.Add(bean);
                    }
                    else {
                        for (goodListIndex = 0; goodListIndex < mALLGood.Count; goodListIndex++) {
                            if (bean.sortID < mALLGood[goodListIndex].sortID)
                            {
                                mALLGood.Insert(goodListIndex,bean);
                                break;
                            }
                        }
                        if (goodListIndex == mALLGood.Count) {
                            mALLGood.Add(bean);
                        }
                    }

 //                   mALLGood.Add(bean);

                }
                else if (date.type == TYPE_BOOK)
                {
                    mBook.Add(date.id);
                }
                else if (date.type == TYPE_CLEAN_NET)
                {
                    isCleanNet = true;
                }
                else if (date.type == TYPE_GUIDE)
                {
                  //  long id = long.Parse(date.extan);
                    mGuide.Add(date.id);
//                    Debug.Log("读取数据库 已经引导 " + date.id);
                }
                else if (date.type == TYPE_ACTIVITY_BUTTON)
                {
                    //  long id = long.Parse(date.extan);
                    ActiveButtonBean bean = stringToActiveButton(date.extan);
                    bean.buttonType = date.id;
                    mActiveList.Add(bean);
//                    Debug.Log("活动按钮显示  buttonType" + bean.buttonType);
                }
                else if (date.type == TYPE_GAME)
                {
                    if (date.id == GAME_ID_LEVEL)
                    {
                        mGameLevel = BaseDateHelper.encodeLong(long.Parse(date.extan));
                    }
                    else if (date.id == GAME_ID_HERO)
                    {
                        mHeroLevel = BaseDateHelper.encodeLong(long.Parse(date.extan));
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
                        //                        Debug.Log("读取数据库 mMojing" + date.extan);
                        mMojing = BigNumber.getBigNumForString(date.extan);

                    }
                    else if (date.id == GAME_ID_OUTLINE_MAX)
                    {
                        //                        Debug.Log("读取数据库 mMojing" + date.extan);
                        mOutLineEachjing = BigNumber.getBigNumForString(date.extan);

                    }
                    else if (date.id == GAME_ID_TIME)
                    {
                        mOutTime = long.Parse(date.extan);
                        //                        Debug.Log("读取数据库 上次离线时间" + mOutTime);
                    }
                    else if (date.id == GAME_ID_PLAYER_MAX_LEVEL)
                    {
                        mMaxLevel = BaseDateHelper.encodeLong(long.Parse(date.extan));
                        //                        Debug.Log("读取数据库 上次离线时间" + mOutTime);
                    }
                    else if (date.id == GAME_ID_NET_LEVEL)
                    {
                        mNetLevel = BaseDateHelper.encodeLong(long.Parse(date.extan));
                        //                        Debug.Log("读取数据库 上次离线时间" + mOutTime);
                    }
                    else if (date.id == GAME_ID_IS_UPDATE)
                    {
                        isUpdate = long.Parse(date.extan);
                        //                        Debug.Log("读取数据库 是否已更新" + mOutTime);
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
                        //                        Debug.Log("读取数据库 上次离线时间" + mOutTime);
                    }
                    else if (date.id == GAME_ID_POINT_BACKPACK)
                    {
                        isShowBackpackPoint = long.Parse(date.extan);
                        //                        Debug.Log("读取数据库 上次离线时间" + mOutTime);
                    }
                    else if (date.id == GAME_ID_POINT_LUNHUI)
                    {
                        isShowLuihuiPoint = long.Parse(date.extan);
                        //                        Debug.Log("读取数据库 上次离线时间" + mOutTime);
                    }
                    else if (date.id == GAME_ID_POINT_CARD)
                    {
                        isShowCardPoint = long.Parse(date.extan);
                        //                        Debug.Log("读取数据库 上次离线时间" + mOutTime);
                    }
                    else if (date.id == GAME_ID_FRIST_START)
                    {
                        isFristStartGame = long.Parse(date.extan);
                        //                        Debug.Log("读取数据库 上次离线时间" + mOutTime);
                    }
                    else if (date.id == GAME_ID_NO_LUNHUI)
                    {
                        isLuiHui = long.Parse(date.extan);
                        //                        Debug.Log("读取数据库 是否轮回 " + isLuiHui);
                    }
                    else if (date.id == GAME_ID_IS_VOICE)
                    {
                        isVoice = long.Parse(date.extan);
                        //                       Debug.Log("读取数据库 是否开启音量 " + isVoice);
                    }
                    else if (date.id == GAME_ID_LUNHUI_DEAL)
                    {
                        isLuiHuiDeal = long.Parse(date.extan);
                        //                        Debug.Log("读取数据库 轮回后死亡 " + isLuiHuiDeal);
                    }
                    else if (date.id == GAME_ID_GOOD_MAXID)
                    {
                        mMaxGoodId = long.Parse(date.extan);
                        //                        Debug.Log("读取数据库 最大物品id " + mMaxGoodId);
                    }
                    else if (date.id == GAME_ID_POINT_PETTABLE)
                    {
                        isShowPetTablePoint = long.Parse(date.extan);
                        //                        Debug.Log("读取数据库 是否显示宠物小红点 " + isShowPetTablePoint);
                    }
                    else if (date.id == GAME_ID_MAX_TIME)
                    {
                        mMaxOutTime = long.Parse(date.extan);
                        //                        Debug.Log("读取数据库 最大离线时间 " + mMaxOutTime);
                    }
                    else if (date.id == GAME_ID_PLAYER_NAME)
                    {
                        mPlayName = date.extan;
                        //                        Debug.Log("读取数据库 用户名称 " + mPlayName);
                    }
                    else if (date.id == GAME_ID_PLAYER_VOCATION_1)
                    {
                        mPlayVocation.Add(0, long.Parse(date.extan));
                    }
                    else if (date.id == GAME_ID_PLAYER_VOCATION_2)
                    {
                        mPlayVocation.Add(1, long.Parse(date.extan));
                    }
                    else if (date.id == GAME_ID_PLAYER_VOCATION_3)
                    {
                        mPlayVocation.Add(2, long.Parse(date.extan));
                    }
                    else if (date.id == GAME_ID_PLAYER_VOCATION_4)
                    {
                        mPlayVocation.Add(3, long.Parse(date.extan));
                    }
                    else if (date.id == GAME_ID_PLAYER_VOCATION_5)
                    {
                        mPlayVocation.Add(4, long.Parse(date.extan));
                    }
                    else if (date.id == GAME_ID_PLAYER_VOCATION)
                    {
                        mCurrentVocation = long.Parse(date.extan);
                    }
                    else if (date.id == GAME_ID_OUTLINE_VALUE)
                    {
                        mOutLineGet = BigNumber.getBigNumForString(date.extan);
                        //                        Debug.Log("读取数据库 离线获取每分钟收益 " + mOutLineGet.toStringWithUnit());
                    }
                    else if (date.id == GAME_ID_VERSION_CODE)
                    {
                        mVersionCode = long.Parse(date.extan);
                        //                        Debug.Log("读取数据库 版本号 " + mVersionCode);
                    }
                    else if (date.id == GAME_ID_LEVEL_CARD)
                    {
                        string[] strs = date.extan.Split(',');
                        mCardLevel = long.Parse(strs[0]);
                        mCardListId = long.Parse(strs[1]);
                    }
                    else if (date.id == GAME_ID_SETTING_CLOSED_CHUANGYE)
                    {
                        isCloseChuangye = long.Parse(date.extan);
                    }
                    else if (date.id == GAME_ID_SETTING_CLOSED_YUEQIANG)
                    {
                        isCloseYueqiang = long.Parse(date.extan);
                    }
                    else if (date.id == GAME_ID_HAD_LUNHUI)
                    {
                        isHadLunhui = long.Parse(date.extan);
                    }
                    else if (date.id == GAME_ID_CAN_LUNHUI)
                    {
                        isCanLunhui = BaseDateHelper.encodeLong(long.Parse(date.extan));
                    }
                    else if (date.id == GAME_ID_TARGET_SPEED)
                    {
                        mTargetSpeed = BaseDateHelper.encodeLong(long.Parse(date.extan));
                    }
                    else if (date.id == GAME_ID_VOCATION_COUNT_MAX)
                    {
                        mVocationCount = long.Parse(date.extan);
                    }
                    else if (date.id == GAME_ID_TOKEN) {
                        mToken = date.extan;
                    }
                }
            }



            if ( maxGoodIdTmp > mMaxGoodId)
            {
                mMaxGoodId = maxGoodIdTmp;                    
                Debug.Log("=======================maxGoodIdTmp > mMaxGoodId================================");
            }
            for (int i = 0; i < mALLGood.Count; ) {
                PlayerBackpackBean bean = mALLGood[i];
                if (bean.goodId >= 4000001 && bean.goodId <= 4000099 && bean.goodType != SQLDate.GOOD_TYPE_PET && bean.goodType != SQLDate.GOOD_TYPE_USER_PET)
                {
                    Debug.Log("读取数据库 错误宠物 " + bean.toString());
                    deleteGood(bean);
                        mALLGood.RemoveAt(i);
                        continue;
                }
                else {
                    if (goodIdMap.ContainsKey(bean.sqlGoodId))
                    {
                        Debug.Log("读取数据库 重复ID " + bean.toString());
                        long oldId = bean.sqlGoodId;

                        mMaxGoodId++;
                        bean.sqlGoodId = mMaxGoodId;
                        changeGoodSqlId(bean, oldId);
                        goodIdMap.Add(bean.sqlGoodId, true);
                    }
                    else {
                        goodIdMap.Add(bean.sqlGoodId, true);
                    }
                }
                if (bean.goodType == SQLDate.GOOD_TYPE_USER_CARD)
                {
                    mCard.Add(bean);
                }
                else if (bean.goodType == SQLDate.GOOD_TYPE_ZHUANGBEI)
                {
                    mUser.Add(bean);
                }
                else if (bean.goodType == SQLDate.GOOD_TYPE_USER_PET || bean.goodType == SQLDate.GOOD_TYPE_PET)
                {
                    mPet.Add(bean);
                }
                i++;
            }
            List<PetJsonBean>  petList = JsonUtils.getIntance().getPet();
            Debug.Log("读取数据库 mMaxLevel" + BaseDateHelper.decodeLong(mMaxLevel));
            foreach (PetJsonBean pet in petList) {
                Debug.Log("读取数据库 id=" + pet.id+ " pet.activateLevel="+ pet.activateLevel);
                if (pet.activateLevel < BaseDateHelper.decodeLong(mMaxLevel)) {
                    bool isHave = false;
                    for (int i = 0; i < mPet.Count; i++) {
                        PlayerBackpackBean petBean = mPet[i];
                        if (petBean.goodId == pet.id) {
                            isHave = true;
                            break;
                        }
                    }
                    Debug.Log("读取数据库 isHave=" + isHave);
                    if (!isHave) {
                        PlayerBackpackBean newPet = new PlayerBackpackBean();
                        newPet.goodId = pet.id;
                        newPet.sortID = -1;
                        newPet.count = 1;
                        newPet.tabId = -1;
                        newPet.isShowPoint = -1;
                        mMaxGoodId++;
                        newPet.sqlGoodId = mMaxGoodId;
                        newPet.goodType  = SQLDate.GOOD_TYPE_PET;                     
                        addGood(newPet);
                        mALLGood.Add(newPet);
                        mPet.Add(newPet);
                    }
                }
            }
            if (mMaxGoodId == -1)
            {
                mMaxGoodId = 1;
                addGame(GAME_ID_GOOD_MAXID, mMaxGoodId);
            }
            else
            {
                updateGame(GAME_ID_GOOD_MAXID, mMaxGoodId);
            }
            if (string.IsNullOrEmpty(mPlayName)) {
                string name = NetServer.mDeviceID;
                name = "用户" + name.Substring(0, 4);
                updateName(name);
            }

            Debug.Log("读取数据库 物品数量" + mALLGood.Count);
            GameManager.getIntance().mInitDec = JsonUtils.getIntance().getStringById(100031);
            JsonUtils.getIntance().reReadAboutLevelFile(BaseDateHelper.decodeLong(mGameLevel));
        }
    }

    private PlayerBackpackBean getBeanFromStr(string str) {
        PlayerBackpackBean bean = new PlayerBackpackBean();
        string[] strs = str.Split(';');
        if (str != null && str.Length > 0) {
            foreach (string s in strs) {
                string[] ss = s.Split(',');
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
                        ss[1] = ss[1].Replace("E ", "E+");
                        att.value = double.Parse(ss[1]);
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

    public void updateIsCleanNet() {
        if (!isCleanNet) {
            SQLDate date = new SQLDate();
            date.type = TYPE_CLEAN_NET;
            date.isDelete = 2;
            date.isClean = SQLDate.CLEAR_NO;
            //   if (id != GAME_ID_TOKEN) {
            SQLManager.getIntance().InsertDataToSQL(date);
        }

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
    public bool haveGui(long id)
    {
        if (mGuide.Count == 0)
        {
            return false;
        }
        else {
            foreach (long g in mGuide) {
                if (g == id) {
                    return true;
                }
            }
        }
        return false;
    }
    public List<PlayerBackpackBean> getAllGood()
    {
        return mALLGood;
    }
    public List<ActiveButtonBean> getActiveList()
    {
        return mActiveList;
    }

    public List<PlayerBackpackBean> getCard()
    {
        return mCard;
    }
    public List<PlayerBackpackBean> getUserd()
    {
        return mUser;
    }
    public List<PlayerBackpackBean> getPet()
    {
        return mPet;
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
        date.goodType = good.goodType;
        date.getClean();
        SQLManager.getIntance().InsertDataToSQL(date);
    }
    public void addBook(long book)
    {
            //    SQLManager.getIntance().InsertDataToSQL( new[] { "" + TYPE_BOOK, "" + book, "1", "" + SQLDate.DEFAULT_GOOD_ID, "" + SQLDate.GOOD_TYPE_NOGOOD, isclean });
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
        SQLManager.getIntance().InsertDataToSQL(date);
    }

    public void deleteGood(PlayerBackpackBean good)
    {
        SQLDate date = new SQLDate();
        date.goodId = good.sqlGoodId;
        date.type = TYPE_GOOD;
        date.getClean();
        SQLManager.getIntance().deleteGood(date);
    }

    public void deleteLuihui() {
        mDropDeviceCount.Clear();
        mALLGood.Clear();
        mUser.Clear();
        mCard.Clear();

        mHeroLevel = BaseDateHelper.encodeLong(-1);
        mGameLevel = BaseDateHelper.encodeLong(-9999L) ;
        mMaxGoodId = -1;
        isShowCardPoint = -1;
        isShowBackpackPoint = -1;
        isShowPlayerPoint = -1;
        mCardLevel = -1;
        isLuiHuiDeal = -1;
        mMojing = new BigNumber();
        mOutLineEachjing = new BigNumber();
        SQLManager.getIntance().deleteLuiHui();
        updateGameLevel(BaseDateHelper.encodeLong(1));
        updateHeroLevel(BaseDateHelper.encodeLong(1));
        GameManager.getIntance().mCurrentLevel = BaseDateHelper.encodeLong(1);
        GameManager.getIntance().mHeroLv = BaseDateHelper.encodeLong(1);
        GameManager.getIntance().mCurrentCrystal = mMojing;
        
    }

    public void changeGoodTyppe(PlayerBackpackBean good) {
        SQLDate date = new SQLDate();
        date.type = TYPE_GOOD;
        date.id = good.goodId;
        date.goodId = good.sqlGoodId;
        date.goodType = good.goodType;
        date.extan = getGoodExtra(good);
        date.getClean();
        SQLManager.getIntance().changeGoodType(date);

    }
    public void changeGoodSqlId(PlayerBackpackBean good,long old)
    {
        SQLDate date = new SQLDate();
        date.type = TYPE_GOOD;
        date.id = good.goodId;
        date.goodId = good.sqlGoodId;
        date.goodType = good.goodType;
        date.extan = ""+ old;
        date.getClean();
        SQLManager.getIntance().changeGoodSql(date, old);

    }

    public static string getGoodExtra(PlayerBackpackBean good) {
        string value = "count," + good.count + ";";
        if (good.attributeList != null && good.attributeList.Count > 0)
        {
            foreach (PlayerAttributeBean b in good.attributeList)
            {
                value = value + b.type + "," + b.value + ";";
            }
        }
        value = value + "sortID," + good.sortID + ";";
        value = value + "tabId," + good.tabId + ";";
        value = value + "showPoint," + good.isShowPoint + ";";
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
        SQLManager.getIntance().UpdateZhuangbeiInto(date);
    }
    public void ChangeGoodExtra(PlayerBackpackBean good) {
        SQLDate date = new SQLDate();
        date.type = TYPE_GOOD;
        date.id = good.goodId;
        date.extan = getGoodExtra(good);
        date.goodId = good.sqlGoodId;
        date.goodType = good.goodType;
        date.getClean();
        SQLManager.getIntance().UpdateZhuangbeiInto(date);
    }
    public void ChangeLuiHui(long id,long level)
    {
        SQLDate date = new SQLDate();
        date.extan = "" + BaseDateHelper.decodeLong(level) ;
        date.type = TYPE_LUNHUI;
        date.id = id;
        date.getClean();
        SQLManager.getIntance().UpdateInto(date);
    }
    public void updateGameLevel( long value)
    {
        if (mGameLevel == BaseDateHelper.encodeLong(-9999L))
        {
            addGame(GAME_ID_LEVEL, BaseDateHelper.decodeLong(value));
           
        }
        else
        {
            updateGame(GAME_ID_LEVEL, BaseDateHelper.decodeLong(value));
        }
        mGameLevel = value;
        if (mMaxLevel == BaseDateHelper.encodeLong(-1) ||
             BaseDateHelper.decodeLong(mGameLevel) > BaseDateHelper.decodeLong(mMaxLevel)) {
            updateMaxLevel(mGameLevel);
        }
        if (mNetLevel == BaseDateHelper.encodeLong(-1) ||
            BaseDateHelper.decodeLong(mGameLevel) > BaseDateHelper.decodeLong(mNetLevel))
        {
            updateNetLevel(mGameLevel);
        }
    }
    public void updateVersionCode(long version)
    {
        if (mVersionCode == -1)
        {
            addGame(GAME_ID_VERSION_CODE, version);

        }
        else
        {
            updateGame(GAME_ID_VERSION_CODE, version);
        }
        updateNetLevel(BaseDateHelper.encodeLong(-10));
        mVersionCode = version;
    }
    public void updateVocationCount(long count)
    {
        if (mVocationCount == 0)
        {
            addGame(GAME_ID_VOCATION_COUNT_MAX, count);

        }
        else
        {
            updateGame(GAME_ID_VOCATION_COUNT_MAX, count);
        }
        mVocationCount = count;
    }


    public void updateIsUpdate()
    {
   //     Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>updateIsUpdate<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
        if (isUpdate == -1)
        {
            addGame(GAME_ID_IS_UPDATE, 1);

        }
        else
        {
            updateGame(GAME_ID_IS_UPDATE, 1);
        }
        isUpdate = 1;
   //     Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>isUpdate="+ isUpdate + "<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
    }
    public void updateTarget(long value)
    {
        if (mTargetSpeed == BaseDateHelper.encodeLong(-1))
        {
            addGame(GAME_ID_TARGET_SPEED, 1);

        }
        else
        {
            updateGame(GAME_ID_TARGET_SPEED, BaseDateHelper.decodeLong(value));
        }
        mTargetSpeed = value;
    }


    public void addHadLunhui()
    {
        if (isHadLunhui == -1)
        {
            addGame(GAME_ID_HAD_LUNHUI, 1);

        }
        isHadLunhui = 1;
    }
    public void UpdateCanLunhui(long level)
    {
        if (isCanLunhui == BaseDateHelper.encodeLong(-1))
        {
            addGame(GAME_ID_CAN_LUNHUI, BaseDateHelper.decodeLong(level));

        }
        else
        {
            updateGame(GAME_ID_CAN_LUNHUI,  BaseDateHelper.decodeLong(level));
        }
        isCanLunhui = level;
    }
    private void updateMaxLevel(long level)
    {
        if (mMaxLevel == BaseDateHelper.encodeLong(-1))
        {
            addGame(GAME_ID_PLAYER_MAX_LEVEL, BaseDateHelper.decodeLong(level));

        }
        else
        {
            updateGame(GAME_ID_PLAYER_MAX_LEVEL, BaseDateHelper.decodeLong(level));
        }
        mMaxLevel = level;
    }
    private void updateNetLevel(long level)
    {
        if (mNetLevel == BaseDateHelper.encodeLong(-1))
        {
            addGame(GAME_ID_NET_LEVEL, BaseDateHelper.decodeLong(level));

        }
        else
        {
            updateGame(GAME_ID_NET_LEVEL, BaseDateHelper.decodeLong(level));
        }
        mNetLevel = level;
    }


    public void updateName(string newName)
    {
        Debug.Log("sqlhelper  updateName newName = " + newName);
        if (mPlayName  == null)
        {
            addGame(GAME_ID_PLAYER_NAME, newName);

        }
        else
        {
            updateGame(GAME_ID_PLAYER_NAME, newName);
        }
        mPlayName = newName;
    }
    public void updateToken(string token)
    {
        Debug.Log("sqlhelper  updateName newName = " + token);
        if (string.IsNullOrEmpty(mToken))
        {
            addGame(GAME_ID_TOKEN, token);

        }
        else
        {
            updateGame(GAME_ID_TOKEN, token);
        }
        mToken = token;
        GameObject.Find("setting_ui").GetComponent<SettingUiControl>().setTokenId();
    }

    public void updateOutLineGet(BigNumber value) {
        
        if (mOutLineGet == null)
        {            
            addGame(GAME_ID_OUTLINE_VALUE, value.toString());
        }
        else {
            updateGame(GAME_ID_OUTLINE_VALUE, value.toString());
        }
        mOutLineGet = value;
    }

    public void updateOutTime()
    {
   //     Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>updateOutTime<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
        long value = TimeUtils.GetTimeStamp(); 
        if (mOutTime == -1)
        {
            addGame(GAME_ID_TIME, value);
         
        }
        else
        {
            if (value > mMaxOutTime) {
                updateMaxOutTime(value);
            }else if(value < mMaxOutTime - GameManager.ERROR_TIME_MIN){
                GameManager.getIntance().isError = true;
            }
            updateGame(GAME_ID_TIME, value);
        }
        mOutTime = value;
    //    Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>isUpdate =" + isUpdate + "<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
        if (isUpdate != -1) {
            SQLManager.getIntance().updateToNet();
        }        
    }
    private void updateMaxOutTime(long time)
    {
        if (mMaxOutTime == -1)
        {
            addGame(GAME_ID_MAX_TIME, time);

        }
        else
        {
            updateGame(GAME_ID_MAX_TIME, time);
        }
        mMaxOutTime = time;
    }

    public void deleteAll() {
        SQLManager.getIntance().deleteAll();
    }

    public void updateActiveButton(ActiveButtonBean value)
    {
        Debug.Log("addGuide  =" + value);
        SQLDate date = new SQLDate();
        date.extan = activeButtonToString(value);
        date.type = TYPE_ACTIVITY_BUTTON;
        date.id = value.buttonType;
        date.getClean();
        //mActiveList.Add(value);
        SQLManager.getIntance().updateIdAndType(date);
        // addGame(GAME_ID_GUIDE, value);
    }


    public void addActiveButton(ActiveButtonBean value) {
        Debug.Log("addGuide  =" + value);
        SQLDate date = new SQLDate();
        date.extan = activeButtonToString(value);
        date.type = TYPE_ACTIVITY_BUTTON;
        date.id = value.buttonType;
        date.getClean();
        mActiveList.Add(value);
        SQLManager.getIntance().InsertDataToSQL(date);
       // addGame(GAME_ID_GUIDE, value);
    }
    public void deleteActiveButton(ActiveButtonBean value)
    {

        Debug.Log("deleteActiveButton  =" + value);
        SQLDate date = new SQLDate();
        date.extan = "-1";
        date.type = TYPE_ACTIVITY_BUTTON;
        date.id = value.buttonType;
        date.getClean();
        mActiveList.Remove(value);
        SQLManager.getIntance().deleteIdAndType(date);
    }

    private string activeButtonToString(ActiveButtonBean value) {
        string str = "";
        str = "" + value.adType;
        return str;
    }
    private ActiveButtonBean stringToActiveButton(string value)
    {

        ActiveButtonBean button = new ActiveButtonBean();
        if (value != null && value.Length > 0)
        {
            if (value.Contains(","))
            {
                value = value.Split(',')[0];
            }
            button.adType = long.Parse(value);
        }
        return button;
    }


    public void addGuide(long value)
    {
        SQLManager.getIntance().deleteGuide(value);
//        Debug.Log("addGuide  =" + value);
        SQLDate date = new SQLDate();
        date.extan = "-1";
        date.type = TYPE_GUIDE;
        date.id = value;
        date.getClean();
        mGuide.Add(value);
        SQLManager.getIntance().InsertDataToSQL(date);
        // addGame(GAME_ID_GUIDE, value);
    }



    public void updateHeroLevel(long value)
    {
//        Debug.Log("mHeroLevel  = = " + mHeroLevel);
        if (mHeroLevel == BaseDateHelper.encodeLong(-1))
        {
            addGame(GAME_ID_HERO, BaseDateHelper.decodeLong(value));
           
        }
        else
        {
            updateGame(GAME_ID_HERO, BaseDateHelper.decodeLong(value));
        }
        mHeroLevel = value;
 //       Debug.Log("mHeroLevel  = = " + mHeroLevel);
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
    public void updateIsCloseYueqiang(bool value)
    {
        long valueInt = value ? 1 : 2;
        if (isCloseYueqiang == -1)
        {
            addGame(GAME_ID_SETTING_CLOSED_YUEQIANG, valueInt);
        }
        else
        {
            updateGame(GAME_ID_SETTING_CLOSED_YUEQIANG, valueInt);
        }
        isCloseYueqiang = valueInt;
    }
    public void updateIsCloseChuangyue(bool value)
    {
        long valueInt = value ? 1 : 2;
        if (isCloseChuangye == -1)
        {
            addGame(GAME_ID_SETTING_CLOSED_CHUANGYE, valueInt);
        }
        else
        {
            updateGame(GAME_ID_SETTING_CLOSED_CHUANGYE, valueInt);
        }
        isCloseChuangye = valueInt;
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
    public void updatePointPetTable(long value)
    {
        if (isShowPetTablePoint == value)
        {
            return;
        }
        if (isShowPetTablePoint == -1)
        {
            addGame(GAME_ID_POINT_PETTABLE, value);
        }
        else
        {
            updateGame(GAME_ID_POINT_PETTABLE, value);
        }
        isShowPetTablePoint = value;
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


    public void updateVocation(long value,bool isHaveSkill)
    {
        if (mCurrentVocation == -1)
        {
            addGame(GAME_ID_PLAYER_VOCATION, value);
        }
        else {
            updateGame(GAME_ID_PLAYER_VOCATION, value);
            
        }
        mCurrentVocation = value;
        if (!isHaveSkill) {
            if (mPlayVocation.ContainsKey(0))
            {
                updateGame(GAME_ID_PLAYER_VOCATION_LIST[0], value);
            }
            else
            {
                mPlayVocation.Add(0, value);
                addGame(GAME_ID_PLAYER_VOCATION_LIST[0], value);
            }
            return;
        }

        for (int i = 0; i < GAME_ID_PLAYER_VOCATION_LIST.Length; i++) {
            if (mPlayVocation.ContainsKey(i))
            {
                continue;
            }
            else {
                mPlayVocation.Add(i, value);
                addGame(GAME_ID_PLAYER_VOCATION_LIST[i], value);
                break;
            }
        }
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
    public void updateOutLineMojing(BigNumber value)
    {
        if (mMojing.isEmpty())
        {
            addGame(GAME_ID_OUTLINE_MAX, value.toString());
         
        }
        else
        {
            updateGame(GAME_ID_OUTLINE_MAX, value.toString());
        }
        mOutLineEachjing = value;
    }


    private void updateGame(long id, string value)
    {
//                Debug.Log("=================================updateGame value== " + value+ "id="+id);
        SQLDate date = new SQLDate();
        date.extan = value;
        date.type = TYPE_GAME;
        date.id = id;
        date.getClean();
     //   if (id != GAME_ID_TOKEN)
     //   {
            SQLManager.getIntance().UpdateInto(date);
     //   }     

    }
    private void addGame(long id, string value)
    {
  //              Debug.Log("==================================addGame value== " + value+ " id="+ id);
        SQLDate date = new SQLDate();
        date.extan =  value ;
        date.type = TYPE_GAME;
        date.id = id;
        date.getClean();
     //   if (id != GAME_ID_TOKEN) {
            SQLManager.getIntance().InsertDataToSQL(date);
    //    }
       

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

    public void updateLevelCardId(long level, long id) {
        if (mCardLevel == -1)
        {
            addGame(GAME_ID_LEVEL_CARD, level + "," + id);
        }
        else {
            updateGame(GAME_ID_LEVEL_CARD, level + "," + id);
        }
        mCardLevel = level;
        mCardListId = id;
    }


    public void updateDrop(long id, long value) {
        string value1 = ""+ value;
        SQLDate date = new SQLDate();
        date.extan = "" + value;
        date.type = TYPE_DROP;
        date.id = id;
        date.getClean();
        SQLManager.getIntance().UpdateInto(date);
    }

    public void addDrop(long id, long value)
    {
        SQLDate date = new SQLDate();
        date.extan = "" + value;
        date.type = TYPE_DROP;
        date.id = id;
        date.getClean();
        SQLManager.getIntance().InsertDataToSQL(date);
    }

    private void updateGame(long id, long value)
    {
        SQLDate date = new SQLDate();
        date.extan = "" + value;
        date.type = TYPE_GAME;
        date.id = id;
        date.getClean();

        SQLManager.getIntance().UpdateInto(date);
        
    }
    private void addGame(long id, long value)
    {

        SQLDate date = new SQLDate();
        date.extan = "" + value;
        date.type = TYPE_GAME;
        date.id = id;
        date.getClean();
        SQLManager.getIntance().InsertDataToSQL(date);
        
    }
}
