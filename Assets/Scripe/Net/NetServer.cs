using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using LitJson;

public class NetServer 
{
    public static int ERROR_DOED_TOKEN_FAULT = -1001;
    public static int ERROR_DOED_GET_LOCAL_FAULT = -1002;
    public static int ERROR_EXIT_FAULT = -8866;

    public bool isUpdate = false;
    public bool isNew = false;
    List<SqlNetDate> mList = null;
    public long mTime = -1;

    public static string mDeviceID = "";

   //private static string URL_ROOT =  "http://120.79.249.55:8889";
    private static string URL_ROOT = "http://120.79.249.55:8809";

    public void updateNet(List<SqlNetDate> list) {
        if (GameManager.isTestVersion)
        {
            return;
        }
        Debug.Log("NetServer  updateNet");
        isUpdate = true;
        mList = list;
        Thread th1 = new Thread(threadRun);
        th1.Start();
    }

    private JsonData getJsonRootData(string user) {
        JsonData json = new JsonData();
        json["user"] = user;
        json["version"] = 1;
        json["channel"] = GameManager.CHANNEL_CODE;
        if (!string.IsNullOrEmpty(SQLHelper.getIntance().mToken))
        {
            json["token"] = SQLHelper.getIntance().mToken;
        }
        return json;
    }
    private JsonData getJsonRootData()
    {

        return getJsonRootData(mDeviceID);
    }

    private JsonData getActionDate(SqlNetDate date) {
        JsonData jb = new JsonData();
        jb["action"] = date.action;
        jb["type"] = date.date.type;
        jb["id"] = date.date.id;
        jb["goodId"] = date.date.goodId;
        jb["goodtype"] = date.date.goodType;
        jb["isclean"] = date.date.isClean;
        jb["extra"] = date.date.extan;
        return jb;
    }


    private void threadRun() {

        JsonData json = getJsonRootData();

        if (mList != null)
        {
            Debug.Log("NetServer  updateNet mList.Count = " + mList.Count);
        }
        else {
            Debug.Log("NetServer  updateNet mList = " + mList);
        }
        if (mList != null && mList.Count > 0) {
            JsonData array = new JsonData();
            for (int i = 0; i< mList.Count;) {
                SqlNetDate date = mList[i];
                Debug.Log("NetServer  updateNet date.date.type = " + date.date.type);
                Debug.Log("NetServer  updateNet  date.date.id = " + date.date.id);
                if (date.date.type == SQLHelper.TYPE_GAME && date.date.id == SQLHelper.GAME_ID_IS_NET) {
                    mList.Remove(date);
                    continue;
                }
                array.Add(getActionDate(date));
                i++;
            }
            Debug.Log("NetServer  updateNet array.Count = " + array.Count);
            if (array.Count > 0) {
                json["date"] = array;
            }
            
        };
        Debug.Log("NetServer  updateNet json = "+ json.ToJson());
        Dictionary<string, string> dir = new Dictionary<string, string>();
        dir.Add("Content-Type", "application/json");
     //   dir.Add("Connection", "close");
        byte[] pData = System.Text.Encoding.UTF8.GetBytes(json.ToJson().ToCharArray());

        WWW www = new WWW(URL_ROOT+"/ourgame", pData, dir);
        while (!www.isDone) {
            Thread.Sleep(100);
        }

        if (www.error == null) 
        {
            Debug.Log("Upload complete! "+ www.text);
            if (www.text != null && www.text.Length > 0)
            {
                JsonData jb = JsonMapper.ToObject(www.text);
                int status = int.Parse(jb["status"].ToString());
                long getTime = long.Parse(jb["time"].ToString());
                GameManager.getIntance().mNewAPKVersionCode = long.Parse(jb["version"].ToString());
                string token = jb["token"].ToString(); 
                dealRepond(getTime, status, token,false);
                if (status == 0) {
                    GameManager.getIntance().isUpdateToNetEnd = true;
                    GameManager.getIntance().isUpdateToNetIsSuccess = true;


                }               
                mList = null;             
            }
            isUpdate = false;
        }
        else
        {
            isUpdate = false;
            GameManager.getIntance().isUpdateToNetEnd = true;
            GameManager.getIntance().isUpdateToNetIsSuccess = false;
            mList = null;
            Debug.Log("Http错误代码:" + www.error);
        }        
    }
    public string mLocal;



