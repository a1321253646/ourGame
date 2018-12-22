using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using Mono.Data.Sqlite;
using UnityEngine;
public class SQLManager : MonoBehaviour
{
    /// <summary>
    /// 数据库连接对象
    /// </summary>
    private SqliteConnection connection;
    /// <summary>
    /// 数据库命令
    /// </summary>
    private SqliteCommand command;
    /// <summary>
    /// 数据读取定义
    /// </summary>
    private SqliteDataReader reader;
    /// <summary>
    /// 本地数据库名字
    /// </summary>
    public string sqlName;
    public string tabName;

    private string sqlName_old = "local88";
    private string tabName_old = "local88";

    private string sqlName_new = "local889";
    private string tabName_new = "local889";

    object mLock = new object();

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
        if (!GameManager.isAndroid)
        {
            this.sqlName = sqlName;
            this.tabName = tabName;
            this.CreateSQL();
        }
        int updateStatus = this.OpenSQLaAndConnect();
        th1 = new Thread(threadRun);
        th1.Start();
        return updateStatus;
    }

    //创建数据库文件
    public void CreateSQL()
    {
        if (!File.Exists(Application.dataPath + "/Resources/" + this.sqlName))
        {
            Debug.Log("  数据库 文件没存在 ");
            this.connection = new SqliteConnection("data source=" + Application.dataPath + "/Resources/" + this.sqlName);
            this.connection.Open();
            Debug.Log("  连接数据库  connection= "+ connection);
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
            this.connection.Close();
            return;
        }
    }
    //打开数据库
    public int OpenSQLaAndConnect()
    {
        bool isHaveLocal = false;
        if (GameManager.isAndroid)
        {
            sqlName = sqlName_new;
            tabName = tabName_new;
            string appDBPath = Application.persistentDataPath + "/" + sqlName;
            if (!File.Exists(appDBPath))//创建新的数据库
            {
                isHaveLocal= creatLocalAndroid();
                //用www先从Unity中下载到数据库
                /*               Debug.Log("  Android 拷贝数据库 = ");
                               WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + sqlName);
                               while (!loadDB.isDone)
                               {
                                   Debug.Log("1");
                               }
                               //拷贝至规定的地方
                               File.WriteAllBytes(appDBPath, loadDB.bytes);*/

            }
            else {
                isHaveLocal = true;
                this.connection = new SqliteConnection("URI=file:" + appDBPath);
            }
        }
        else {
            Debug.Log("  打开数据库 = "+ this.sqlName);
            this.connection = new SqliteConnection("data source=" + Application.dataPath + "/Resources/" + this.sqlName);
        }
        this.connection.Open();
        Debug.Log("  打开数据库 结束 ");
        if (GameManager.isAndroid)
        {
            string commPath = "SELECT* FROM " + tabName + " WHERE  ID=" + SQLHelper.GAME_ID_IS_UPDATE + " AND TYPE=" + SQLHelper.TYPE_GAME; //SELECT* FROM Persons WHERE firstname = 'Thomas' OR lastname = 'Carter'
            SqliteDataReader reader = ExecuteSQLCommand(commPath);
            int count = 0;
            while (this.reader.Read())
            {
                count++;
            }
            if (count > 0)
            {
                return 0;
            }
            else
            {
                if (isHaveLocal)
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }
        }
        else {
            return 0;
        }
     //   GameObject.Find("game_begin").GetComponent<GameBeginControl>().init();
    }

    private bool creatLocalAndroid (){
        bool isHavaLocal = false;
        sqlName = sqlName_old;
        tabName = tabName_old;
        string appDBPath = Application.persistentDataPath + "/" + sqlName;
        if (File.Exists(appDBPath))
        {//有旧的存档
            isHavaLocal = true;
            this.connection = new SqliteConnection("URI=file:" + appDBPath);
            this.connection.Open();
            List<SQLDate> list = readAllTableOld();
            this.connection.Close();
            sqlName = sqlName_new;
            tabName = tabName_new;
            creatLocl888Android();
            appDBPath = Application.persistentDataPath + "/" + sqlName;
            this.connection = new SqliteConnection("URI=file:" + appDBPath);
            this.connection.Open();
            long goodId = 0;
            foreach (SQLDate date in list) {
                if (date.type == SQLHelper.TYPE_GOOD)
                {
                    if (date.id > InventoryHalper.TABID_3_START_ID)
                    {
                        date.goodType = SQLDate.GOOD_TYPE_CARD;
                    }
                    else {
                        date.goodType = SQLDate.GOOD_TYPE_BACKPACK;
                    }
                    
                    goodId++;
                    date.goodId = goodId;
                    
                    date.isClean = SQLDate.CLEAR;
                }
                else if (date.type == SQLHelper.TYPE_CARD)
                {

                    PlayerBackpackBean newBean = new PlayerBackpackBean();
                    CardJsonBean cj = JsonUtils.getIntance().getCardInfoById(date.id);
                    newBean.goodId = date.id;
                    newBean.sortID = cj.sortID;
                    newBean.count = 1;
                    newBean.tabId = cj.tabid;
                    newBean.isShowPoint = 2;

                    date.extan = SQLHelper.getGoodExtra(newBean);
                    date.type = SQLHelper.TYPE_GOOD;
                    date.goodType = SQLDate.GOOD_TYPE_USER_CARD;
                    goodId++;
                    date.goodId = goodId;
                    date.isClean = SQLDate.CLEAR;
                }
                else if (date.type == SQLHelper.TYPE_ZHUANGBEI)
                {
                    date.type = SQLHelper.TYPE_GOOD;
                    date.goodType = SQLDate.GOOD_TYPE_ZHUANGBEI;
                    goodId++;
                    date.goodId = goodId;
                    date.isClean = SQLDate.CLEAR;
                }
                else if (date.type == SQLHelper.TYPE_LUNHUI)
                {
                    date.goodType = SQLDate.GOOD_TYPE_NOGOOD;
                    date.goodId = SQLDate.DEFAULT_GOOD_ID;
                    date.isClean = SQLDate.CLEAR_NO;
                }
                else if (date.type == SQLHelper.TYPE_DROP)
                {
                    date.goodType = SQLDate.GOOD_TYPE_NOGOOD;
                    date.goodId = SQLDate.DEFAULT_GOOD_ID;
                    date.isClean = SQLDate.CLEAR;
                }
                else if (date.type == SQLHelper.TYPE_GUIDE)
                {
                    date.goodType = SQLDate.GOOD_TYPE_NOGOOD;
                    date.goodId = SQLDate.DEFAULT_GOOD_ID;
                    date.isClean = SQLDate.CLEAR_NO;
                }
                else if (date.type == SQLHelper.TYPE_GAME)
                {
                    date.goodType = SQLDate.GOOD_TYPE_NOGOOD;
                    date.goodId = SQLDate.DEFAULT_GOOD_ID;
                    date.getClean();
                }
                InsertDataToSQL(date, true);
            }
            SQLDate count = new SQLDate();
            count.type = SQLHelper.TYPE_GAME;
            count.id = SQLHelper.GAME_ID_GOOD_MAXID;
            count.isClean = SQLDate.CLEAR_NO;
            count.goodType = SQLDate.GOOD_TYPE_NOGOOD;
            count.goodId = SQLDate.DEFAULT_GOOD_ID;
            count.extan = "" + goodId;
            InsertDataToSQL(count, true);
            list.Add(count);
            updateToNet(list);
        }
        else {//没有旧的存档
            creatLocl888Android();
            appDBPath = Application.persistentDataPath + "/" + sqlName;
            this.connection = new SqliteConnection("URI=file:" + appDBPath);
        }
        return isHavaLocal;
    }

    private void creatLocl888Android() {
        sqlName = sqlName_new;
        tabName = tabName_new;
        string appDBPath = Application.persistentDataPath + "/" + sqlName;
        this.connection = new SqliteConnection("URI=file:" + appDBPath);
        this.connection.Open();
        Debug.Log("  连接数据库  connection= " + connection);
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
        this.connection.Close();
    }

    private void updateToNet(List<SQLDate> list) {
        foreach (SQLDate date in list) {
            mNetHelper.changeInto(date);
        }
    }

    /// <summary>
    ///执行SQL命令,并返回一个SqliteDataReader对象
    /// <param name="queryString"></param>
    public SqliteDataReader ExecuteSQLCommand(string queryString)
    {
        Debug.Log("ExecuteSQLCommand command  "+ queryString);
        Debug.Log("ExecuteSQLCommand connection  =" + connection);
        Debug.Log("ExecuteSQLCommand connection  =" + connection.State);
        command = connection.CreateCommand();
        Debug.Log("ExecuteSQLCommand  connection.CreateCommand()");
        this.command.CommandText = queryString;
        this.reader = this.command.ExecuteReader();
        Debug.Log("ExecuteSQLCommand  reader = "+ reader.ToString());
        return this.reader;
    }

    /// <summary>
    /// 通过调用SQL语句，在数据库中创建一个表，顶定义表中的行的名字和对应的数据类型
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="columnNames"></param>
    /// <param name="dataTypes"></param>
    public SqliteDataReader CreateSQLTable(string tableName, string commandStr = null, string[] columnNames = null, string[] dataTypes = null)
    {

        return ExecuteSQLCommand(commandStr);
    }
    /// <summary>
    /// 关闭数据库连接,注意这一步非常重要，最好每次测试结束的时候都调用关闭数据库连接
    /// 如果不执行这一步，多次调用之后，会报错，数据库被锁定，每次打开都非常缓慢
    /// </summary>
    public void CloseSQLConnection()
    {
        if (this.command != null)
        {
            this.command.Cancel();
        }

        if (this.reader != null)
        {
            this.reader.Close();
        }

        if (this.connection != null)
        {
            this.connection.Close();

        }
        this.command = null;
        this.reader = null;
        this.connection = null;        
        Debug.Log("已经断开数据库连接");
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
        if (isNow)
        {
            ExecuteSQLCommand(commandString);
        }
        else {
            addList(commandString);
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
        addList(commPath);
        mNetHelper.changeInto(date);
    }


    public void deleteGood(SQLDate date)
    {
        string commandString = "DELETE FROM " + tabName + " WHERE GOODID =" + date.goodId; 
        // ExecuteSQLCommand(commandString);
        addList(commandString);
        mNetHelper.delectInfo(date);
    }
    public void deleteLuiHui()
    {
        string commandString = "DELETE FROM " + tabName + " WHERE ISCLENAN =1" ;   
        // ExecuteSQLCommand(commandString);
        addList(commandString);
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
        addList(commPath);
        Debug.Log("更新数据成功!");
        mNetHelper.changeInto(date);
        return true;
    }

    public void updateToNet() {
        Debug.Log("更新后台数据!");
        mNetHelper.updateToNet();
    }

    public bool UpdateInto(SQLDate date)
    {
        string commPath = "UPDATE " + tabName + " SET EXTAN='" + date.extan+"'";
        commPath += " WHERE Type=" + date.type + " AND ID=" + date.id;
        // ExecuteSQLCommand(commPath);
        addList(commPath);
        Debug.Log("更新数据成功!");
        mNetHelper.changeInto(date);
        return true;
    }

    public void saveLocal(string str) {
        Debug.Log("saveLocal");
        Newtonsoft.Json.Linq.JObject jb = Newtonsoft.Json.Linq.JObject.Parse(str);
        var arrdata = jb.Value<Newtonsoft.Json.Linq.JArray>("date");
        Debug.Log(" Newtonsoft.Json.Linq.JArray.Parse(str);");
        List<SQLDate> list = arrdata.ToObject<List<SQLDate>>();
        Debug.Log(" arrdata.ToObject<List<SQLDate>>();");

        string comm = "DELETE FROM " + tabName;
        ExecuteSQLCommand(comm);
        SQLNetManager.getIntance().cleanAllLocal();
        foreach (SQLDate date in list) {
            Debug.Log(" arrdata.ToObject<List<SQLDate>>();");
            InsertDataToSQL(date, true,false);
        }
        Debug.Log("saveLocal end");
    }


    private List<string> mWaitList = new List<string>();

    private void addList(string command) {

        lock (mLock) {
            Debug.Log("addList command="+ command);
            mWaitList.Add(command);
        }
    }
    private void removeList(string command) {
        lock (mLock)
        {
            mWaitList.Remove(command);
        }
    }

    private string getList(int index) {
        lock (mLock)
        {
            return mWaitList[index];
        }
    }

    private bool listIsEmpty() {
        lock (mLock)
        {
            return mWaitList.Count == 0;
        }
    }

    private void threadRun() {
        while (connection != null) {
            Debug.Log("======================================threadRun command count");
            if (listIsEmpty())
            {
                Thread.Sleep(1000);
            }
            else {
                string command = getList(0);
                Debug.Log("threadRun command = " + command);
                ExecuteSQLCommand(command);
             //   Debug.Log("threadRun command success " );
                removeList(command);
            }
        }
    }

    /// <summary>
    /// 从数据库中查询相关的数据
    /// </summary>
    void QueryDataFromSQL()
    {

    }

    int GetRowCount()
    {
        this.command = this.connection.CreateCommand();
        this.command.CommandText = "select * from "+ tabName;
        this.reader = this.command.ExecuteReader();
        DataTable table = new DataTable();
        table.Load(this.reader);
        Debug.Log(table.Rows.Count);
        return table.Rows.Count;
    }

    public List<SQLDate> readAllTable() {
        this.command = this.connection.CreateCommand();
        this.command.CommandText = "select * from "+tabName;
        this.reader = this.command.ExecuteReader();
        List<SQLDate> list = new List<SQLDate>();
        while (this.reader.Read())
        {
            SQLDate date = new SQLDate();
            date.type = reader.GetInt64(reader.GetOrdinal("Type"));
            date.id = reader.GetInt64(reader.GetOrdinal("ID"));
            date.extan = reader.GetString(reader.GetOrdinal("Extan"));
            date.goodId = reader.GetInt64(reader.GetOrdinal("GOODID"));
            date.goodType = reader.GetInt64(reader.GetOrdinal("GOODTYPE"));
            date.isClean = reader.GetInt64(reader.GetOrdinal("ISCLENAN"));
            list.Add(date);
        }
        if (list.Count > 0)
        {
            return list;
        }
        else {
            return null;
        }
    }
    public List<SQLDate> readAllTableOld()
    {
        this.command = this.connection.CreateCommand();
        this.command.CommandText = "select * from " + tabName;
        this.reader = this.command.ExecuteReader();
        List<SQLDate> list = new List<SQLDate>();
        while (this.reader.Read())
        {
            SQLDate date = new SQLDate();
            date.type = reader.GetInt64(reader.GetOrdinal("Type"));
            date.id = reader.GetInt64(reader.GetOrdinal("ID"));
            date.extan = reader.GetString(reader.GetOrdinal("Extan"));
            date.extan = date.extan.Replace("，", ",");
            date.extan = date.extan.Replace("。", ".");
            list.Add(date);
            // Debug.Log(reader.GetInt32(reader.GetOrdinal("Time")));
            // Debug.Log(tempCount);
        }
        if (list.Count > 0)
        {
            return list;
        }
        else
        {
            return null;
        }
    }


    /// <summary>
    /// 读取数据表中的数据，返回满足条件的数据
    /// </summary>
    public SqliteDataReader ReadTable( string[] items, string[] colNames, string[] operations, string[] colValues)
    {
        string queryString = "SELECT " + items[0];
        for (int i = 1; i < items.Length; i++)
        {
            queryString += ", " + items[i];
        }
        queryString += " FROM " + tabName + " WHERE " + colNames[0] + " " + operations[0] + " " + colValues[0];
        for (int i = 0; i < colNames.Length; i++)
        {
            queryString += " AND " + colNames[i] + " " + operations[i] + " " + colValues[0] + " ";
        }
        return this.ExecuteSQLCommand(queryString);
    }
    private void OnApplicationQuit()
    {
        //当程序退出时关闭数据库连接，不然会重复打开数据卡，造成卡顿
        this.CloseSQLConnection();
        SQLNetManager.getIntance().OnApplicationQuit();
        Debug.Log("程序退出");
    }
}