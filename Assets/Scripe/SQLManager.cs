using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
#if UNITY_STANDALONE
using Mono.Data.Sqlite;
#endif

using UnityEngine;
public class SQLManager
{
#if UNITY_STANDALONE
     private SqliteDataReader reader;
#endif

    /// <summary>
    /// 本地数据库名字
    /// </summary>
    public string sqlName;
    public string tabName;

    private string sqlName_new = "local891";
    private string tabName_new = "local891";

    object mLock = new object();
#if UNITY_STANDALONE
      SqliteConnection mConnet = null;
#endif

    private static int IDCount;
    Thread th1 = null;
   // NetHelper mNetHelper = new NetHelper();

    private SQLManager() {

    }

    private static SQLManager mIntance = new SQLManager();
    public static SQLManager getIntance() {
        return mIntance;
    }

    public int init(string sqlName, string tabName) {
#if UNITY_ANDROID

        //        if (!GameManager.isAndroid)
        //        {

#endif

#if UNITY_IOS
                this.sqlName = sqlName_new;
            this.tabName = sqlName_new;
            mPathRoot = Application.persistentDataPath;
#endif


#if UNITY_STANDALONE
        //        }
        //        else {
        this.sqlName = sqlName;
        this.tabName = tabName;
        mPathRoot = Application.dataPath;
        this.CreateSQL();
        //        }
#endif

        th1 = new Thread(threadRun);
        th1.Start();
        return 0;
    }


    private string getSqlFilePath() {
        string path = "";
#if UNITY_ANDROID
        path = mPathRoot + "/" + sqlName;
#endif

#if UNITY_IOS
        path = mPathRoot + "/" + sqlName;
#endif

#if UNITY_STANDALONE
        path = mPathRoot + "/Resources/" + this.sqlName; ;
#endif
        return path;
    }
    private string mPathRoot = "";

    private string getSqlPath() {
        string path = "";
#if UNITY_ANDROID
        path = "URI=file:" + mPathRoot + "/" + sqlName;
#endif
#if UNITY_IOS
        path ="data source=" + mPathRoot + "/" + sqlName;
#endif
#if UNITY_STANDALONE
        path = "data source=" + mPathRoot + "/Resources/" + this.sqlName;
#endif
        return path;
    }
    //创建数据库文件
    public void CreateSQL()
    {
        Debug.Log("getSqlFilePath ==" + getSqlFilePath());
        if (!File.Exists(getSqlFilePath()))
        {
            Debug.Log("  数据库 文件没存在 ");
            Debug.Log("  连接数据库 ");
            this.CreateSQLTable(
                tabName,
            "CREATE TABLE " + tabName + "(" +
            "TYPE          INT ," +
            "ID            INT ," +
            "EXTAN         TEXT ," +
            "GOODID        INT ," +
            "GOODTYPE      INT ," +
            "ISCLENAN      INT ," +
            "ISNET      INT ," +
            "ISDELETE      INT )",
                null,
                null
            );
            return;
        }
    }

    public void startThread() {
        th1 = new Thread(threadRun);
        th1.Start();
    }

    public void initPathRoot() {
        sqlName = sqlName_new;
        tabName = tabName_new;
        mPathRoot = Application.persistentDataPath;
    }
    public void open()
    {
#if UNITY_ANDROID
        if (File.Exists(getSqlFilePath()))
        {
            Debug.Log(" mysql createTable");
            GameObject.Find("Manager").GetComponent<SqlControlToNative>().createTable(getSqlFilePath(), tabName);
        }
#endif
    }



    public long getPlayVocation() {
        if (File.Exists(getSqlFilePath())) {
#if UNITY_ANDROID
            return GameObject.Find("Manager").GetComponent<SqlControlToNative>().getPlayVocation();
#endif
#if UNITY_STANDALONE
            List<SQLDate> list = new List<SQLDate>();
            if (mConnet == null)
            {
                mConnet = new SqliteConnection(getSqlPath());
                mConnet.Open();
            }

            //        using (SqliteConnection cnn =)
            using (reader)
            {
                Debug.Log("readAllTable");

                SqliteCommand command = mConnet.CreateCommand();
                command.CommandText = "select * from  " + tabName + " WHERE  ID=" + SQLHelper.GAME_ID_PLAYER_VOCATION + " AND TYPE=" + SQLHelper.TYPE_GAME+" AND ISDELETE=1";
                this.reader = command.ExecuteReader();
                while (this.reader.Read())
                {
                   long vocation = long.Parse(reader.GetString(reader.GetOrdinal("EXTAN")));
                    return vocation;
                }
            }
#endif

        }
        return -1;
    }

