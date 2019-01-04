using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json.Linq;
using System;

public class NetServer 
{
    public bool isUpdate = false;
    List<SqlNetDate> mList = null;
    public long mTime = -1;

    public void updateNet(List<SqlNetDate> list) {
        Debug.Log("NetServer  updateNet");
        isUpdate = true;
        mList = list;
        Thread th1 = new Thread(threadRun);
        th1.Start();
    }

    private void threadRun() {
        JObject json = new JObject();
        json.Add("user", SystemInfo.deviceUniqueIdentifier);
        if (mList != null && mList.Count > 0) {
            JArray array = new JArray();
            for (int i = 0; i< mList.Count;i++) {
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
                array.Add(jb);
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

        WWW www = new WWW("http://120.79.249.55:8809/ourgame", pData, dir);
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
                setGetTime(getTime);
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

    }
    private static NetServer mIntance = new NetServer();
    public static NetServer  getIntance()
    {
        return mIntance;
    }
    public void getLocl()
    {
        JObject json = new JObject();
        json.Add("user", SystemInfo.deviceUniqueIdentifier);
        JArray array = new JArray();
        JObject jb = new JObject();
        jb.Add("action", 5);
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

        WWW www = new WWW("http://120.79.249.55:8809/ourgame", pData, dir);
        while (!www.isDone)
        {
            Thread.Sleep(100);
        }
        if (www.error == null )
        {
            Debug.Log("Upload complete! www.text ="+ www.text);
           
            if (www.text != null && www.text.Length > 0)
            {
                JObject jb2 = JObject.Parse(www.text);
                int status = jb2.Value<int>("status");
                long getTime = jb2.Value<long>("time");
                setGetTime(getTime);
                if (status == 0)
                {
                    Debug.Log("Upload complete! www.text leng = " + www.text.Length);
                    mLocal = www.text;
                }

            }
            else {
                mLocal = null;
            }
        }
        else
        {
            Debug.Log("Http错误代码:" + www.error);
            mLocal = null;
        }
    }
    public void getRanking()
    {
        Debug.Log("NetServer  getRanking");
        Thread th1 = new Thread(getRankingList);
        th1.Start();
    }

    private void getRankingList()
    {
        JObject json = new JObject();
        json.Add("user", SystemInfo.deviceUniqueIdentifier);
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

        WWW www = new WWW("http://120.79.249.55:8809/ourgame", pData, dir);
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
                setGetTime(getTime);
                if (status == 0)
                {
                    var arrdata = jb2.Value<JArray>("date");
                    Debug.Log(" Newtonsoft.Json.Linq.JArray.Parse(str);");
                    GameManager.getIntance().mRankingList = arrdata.ToObject<List<RankingListDateBean>>();
                    GameManager.getIntance().mRankingListUpdate = true;
                }

            }
            else
            {
                mLocal = null;
            }
        }
        else
        {
            Debug.Log("Http错误代码:" + www.error);
            mLocal = null;
        }
    }

    public bool isHaveLocal() {
        if (mLocal != null && mLocal.Length > 0) {
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
               
            }         
        }
        return false;
    }

    private void setGetTime(long time) {
        if (time > mTime) {
            mTime = time;
            if (SQLHelper.getIntance().mMaxOutTime != -1) {
                if (SQLHelper.getIntance().mMaxOutTime > mTime + 360000 || SQLHelper.getIntance().mMaxOutTime < mTime - 360000)
                {
 //                   GameManager.getIntance().isError = true;
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

        WWW www = new WWW("http://120.79.249.55:8809/ourgame", pData, dir);
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