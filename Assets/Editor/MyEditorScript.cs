using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.Build.Reporting;

public class MyEditorScript : Editor
{
    [MenuItem("Tool/BuildApk")]
    static void Build()
    {
        //  TextAsset textAsset = LoadXML.Instance.LoadTextAsset("Assets/Channel/channel.xml");

        // 读取 channel.xml 将每一行的数据赋值给 一个 BuildConfig 对象，添加到字典中
        // 最终将 BuildConfig 对象字典返回
        // Dictionary<object, BuildConfig> buildConfigDic = LoadXML.Instance.GetTableData<BuildConfig>("channel", textAsset.text);

        var arrdata = Newtonsoft.Json.Linq.JArray.Parse(readFile());
        List<BuildConfig> list = arrdata.ToObject<List<BuildConfig>>();

        // 遍历字典
        foreach (BuildConfig kv in list)
        {
            // 获取每个渠道的配置
            BuildConfig _buildConfig = kv;


            // 执行打包
            Build(_buildConfig);
        }
    }

    private static string readFile()
    {
        //TextAsset jsonText = Resources.Load(fileName) as TextAsset;
        string str = null;
#if UNITY_ANDROID || UNITY_IOS
        str = Resources.Load<TextAsset>("company").text;
#endif
#if UNITY_STANDALONE
        str = loadFile(Application.dataPath + "/Resources", "company.json");
#endif
        //string str = loadFile(Application.dataPath + "/Resources", fileName);
        //string str = Resources.Load<TextAsset>(  fileName).text;
        Debug.Log("readFile :\n " + str);
        return str;
    }

    private static void Build(BuildConfig buildConfig)
    {
        // 产品名
        Debug.Log("开始打包 : " + buildConfig.apkName);
        PlayerSettings.productName = buildConfig.productName;
        // 包名
        PlayerSettings.applicationIdentifier = buildConfig.bundleIdentifier;

        // 
        PlayerSettings.bundleVersion = buildConfig.bundleVersion + "." + buildConfig.id;
        PlayerSettings.Android.bundleVersionCode = buildConfig.bundleVersionCode;
        // keystore 路径, G:\keystore\one.keystore
        PlayerSettings.Android.keystoreName = buildConfig.keystorePath;
        // one.keystore 密码
        PlayerSettings.Android.keystorePass = buildConfig.keystorePass;

        // one.keystore 别名
        PlayerSettings.Android.keyaliasName = buildConfig.keyaliasName;
        // 别名密码
        PlayerSettings.Android.keyaliasPass = buildConfig.keyaliasPass;

        // Load 应用图标，暂时放 Resource 下了
        Texture2D icon = Resources.Load(buildConfig.defaultIcon) as Texture2D;
        // 设置应用图标
        PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Android, new Texture2D[] { icon, icon, icon, icon, icon, icon });

        BuildTargetGroup buildTargetGroup = BuildTargetGroup.Android;
        // 设置宏定义
        PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, buildConfig.scriptingDefine); // 宏定义

        List<string> levels = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            // 获取有效的 Scene
            levels.Add(scene.path);
        }
        
        BuildTarget buildTarget = BuildTarget.Android;
        // 切换到 Android 平台
        EditorUserBuildSettings.SwitchActiveBuildTarget(buildTarget);

        // 打包出 APK 名
        string apkName = string.Format("{0}.apk", buildConfig.apkName);
        // 执行打包
        BuildReport res = BuildPipeline.BuildPlayer(levels.ToArray(), apkName, buildTarget, BuildOptions.None);

        Debug.Log("开始打包 : " + res);

     //   if (res.Length > 0)
      //  {
         //   throw new Exception("BuildPlayer failure: " + res);

      //  }

        AssetDatabase.Refresh();
    }
}