    public long getLevel() {
        if (File.Exists(getSqlFilePath()))
        {
#if UNITY_ANDROID
            return GameObject.Find("Manager").GetComponent<SqlControlToNative>().getLevel();
#endif
#if UNITY_STANDALONE
            List<SQLDate> list = new List<SQLDate>();
            if (mConnet == null)
            {
                mConnet = new SqliteConnection(getSqlPath());
                mConnet.Open();
            }

            //        using (SqliteConnection cnn =)
            using (reader)
            {
//                Debug.Log("readAllTable");

                SqliteCommand command = mConnet.CreateCommand();
                command.CommandText = "select * from  " + tabName + " WHERE  ID=" + SQLHelper.GAME_ID_LEVEL + " AND TYPE=" + SQLHelper.TYPE_GAME + " AND ISDELETE=1";
                this.reader = command.ExecuteReader();
                long vocation = -1;
                while (this.reader.Read())
                {
                    vocation = long.Parse(reader.GetString(reader.GetOrdinal("EXTAN")));
                    
                }
                return vocation;
            }
#endif

        }
        return -1;
    }
    public int initNoNet()
    {
        Debug.Log(" mysql initNoNet " );
        sqlName = sqlName_new;
        tabName = tabName_new;
        mPathRoot = Application.persistentDataPath;

        if (!File.Exists(getSqlFilePath()))
        {
            GameManager.getIntance().mInitDec = JsonUtils.getIntance().getStringById(100026);
            creatLocl888Android();
            sqlName = sqlName_new;
            tabName = tabName_new;
            return 3;
        }
        else {
            sqlName = sqlName_new;
            tabName = tabName_new;
        }
        GameManager.getIntance().mInitDec = JsonUtils.getIntance().getStringById(100028);
        return 2;
    }

    public void alterTableByVersion()
    {
        if (File.Exists(getSqlFilePath()))
        {
#if UNITY_ANDROID
            GameObject.Find("Manager").GetComponent<SqlControlToNative>().alterTableForIsNetAndIsDelete();
            SQLDate data1 = new SQLDate();
            data1.type = SQLHelper.TYPE_ENCODE_VERSION;
            data1.id = 1;
            data1.isClean = 2;
            GameObject.Find("Manager").GetComponent<SqlControlToNative>().inSertDate(data1);
            return;
#endif
#if UNITY_STANDALONE
 List<SQLDate> list = new List<SQLDate>();
            if (mConnet == null)
            {
                mConnet = new SqliteConnection(getSqlPath());
                mConnet.Open();
            }

            //        using (SqliteConnection cnn =)
            using (reader)
            {
                Debug.Log("readAllTable");
                SqliteCommand command = mConnet.CreateCommand();
                command.CommandText = "select * from  " + tabName + " WHERE   TYPE=" + SQLHelper.TYPE_ENCODE_VERSION ;
                this.reader = command.ExecuteReader();
                while (this.reader.Read())
                {
                    GameManager.getIntance().mCurrentSqlVersion = reader.GetInt64(reader.GetOrdinal("ID"));
                    Debug.Log("GameManager.getIntance().mCurrentSqlVersion ==" + GameManager.getIntance().mCurrentSqlVersion);
                    Debug.Log("this.reader.Read()");
                    break;
                }
                Debug.Log("end GameManager.getIntance().mCurrentSqlVersion =="+ GameManager.getIntance().mCurrentSqlVersion);
                if (GameManager.getIntance().mCurrentSqlVersion < 1) {
                    GameManager.getIntance().mCurrentSqlVersion = 1;

                    SqliteCommand command1 = mConnet.CreateCommand();
                    command1.CommandText = "alter table " + tabName + " add ISNET int default 1 ";
                    reader = command1.ExecuteReader();
                    reader.Close();

                    SqliteCommand command2 = mConnet.CreateCommand();
                    command2.CommandText = "alter table " + tabName + " add ISDELETE int default 1";
                    reader = command2.ExecuteReader();
                    reader.Close();

                    SqliteCommand command3 = mConnet.CreateCommand();
                    string commandString = "INSERT INTO " + tabName + " VALUES (";
                    SQLDate data = new SQLDate();
                    data.type = SQLHelper.TYPE_ENCODE_VERSION;
                    data.id = 1;
                    data.isClean = 2;
                    commandString += data.type;
                    commandString += "," + data.id;
                    commandString += "," + "'" + data.extan + "'";
                    commandString += "," + data.goodId;
                    commandString += "," + data.goodType;
                    commandString += "," + data.isClean;
                    commandString += "," + data.isNet;
                    commandString += "," + data.isDelete + ")";
                    command3.CommandText = commandString;
                    reader = command3.ExecuteReader();
                    reader.Close();

                }

            }
#endif


        }
    }

