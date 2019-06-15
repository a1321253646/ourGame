using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class SqlControlToNative : MonoBehaviour
{
    public void createTable(string sqlName, string tableName)
    {
        Debug.Log(" mysql createTable");
        string[] param = new string[2];
        param[0] = sqlName;
        param[1] = tableName;
        callNativegetVoid("createTable", param);
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
        Debug.Log(" mysql onUodateInfoByTypeAndId");
        string str = sqldateToJsonStr(date);
        string[] param = new string[1];
        param[0] = str;
        callNativegetVoid("onUodateInfoByTypeAndId", param);
    }

    public void alterTableForIsNetAndIsDelete()
    {
        Debug.Log(" mysql alterTableForIsNetAndIsDelete");
        callNativegetVoid("alterTableForIsNetAndIsDelete", null);
    }

    public bool isUpdate()
    {
        Debug.Log(" mysql isUpdate");
        return callNativegetBool("isUpdate", null);
    }

    public void clearAllDelete()
    {
        Debug.Log(" mysql clearAllDelete");
        callNativegetVoid("clearAllDelete", null);
    }

    public void deleteGuide(string id)
    {
        Debug.Log(" mysql deleteGuide");
        string[] param = new string[1];
        param[0] = id;
        callNativegetVoid("deleteGuide", param);
    }

    public void inSertDate(SQLDate date)
    {
        Debug.Log(" mysql inSertDate");
        string str = sqldateToJsonStr(date);
        string[] param = new string[1];
        param[0] = str;
        callNativegetVoid("inSertDate", param);
    }

    public void changeGoodType(SQLDate date)
    {
        Debug.Log(" mysql changeGoodType");
        string str = sqldateToJsonStr(date);
        string[] param = new string[1];
        param[0] = str;
        callNativegetVoid("changeGoodType", param);
    }

    public void changeGoodSql(SQLDate date, string oldstr)
    {
        Debug.Log(" mysql changeGoodSql");
        string str = sqldateToJsonStr(date);
        string[] param = new string[2];
        param[0] = str;
        param[1] = oldstr;
        callNativegetVoid("changeGoodSql", param);
    }

    public void updateIdAndType(SQLDate date)
    {
        Debug.Log(" mysql updateIdAndType");
        string str = sqldateToJsonStr(date);
        string[] param = new string[1];
        param[0] = str;
        callNativegetVoid("updateIdAndType", param);
    }
    public void deleteIdAndType(SQLDate date)
    {
        Debug.Log(" mysql deleteIdAndType");
        string str = sqldateToJsonStr(date);
        string[] param = new string[1];
        param[0] = str;
        callNativegetVoid("deleteIdAndType", param);
    }

    public void deleteGood(SQLDate date)
    {
        Debug.Log(" mysql deleteGood");
        string str = sqldateToJsonStr(date);
        string[] param = new string[1];
        param[0] = str;
        callNativegetVoid("deleteGood", param);

    }
    public void deleteLuiHui()
    {
        Debug.Log(" mysql deleteLuiHui");
        callNativegetVoid("deleteLuiHui", null);
    }

    public void UpdateZhuangbeiInto(SQLDate date)
    {
        Debug.Log(" mysql UpdateZhuangbeiInto");
        string str = sqldateToJsonStr(date);
        string[] param = new string[1];
        param[0] = str;
        callNativegetVoid("UpdateZhuangbeiInto", param);
    }

    public void updateEndNet(List<SQLDate> date)
    {
        Debug.Log(" mysql updateEndNet");
        string str = sqldateListToJsonStr(date);
        string[] param = new string[1];
        param[0] = str;
        callNativegetVoid("updateEndNet", param);
    }

    public void deleteCleanNet()
    {
        Debug.Log(" mysql deleteCleanNet");
        callNativegetVoid("deleteCleanNet", null);
    }

    public void removeDeleteDate()
    {
        Debug.Log(" mysql removeDeleteDate");
        callNativegetVoid("removeDeleteDate", null);
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
        Debug.Log(" mysql saveLocal");
        string str = sqldateListToJsonStr(date);
        string[] param = new string[1];
        param[0] = str;
        callNativegetVoid("saveLocal", param);
    }


    public void delectAll(string tableName)
    {
        Debug.Log(" mysql delectAll");
        string[] param = new string[1];
        param[0] = tableName;
        callNativegetVoid("delectAll", param);
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
        string back;
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        if (mObject == null)
        {
            back = jo.Call<string>(methon);
        }
        else
        {
            back = jo.Call<string>(methon, mObject);
        }
        return jsonStringToSqldate(back);
    }
    private List<SQLDate> callNativegetSqldateList(string methon, string[] mObject)
    {
        string back;
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
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
    private void callNativegetVoid(string methon, string[] mObject)
    {
        Debug.Log(" mysql callNativegetVoid");
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        Debug.Log(" mysql AndroidJavaClass");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        Debug.Log(" mysql AndroidJavaObject");
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
        Debug.Log(" mysql callNativegetLong");
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
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
        Debug.Log(" mysql callNativegetBool");
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        if (mObject == null)
        {
            return jo.Call<bool>(methon);
        }
        else
        {
            return jo.Call<bool>(methon, mObject);
        }
    }
}
