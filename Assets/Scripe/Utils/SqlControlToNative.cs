using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System;

public class SqlControlToNative : MonoBehaviour
{

    AndroidJavaObject jo  = null;

    public List<Action> mAction  = new List<Action>();

    private void Update()
    {
        Debug.Log(" mysql deal in mAction count ="+ mAction.Count);
        if (mAction.Count > 0) {
            
            Action ac = mAction[0];
            mAction.Remove(ac);
            ac();
        }
    }

    public void createTable(string sqlName, string tableName)
    {
            Debug.Log(" mysql createTable");
            string[] param = new string[2];
            param[0] = sqlName;
            param[1] = tableName;
            callNativegetVoid("createTable", param,true);
    }
    private AndroidJavaObject getAndroidJavaObject() {
        if (jo == null) {
            Debug.Log(" mysql callNativegetVoid");
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            Debug.Log(" mysql AndroidJavaClass");
            jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            Debug.Log(" mysql AndroidJavaObject");
        }
        return jo;
    }
    public long getLevel()
    {
        Debug.Log(" mysql getLevel");
        return callNativegetLong("getLevel", null);
    }

    public long getPlayVocation()
    {
        Debug.Log(" mysql getPlayVocation");
        return callNativegetLong("getPlayVocation", null);
    }

    public void onUodateInfoByTypeAndId(SQLDate date)
    {
      //  Loom.QueueOnMainThread((param1) =>
      //  {
            Debug.Log(" mysql onUodateInfoByTypeAndId");
            string str = sqldateToJsonStr(date);
            string[] param = new string[1];
            param[0] = str;
            callNativegetVoid("onUodateInfoByTypeAndId", param);
      //  }, null);

    }

    public void alterTableForIsNetAndIsDelete()
    {
       // Loom.QueueOnMainThread((param1) =>
       // {
            Debug.Log(" mysql alterTableForIsNetAndIsDelete");
            callNativegetVoid("alterTableForIsNetAndIsDelete", null,true);
       // }, null);

    }

    public bool isUpdate()
    {
        Debug.Log(" mysql isUpdate");
        return callNativegetBool("isUpdate", null);
    }

    public void clearAllDelete()
    {
       // Loom.QueueOnMainThread((param1) =>
       // {
            Debug.Log(" mysql clearAllDelete");
            callNativegetVoid("clearAllDelete", null);
       // }, null);

    }

    public void deleteGuide(string id)
    {
       // Loom.QueueOnMainThread((param1) =>
       // {
            Debug.Log(" mysql deleteGuide");
            string[] param = new string[1];
            param[0] = id;
            callNativegetVoid("deleteGuide", param);
        //}, null);
        
    }
    public void inSertDate(SQLDate date) {
        inSertDate(date, false);
    }
    public void inSertDate(SQLDate date ,bool isNow)
    {
       // Loom.QueueOnMainThread((param1) =>
       // {
            Debug.Log(" mysql inSertDate");
            string str = sqldateToJsonStr(date);
            string[] param = new string[1];
            param[0] = str;
            callNativegetVoid("inSertDate", param, isNow);
        //}, null);

    }

    public void changeGoodType(SQLDate date)
    {
     //   Loom.QueueOnMainThread((param1) =>
     //   {
            Debug.Log(" mysql changeGoodType");
            string str = sqldateToJsonStr(date);
            string[] param = new string[1];
            param[0] = str;
            callNativegetVoid("changeGoodType", param);
      //  }, null);

    }

    public void changeGoodSql(SQLDate date, string oldstr)
    {
    //    Loom.QueueOnMainThread((param1) =>
    //    {
            Debug.Log(" mysql changeGoodSql");
            string str = sqldateToJsonStr(date);
            string[] param = new string[2];
            param[0] = str;
            param[1] = oldstr;
            callNativegetVoid("changeGoodSql", param);
     //   }, null);

    }

    public void updateIdAndType(SQLDate date)
    {
       // Loom.QueueOnMainThread((param1) =>
       // {
            Debug.Log(" mysql updateIdAndType");
            string str = sqldateToJsonStr(date);
            string[] param = new string[1];
            param[0] = str;
            callNativegetVoid("updateIdAndType", param);
       // }, null);

    }
    public void deleteIdAndType(SQLDate date)
    {
      //  Loom.QueueOnMainThread((param1) =>
      //  {
            Debug.Log(" mysql deleteIdAndType");
            string str = sqldateToJsonStr(date);
            string[] param = new string[1];
            param[0] = str;
            callNativegetVoid("deleteIdAndType", param);
      //  }, null);

    }

    public void deleteGood(SQLDate date)
    {
      //  Loom.QueueOnMainThread((param1) =>
      //  {
            Debug.Log(" mysql deleteGood");
            string str = sqldateToJsonStr(date);
            string[] param = new string[1];
            param[0] = str;
            callNativegetVoid("deleteGood", param);
      //  }, null);


    }
    public void deleteLuiHui()
    {
        //Loom.QueueOnMainThread((param1) =>
        //{
            Debug.Log(" mysql deleteLuiHui");
            callNativegetVoid("deleteLuiHui", null);
        //}, null);

    }