    public bool isUpdateed() {
#if UNITY_ANDROID
        return GameObject.Find("Manager").GetComponent<SqlControlToNative>().isUpdate();
#endif
#if UNITY_STANDALONE
        bool back = true;
        string comm = "select * from  " + tabName + " WHERE  ID=" + SQLHelper.GAME_ID_IS_UPDATE + " AND TYPE=" + SQLHelper.TYPE_GAME + " AND ISDELETE=1";
        if (mConnet == null)
        {
            mConnet = new SqliteConnection(getSqlPath());
            mConnet.Open();
        }
        using (reader)
        {
            Debug.Log("ExecuteSQLCommand queryString=" + comm);

            SqliteCommand command = mConnet.CreateCommand();
            command.CommandText = comm;
            reader = command.ExecuteReader();
            int count = 0;
            while (this.reader.Read()) {
                count++;
            }
            if (count > 0) {
                back = false;
            }
            reader.Close();
            //           cnn.Close();            
        }
        return back;
#endif

    }

    private void creatLocl888Android() {
        Debug.Log(" mysql creatLocl888Android ");
        sqlName = sqlName_new;
        tabName = tabName_new;
#if UNITY_ANDROID
        Debug.Log(" mysql creatLocl888Android begin getSqlFilePath()="+ getSqlFilePath()+ " tabName="+ tabName);
        GameObject.Find("Manager").GetComponent<SqlControlToNative>().createTable(getSqlFilePath(), tabName);
        Debug.Log(" mysql creatLocl888Android end");
        return;
#endif
        Debug.Log("  连接数据库 ");
        this.CreateSQLTable(
            tabName,
            "CREATE TABLE " + tabName + "(" +
            "TYPE          INT ," +
            "ID            INT ," +
            "EXTAN         TEXT ,"+
            "GOODID        INT ," +
            "GOODTYPE      INT ," +
            "ISCLENAN      INT ," +
            "ISNET      INT ," +
            "ISDELETE      INT )",
            null,
            null
        );
    }


    /// <summary>
    ///执行SQL命令,并返回一个SqliteDataReader对象
    /// <param name="queryString"></param>
    public void ExecuteSQLCommand(string queryString)
    {
#if UNITY_STANDALONE
        if (mConnet == null) {
            mConnet = new SqliteConnection(getSqlPath());
            mConnet.Open();
        }
        using (reader) {
            Debug.Log("ExecuteSQLCommand queryString="+ queryString);
            
            SqliteCommand command = mConnet.CreateCommand();
            command.CommandText = queryString;
            reader = command.ExecuteReader();
            reader.Close();           
        }
#endif
    }

    /// <summary>
    /// 通过调用SQL语句，在数据库中创建一个表，顶定义表中的行的名字和对应的数据类型
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="columnNames"></param>
    /// <param name="dataTypes"></param>
    public void CreateSQLTable(string tableName, string commandStr = null, string[] columnNames = null, string[] dataTypes = null)
    {

        ExecuteSQLCommand(commandStr);
        string commandString = "INSERT INTO " + tabName + " VALUES (";
        SQLDate data = new SQLDate();
        data.type = SQLHelper.TYPE_ENCODE_VERSION;
        data.id = 1;
        data.isClean = 2;
        commandString += data.type;
        commandString += "," + data.id;
        commandString += "," + "'" + data.extan + "'";
        commandString += "," + data.goodId;
        commandString += "," + data.goodType;
        commandString += "," + data.isClean;
        commandString += "," + data.isNet;
        commandString += "," + data.isDelete + ")";
        ExecuteSQLCommand(commandString);
    }
    /// <summary>
    /// 关闭数据库连接,注意这一步非常重要，最好每次测试结束的时候都调用关闭数据库连接
    /// 如果不执行这一步，多次调用之后，会报错，数据库被锁定，每次打开都非常缓慢
    /// </summary>
    public void CloseSQLConnection()
    {
#if UNITY_STANDALONE
        Debug.Log("已经断开数据库连接");
        if (mConnet != null)
        {
            mConnet.Close();
            mConnet.Dispose();
            mConnet = null;
        }
#endif

    }

    public void deleteGuide(long id) {
#if UNITY_ANDROID
        string commandString = "UPDATE  " + tabName + " SET ISDELETE=2,  ISNET=1 WHERE TYPE=" + SQLHelper.TYPE_GUIDE + " AND ID=" + id + " AND ISDELETE=1";
#endif
#if UNITY_STANDALONE
        string commandString = "DELETE FROM " + tabName + " WHERE TYPE=" + SQLHelper.TYPE_GUIDE + " AND ID=" + id + " AND ISDELETE=1";
#endif


        // ExecuteSQLCommand(commandString);
        SqlWaitListAddBean bean = new SqlWaitListAddBean();
        bean.action = 3;
        bean.methonId = MethonName.deleteGuide;
        bean.command = commandString;
        addList(bean);
    }

    /// <summary>
    /// 向数据库中添加数据文件
    /// </summary>
    /// 

