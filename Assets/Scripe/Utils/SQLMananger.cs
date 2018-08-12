using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;
using System.IO;

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
    public void Start()
    {
        this.CreateSQL();
        this.OpenSQLaAndConnect();
    }
    //创建数据库文件
    public void CreateSQL()
    {
        if (!File.Exists(Application.streamingAssetsPath + "/" + this.sqlName))
        {
            this.connection = new SqliteConnection("data source=" + Application.streamingAssetsPath + "/" + this.sqlName);
            this.connection.Open();
            this.CreateSQLTable(
                "用户",
                "CREATE TABLE 用户(" +
                "ID             INT ," +
                "Name           TEXT," +
                "Age            INT ," +
                "Score          INT," +
                "Time           INT)",
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
        this.connection = new SqliteConnection("data source=" + Application.streamingAssetsPath + "/" + this.sqlName);
        this.connection.Open();
    }
    /// <summary>
    ///执行SQL命令,并返回一个SqliteDataReader对象
    /// <param name="queryString"></param>
    public SqliteDataReader ExecuteSQLCommand(string queryString)
    {
        this.command = this.connection.CreateCommand();
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
        //string queryString = "CREATE TABLE " + tableName + "( " + columnNames[0] + " " + dataTypes[0];
        //for (int i = 1; i < columnNames.Length; i++)
        //{
        //    queryString += ", " + columnNames[i] + " " + dataTypes[i];
        //}
        //queryString += "  ) ";
        //  return ExecuteSQLCommand(queryString);

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
    //当退出应用程序的时候关闭数据库连接
    private void OnApplicationQuit()
    {
        //当程序退出时关闭数据库连接，不然会重复打开数据卡，造成卡顿
        this.CloseSQLConnection();
        Debug.Log("程序退出");
    }
}