    public void UpdateZhuangbeiInto(SQLDate date)
    {
       // Loom.QueueOnMainThread((param1) =>
       // {
            Debug.Log(" mysql UpdateZhuangbeiInto");
            string str = sqldateToJsonStr(date);
            string[] param = new string[1];
            param[0] = str;
            callNativegetVoid("UpdateZhuangbeiInto", param);
        //}, null);

    }

    public void updateEndNet(List<SQLDate> date)
    {
        //Loom.QueueOnMainThread((param1) =>
        //{
            Debug.Log(" mysql updateEndNet");
            string str = sqldateListToJsonStr(date);
            string[] param = new string[1];
            param[0] = str;
            callNativegetVoid("updateEndNet", param);
        //}, null);

    }

    public void deleteCleanNet()
    {
        //Loom.QueueOnMainThread((param1) =>
        //{
            Debug.Log(" mysql deleteCleanNet");
            callNativegetVoid("deleteCleanNet", null);
        //}, null);

    }

    public void removeDeleteDate()
    {
       // Loom.QueueOnMainThread((param1) =>
       // {
            Debug.Log(" mysql removeDeleteDate");
            callNativegetVoid("removeDeleteDate", null);
        //}, null);

    }

    public List<SQLDate> getNetDate()
    {
        Debug.Log(" mysql getNetDate");
        return callNativegetSqldateList("getNetDate", null);
    }

    public List<SQLDate> getAll()
    {
        Debug.Log(" mysql getAll");
        return callNativegetSqldateList("getAll", null);
    }
    public void saveLocal(List<SQLDate> date)
    {
       // Loom.QueueOnMainThread((param1) =>
       // {
            Debug.Log(" mysql saveLocal");
            string str = sqldateListToJsonStr(date);
            string[] param = new string[1];
            param[0] = str;
            callNativegetVoid("saveLocal", param);
        //}, null);

    }


    public void delectAll(string tableName)
    {
       // Loom.QueueOnMainThread((param1) =>
       // {
            Debug.Log(" mysql delectAll");
            string[] param = new string[1];
            param[0] = tableName;
            callNativegetVoid("delectAll", param);
       // }, null);

    }
    private SQLDate jsonStringToSqldate(string str)
    {

        return JsonMapper.ToObject<SQLDate>(str);
    }

    private List<SQLDate> jsonStringToSqldateList(string str)
    {

        return JsonMapper.ToObject<List<SQLDate>>(str);
    }

    private string sqldateToJsonStr(SQLDate date)
    {

        return JsonMapper.ToJson(date);
    }

    private string sqldateListToJsonStr(List<SQLDate> list)
    {
        return JsonMapper.ToJson(list);;
    }



    private SQLDate callNativegetSqldate(string methon, string[] mObject)
    {
        Debug.Log(" mysql callNativegetSqldateList");
        string back;
        if (mObject == null)
        {
            back = getAndroidJavaObject().Call<string>(methon);
        }
        else
        {
            back = getAndroidJavaObject().Call<string>(methon, mObject);
        }
        return jsonStringToSqldate(back);
    }
    private List<SQLDate> callNativegetSqldateList(string methon, string[] mObject)
    {
        Debug.Log(" mysql callNativegetSqldateList");
        string back;

        if (mObject == null)
        {

            back = getAndroidJavaObject().Call<string>(methon);
        }
        else
        {
            back = getAndroidJavaObject().Call<string>(methon, mObject);
        }
        return jsonStringToSqldateList(back);
    }

    private void callNativegetVoid(string methon, string[] mObject) {
        callNativegetVoid(methon, mObject, false);
    }
    private void callNativegetVoid( string methon, string[] mObject,bool isNow)
    {
        Action ac = () =>
        {

            Debug.Log(" mysql callNativegetVoid");
            if (mObject == null)
            {
                getAndroidJavaObject().Call(methon);
            }
            else
            {
                getAndroidJavaObject().Call(methon, mObject);
            }
        };
        if (isNow)
        {
            ac();
        }
        else {
            mAction.Add(ac);
            Debug.Log(" mysql mAction size = "+ mAction.Count);
        }
        

    }
    private long callNativegetLong(string methon, string[] mObject)
    {
        Debug.Log(" mysql callNativegetLong");

        if (mObject == null)
        {
            return getAndroidJavaObject().Call<long>(methon);
        }
        else
        {
            return getAndroidJavaObject().Call<long>(methon, mObject);
        }
    }
    private bool callNativegetBool(string methon, string[] mObject)
    {
        Debug.Log(" mysql callNativegetBool");
        if (mObject == null)
        {
            return getAndroidJavaObject().Call<bool>(methon);
        }
        else
        {
            return getAndroidJavaObject().Call<bool>(methon, mObject);
        }
    }
}