    public void clearAllDelete() {
#if UNITY_ANDROID
        GameObject.Find("Manager").GetComponent<SqlControlToNative>().clearAllDelete();
        return;
#endif
        string commandString2 = "DELETE FROM " + tabName + " WHERE ISDELETE=2";
        ExecuteSQLCommand(commandString2);
    }

    public void InsertDataToSQL(SQLDate date) {
        InsertDataToSQL(date, false,true);
    }
    public void InsertDataToSQL(SQLDate data, bool isNow) {
        InsertDataToSQL(data, isNow, true);
    }

    public void InsertDataToSQL(SQLDate data, bool isNow,bool isUpToNet)
    {
        if (!isUpToNet) {
            data.isNet = 2;
        }
        string commandString = "INSERT INTO " + tabName + " VALUES (";

        commandString +=   data.type;
        commandString += "," + data.id;
        commandString += "," + "'" + data.extan + "'";
        commandString += "," + data.goodId;
        commandString += "," + data.goodType;
        commandString += "," + data.isClean;
        commandString += "," + data.isNet;
        commandString += "," + data.isDelete + ")";

        SqlWaitListAddBean bean = new SqlWaitListAddBean();
        bean.action = 1;
        bean.date = data;
        bean.methonId = MethonName.inSertDate;
        bean.command = commandString;
        if (isNow)
        {
#if UNITY_ANDROID
            GameObject.Find("Manager").GetComponent<SqlControlToNative>().inSertDate(data);
            return;
#endif
            ExecuteSQLCommand(commandString);            
        }
        else {
            addList(bean);
        }      
        //     
    }
    public void changeGoodType(SQLDate date)
    {
        string commPath = "UPDATE " + tabName + " SET ISNET=1, GOODTYPE=" + date.goodType ;
        commPath += " WHERE GOODID=" + date.goodId + " AND ISDELETE=1";
        SqlWaitListAddBean bean = new SqlWaitListAddBean();
        bean.action = 4;
        bean.date = date;
        bean.command = commPath;
        bean.methonId = MethonName.changeGoodType;
        addList(bean);
     //   ExecuteSQLCommand(commPath);
     //   mNetHelper.changeInto(date);
    }
    public void changeGoodSql(SQLDate date,long old)
    {
        string commPath = "UPDATE " + tabName + " SET ISNET=1, GOODID=" + date.goodId;
        commPath += " WHERE GOODID=" + old + " AND TYPE="+date.type+ " AND ID="+ date.id + " AND ISDELETE=1";
        SqlWaitListAddBean bean = new SqlWaitListAddBean();
        bean.action = 4;
        bean.date = date;
        bean.command = commPath;
        bean.old = old;
        bean.methonId = MethonName.changeGoodSql;
        addList(bean);
        //   ExecuteSQLCommand(commPath);
      //  mNetHelper.changeInto(date);
    }
    public void updateIdAndType(SQLDate date)
    {
        string commPath = "UPDATE " + tabName + " SET ISNET=1, EXTAN='" + date.extan + "'";
        commPath = commPath+ " WHERE TYPE=" + date.type + " AND ID=" + date.id + " AND ISDELETE=1";
        // ExecuteSQLCommand(commandString);
        SqlWaitListAddBean bean = new SqlWaitListAddBean();
        bean.action = 2;
        bean.date = date;
        bean.command = commPath;
        bean.methonId = MethonName.updateIdAndType;
        addList(bean);
        //   ExecuteSQLCommand(commandString);
       // mNetHelper.changeInto(date);
    }

    public void deleteIdAndType(SQLDate date)
    {
#if UNITY_ANDROID || UNITY_IOS
        string commandString = "UPDATE  " + tabName + " SET ISNET=1, ISDELETE=2 WHERE TYPE=" +  date.type + " AND ID=" +  date.id + " AND ISDELETE=1";
#endif
#if UNITY_STANDALONE
        string commandString = "DELETE FROM " + tabName + " WHERE TYPE=" + date.type + " AND ID=" + date.id + " AND ISDELETE=1";
#endif
        SqlWaitListAddBean bean = new SqlWaitListAddBean();
        bean.action = 3;
        bean.date = date;
        bean.command = commandString;
        // ExecuteSQLCommand(commandString);
        removeListByTypeAndId(date.id, date.type);
        bean.methonId = MethonName.deleteIdAndType;
        addList(bean);
        //   ExecuteSQLCommand(commandString);
     //   mNetHelper.delectInfo(date);
    }

