using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;


public class OutputLog : MonoBehaviour
{
    public static OutputLog instance = null;
    private string err;
    private string log;
    private string filePath;
    private bool isDebugMode = false;

    void Awake()
    {
        filePath = "/storage/emulated/0/ourgame"+ TimeUtils.GetTimeStamp() + ".txt";
        if (System.IO.File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        instance = this;
        Application.logMessageReceived += HandleLog;
        Debug.Log(filePath);
    }

    void OnDestroy()
    {
        instance = null;
        Application.logMessageReceived -= HandleLog;
    }

    private List<string> writeTxt = new List<string>();
    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Error)
        {
            err += "\n";
            err += logString;
            err += "\n";
            err += stackTrace;
            writeTxt.Add("[Err]" + logString);
            writeTxt.Add(stackTrace);
        }
        else
        {
            log += "\n";
            log += logString;
            writeTxt.Add("[log]" + logString);
            writeTxt.Add(stackTrace);
        }
    }

    void Start()
    {
       // GameObject.Find("Cube").AddComponent<T1>();
    }

    void Update()
    {
        if (writeTxt.Count <= 0)
        {
            return;
        }
        string[] temp = writeTxt.ToArray();
        writeTxt.Clear();
        using (StreamWriter writer = new StreamWriter(filePath, true, Encoding.UTF8))
        {
            foreach (string t in temp)
            {
                writer.WriteLine(t);
            }
        }
    }
}