    private NetServer() {
#if UNITY_ANDROID
        mDeviceID = SystemInfo.deviceUniqueIdentifier;
#endif
#if UNITY_IOS

#endif

    }
    private static NetServer mIntance = new NetServer();
    public static NetServer  getIntance()
    {
        return mIntance;
    }
    public bool getLocl(string token,bool isReplace,bool skipMac)
    {

        if (GameManager.isTestVersion)
        {
            return true;
        }

        string name = mDeviceID;
        if (skipMac)
        {
            name = "skip_" + mDeviceID;
        }
        JsonData json = getJsonRootData(name);
        mLocal = null;
        JsonData array = new JsonData();
        JsonData jb = new JsonData();
        jb["action"] = 5;
        jb["type"] = -1;
        jb["id"] = -1;
        jb["goodId"] = -1;
        jb["goodtype"] = -1;
        jb["isclean"] = -1;
        
        if (!string.IsNullOrEmpty(token))
        {
            jb["extra"] = "-1";
        }
        array.Add(jb);
        json["date"] = array;
        Debug.Log("NetServer  updateNet json = "+ json.ToJson());
        Dictionary<string, string> dir = new Dictionary<string, string>();
        dir.Add("Content-Type", "application/json");
        //   dir.Add("Connection", "close");
        byte[] pData = System.Text.Encoding.UTF8.GetBytes(json.ToJson().ToCharArray());

        WWW www = new WWW(URL_ROOT+"/ourgame", pData, dir);
        while (!www.isDone)
        {
            Thread.Sleep(100);
        }
        if (www.error == null )
        {
            Debug.Log("Upload complete! www.text ="+ www.text);
           
            if (www.text != null && www.text.Length > 0 && !www.text.Equals("error"))
            {
                JsonData jb2 = JsonMapper.ToObject(www.text);
                int status = int.Parse(jb2["status"].ToString());
                Debug.Log("Upload complete! status =" + status);
                long getTime = long.Parse(jb2["time"].ToString());
                Debug.Log("Upload complete! getTime =" + getTime);
                GameManager.getIntance().mNewAPKVersionCode = long.Parse(jb2["version"].ToString());
                Debug.Log("Upload complete! mNewAPKVersionCode =" + GameManager.getIntance().mNewAPKVersionCode);
                string token2 = jb2["token"].ToString();
                Debug.Log("Upload complete! token =" + token2);
                isNew = bool.Parse(jb2["isNew"].ToString());
                Debug.Log("Upload complete! isNew =" + isNew);
                dealRepond(getTime, status,token2,true);
                if (status == 0)
                {
                    Debug.Log("Upload complete! www.text leng = " + www.text.Length);
                    mLocal = www.text;
                    if (isReplace && isHaveLocal()) {
                        SQLManager.getIntance().saveLocal(NetServer.getIntance().getLocal());
                        GameObject.Find("reload").GetComponent<ReloadControl>().reload(false);
                    }
                }
            }
            else {
                mLocal = null;
            }
            return true;
        }
        else
        {
            Debug.Log("Http错误代码:" + www.error);
            mLocal = null;
            return false;
        }
    }
    public void getRanking()
    {
        if (GameManager.isTestVersion)
        {
            return;
        }
        Debug.Log("NetServer  getRanking");
        Thread th1 = new Thread(getRankingList);
        th1.Start();
    }
    private String mBill;
    public void updateBill(String bill) {
        mBill = bill;
        Thread th1 = new Thread(billDeal);
        th1.Start();

    }

    private void billDeal()
    {
        if (string.IsNullOrEmpty(mBill))
        {
            return;
        }
        JsonData json = new JsonData();
        json["extendsInfo"] = mBill;
        mBill = null;
        Dictionary<string, string> dir = new Dictionary<string, string>();
        dir.Add("Content-Type", "application/json");
        byte[] pData = System.Text.Encoding.UTF8.GetBytes(json.ToJson().ToCharArray());
        WWW www = new WWW(URL_ROOT + "/phonebtgame", pData, dir);
        while (!www.isDone)
        {
            Thread.Sleep(100);
        }
        if (www.error == null)
        {
            Debug.Log("Upload complete! www.text =" + www.text);
        }
    }