    public void deleteGood(SQLDate date)
    {
#if UNITY_ANDROID || UNITY_IOS
        string commandString = "UPDATE  " + tabName + " SET ISNET=1, ISDELETE=2 WHERE GOODID=" +  date.goodId + " AND ISDELETE=1";
#endif
#if UNITY_STANDALONE
        string commandString = "DELETE FROM " + tabName + " WHERE GOODID =" + date.goodId + " AND ISDELETE=1";
#endif
        // ExecuteSQLCommand(commandString);
        SqlWaitListAddBean bean = new SqlWaitListAddBean();
        bean.action = 3;
        bean.date = date;
        bean.command = commandString;
        removeListByGoodId(date.goodId);
        bean.methonId = MethonName.deleteGood;
        addList(bean);
     //   ExecuteSQLCommand(commandString);
     //   mNetHelper.delectInfo(date);
    }
    public void deleteLuiHui()
    {
#if UNITY_ANDROID || UNITY_IOS
        string commandString = "UPDATE  " + tabName + " SET ISNET=1, ISDELETE=2 WHERE ISCLENAN =1"+ " AND ISDELETE=1";
#endif
#if UNITY_STANDALONE
        string commandString = "DELETE FROM " + tabName + " WHERE ISCLENAN =1" + " AND ISDELETE=1";
#endif


        // ExecuteSQLCommand(commandString);
        SqlWaitListAddBean bean = new SqlWaitListAddBean();
        bean.action = 3;
        bean.date = null;
        bean.command = commandString;
        bean.methonId = MethonName.deleteLuiHui;
        addList(bean);
     //   ExecuteSQLCommand(commandString);
      //  mNetHelper.cleanLuihui();
    }
    /// <summary>
    /// 更新表中数据
    /// </summary>
    /// <param name="tableName">表名</param>
    /// <param name="colNames">列名</param>
    /// <param name="colValues">更新的数据</param>
    /// <param name="selectKey">主键</param>
    /// <param name="selectValue">主键值</param>

    public bool UpdateZhuangbeiInto(SQLDate date)
    {
        string commPath = "UPDATE " + tabName + " SET EXTAN='" + date.extan+ "', ISNET=1";
        commPath += " WHERE GOODID=" +date.goodId + " AND ISDELETE=1";
        // ExecuteSQLCommand(commPath);
        SqlWaitListAddBean bean = new SqlWaitListAddBean();
        bean.action = 2;
        bean.date = date;
        bean.command = commPath;
        bean.methonId = MethonName.UpdateZhuangbeiInto;
        addList(bean);
     //   ExecuteSQLCommand(commPath);
        Debug.Log("更新数据成功!");
      //  mNetHelper.changeInto(date);
        return true;
    }
    private bool isUpdateIng = false;
    List<SqlNetDate> mUpdateList = null;
    public void updateToNetEnd(bool isSuccess) {
        isUpdateIng = false;
        if(mUpdateList == null) {
            return;    
        }
        if (isSuccess) {
#if UNITY_ANDROID || UNITY_IOS
            List<SQLDate> list = new List<SQLDate>();

            foreach (SqlNetDate date in mUpdateList)
            {
                SQLDate tmp = date.date;
                list.Add(tmp);
            }
            GameObject.Find("Manager").GetComponent<SqlControlToNative>().updateEndNet(list);
#else
        foreach (SqlNetDate date in mUpdateList) {
            string commandString = "UPDATE  " + tabName + " SET  ISNET=2" +
                " WHERE TYPE=" + date.date.type +
                " AND ID=" + date.date.id +
                " AND EXTAN="+"'"+date.date.extan+"'"+
                " AND GOODID=" + date.date.goodId +
                " AND GOODTYPE=" + date.date.goodType +
                " AND ISCLENAN=" + date.date.isClean +
                " AND ISNET=" + date.date.isNet +
                " AND ISDELETE=" + date.date.isDelete  ;
             ExecuteSQLCommand(commandString);
        }
#endif
        }
        if (SQLHelper.getIntance().isCleanNet) {
#if UNITY_ANDROID || UNITY_IOS
            GameObject.Find("Manager").GetComponent<SqlControlToNative>().deleteCleanNet();
            SQLHelper.getIntance().isCleanNet = false;
#else
            string commandString1 = "DELETE FROM " + tabName + " WHERE TYPE="+SQLHelper.TYPE_CLEAN_NET;
            ExecuteSQLCommand(commandString1);
            SQLHelper.getIntance().isCleanNet = false;
#endif

        }
#if UNITY_ANDROID || UNITY_IOS
        GameObject.Find("Manager").GetComponent<SqlControlToNative>().removeDeleteDate();
#else
        string commandString2 = "DELETE FROM " + tabName + " WHERE ISNET=2 AND ISDELETE=2";
        ExecuteSQLCommand(commandString2);
#endif

    }

