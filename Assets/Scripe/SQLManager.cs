using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using Mono.Data.Sqlite;
using UnityEngine;
public class SQLManager
{
    private SqliteDataReader reader;
    /// <summary>
    /// 本地数据库名字
    /// </summary>
    public string sqlName;
    public string tabName;

    private string sqlName_old = "local88";
    private string tabName_old = "local88";

    private string sqlName_new = "local891";
    private string tabName_new = "local891";

    object mLock = new object();
    SqliteConnection mConnet = null;
    private static int IDCount;
    Thread th1 = null;
    NetHelper mNetHelper = new NetHelper();

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
    public static string SQL_NAME_NET_BACK = "_net2";
    public string getSqlTableName() {
        return tabName + SQL_NAME_NET_BACK;
    }
    public string getSqlSqlName()
    {
        return sqlName + SQL_NAME_NET_BACK;
    }

    public string getSqlNetFilePath()
    {
        string path = "";
#if UNITY_ANDROID
        path = mPathRoot + "/" + sqlName + SQL_NAME_NET_BACK;
#endif

#if UNITY_IOS
        path = mPathRoot + "/" + sqlName + SQL_NAME_NET_BACK;
#endif

#if UNITY_STANDALONE
        path = mPathRoot + "/Resources/" + this.sqlName;
#endif
        return path;
    }

    public string getSqlNetPath()
    {
        string path = "";
#if UNITY_ANDROID
        path = "URI=file:" + mPathRoot + "/" + sqlName + SQL_NAME_NET_BACK;
#endif
#if UNITY_IOS 
        path ="data source=" + mPathRoot + "/" + sqlName + SQL_NAME_NET_BACK;
#endif
#if UNITY_STANDALONE
        path = "data source=" + mPathRoot + "/Resources/" + this.sqlName;
#endif
        return path;
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
            "ISCLENAN      INT )",
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

    public long getPlayVocation() {
        if (File.Exists(getSqlFilePath())) {
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
                command.CommandText = "select * from  " + tabName + " WHERE  ID=" + SQLHelper.GAME_ID_PLAYER_VOCATION + " AND TYPE=" + SQLHelper.TYPE_GAME;
                this.reader = command.ExecuteReader();
                while (this.reader.Read())
                {
                   long vocation = long.Parse(reader.GetString(reader.GetOrdinal("EXTAN")));
                    return vocation;
                }
            }
        }
        return -1;
    }

