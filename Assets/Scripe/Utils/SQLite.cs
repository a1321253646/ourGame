using UnityEngine;
using Mono.Data.Sqlite;

public class SQLite
{

    private SqliteConnection connection;
    private SqliteCommand command;


    public SQLite(string path)
    {
        connection = new SqliteConnection(path);    // 创建SQLite对象的同时，创建SqliteConnection对象
        Debug.Log("path = " + path);
        connection.Open();                          // 打开数据库链接
        Debug.Log("connection = " + connection);
        command = connection.CreateCommand();
      //  return command;

        //Command();
    }


    public SqliteCommand Command()
    {
        command = connection.CreateCommand();
        return command;
    }

    // 【增加数据】
    public SqliteDataReader InsertData(string table_name, string[] fieldNames, object[] values)
    {
        // 如果字段的个数，和数据的个数不相等，无法执行插入的语句，所以返回一个null
        if (fieldNames.Length != values.Length)
        {
            return null;
        }

        command.CommandText = "insert into " + table_name + "(";

        for (int i = 0; i < fieldNames.Length; i++)
        {
            command.CommandText += fieldNames[i];
            if (i < fieldNames.Length - 1)
            {
                command.CommandText += ",";
            }
        }

        command.CommandText += ")" + "values (";

        for (int i = 0; i < values.Length; i++)
        {
            command.CommandText += values[i];

            if (i < values.Length - 1)
            {
                command.CommandText += ",";
            }
        }

        command.CommandText += ")";

        Debug.Log(command.CommandText);

        return command.ExecuteReader();

    }


    // 【删除数据】
    public SqliteDataReader DeleteData(string table_name, string[] conditions)
    {
        command.CommandText = "delete from " + table_name + " where " + conditions[0];

        for (int i = 1; i < conditions.Length; i++)
        {
            // or：表示或者
            // and：表示并且
            command.CommandText += " or " + conditions[i];
        }

        return command.ExecuteReader();
    }

    // 【修改数据】

    public SqliteDataReader UpdateData(string table_name, string[] values, string[] conditions)
    {
        command.CommandText = "update " + table_name + " set " + values[0];

        for (int i = 1; i < values.Length; i++)
        {
            command.CommandText += "," + values[i];
        }

        command.CommandText += " where " + conditions[0];
        for (int i = 1; i < conditions.Length; i++)
        {
            command.CommandText += " or " + conditions[i];
        }

        return command.ExecuteReader();
    }

    // 【查询数据】
    public SqliteDataReader SelectData(string table_name, string[] fields)
    {
        command.CommandText = "select " + fields[0];
        for (int i = 1; i < fields.Length; i++)
        {
            command.CommandText += "," + fields[i];
        }
        command.CommandText += " from " + table_name;

        return command.ExecuteReader();
    }


    // 【查询整张表的数据】
    public SqliteDataReader SelectFullTableData(string table_name)
    {
        command.CommandText = "select * from " + table_name;
        return command.ExecuteReader();
    }


    // 【关闭数据库】
    public void CloseDataBase()
    {
        connection.Close();
        command.Cancel();
    }

}