    public void updateToNet() {
        Debug.Log("更新后台数据!");
        Thread th1 = new Thread(() =>
        {
            isUpdateIng = true;
            try
            {
                mUpdateList = new List<SqlNetDate>();
#if UNITY_ANDROID || UNITY_IOS
                List<SQLDate> list = GameObject.Find("Manager").GetComponent<SqlControlToNative>().getNetDate();
                foreach (SQLDate tmp in list) {
                    SqlNetDate bean = new SqlNetDate();
                    bean.date = tmp;
                    if (tmp.isDelete == 2)
                    {
                        bean.action = 2;
                    }
                    else
                    {
                        bean.action = 1;
                    }
                    mUpdateList.Add(bean);
                }
#endif
#if UNITY_STANDALONE
                string comm = "select * from  " + tabName + " WHERE  ISNET=1";
                if (mConnet == null)
                {
                    mConnet = new SqliteConnection(getSqlPath());
                    mConnet.Open();
                }
                using (reader)
                {
                    if (SQLHelper.getIntance().isCleanNet) {
                        clearAllDelete();
                        SQLDate date = new SQLDate();
                        SqlNetDate bean = new SqlNetDate();
                        bean.date = date;
                        bean.action = 4;
                        mUpdateList.Add(bean);
                    }

                    Debug.Log("ExecuteSQLCommand queryString=" + comm);

                    SqliteCommand command = mConnet.CreateCommand();
                    command.CommandText = comm;
                    reader = command.ExecuteReader();
                    while (this.reader.Read())
                    {
                        SQLDate date = new SQLDate();
                        SqlNetDate bean = new SqlNetDate();
                        bean.date = date;


                        date.type = reader.GetInt64(reader.GetOrdinal("TYPE"));
                        date.id = reader.GetInt64(reader.GetOrdinal("ID"));
                        date.extan = reader.GetString(reader.GetOrdinal("EXTAN"));
                        date.goodId = reader.GetInt64(reader.GetOrdinal("GOODID"));
                        date.goodType = reader.GetInt64(reader.GetOrdinal("GOODTYPE"));
                        date.isClean = reader.GetInt64(reader.GetOrdinal("ISCLENAN"));
                        if (GameManager.getIntance().mCurrentSqlVersion == 1)
                        {
                            date.isDelete = reader.GetInt64(reader.GetOrdinal("ISDELETE"));
                            date.isNet = reader.GetInt64(reader.GetOrdinal("ISNET"));
                        }
                        if (date.isDelete == 2)
                        {
                            bean.action = 2;
                        }
                        else {
                            bean.action = 1;
                        }
                        mUpdateList.Add(bean);
                    }
                    reader.Close();
                    //           cnn.Close();            
                }
#endif
                NetServer.getIntance().updateNet(mUpdateList);
            }
            catch (System.Exception e)
            {
                Debug.Log("处理存档出错");
                Debug.Log(e.Message);

            }

        });
        th1.Start();

    }
    public bool UpdateInto(SQLDate date) {
        return UpdateInto(date, false);
    }

    public bool UpdateInto(SQLDate date,bool isNow)
    {
        string commPath = "UPDATE " + tabName + " SET EXTAN='" + date.extan+"', ISNET=1";
        commPath += " WHERE Type=" + date.type + " AND ID=" + date.id + " AND ISDELETE=1";
        // ExecuteSQLCommand(commPath);
        SqlWaitListAddBean bean = new SqlWaitListAddBean();
        bean.action = 2;
        bean.date = date;
        bean.methonId = MethonName.updateIdAndType;
        bean.command = commPath;
        if (isNow)
        {
#if UNITY_ANDROID || UNITY_IOS
            GameObject.Find("Manager").GetComponent<SqlControlToNative>().updateIdAndType(date);
#else
            ExecuteSQLCommand(commPath);
#endif


        }
        else {
             addList(bean);
            
        }
        
//        Debug.Log("更新数据成功!");
      //  mNetHelper.changeInto(date);
        return true;
    }

    public void saveLocal(string str) {
        Debug.Log("saveLocal");
        List<SQLDate> list = null;
        if (str != null && str.Length > 0) {
            Newtonsoft.Json.Linq.JObject jb = Newtonsoft.Json.Linq.JObject.Parse(str);
            int status = jb.Value<int>("status");
            if (status == 0) {
                var arrdata = jb.Value<Newtonsoft.Json.Linq.JArray>("date");
                Debug.Log(" Newtonsoft.Json.Linq.JArray.Parse(str);");
                list = arrdata.ToObject<List<SQLDate>>();
                Debug.Log(" arrdata.ToObject<List<SQLDate>>();");
            }
        }
        SQLDate data = new SQLDate();
        data.type = SQLHelper.TYPE_ENCODE_VERSION;
        data.id = 1;
        data.isClean = 2;


#if UNITY_ANDROID
        GameObject.Find("Manager").GetComponent<SqlControlToNative>().delectAll(tabName);
        GameObject.Find("Manager").GetComponent<SqlControlToNative>().inSertDate(data);
#endif
#if UNITY_STANDALONE
         string comm = "DELETE FROM " + tabName;
    //    mNetHelper.cleanAllLocal();

        ExecuteSQLCommand(comm);
        InsertDataToSQL(data, false, true);
#endif




        Debug.Log("saveLocal end");
        //      SQLNetManager.getIntance().cleanAllLocal();
        if (list == null || list.Count == 0) {
            return;
        }
        foreach (SQLDate date in list) {
            date.isNet = 2;
            if (date.type == SQLHelper.TYPE_ENCODE_VERSION) {
                continue;
            }
            Debug.Log(" arrdata.ToObject<List<SQLDate>>();");
#if UNITY_ANDROID
            GameObject.Find("Manager").GetComponent<SqlControlToNative>().inSertDate(data);
#endif
#if UNITY_STANDALONE

             InsertDataToSQL(date, true,NetServer.getIntance().isNew);
#endif
        }
    }

