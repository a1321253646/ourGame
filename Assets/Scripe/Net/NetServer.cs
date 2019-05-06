using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json.Linq;
using System;

public class NetServer 
{
    public static int ERROR_DOED_TOKEN_FAULT = -1001;
    public static int ERROR_DOED_GET_LOCAL_FAULT = -1002;

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

    private void threadRun() {
        JObject json = new JObject();
        json.Add("user", mDeviceID);
        if (!string.IsNullOrEmpty(SQLHelper.getIntance().mToken))
        {
            json.Add("token", SQLHelper.getIntance().mToken);
        }
        if (mList != null && mList.Count > 0) {
            JArray array = new JArray();
            for (int i = 0; i< mList.Count;) {
                SqlNetDate date = mList[i];
                if (date.date.type == SQLHelper.TYPE_GAME && date.date.id == SQLHelper.GAME_ID_IS_NET) {
                    mList.Remove(date);
                    continue;
                }
                JObject jb = new JObject();
                jb.Add("action", date.action);
                jb.Add("type", date.date.type);
                jb.Add("id", date.date.id);
                jb.Add("goodId", date.date.goodId);
                jb.Add("goodtype", date.date.goodType);
                jb.Add("isclean", date.date.isClean);
                jb.Add("extra", date.date.extan);
                Debug.Log("NetServer  updateNet jb = " + jb.ToString());
                array.Add(jb);
                i++;
            }
            if (array.Count > 0) {
                json.Add("date",array);
            }
            
        };
        Debug.Log("NetServer  updateNet json = "+ json.ToString());
        Dictionary<string, string> dir = new Dictionary<string, string>();
        dir.Add("Content-Type", "application/json");
     //   dir.Add("Connection", "close");
        byte[] pData = System.Text.Encoding.UTF8.GetBytes(json.ToString().ToCharArray());

        WWW www = new WWW(URL_ROOT+"/ourgame", pData, dir);
        while (!www.isDone) {
            Thread.Sleep(100);
        }

        if (www.error == null) 
        {
            Debug.Log("Upload complete! "+ www.text);
            if (www.text != null && www.text.Length > 0)
            {
                JObject jb = JObject.Parse(www.text);
                int status = jb.Value<int>("status");
                long getTime = jb.Value<long>("time");
                GameManager.getIntance().mNewAPKVersionCode = jb.Value<long>("version");
                string token = jb.Value<string>("token");
                dealRepond(getTime, status, token,false);
                if (status == 0) {
                    SQLNetManager.getIntance().updateDate(true);
                }               
                mList = null;             
            }
            isUpdate = false;
        }
        else
        {
            isUpdate = false;
            SQLNetManager.getIntance().updateDate(false);
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
        JObject json = new JObject();
        if (skipMac)
        {
            json.Add("user", "skip_" + mDeviceID);
        }
        else {
            json.Add("user", mDeviceID);
        }
        
        if (!string.IsNullOrEmpty(SQLHelper.getIntance().mToken))
        {
            json.Add("token", SQLHelper.getIntance().mToken);
        }
        //json.Add("user", "7a3cff28cdeddeb1220b926073d818d8");
        mLocal = null;
        JArray array = new JArray();
        JObject jb = new JObject();
        jb.Add("action", 5);
        jb.Add("type", -1);
        jb.Add("id", -1);
        jb.Add("goodId", -1);
        jb.Add("goodtype", -1);
        jb.Add("isclean", -1);
        if (!string.IsNullOrEmpty(token))
        {
            jb.Add("extra", token);
        }
        
        array.Add(jb);
        json.Add("date", array);
        Dictionary<string, string> dir = new Dictionary<string, string>();
        dir.Add("Content-Type", "application/json");
        //   dir.Add("Connection", "close");
        byte[] pData = System.Text.Encoding.UTF8.GetBytes(json.ToString().ToCharArray());

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
                JObject jb2 = JObject.Parse(www.text);
                int status = jb2.Value<int>("status");
                long getTime = jb2.Value<long>("time");
                GameManager.getIntance().mNewAPKVersionCode = jb2.Value<long>("version");
                string token2 = jb2.Value<string>("token");
                isNew = jb2.Value<bool>("isnew");
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
        JObject json = new JObject();
        json.Add("user", mDeviceID);
        if (!string.IsNullOrEmpty(SQLHelper.getIntance().mToken))
        {
            json.Add("token", SQLHelper.getIntance().mToken);
        }
        JArray array = new JArray();
        JObject jb = new JObject();
        jb.Add("action", 7);
        jb.Add("type", -1);
        jb.Add("id", -1);
        jb.Add("goodId", -1);
        jb.Add("goodtype", -1);
        jb.Add("isclean", -1);
        jb.Add("extra", "-1");
        array.Add(jb);
        json.Add("date", array);
        Dictionary<string, string> dir = new Dictionary<string, string>();
        dir.Add("Content-Type", "application/json");
        //   dir.Add("Connection", "close");
        byte[] pData = System.Text.Encoding.UTF8.GetBytes(json.ToString().ToCharArray());

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
                JObject jb2 = JObject.Parse(www.text);
                int status = jb2.Value<int>("status");
                long getTime = jb2.Value<long>("time");
                GameManager.getIntance().mNewAPKVersionCode = jb2.Value<long>("version");
                GameManager.getIntance().mIsMust = jb2.Value<long>("ismust");
                GameManager.getIntance().mUpdateStr = jb2.Value<string>("date");
            }
        }
        else
        {
            Debug.Log("Http错误代码:" + www.error);
            GameManager.getIntance().isHaveNoteUpdate = false;
        }
    }



    private void getRankingList()
    {
        JObject json = new JObject();
        json.Add("user", mDeviceID);
        if (!string.IsNullOrEmpty(SQLHelper.getIntance().mToken))
        {
            json.Add("token", SQLHelper.getIntance().mToken);
        }
        JArray array = new JArray();
        JObject jb = new JObject();
        jb.Add("action", 6);
        jb.Add("type", -1);
        jb.Add("id", -1);
        jb.Add("goodId", -1);
        jb.Add("goodtype", -1);
        jb.Add("isclean", -1);
        jb.Add("extra", "-1");
        array.Add(jb);
        json.Add("date", array);
        Dictionary<string, string> dir = new Dictionary<string, string>();
        dir.Add("Content-Type", "application/json");
        //   dir.Add("Connection", "close");
        byte[] pData = System.Text.Encoding.UTF8.GetBytes(json.ToString().ToCharArray());

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
                JObject jb2 = JObject.Parse(www.text);
                int status = jb2.Value<int>("status");
                long getTime = jb2.Value<long>("time");
                string token = jb2.Value<string>("token");
                GameManager.getIntance().mNewAPKVersionCode = jb2.Value<long>("version");
                dealRepond(getTime, status, token,false);
                if (status == 0)
                {
                    var arrdata = jb2.Value<JArray>("date");
                    Debug.Log(" Newtonsoft.Json.Linq.JArray.Parse(str);");
                    GameManager.getIntance().mRankingList = arrdata.ToObject<List<RankingListDateBean>>();
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
                JObject jb2 = JObject.Parse(mLocal);
                var arrdata = jb2.Value<JArray>("date");
                Debug.Log(" Newtonsoft.Json.Linq.JArray.Parse(str);");
                List<SQLDate> list = arrdata.ToObject<List<SQLDate>>();
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
        else if(statue == ERROR_DOED_GET_LOCAL_FAULT) {
            GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showUi("输入编码不存在", LuiHuiTips.TYPPE_EMPTY_TOKEN);
            GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showSelf();
            return;
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

    /*    public void clearAllNet() {
        JObject json = new JObject();
        json.Add("user", SystemInfo.deviceUniqueIdentifier);
        JArray array = new JArray();
        JObject jb = new JObject();
        jb.Add("action", 4);
        jb.Add("type", -1);
        jb.Add("id", -1);
        jb.Add("goodId", -1);
        jb.Add("goodtype", -1);
        jb.Add("isclean", -1);
        jb.Add("extra", "-1");
        array.Add(jb);
        json.Add("date", array);
        Dictionary<string, string> dir = new Dictionary<string, string>();
        dir.Add("Content-Type", "application/json");
        //   dir.Add("Connection", "close");
        byte[] pData = System.Text.Encoding.ASCII.GetBytes(json.ToString().ToCharArray());

        WWW www = new WWW(URL_ROOT+"/ourgame", pData, dir);
        while (!www.isDone)
        {
            Thread.Sleep(100);
        }
        if (www.error == null && www.text != null && www.text.Equals("ok"))
        {
            Debug.Log("Upload complete!");
        }
        else
        {
            Debug.Log("Http错误代码:" + www.error);
            SqlNetDate date = new SqlNetDate();
            date.action = 6;
            SQLNetManager.getIntance().addList(date);
        }
    }*/


}