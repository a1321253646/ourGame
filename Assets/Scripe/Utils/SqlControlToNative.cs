using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public class SqlControlToNative 
{
    AndroidJavaObject jo;

    public void createTable(string sqlName, string tableName)
    {
        string[] param = new string[2];
        param[0] = sqlName;
        param[1] = tableName;
        callNativegetVoid("createTable", param);
    }

    public long getLevel()
    {
        return callNativegetLong("getLevel", null);
    }

    public long getPlayVocation()
    {
        return callNativegetLong("getPlayVocation", null);
    }

    public void onUodateInfoByTypeAndId(SQLDate date)
    {
        string str = sqldateToJsonStr(date);
        string[] param = new string[1];
        param[0] = str;
        callNativegetVoid("onUodateInfoByTypeAndId", param);
    }

    public void alterTableForIsNetAndIsDelete()
    {
        callNativegetVoid("alterTableForIsNetAndIsDelete", null);
    }

    public bool isUpdate()
    {
        return callNativegetBool("isUpdate", null);
    }

    public void clearAllDelete()
    {
        callNativegetVoid("clearAllDelete", null);
    }

    public void deleteGuide(string id)
    {
        string[] param = new string[1];
        param[0] = id;
        callNativegetVoid("deleteGuide", param);
    }

    public void inSertDate(SQLDate date)
    {
        string str = sqldateToJsonStr(date);
        string[] param = new string[1];
        param[0] = str;
        callNativegetVoid("inSertDate", param);
    }

    public void changeGoodType(SQLDate date)
    {
        string str = sqldateToJsonStr(date);
        string[] param = new string[1];
        param[0] = str;
        callNativegetVoid("changeGoodType", param);
    }

    public void changeGoodSql(SQLDate date, string oldstr)
    {
        string str = sqldateToJsonStr(date);
        string[] param = new string[2];
        param[0] = str;
        param[1] = oldstr;
        callNativegetVoid("changeGoodSql", param);
    }

    public void updateIdAndType(SQLDate date)
    {
        string str = sqldateToJsonStr(date);
        string[] param = new string[1];
        param[0] = str;
        callNativegetVoid("updateIdAndType", param);
    }
    public void deleteIdAndType(SQLDate date)
    {
        string str = sqldateToJsonStr(date);
        string[] param = new string[1];
        param[0] = str;
        callNativegetVoid("deleteIdAndType", param);
    }

    public void deleteGood(SQLDate date)
    {
        string str = sqldateToJsonStr(date);
        string[] param = new string[1];
        param[0] = str;
        callNativegetVoid("deleteGood", param);

    }
    public void deleteLuiHui()
    {
        callNativegetVoid("deleteGood", null);
    }

    public void UpdateZhuangbeiInto(SQLDate date)
    {
        string str = sqldateToJsonStr(date);
        string[] param = new string[1];
        param[0] = str;
        callNativegetVoid("UpdateZhuangbeiInto", param);
    }

    public void updateEndNet(List<SQLDate> date)
    {
        string str = sqldateListToJsonStr(date);
        string[] param = new string[1];
        param[0] = str;
        callNativegetVoid("updateEndNet", param);
    }

    public void deleteCleanNet()
    {
        callNativegetVoid("deleteCleanNet", null);
    }

    public void removeDeleteDate()
    {
        callNativegetVoid("removeDeleteDate", null);
    }

    public List<SQLDate> getNetDate()
    {
        return  callNativegetSqldateList("getNetDate", null);
    }

    public List<SQLDate> getAll()
    {
        return callNativegetSqldateList("getAll", null);
    }
    public void saveLocal(List<SQLDate> date)
    {
        string str = sqldateListToJsonStr(date);
        string[] param = new string[1];
        param[0] = str;
        callNativegetVoid("saveLocal", param);
    }


    public void delectAll(string tableName)
    {
        string[] param = new string[1];
        param[0] = tableName;
        callNativegetVoid("delectAll", param);
    }


    private void callNativegetVoid(string methon, string[] mObject)
    {

        if (mObject == null)
        {
            jo.Call(methon);
        }
        else
        {
            jo.Call(methon, mObject);
        }
    }
    private long callNativegetLong(string methon, string[] mObject)
    {

        if (mObject == null)
        {
            return jo.Call<long>(methon);
        }
        else
        {
            return jo.Call<long>(methon, mObject);
        }
    }
    private bool callNativegetBool(string methon, string[] mObject)
    {

        if (mObject == null)
        {
            return jo.Call<bool>(methon);
        }
        else
        {
            return jo.Call<bool>(methon, mObject);
        }
    }

    private SQLDate jsonStringToSqldate(string str)
    {

        var arrdata = JArray.Parse(str);
        return arrdata.ToObject<SQLDate>();
    }

    private List<SQLDate> jsonStringToSqldateList(string str)
    {

        var arrdata = JArray.Parse(str);
        return arrdata.ToObject<List<SQLDate>>();
    }

    private string sqldateToJsonStr(SQLDate date)
    {
        JObject jb = new JObject();
        jb.Add("type", date.type);
        jb.Add("id", date.id);
        jb.Add("goodId", date.goodId);
        jb.Add("goodtype", date.goodType);
        jb.Add("isclean", date.isClean);
        jb.Add("extra", date.extan);
        jb.Add("isDelete", date.isDelete);
        jb.Add("isNet", date.isNet);
        return jb.ToString();
    }

    private string sqldateListToJsonStr(List<SQLDate> list)
    {
        JArray array = new JArray();

        foreach (SQLDate date in list)
        {
            JObject jb = new JObject();
            jb.Add("type", date.type);
            jb.Add("id", date.id);
            jb.Add("goodId", date.goodId);
            jb.Add("goodtype", date.goodType);
            jb.Add("isclean", date.isClean);
            jb.Add("extra", date.extan);
            jb.Add("isDelete", date.isDelete);
            jb.Add("isNet", date.isNet);
            array.Add(jb);
        }
       
        return array.ToString();
    }



    private SQLDate callNativegetSqldate(string methon ,string[]  mObject) {
        string back;

        if (mObject == null)
        {
            back = jo.Call<string>(methon);
        }
        else {
            back = jo.Call<string>(methon, mObject);
        }
        return jsonStringToSqldate(back);
    }
    private List<SQLDate> callNativegetSqldateList(string methon, string[] mObject)
    {
        string back;

        if (mObject == null)
        {
            back = jo.Call<string>(methon);
        }
        else
        {
            back = jo.Call<string>(methon, mObject);
        }
        return jsonStringToSqldateList(back);
    }

    private static SqlControlToNative mIntance = new SqlControlToNative();
    private SqlControlToNative() {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
    }
    public static SqlControlToNative getIntance() {
        return mIntance;
    }
}