    /// <summary>
    /// 从数据库中查询相关的数据
    /// </summary>
    void QueryDataFromSQL()
    {

    }

    public List<SQLDate> readAllTable() {
#if UNITY_ANDROID
        return GameObject.Find("Manager").GetComponent<SqlControlToNative>().getAll();
#endif
#if UNITY_STANDALONE
        List<SQLDate> list = new List<SQLDate>();
        if (mConnet == null) {
            mConnet = new SqliteConnection(getSqlPath());
            mConnet.Open();
        }
      
//        using (SqliteConnection cnn =)
        using (reader)
        {
            Debug.Log("readAllTable");
            
            SqliteCommand command = mConnet.CreateCommand();
            command.CommandText = "select * from " + tabName;
            this.reader = command.ExecuteReader();
            while (this.reader.Read())
            {
                SQLDate date = new SQLDate();
                date.type = reader.GetInt64(reader.GetOrdinal("TYPE"));
                date.id = reader.GetInt64(reader.GetOrdinal("ID"));
                date.extan = reader.GetString(reader.GetOrdinal("EXTAN"));
                date.goodId = reader.GetInt64(reader.GetOrdinal("GOODID"));
                date.goodType = reader.GetInt64(reader.GetOrdinal("GOODTYPE"));
                date.isClean = reader.GetInt64(reader.GetOrdinal("ISCLENAN"));
                if (GameManager.getIntance().mCurrentSqlVersion == 1) {
                    date.isDelete = reader.GetInt64(reader.GetOrdinal("ISDELETE"));
                    date.isNet = reader.GetInt64(reader.GetOrdinal("ISNET"));
                }
                Debug.Log("readAllTable date.type  = " + date.type + " id = " + date.id + " extan = " + date.extan + " date.goodId" + date.goodId + " date.goodType=" + date.goodType + " date.isClean= " + date.isClean);
                Debug.Log("readAllTable date.type  = " + date.id);
                list.Add(date);
            }
//            cnn.Close();
        }
//        SqliteConnection.ClearAllPools();
//        GC.Collect();
//        GC.WaitForPendingFinalizers();
        if (list.Count > 0)
        {
            return list;
        }
        else {
            return null;
        }
#endif

    }

    internal void deleteAll()
    {
        SQLDate data = new SQLDate();
        data.type = SQLHelper.TYPE_ENCODE_VERSION;
        data.id = 1;
        data.isClean = 2;
#if UNITY_ANDROID
        GameObject.Find("Manager").GetComponent<SqlControlToNative>().delectAll(tabName);
        GameObject.Find("Manager").GetComponent<SqlControlToNative>().inSertDate(data);
        return;
#endif
        string comm = "DELETE FROM " + tabName;
     //   mNetHelper.cleanAllLocal();
        ExecuteSQLCommand(comm);

        InsertDataToSQL(data, true, NetServer.getIntance().isNew);
        Debug.Log("saveLocal end");
    }

    public enum MethonName
    {
        defult,
        createTable,
        getLevel,
        getPlayVocation,
        onUodateInfoByTypeAndId,
        alterTableForIsNetAndIsDelete,
        isUpdate,
        clearAllDelete,
        inSertDate,
        changeGoodType,
        changeGoodSql,
        updateIdAndType,
        deleteIdAndType,
        deleteGood,
        deleteLuiHui,
        UpdateZhuangbeiInto,
        updateEndNet,
        deleteCleanNet,
        removeDeleteDate,
        getNetDate,
        getAll,
        saveLocal,
        delectAll,
        deleteGuide
    }

    public class SqlWaitListAddBean
    {
        public int action = -1; //1 为添加 2为更新 3为删除 4 修改物品状态
        public MethonName methonId = MethonName.defult;
        public long old =-1;
        public string command;
        public SQLDate date;
    }

    private List<SqlWaitListAddBean> mWaitList = new List<SqlWaitListAddBean>();
    private void removeListByGoodId(long goodId)
    {
        lock (mLock)
        {
            for (int i = 0; i < mWaitList.Count;)
            {
                if (mWaitList[i].date != null && mWaitList[i].date.type == SQLHelper.TYPE_GOOD && mWaitList[i].date.goodId == goodId)
                {
                    mWaitList.RemoveAt(i);
                    continue;
                }
                i++;
            }
        }
    }