    public void getUpdateInfoRun() {
        if (GameManager.isTestVersion)
        {
            return;
        }
        GameManager.getIntance().isHaveNoteUpdate = true;
        Debug.Log("NetServer  getRanking");
        Thread th1 = new Thread(getUpdateInfo);
        th1.Start();
    }

    private void getUpdateInfo() {
        JsonData json = getJsonRootData();

        JsonData array = new JsonData();
        JsonData jb = new JsonData();

        jb["action"] = 7;
        jb["type"] = -1;
        jb["id"] = -1;
        jb["goodId"] = -1;
        jb["goodtype"] = -1;
        jb["isclean"] = -1;
        jb["extra"] = "-1";

        array.Add(jb);
        json["date"] = array;
        Debug.Log("NetServer  updateNet json = " + json.ToJson());
        Dictionary<string, string> dir = new Dictionary<string, string>();
        dir.Add("Content-Type", "application/json");
        //   dir.Add("Connection", "close");
        byte[] pData = System.Text.Encoding.UTF8.GetBytes(json.ToJson().ToCharArray());

        WWW www = new WWW(URL_ROOT + "/ourgame", pData, dir);
        while (!www.isDone)
        {
            Thread.Sleep(100);
        }
        if (www.error == null)
        {
            Debug.Log("Upload complete! www.text =" + www.text);

            if (www.text != null && www.text.Length > 0)
            {
                JsonData jb2 = JsonMapper.ToObject(www.text);
                int status = int.Parse(jb2["status"].ToString());
                long getTime = long.Parse(jb2["time"].ToString());
                GameManager.getIntance().mNewAPKVersionCode = long.Parse(jb2["version"].ToString());
                string token = jb2["token"].ToString();
                dealRepond(getTime, status, token, false);
                GameManager.getIntance().mIsMust = long.Parse(jb2["ismust"].ToString());
                GameManager.getIntance().mUpdateStr = jb2["date"].ToString();
            }
        }
        else
        {
            Debug.Log("Http错误代码:" + www.error);
            GameManager.getIntance().isHaveNoteUpdate = false;
        }
    }

    private float mClearTimeScale = 1;
    public void clearAllLocal() {
        mClearTimeScale = Time.timeScale;
        Time.timeScale = 0;

        JsonData json = getJsonRootData();

        JsonData array = new JsonData();
        JsonData jb = new JsonData();

        jb["action"] = 4;
        jb["type"] = -1;
        jb["id"] = -1;
        jb["goodId"] = -1;
        jb["goodtype"] = -1;
        jb["isclean"] = -1;
        jb["extra"] = "-1";

        array.Add(jb);
        json["date"] = array;
        Debug.Log("NetServer  updateNet json = " + json.ToJson());
        Dictionary<string, string> dir = new Dictionary<string, string>();
        dir.Add("Content-Type", "application/json");
        //   dir.Add("Connection", "close");
        byte[] pData = System.Text.Encoding.UTF8.GetBytes(json.ToJson().ToCharArray());

        WWW www = new WWW(URL_ROOT + "/ourgame", pData, dir);
        while (!www.isDone)
        {
            Thread.Sleep(100);
        }
        if (www.error == null)
        {
            Debug.Log("Upload complete! www.text =" + www.text);

            if (www.text != null && www.text.Length > 0)
            {
                JsonData jb2 = JsonMapper.ToObject(www.text);
                int status = int.Parse(jb2["status"].ToString());
                long getTime = long.Parse(jb2["time"].ToString());
                GameManager.getIntance().mNewAPKVersionCode = long.Parse(jb2["version"].ToString());
                string token = jb2["token"].ToString();

                dealRepond(getTime, status, token, false);
                if (status == 0)
                {
                    
                    SQLManager.getIntance().saveLocal("");
                    GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showUi("已经为你清空全部数据，将退出游戏重新开始", LuiHuiTips.TYPPE_ERROR_DATE);
                    GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showSelf();
                    Time.timeScale = 0;
                    return;
                }
            }
            
        }
        else
        {
            Debug.Log("Http错误代码:" + www.error);
        }
        Time.timeScale = mClearTimeScale;
        
    }


