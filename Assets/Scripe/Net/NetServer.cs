using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json.Linq;

public class NetServer 
{
    public bool isUpdate = false;
    List<SqlNetDate> mList = null;
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
            foreach (SqlNetDate date in mList) {
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
        byte[] pData = System.Text.Encoding.ASCII.GetBytes(json.ToString().ToCharArray());

        WWW www = new WWW("http://192.168.0.111:8809/ourgame", pData, dir);
        while (!www.isDone) {
            Thread.Sleep(100);
        }
        if (www.error == null) 
        {
            Debug.Log("Upload complete!");
            if (www.text != null && www.text.Equals("ok"))
            {
                SQLNetManager.getIntance().updateDate(mList,true);
                mList = null;             
            }
            isUpdate = false;
        }
        else
        {
            isUpdate = false;
            SQLNetManager.getIntance().updateDate(null,false);
            mList = null;
            Debug.Log("Http错误代码:" + www.error);

        }

    }

    private NetServer() {

    }
    private static NetServer mIntance = new NetServer();
    public static NetServer  getIntance()
    {
        return mIntance;
    }
}