    private void removeListByTypeAndId(long type, long id)
    {
        lock (mLock)
        {
            for (int i = 0; i < mWaitList.Count;)
            {
                if (mWaitList[i].date != null && mWaitList[i].date.id == id && mWaitList[i].date.type == type)
                {
                    mWaitList.RemoveAt(i);
                    continue;
                }
                i++;
            }
        }
    }


    private void addList(SqlWaitListAddBean command)
    {

        lock (mLock)
        {
            //            Debug.Log("addList command="+ command);
            if (command.date != null)
            {
                for (int i = 0; i < mWaitList.Count;)
                {
                    if (mWaitList[i].date != null && mWaitList[i].date.id == command.date.id && mWaitList[i].date.type == command.date.type)
                    {
                        if (command.date.type == 2)
                        {

                            if (command.date.goodId == mWaitList[i].date.goodId && command.date.id == mWaitList[i].date.id)
                            {
                                if (command.action == 3)
                                {
                                    mWaitList.RemoveAt(i);
                                    continue;
                                }
                                else if (command.action == mWaitList[i].action)
                                {
                                    mWaitList.RemoveAt(i);
                                    continue;
                                }

                            }
                        }
                        else
                        {
                            if (command.action == 3)
                            {
                                mWaitList.RemoveAt(i);
                                continue;
                            }
                            else if (command.action == mWaitList[i].action)
                            {
                                mWaitList.RemoveAt(i);
                                continue;
                            }

                            //    mWaitList.RemoveAt(i);
                        }
                    }
                    i++;
                }
            }
            mWaitList.Add(command);
        }
    }
    private void removeList(SqlWaitListAddBean command)
    {
        lock (mLock)
        {
            mWaitList.Remove(command);
        }
    }

    private SqlWaitListAddBean getList(int index)
    {
        lock (mLock)
        {
            return mWaitList[index];
        }
    }

    public long getListCount()
    {
        lock (mLock)
        {
            return mWaitList.Count;
        }
    }

    private bool listIsEmpty()
    {
        lock (mLock)
        {
            return mWaitList.Count == 0;
        }
    }

    private void threadRun()
    {
        while (true)
        {
            //            Debug.Log("======================================threadRun command count");
            if (listIsEmpty())
            {
                Thread.Sleep(1000);
            }
            else
            {
                SqlWaitListAddBean bean = getList(0);
                string command = bean.command;
                //              Debug.Log("threadRun command = " + command);
#if UNITY_ANDROID || UNITY_IOS
                if (bean.methonId == MethonName.deleteGuide) {
                    GameObject.Find("Manager").GetComponent<SqlControlToNative>().deleteGuide(bean.date.id+"");
                }
                else if(bean.methonId == MethonName.inSertDate) {
                    GameObject.Find("Manager").GetComponent<SqlControlToNative>().inSertDate(bean.date);
                }
                else if (bean.methonId == MethonName.changeGoodType)
                {
                    GameObject.Find("Manager").GetComponent<SqlControlToNative>().changeGoodType(bean.date);
                }
                else if (bean.methonId == MethonName.changeGoodSql)
                {
                    GameObject.Find("Manager").GetComponent<SqlControlToNative>().changeGoodSql(bean.date,bean.old+"");
                }
                else if (bean.methonId == MethonName.updateIdAndType)
                {
                    GameObject.Find("Manager").GetComponent<SqlControlToNative>().updateIdAndType(bean.date);
                }
                else if (bean.methonId == MethonName.deleteIdAndType)
                {
                    GameObject.Find("Manager").GetComponent<SqlControlToNative>().deleteIdAndType(bean.date);
                }
                else if (bean.methonId == MethonName.deleteGood)
                {
                    GameObject.Find("Manager").GetComponent<SqlControlToNative>().deleteGood(bean.date);
                }
                else if (bean.methonId == MethonName.deleteLuiHui)
                {
                    GameObject.Find("Manager").GetComponent<SqlControlToNative>().deleteLuiHui();
                }
                else if (bean.methonId == MethonName.UpdateZhuangbeiInto)
                {
                    GameObject.Find("Manager").GetComponent<SqlControlToNative>().UpdateZhuangbeiInto(bean.date);
                }
#else
            ExecuteSQLCommand(commPath);
#endif
                removeList(bean);
            }
        }
    }


    private void OnApplicationQuit()
    {
        //当程序退出时关闭数据库连接，不然会重复打开数据卡，造成卡顿
        this.CloseSQLConnection();
     //   SQLNetManager.getIntance().OnApplicationQuit();
        Debug.Log("程序退出");
    }
}