    public long getLevel() {
        if (File.Exists(getSqlFilePath()))
        {
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
                command.CommandText = "select * from  " + tabName + " WHERE  ID=" + SQLHelper.GAME_ID_LEVEL + " AND TYPE=" + SQLHelper.TYPE_GAME;
                this.reader = command.ExecuteReader();
                while (this.reader.Read())
                {
                    long vocation = long.Parse(reader.GetString(reader.GetOrdinal("EXTAN")));
                    return vocation;
                }
            }
        }
        return -1;
    }
    public int initNoNet()
    {
        sqlName = sqlName_new;
        tabName = tabName_new;
        mPathRoot = Application.persistentDataPath;

        if (!File.Exists(getSqlFilePath()))
        {
            GameManager.getIntance().mInitDec = JsonUtils.getIntance().getStringById(100026);
            creatLocl888Android();
            sqlName = sqlName_old;
            tabName = tabName_old;
            if (File.Exists(getSqlFilePath()))
            {
                GameManager.getIntance().mInitDec = JsonUtils.getIntance().getStringById(100027);
                List<SQLDate> list = readAllTableOld();

                sqlName = sqlName_new;
                tabName = tabName_new;

                List<SQLDate> newList = new List<SQLDate>();
                int level = -100;
                Debug.Log("readAllTableOld");
                if (list != null && list.Count > 0)
                {
                    foreach (SQLDate date in list)
                    {
                        Debug.Log("type = " + date.type + " id= " + date.id + " extra= " + date.extan);

                        if (date.type == SQLHelper.TYPE_LUNHUI)
                        {
                            date.goodType = SQLDate.GOOD_TYPE_NOGOOD;
                            date.goodId = SQLDate.DEFAULT_GOOD_ID;
                            date.isClean = SQLDate.CLEAR_NO;
                            newList.Add(date);
                        }
                        else if (date.type == SQLHelper.TYPE_GAME && date.id == SQLHelper.GAME_ID_LUNHUI)
                        {
                            date.goodType = SQLDate.GOOD_TYPE_NOGOOD;
                            date.goodId = SQLDate.DEFAULT_GOOD_ID;
                            date.getClean();

                            newList.Add(date);
                        }
                        else if (date.type == SQLHelper.TYPE_GAME && date.id == SQLHelper.GAME_ID_LEVEL)
                        {
                            level = int.Parse(date.extan);
                        }
                    }
                    if (level != -100 && list.Count > 0)
                    {
                        foreach (SQLDate date in newList)
                        {
                            if (date.type == SQLHelper.TYPE_GAME && date.id == SQLHelper.GAME_ID_LUNHUI)
                            {
                                if (level != -100)
                                {
                          //          Debug.Log(" =============== level = " + level);
                                    BigNumber old = BigNumber.getBigNumForString(date.extan);
                            //        Debug.Log("=============== old = " + old.toString());
                                    if (level < 20)
                                    {
                                        
                                        old = BigNumber.add(old, BigNumber.getBigNumForString("2000"));
                                        date.extan = old.toString();
                                    }
                                    else
                                    {
                                        
                                        Level leveldate = JsonUtils.getIntance().getLevelData(level);
                                        BigNumber big = BigNumber.getBigNumForString(date.extan);
                              //          Debug.Log("=============== add = " + leveldate.getReincarnation().toString());
                                        big = BigNumber.add(big, leveldate.getReincarnation());
                                        date.extan = big.toString();
                                    }
                                //    Debug.Log("new = " + date.extan);
                                }
                            }
                            InsertDataToSQL(date, true);
                        }
                    }
/*                    SQLDate count = new SQLDate();
                    count.type = SQLHelper.TYPE_GAME;
                    count.id = SQLHelper.GAME_ID_LEVEL;
                    count.isClean = SQLDate.CLEAR;
                    count.goodType = SQLDate.GOOD_TYPE_NOGOOD;
                    count.goodId = SQLDate.DEFAULT_GOOD_ID;
                    count.extan = "1";
                    InsertDataToSQL(count, true);

                    count = new SQLDate();
                    count.type = SQLHelper.TYPE_GUIDE;
                    count.id = 1;
                    count.isClean = SQLDate.CLEAR_NO;
                    count.goodType = SQLDate.GOOD_TYPE_NOGOOD;
                    count.goodId = SQLDate.DEFAULT_GOOD_ID;
                    count.extan = "-1";
                    InsertDataToSQL(count, true);

                    count = new SQLDate();
                    count.type = SQLHelper.TYPE_GUIDE;
                    count.id = 2;
                    count.isClean = SQLDate.CLEAR_NO;
                    count.goodType = SQLDate.GOOD_TYPE_NOGOOD;
                    count.goodId = SQLDate.DEFAULT_GOOD_ID;
                    count.extan = "-1";
                    InsertDataToSQL(count, true);

                    count = new SQLDate();
                    count.type = SQLHelper.TYPE_GUIDE;
                    count.id = 3;
                    count.isClean = SQLDate.CLEAR_NO;
                    count.goodType = SQLDate.GOOD_TYPE_NOGOOD;
                    count.goodId = SQLDate.DEFAULT_GOOD_ID;
                    count.extan = "-1";
                    InsertDataToSQL(count, true);

                    count = new SQLDate();
                    count.type = SQLHelper.TYPE_GUIDE;
                    count.id = 4;
                    count.isClean = SQLDate.CLEAR_NO;
                    count.goodType = SQLDate.GOOD_TYPE_NOGOOD;
                    count.goodId = SQLDate.DEFAULT_GOOD_ID;
                    count.extan = "-1";
                    InsertDataToSQL(count, true);

                    count = new SQLDate();
                    count.type = SQLHelper.TYPE_GUIDE;
                    count.id = 5;
                    count.isClean = SQLDate.CLEAR_NO;
                    count.goodType = SQLDate.GOOD_TYPE_NOGOOD;
                    count.goodId = SQLDate.DEFAULT_GOOD_ID;
                    count.extan = "-1";
                    InsertDataToSQL(count, true);

                    count = new SQLDate();
                    count.type = SQLHelper.TYPE_GAME;
                    count.id = SQLHelper.GAME_ID_POINT_LUNHUI;
                    count.isClean = SQLDate.CLEAR;
                    count.goodType = SQLDate.GOOD_TYPE_NOGOOD;
                    count.goodId = SQLDate.DEFAULT_GOOD_ID;
                    count.extan = "1";
                    InsertDataToSQL(count, true);

                    count = new SQLDate();
                    count.type = SQLHelper.TYPE_GAME;
                    count.id = SQLHelper.GAME_ID_FRIST_START;
                    count.isClean = SQLDate.CLEAR_NO;
                    count.goodType = SQLDate.GOOD_TYPE_NOGOOD;
                    count.goodId = SQLDate.DEFAULT_GOOD_ID;
                    count.extan = "1";
                    InsertDataToSQL(count, true);*/

                    return 1;
                }
                else {
                    return 3;
                }
            }
            else
            {
                sqlName = sqlName_new;
                tabName = tabName_new;
                return 3;
            }
        }
        else {
            sqlName = sqlName_new;
            tabName = tabName_new;
        }
        GameManager.getIntance().mInitDec = JsonUtils.getIntance().getStringById(100028);
        return 2;
    }

    public bool isUpdateed() {
        bool back = true;
        string comm = "select * from  " + tabName + " WHERE  ID=" + SQLHelper.GAME_ID_IS_UPDATE + " AND TYPE=" + SQLHelper.TYPE_GAME;
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
    }

    private void creatLocl888Android() {
        sqlName = sqlName_new;
        tabName = tabName_new;
        Debug.Log("  连接数据库 ");
        this.CreateSQLTable(
            tabName,
            "CREATE TABLE " + tabName + "(" +
            "TYPE          INT ," +
            "ID            INT ," +
            "EXTAN         TEXT ,"+
            "GOODID        INT ," +
            "GOODTYPE      INT ," +
            "ISCLENAN      INT )",
            null,
            null
        );
    }

    

    public void copyToNet() {
        if (!File.Exists(getSqlNetFilePath()))
        {
            GameManager.getIntance().mInitDec = JsonUtils.getIntance().getStringById(100036);
            Debug.Log("!File.Exists = " + getSqlNetFilePath());
            GameManager.getIntance().mInitDec = JsonUtils.getIntance().getStringById(100032);
            List<SQLDate> list = readAllTable();
            GameManager.getIntance().mInitDec = JsonUtils.getIntance().getStringById(100033);
            if (list == null)
            {
                GameManager.getIntance().mInitDec = JsonUtils.getIntance().getStringById(100034);
                return;
            }
            try
            {
                foreach (SQLDate date in list)
                {
                    mNetHelper.changeInto(date);
                }
            }
            catch (Exception e){
                Debug.Log("Exception = " + e);
            }

            GameManager.getIntance().mInitDec = JsonUtils.getIntance().getStringById(100034);
        }
        else {
            GameManager.getIntance().mInitDec = JsonUtils.getIntance().getStringById(100035);
            Debug.Log("File.Exists = " + getSqlNetFilePath());
        }
    }


    public void clearAllNet() {
        mNetHelper.cleanAllNet();
    }

    /// <summary>
    ///执行SQL命令,并返回一个SqliteDataReader对象
    /// <param name="queryString"></param>
    public void ExecuteSQLCommand(string queryString)
    {
        if (mConnet == null) {
            mConnet = new SqliteConnection(getSqlPath());
            mConnet.Open();
        }
        using (reader) {
//            Debug.Log("ExecuteSQLCommand queryString="+ queryString);
            
            SqliteCommand command = mConnet.CreateCommand();
            command.CommandText = queryString;
            reader = command.ExecuteReader();
            reader.Close();
 //           cnn.Close();            
        }
//        SqliteConnection.ClearAllPools();
//        GC.Collect();
//        GC.WaitForPendingFinalizers();
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
    }
    /// <summary>
    /// 关闭数据库连接,注意这一步非常重要，最好每次测试结束的时候都调用关闭数据库连接
    /// 如果不执行这一步，多次调用之后，会报错，数据库被锁定，每次打开都非常缓慢
    /// </summary>
    public void CloseSQLConnection()
    {      
        Debug.Log("已经断开数据库连接");
        if (mConnet != null)
        {
            mConnet.Close();
            mConnet.Dispose();
            mConnet = null;
        }
    }

    public void deleteGuide(long id) {
        string commandString = "DELETE FROM " + tabName + " WHERE TYPE=" + SQLHelper.TYPE_GUIDE + " AND ID=" + id;
        // ExecuteSQLCommand(commandString);
        SqlWaitListAddBean bean = new SqlWaitListAddBean();
        bean.action = 3;
        bean.date = null;
        bean.command = commandString;
        addList(bean);
    }

    /// <summary>
    /// 向数据库中添加数据文件
    /// </summary>
    /// 
    public void InsertDataToSQL(SQLDate date) {
        InsertDataToSQL(date, false,true);
    }
    public void InsertDataToSQL(SQLDate data, bool isNow) {
        InsertDataToSQL(data, isNow, true);
    }

    public void InsertDataToSQL(SQLDate data, bool isNow,bool isUpToNet)
    {
        string commandString = "INSERT INTO " + tabName + " VALUES (";

        commandString +=   data.type;
        commandString += "," + data.id;
        commandString += "," + "'" + data.extan + "'";
        commandString += "," + data.goodId;
        commandString += "," + data.goodType;
        commandString += "," + data.isClean + ")";
        
        SqlWaitListAddBean bean = new SqlWaitListAddBean();
        bean.action = 1;
        bean.date = data;
        bean.command = commandString;
        if (isNow)
        {
            ExecuteSQLCommand(commandString);            
        }
        else {
            addList(bean);
        }
        if (isUpToNet) {
            mNetHelper.changeInto(data);
        }       
        //     
    }
    public void changeGoodType(SQLDate date)
    {
        string commPath = "UPDATE " + tabName + " SET GOODTYPE=" + date.goodType;
        commPath += " WHERE GOODID=" + date.goodId;
        SqlWaitListAddBean bean = new SqlWaitListAddBean();
        bean.action = 4;
        bean.date = date;
        bean.command = commPath;
        addList(bean);
     //   ExecuteSQLCommand(commPath);
        mNetHelper.changeInto(date);
    }
    public void changeGoodSql(SQLDate date,long old)
    {
        string commPath = "UPDATE " + tabName + " SET GOODID=" + date.goodId;
        commPath += " WHERE GOODID=" + old + " AND TYPE="+date.type+ " AND ID="+ date.id;
        SqlWaitListAddBean bean = new SqlWaitListAddBean();
        bean.action = 4;
        bean.date = date;
        bean.command = commPath;
        addList(bean);
        //   ExecuteSQLCommand(commPath);
        mNetHelper.changeInto(date);
    }
    public void updateIdAndType(SQLDate date)
    {
        string commPath = "UPDATE " + tabName + " SET EXTAN='" + date.extan + "'";
        commPath = commPath+ " WHERE TYPE=" + date.type + " AND ID=" + date.id;
        // ExecuteSQLCommand(commandString);
        SqlWaitListAddBean bean = new SqlWaitListAddBean();
        bean.action = 2;
        bean.date = date;
        bean.command = commPath;
        addList(bean);
        //   ExecuteSQLCommand(commandString);
        mNetHelper.changeInto(date);
    }

    public void deleteIdAndType(SQLDate date)
    {
        string commandString = "DELETE FROM " + tabName + " WHERE TYPE=" + date.type+ " AND ID="+date.id;
        SqlWaitListAddBean bean = new SqlWaitListAddBean();
        bean.action = 3;
        bean.date = date;
        bean.command = commandString;
        // ExecuteSQLCommand(commandString);
        removeListByTypeAndId(date.id, date.type);
        addList(bean);
        //   ExecuteSQLCommand(commandString);
        mNetHelper.delectInfo(date);
    }

    public void deleteGood(SQLDate date)
    {
        string commandString = "DELETE FROM " + tabName + " WHERE GOODID =" + date.goodId;
        // ExecuteSQLCommand(commandString);
        SqlWaitListAddBean bean = new SqlWaitListAddBean();
        bean.action = 3;
        bean.date = date;
        bean.command = commandString;
        removeListByGoodId(date.goodId);
        addList(bean);
     //   ExecuteSQLCommand(commandString);
        mNetHelper.delectInfo(date);
    }
    public void deleteLuiHui()
    {
        string commandString = "DELETE FROM " + tabName + " WHERE ISCLENAN =1" ;
        // ExecuteSQLCommand(commandString);
        SqlWaitListAddBean bean = new SqlWaitListAddBean();
        bean.action = 3;
        bean.date = null;
        bean.command = commandString;
        addList(bean);
     //   ExecuteSQLCommand(commandString);
        mNetHelper.cleanLuihui();
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
        string commPath = "UPDATE " + tabName + " SET EXTAN='" + date.extan+"'";
        commPath += " WHERE GOODID=" +date.goodId;
        // ExecuteSQLCommand(commPath);
        SqlWaitListAddBean bean = new SqlWaitListAddBean();
        bean.action = 2;
        bean.date = date;
        bean.command = commPath;
        addList(bean);
     //   ExecuteSQLCommand(commPath);
        Debug.Log("更新数据成功!");
        mNetHelper.changeInto(date);
        return true;
    }

    public void updateToNet() {
        Debug.Log("更新后台数据!");
        mNetHelper.updateToNet();
    }
    public bool UpdateInto(SQLDate date) {
        return UpdateInto(date, false);
    }

    public bool UpdateInto(SQLDate date,bool isNow)
    {
        string commPath = "UPDATE " + tabName + " SET EXTAN='" + date.extan+"'";
        commPath += " WHERE Type=" + date.type + " AND ID=" + date.id;
        // ExecuteSQLCommand(commPath);
        SqlWaitListAddBean bean = new SqlWaitListAddBean();
        bean.action = 2;
        bean.date = date;
        bean.command = commPath;
        if (isNow)
        {
            ExecuteSQLCommand(commPath);
        }
        else {
             addList(bean);
            
        }
        
//        Debug.Log("更新数据成功!");
        mNetHelper.changeInto(date);
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
        string comm = "DELETE FROM " + tabName;
        mNetHelper.cleanAllLocal();

        ExecuteSQLCommand(comm);
        ;
        //      SQLNetManager.getIntance().cleanAllLocal();
        if (list == null || list.Count == 0) {
            return;
        }
        foreach (SQLDate date in list) {
            Debug.Log(" arrdata.ToObject<List<SQLDate>>();");
            InsertDataToSQL(date, true,NetServer.getIntance().isNew);
        }
        Debug.Log("saveLocal end");
    }

    public  class SqlWaitListAddBean {
        public int action = -1; //1 为添加 2为更新 3为删除 4 修改物品状态
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
                if (mWaitList[i].date != null && mWaitList[i].date.type == SQLHelper.TYPE_GOOD && mWaitList[i].date.goodId == goodId )
                {
                    mWaitList.RemoveAt(i);
                    continue;
                }
                i++;
            }
        }
    }

    private void removeListByTypeAndId(long type ,long id)
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


    private void addList(SqlWaitListAddBean command) {

        lock (mLock) {
            //            Debug.Log("addList command="+ command);
            if (command.date != null) {
                for (int i = 0; i < mWaitList.Count; )
                {
                    if (mWaitList[i].date != null && mWaitList[i].date.id == command.date.id && mWaitList[i].date.type == command.date.type) {
                        if (command.date.type == 2)
                        {

                            if (command.date.goodId == mWaitList[i].date.goodId && command.date.id == mWaitList[i].date.id)
                            {
                                if (command.action == 3)
                                {
                                    mWaitList.RemoveAt(i);
                                    continue;
                                }
                                else if(command.action == mWaitList[i].action)
                                {
                                    mWaitList.RemoveAt(i);
                                    continue;
                                }                               
                               
                            }
                        }
                        else {
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
    private void removeList(SqlWaitListAddBean command) {
        lock (mLock)
        {
            mWaitList.Remove(command);
        }
    }

    private SqlWaitListAddBean getList(int index) {
        lock (mLock)
        {
            return mWaitList[index];
        }
    }

    public long getListCount() {
        lock (mLock)
        {
            return mWaitList.Count;
        }
    }

    private bool listIsEmpty() {
        lock (mLock)
        {
            return mWaitList.Count == 0;
        }
    }

    private void threadRun() {
        while (true) {
//            Debug.Log("======================================threadRun command count");
            if (listIsEmpty())
            {
                Thread.Sleep(1000);
            }
            else {
                SqlWaitListAddBean bean = getList(0);
                string command = bean.command;
                //              Debug.Log("threadRun command = " + command);
                ExecuteSQLCommand(command);
                //   Debug.Log("threadRun command success " );
                removeList(bean);
            }
        }
    }

    /// <summary>
    /// 从数据库中查询相关的数据
    /// </summary>
    void QueryDataFromSQL()
    {

    }

    public List<SQLDate> readAllTable() {
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
    }

    internal void deleteAll()
    {
        
    }

    public List<SQLDate> readAllTableOld()
    {
        List<SQLDate> list = new List<SQLDate>();
        using (SqliteConnection cnn = new SqliteConnection(getSqlPath()))
        using (reader) {
            Debug.Log("readAllTableOld");
            cnn.Open();
            SqliteCommand command = cnn.CreateCommand();
            command.CommandText = "select * from " + tabName;
            reader = command.ExecuteReader();
           
            while (this.reader.Read())
            {
                SQLDate date = new SQLDate();
                date.type = reader.GetInt64(reader.GetOrdinal("Type"));
                date.id = reader.GetInt64(reader.GetOrdinal("ID"));
                date.extan = reader.GetString(reader.GetOrdinal("Extan"));
                date.extan = date.extan.Replace("，", ",");
                date.extan = date.extan.Replace("。", ".");
                list.Add(date);
                 Debug.Log(reader.GetInt32(reader.GetOrdinal("Time")));
            }
            cnn.Close();
           
        }
        SqliteConnection.ClearAllPools();
        GC.Collect();
        GC.WaitForPendingFinalizers();
        if (list.Count > 0)
        {
            return list;
        }
        else
        {
            return null;
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