    private void getRankingList()
    {
        JsonData json = getJsonRootData();

        JsonData array = new JsonData();
        JsonData jb = new JsonData();

        jb["action"] = 6;
        jb["type"] = -1;
        jb["id"] = -1;
        jb["goodId"] = -1;
        jb["goodtype"] = -1;
        jb["isclean"] = -1;
        jb["extra"] = "-1";

        array.Add(jb);
        json["date"] = array;
        Debug.Log("NetServer  updateNet json = " + json.ToJson());
        Dictionary<string, string> dir = new Dictionary<string, string>();
        dir.Add("Content-Type", "application/json");
        //   dir.Add("Connection", "close");
       
        byte[] pData = System.Text.Encoding.UTF8.GetBytes(json.ToJson().ToCharArray());

        WWW www = new WWW(URL_ROOT+"/ourgame", pData, dir);
        while (!www.isDone)
        {
            Thread.Sleep(100);
        }
        if (www.error == null)
        {
            Debug.Log("Upload complete! www.text =" + www.text);

            if (www.text != null && www.text.Length > 0)
            {

                JsonData jb2 = JsonMapper.ToObject(www.text);
                int status = int.Parse(jb2["status"].ToString());
                long getTime = long.Parse(jb2["time"].ToString());
                GameManager.getIntance().mNewAPKVersionCode = long.Parse(jb2["version"].ToString());
                string token = jb2["token"].ToString();

                dealRepond(getTime, status, token,false);
                if (status == 0)
                {
                    string date = jb2["date"].ToJson();
                    Debug.Log(" Newtonsoft.Json.Linq.JArray.Parse(str);");
                    if (GameManager.getIntance().mRankingList != null) {
                        GameManager.getIntance().mRankingList.Clear();
                    }

                    GameManager.getIntance().mRankingList = JsonMapper.ToObject<List<RankingListDateBean>>(date);
                    GameManager.getIntance().mRankingListUpdate = true;
                }

            }
        }
        else
        {
            Debug.Log("Http错误代码:" + www.error);
        }
    }

    public bool isHaveLocal() {
        if (GameManager.isTestVersion)
        {
            return false;
        }
        if (mLocal != null && !mLocal.Equals("error") && mLocal.Length > 0) {
            try
            {
                JsonData jb2 = JsonMapper.ToObject(mLocal);
                string date = jb2["date"].ToJson();

                Debug.Log(" Newtonsoft.Json.Linq.JArray.Parse(str);");
                List<SQLDate> list = JsonMapper.ToObject<List<SQLDate>>(date);
                if (list != null && list.Count > 0) {
                    return true;
                }
            }
            catch (Exception e) {
                Debug.Log("isHaveLocal 出错");
                Debug.Log(e.Message);

            }         
        }
        return false;
    }

    private void dealRepond(long time,int statue,string token,bool isChangeToken) {
        Debug.Log("dealRepond token= " + token + " SQLHelper.getIntance().mToken=" + SQLHelper.getIntance().mToken + " isChangeToken="+ isChangeToken);

        if (statue == ERROR_DOED_TOKEN_FAULT)
        {
            GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showUi("该数据编码已经在别的机器登陆", LuiHuiTips.TYPPE_ERROR_TOKEN);
            GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showSelf();
            return;
        }
        else if (statue == ERROR_DOED_GET_LOCAL_FAULT)
        {
            GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showUi("输入编码不存在", LuiHuiTips.TYPPE_EMPTY_TOKEN);
            GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showSelf();
            return;
        }
        else if (statue == ERROR_EXIT_FAULT) {
            Application.Quit();
        }
        if (isChangeToken && !string.IsNullOrEmpty(token)) {
            
            SQLHelper.getIntance().updateToken(token);
        }
        else if (!string.IsNullOrEmpty(token) && string.IsNullOrEmpty(SQLHelper.getIntance().mToken)) {
            SQLHelper.getIntance().updateToken(token);
        }
        if (time > mTime) {
            mTime = time;
            if (SQLHelper.getIntance().mMaxOutTime != -1) {
                if (SQLHelper.getIntance().mMaxOutTime > mTime + GameManager.ERROR_TIME_MIN)
                {
                    GameManager.getIntance().isError = true;
                }
            }

        }
    }

    public string getLocal()
    {
        return mLocal;
    }
}