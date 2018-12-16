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

    object mLock = new object();

    private static int IDCount;
    public void Start()
    {

    }
    public void init() {
        if (!GameManager.isAndroid)
        {
            this.CreateSQL();

        }
        this.OpenSQLaAndConnect();
        Thread th1 = new Thread(threadRun);
        th1.Start();
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
                "CREATE TABLE "+ tabName + "(" +
                "TYPE          INT ," +
                "ID             INT ," +
                "EXTAN          TEXT )",
                null,
                null
            );
            this.connection.Close();
            return;
        }
    }
    //打开数据库
    public void OpenSQLaAndConnect()
    {
        if (GameManager.isAndroid)
        {
            sqlName = "local888";
            tabName = "local888";
            string appDBPath = Application.persistentDataPath + "/" + sqlName;
            if (!File.Exists(appDBPath))//创建新的数据库
            {
                creatLocalAndroid();
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
                this.connection = new SqliteConnection("URI=file:" + appDBPath);
            }
            
        }
        else {
            Debug.Log("  打开数据库 = ");
            this.connection = new SqliteConnection("data source=" + Application.dataPath + "/Resources/" + this.sqlName);
        }
        this.connection.Open();
        Debug.Log("  打开数据库 结束 ");
    //    GameObject.Find("game_begin").GetComponent<GameBeginControl>().init();
    }

    private void creatLocalAndroid (){
        sqlName = "local88";
        tabName = "local88";
        string appDBPath = Application.persistentDataPath + "/" + sqlName;
        if (File.Exists(appDBPath))
        {//有旧的存档
            this.connection = new SqliteConnection("URI=file:" + appDBPath);
            List<SQLDate> list = readAllTable();
            this.connection.Close();
            sqlName = "local888";
            tabName = "local888";
            creatLocl888Android();
            appDBPath = Application.persistentDataPath + "/" + sqlName;
            this.connection = new SqliteConnection("URI=file:" + appDBPath);
            long goodId = 0;
            foreach (SQLDate date in list) {
                if (date.type == SQLHelper.TYPE_GOOD)
                {
                    date.goodType = SQLDate.GOOD_TYPE_BACKPACK;
                    goodId++;
                    date.goodId = goodId;
                    
                    date.isClean = SQLDate.CLEAR;
                }
                else if (date.type == SQLHelper.TYPE_CARD)
                {

                    PlayerBackpackBean newBean = new PlayerBackpackBean();
                    CardJsonBean cj = BackpackManager.getIntance().getCardInfoById(date.id);
                    newBean.goodId = date.id;
                    newBean.sortID = cj.sortID;
                    newBean.count = 1;
                    newBean.tabId = cj.tabid;
                    newBean.isShowPoint = 2;

                    date.extan = SQLHelper.getGoodExtra(newBean);
                    date.type = SQLHelper.TYPE_GOOD;
                    date.goodType = SQLDate.GOOD_TYPE_CARD;
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
                else if (date.type == SQLHelper.TYPE_GAME)
                {
                    date.goodType = SQLDate.GOOD_TYPE_NOGOOD;
                    date.goodId = SQLDate.DEFAULT_GOOD_ID;
                    if (date.id == SQLHelper.GAME_ID_AUTO ||
                       date.id == SQLHelper.GAME_ID_LUNHUI ||
                       date.id == SQLHelper.GAME_ID_TIME ||
                       date.id == SQLHelper.GAME_ID_GUIDE ||
                       date.id == SQLHelper.GAME_ID_POINT_LUNHUI ||
                       date.id == SQLHelper.GAME_ID_NO_LUNHUI ||
                       date.id == SQLHelper.GAME_ID_IS_VOICE)
                    {
                        date.isClean = SQLDate.CLEAR_NO;
                    }
                    else {
                        date.isClean = SQLDate.CLEAR ;
                    }
                    date.isClean = SQLDate.CLEAR;
                }
                InsertDataToSQL(new[] { "" + date.type , "" + date.id,"'" + date.extan + "'" + "" + date.goodId,"" + date.goodType,"" + date.isClean},true);
            }
            SQLDate count = new SQLDate();
            count.type = SQLHelper.TYPE_GAME;
            count.id = SQLHelper.GAME_ID_GOOD_MAXID;
            count.isClean = SQLDate.CLEAR_NO;
            count.goodType = SQLDate.GOOD_TYPE_NOGOOD;
            count.goodId = SQLDate.DEFAULT_GOOD_ID;
            count.extan = "" + goodId;
            InsertDataToSQL(new[] { "" + count.type, "" + count.id, "'" + count.extan + "'" + "" + count.goodId, "" + count.goodType, "" + count.isClean }, true);
            list.Add(count);
            updateToNet(list);
        }
        else {//没有旧的存档
            creatLocl888Android();
            appDBPath = Application.persistentDataPath + "/" + sqlName;
            this.connection = new SqliteConnection("URI=file:" + appDBPath);
        }
    }

    private void creatLocl888Android() {
        sqlName = "local888";
        tabName = "local888";
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

    }




    /// <summary>
    ///执行SQL命令,并返回一个SqliteDataReader对象
    /// <param name="queryString"></param>
    public SqliteDataReader ExecuteSQLCommand(string queryString)
    {
        command = connection.CreateCommand();
        this.command.CommandText = queryString;
        this.reader = this.command.ExecuteReader();
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
    public void InsertDataToSQL(string[] insertValues) {
        InsertDataToSQL(insertValues, false);
    }
    public void InsertDataToSQL( string[] insertValues,bool isNow)
    {
        string commandString = "INSERT INTO " + tabName + " VALUES (" + insertValues[0];
        for (int i = 1; i < insertValues.Length; i++)
        {
            commandString += "," + insertValues[i];
        }
        commandString += ")";
        if (isNow)
        {
            ExecuteSQLCommand(commandString);
        }
        else {
            addList(commandString);
        }
        //     
    }
    public void changeGoodType(long goodId,long type)
    {
        string commPath = "UPDATE " + tabName + " SET GOODTYPE=" + type;
        commPath += " WHERE GOODID=" + goodId;
        addList(commPath);
    }


    public void deleteGood(long goodId)
    {
        string commandString = "DELETE FROM " + tabName + " WHERE GOODID =" + goodId; 
        // ExecuteSQLCommand(commandString);
        addList(commandString);
    }
    public void deleteAllType(long type)
    {
        string commandString = "DELETE FROM " + tabName + " WHERE TYPE =" + type;   
        // ExecuteSQLCommand(commandString);
        addList(commandString);
    }
    /// <summary>
    /// 更新表中数据
    /// </summary>
    /// <param name="tableName">表名</param>
    /// <param name="colNames">列名</param>
    /// <param name="colValues">更新的数据</param>
    /// <param name="selectKey">主键</param>
    /// <param name="selectValue">主键值</param>

    public bool UpdateZhuangbeiInto(long goodId,string extan)
    {
        string commPath = "UPDATE " + tabName + " SET EXTAN=" + extan;
        commPath += " WHERE GOODID=" + goodId;
        // ExecuteSQLCommand(commPath);
        addList(commPath);
        Debug.Log("更新数据成功!");
        return true;
    }

    public bool UpdateInto(string extan, long type, long id)
    {
        string commPath = "UPDATE " + tabName + " SET EXTAN=" + extan;
        commPath += " WHERE Type=" + type + " AND ID=" + id;
        // ExecuteSQLCommand(commPath);
        addList(commPath);
      //  Debug.Log("更新数据成功!");
        return true;
    }

    private List<string> mWaitList = new List<string>();

    private void addList(string command) {

        lock (mLock) {
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
        while (true) {
            if (listIsEmpty())
            {
                Thread.Sleep(1000);
            }
            else {
                string command = getList(0);
                ExecuteSQLCommand(command);
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
        Debug.Log("程序退出");
    }
}