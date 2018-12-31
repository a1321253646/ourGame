using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using Mono.Data.Sqlite;
using UnityEngine;
public class SQLNetManager 
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

    object mLock1 = new object();

    private void init() {
        this.OpenSQLaAndConnect();
        Thread th1 = new Thread(threadRun);
        th1.Start();
    }

    private List<SqlNetDate> mWaitList = new List<SqlNetDate>();

    public void addList(SqlNetDate command)
    {

        lock (mLock1)
        {
            mWaitList.Add(command);
        }
    }
    private void removeList(SqlNetDate command)
    {
        lock (mLock1)
        {
            mWaitList.Remove(command);
        }
    }

    private SqlNetDate getList(int index)
    {
        lock (mLock1)
        {
            return mWaitList[index];
        }
    }

    private bool listIsEmpty()
    {
        lock (mLock1)
        {
            return mWaitList.Count == 0;
        }
    }

    private void threadRun()
    {
        while (true)
        {
     //       Debug.Log("======================================SQLNetManager threadRun command count");
            if (listIsEmpty())
            {
      //          Debug.Log("======================================SQLNetManager Sleep");
                Thread.Sleep(1000);
            }
            else
            {
                SqlNetDate command = getList(0);
                //         Debug.Log("======================================SQLNetManager command.action = "+ command.action);
                if (command.action == 1)
                {
                    changeInto(command.date);
                }
                else if (command.action == 2)
                {
                    delectInfo(command.date);
                }
                else if (command.action == 3)
                {
                    cleanLuihui();
                }
                else if (command.action == 4)
                {
                    cleanAll();
                }
                else if (command.action == 5)
                {
                    if (SQLHelper.getIntance().isUpdate != -1) {
                        updateToNet();
                    }                  
                }
                else if (command.action == 6)
                {
                    cleanAllNet();
                }
                else if (command.action == 7) {

                }
                removeList(command);
            }
        }
    }




    //打开数据库
    public void OpenSQLaAndConnect()
    {
        if (!File.Exists(SQLManager.getIntance().getSqlNetFilePath()))//创建新的数据库
        {
            creatLocl888Android();
        }
        this.connection = new SqliteConnection(SQLManager.getIntance().getSqlNetPath());
        this.connection.Open();
        Debug.Log("  打开数据库 结束 ");
    }

    private void creatLocl888Android() {
        this.connection = new SqliteConnection(SQLManager.getIntance().getSqlNetPath());
        this.connection.Open();
        Debug.Log("  创建数据库  appDBPath= " + SQLManager.getIntance().getSqlNetPath());
        Debug.Log("  创建数据库 ");
        this.CreateSQLTable(
            SQLManager.getIntance().getSqlTableName(),
            "CREATE TABLE " + SQLManager.getIntance().getSqlTableName() + "(" +
            "ACTION      INT ," +//1为更新，2为删除，3为轮回清空，4为全部清空
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
        Debug.Log("SQLNetManager ExecuteSQLCommand command  " + queryString);
        Debug.Log("SQLNetManager ExecuteSQLCommand connection  =" + connection);
        Debug.Log("SQLNetManager ExecuteSQLCommand connection  =" + connection.State);
        command = connection.CreateCommand();
        Debug.Log("SQLNetManager ExecuteSQLCommand  connection.CreateCommand()");
        this.command.CommandText = queryString;
        this.reader = this.command.ExecuteReader();
        Debug.Log("SQLNetManager ExecuteSQLCommand  reader = " + reader.ToString());
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

    private int mNetFault = 0;
    public bool isNet = true;
    public void updateToNet() {
       // Debug.Log("======================updateToNet!");
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("updateToNet 没有网络连接");
         //   mNetFault++;
            if (mNetFault >= 5 && isNet) {
                isNet = false;
                SQLDate date = new SQLDate();
                date.type = SQLHelper.TYPE_GAME;
                date.id = SQLHelper.GAME_ID_IS_NET;
                date.extan = "" + 2;
                date.goodId = -1;
                date.goodType = -1;
                date.isClean = SQLDate.CLEAR_NO;
                changeInto(date);
            }
            return;
        }
        if (NetServer.getIntance().isUpdate) {
            return;
        }
        readAllTable();
        NetServer.getIntance().updateNet(mUpAll);

    }
    List<SqlNetDate> mUpAll = new List<SqlNetDate>();
    Object mUpdateLock = new Object();

    public void updateDate( bool isSuccess) {
        lock (mUpdateLock) {
            if (mUpAll != null && mUpAll.Count > 0)
            {
                foreach (SqlNetDate data in mUpAll)
                {
                    string commandString = "DELETE FROM " + SQLManager.getIntance().getSqlTableName() + " WHERE TYPE=" + data.date.type +
                                                                      " AND ACTION=" + data.action +
                                                                      " AND ID=" + data.date.id +
                                                                      " AND EXTAN='" + data.date.extan + "'" +
                                                                      " AND GOODID=" + data.date.goodId +
                                                                      " AND GOODTYPE=" + data.date.goodType;
                    ExecuteSQLCommand(commandString);
                }
            }
            if (isSuccess)
            {
                SQLDate date = new SQLDate();
                date.type = SQLHelper.TYPE_GAME;
                date.id = SQLHelper.GAME_ID_IS_NET;
                date.extan = "" + 1;
                date.goodId = -1;
                date.goodType = -1;
                date.isClean = SQLDate.CLEAR_NO;
                changeInto(date);
                isNet = true;
                mNetFault = 0;
            }
            else
            {
                mNetFault++;
            }
        }
    }

    public void changeInto(SQLDate data)
    {
        //Debug.Log("======================================SQLNetManager changeInto ");
        if (data.type != SQLHelper.TYPE_GOOD)
        {
           // Debug.Log("======================================SQLNetManager data.type != SQLHelper.TYPE_GOOD ");
          //  string commPath = "SELECT TYPE=" + data.type + " AND ID=" + data.id + " from " + tabName;
            string commPath = "SELECT* FROM " + SQLManager.getIntance().getSqlTableName() + " WHERE  ID=" + data.id + " AND TYPE="+ data.type; //SELECT* FROM Persons WHERE firstname = 'Thomas' OR lastname = 'Carter'
            SqliteDataReader reader = ExecuteSQLCommand(commPath);
           // Debug.Log("======================================SQLNetManager ExecuteSQLCommand end");
          
            int count = 0;
            while (this.reader.Read()) {
           //     Debug.Log("======================================SQLNetManager this.reader.Read()");
                count++;
            }
         //   Debug.Log("======================================SQLNetManager reader.Read() end count = "+ count);
         //   Debug.Log(count);
            if (count < 1)
            {
                commPath = getInsertComm(data);
            }
            else
            {
                commPath = "UPDATE " + SQLManager.getIntance().getSqlTableName() + " SET EXTAN='" + data.extan+"'" + ", ACTION=1";
                commPath += " WHERE TYPE=" + data.type + " AND ID=" + data.id;
            }
         //   Debug.Log("======================================SQLNetManager ExecuteSQLCommand start");
            ExecuteSQLCommand(commPath);
          //  Debug.Log("======================================SQLNetManager ExecuteSQLCommand ");
        }
        else {
          //  Debug.Log("======================================SQLNetManager data.type == SQLHelper.TYPE_GOOD ");
        //    string commPath = "SELECT GOODID=" + data.goodId +" from " + tabName;
            string commPath = "SELECT* FROM " + SQLManager.getIntance().getSqlTableName() + " WHERE  GOODID=" + data.goodId;
           // Debug.Log("======================================SQLNetManager ExecuteSQLCommand start");
            SqliteDataReader reader = ExecuteSQLCommand(commPath);
           // Debug.Log("======================================SQLNetManager ExecuteSQLCommand ");
            int count = 0;
            while (this.reader.Read())
            {
            //    Debug.Log("======================================SQLNetManager this.reader.Read()");
                count++;
            }
            //Debug.Log("======================================SQLNetManager reader.Read() end count = " + count);
            if (count < 1)
            {
                commPath = getInsertComm(data);
            }
            else
            {
                commPath = "UPDATE " + SQLManager.getIntance().getSqlTableName() + " SET EXTAN='" + data.extan + "', ACTION=1"+", GOODTYPE="+data.goodType;
                commPath += " WHERE GOODID=" + data.goodId;
            }
       //     Debug.Log("======================================SQLNetManager ExecuteSQLCommand start");
            ExecuteSQLCommand(commPath);
         //   Debug.Log("======================================SQLNetManager ExecuteSQLCommand ");
        }
    }
    
    public void delectInfo(SQLDate data) {
        string commandString = "DELETE FROM " + SQLManager.getIntance().getSqlTableName() + " WHERE GOODID=" + data.goodId;
        ExecuteSQLCommand(commandString);
        commandString = "INSERT INTO " + SQLManager.getIntance().getSqlTableName() + " VALUES (2,-1,-1,'0'," + data.goodId + ",-1,1)";
        ExecuteSQLCommand(commandString);
    }

    public void cleanLuihui()
    {
        string commandString = "DELETE FROM " + SQLManager.getIntance().getSqlTableName() + " WHERE ISCLENAN=1";
        ExecuteSQLCommand(commandString);
        commandString = "INSERT INTO " + SQLManager.getIntance().getSqlTableName() + " VALUES (3,-1,-1,'0',-1,-1,1)";
        ExecuteSQLCommand(commandString);
    }
    public void cleanAll() {
        cleanAllLocal();
        cleanAllNet();
    }
    public void cleanAllLocal()
    {
        string commandString = "DELETE FROM  " + SQLManager.getIntance().getSqlTableName();
        ExecuteSQLCommand(commandString);
    }
    public void cleanAllNet() {
        string commandString = "INSERT INTO " + SQLManager.getIntance().getSqlTableName() + " VALUES (4,-1,-1,'0',-1,-1,2)";
        ExecuteSQLCommand(commandString);
    }

    private string getInsertComm(SQLDate data)
    {
        string commandString = "INSERT INTO " + SQLManager.getIntance().getSqlTableName() + " VALUES (1,";
        commandString +=  data.type;
        commandString += "," + data.id;
        commandString += "," + "'" + data.extan + "'";
        commandString += "," + data.goodId;
        commandString += "," + data.goodType;
        commandString += "," + data.isClean+")";
        return commandString;
    }

    public void readAllTable() {
        lock (mUpdateLock) {
            this.command = this.connection.CreateCommand();
            this.command.CommandText = "select * from " + SQLManager.getIntance().getSqlTableName();
            this.reader = this.command.ExecuteReader();
            mUpAll.Clear();
            mUpAll = new List<SqlNetDate>();
            while (this.reader.Read())
            {
                SqlNetDate net = new SqlNetDate();
                net.action = reader.GetInt64(reader.GetOrdinal("ACTION"));
                SQLDate date = new SQLDate();
                date.type = reader.GetInt64(reader.GetOrdinal("TYPE"));
                date.id = reader.GetInt64(reader.GetOrdinal("ID"));
                date.extan = reader.GetString(reader.GetOrdinal("EXTAN"));
                date.goodId = reader.GetInt64(reader.GetOrdinal("GOODID"));
                date.goodType = reader.GetInt64(reader.GetOrdinal("GOODTYPE"));
                date.isClean = reader.GetInt64(reader.GetOrdinal("ISCLENAN"));
                Debug.Log("SqlNetDate readAllTable" + date.toString());
                net.date = date;
                mUpAll.Add(net);
            }
        }
    }
    
    private SQLNetManager() {
        init();
    }
    private static SQLNetManager mIntance = new SQLNetManager();
    public static SQLNetManager getIntance()
    {
        return mIntance;
    }


    public void OnApplicationQuit()
    {
        //当程序退出时关闭数据库连接，不然会重复打开数据卡，造成卡顿
        this.CloseSQLConnection();
        Debug.Log("SQLNetManager 程序退出");
    }
}