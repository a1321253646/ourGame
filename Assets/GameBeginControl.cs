﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;
public class GameBeginControl : MonoBehaviour {

    public string sqlName;
    public string tabName;

    // Use this for initialization
    void Start () {

      /*  float width = Screen.width;
        float height = Screen.height;
        Debug.Log("width = " + width + " height=" + height);
        if (16 * width > 9 * height)
        {
            GameObject.Find("Canvas").GetComponent<CanvasScaler>().matchWidthOrHeight = 1;
        }
        else if (16 * width < 9 * height){
            GameObject.Find("Canvas").GetComponent<CanvasScaler>().matchWidthOrHeight = 0;
        }*/
        gameObject.transform.SetSiblingIndex(GameManager.getIntance().getUiLevel());

     //   Resolution[] resolutions = Screen.resolutions;
        //设置当前分辨率  
     //   Screen.SetResolution(resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height, true);

       // Screen.fullScreen = true;

    }

    // Update is called once per frame


    private int isUpdate = 0;
    private bool isUpdateEnd = false;
    Text mLoadingDec;
    void Update () {
        Debug.Log(" GameManager.getIntance().mInitStatus = "+ GameManager.getIntance().mInitStatus);
        if (mLoadingDec == null)
        {
            mLoadingDec = GameObject.Find("game_begin_loading_des").GetComponent<Text>();
        }
        if (GameManager.getIntance().mInitStatus < 11) {
            mLoadingDec.text = GameManager.getIntance().mInitDec;
        }     
        if (GameManager.getIntance().mInitStatus == -1)
        {
            return;
        }
        else if (GameManager.getIntance().mInitStatus == 10)
        {
            GameManager.getIntance().mInitDec = "";
            mLoadingDec.text = GameManager.getIntance().mInitDec;
            GameManager.getIntance().mInitStatus = 11;
            if (SQLHelper.getIntance().isFristStartGame == -1)
            {
                Destroy(GameObject.Find("game_begin_loading"));
                GetComponentInChildren<TypewriterEffect>().init(JsonUtils.getIntance().getStringById(100001));

            }
            else
            {
                AudioSource source = GameObject.Find("information").GetComponent<AudioSource>();
                GameManager.getIntance().playBgm(source, "Sounds/关卡背景音");
                Destroy(gameObject);
                GameObject.Find("Manager").GetComponent<LevelManager>().init();
            }
        }
        else if (GameManager.getIntance().mInitStatus == 0)
        {
            GameManager.getIntance().mInitDec = "开始配置文件初始化";
            GameManager.getIntance().mInitStatus = 1;
            if (GameManager.isAndroid)
            {
                Thread th1 = new Thread(() =>
                {
                    JsonUtils.getIntance().initBefore();
                    JsonUtils.getIntance().init();
                    GameManager.getIntance().mInitStatus = 2;
                });
                th1.Start();
            }
            else
            {
                JsonUtils.getIntance().initBefore();
                JsonUtils.getIntance().init();
                GameManager.getIntance().mInitStatus = 2;
            }
        }
        else if (GameManager.getIntance().mInitStatus == 2)
        {
            GameManager.getIntance().mInitDec = JsonUtils.getIntance().getStringById(100025);
            GameManager.getIntance().mInitStatus = 3;
            if (GameManager.isAndroid)
            {
                Thread th1 = new Thread(() =>
                {
                    isUpdate = SQLManager.getIntance().initNoNet();
                    SQLManager.getIntance().startThread();
                    if (isUpdate == 1)
                    {
                        NetServer.getIntance().getLocl();
                        GameManager.getIntance().mInitStatus = 4;
                    }
                    else if (isUpdate == 2)
                    {
                        if (SQLManager.getIntance().isUpdateed())
                        {
                            SQLManager.getIntance().copyToNet();
                            if (Application.internetReachability != NetworkReachability.NotReachable)
                            {
                                NetServer.getIntance().getLocl();
                                GameManager.getIntance().mInitStatus = 6;
                            }
                            else
                            {
                                GameManager.getIntance().mInitStatus = 8;
                            }


                            isUpdate = 4;
                            GameManager.getIntance().mInitStatus = 6;
                        }
                        else {
                            

                        }

                        
                    }
                    else if (isUpdate == 3) {
                        if (Application.internetReachability != NetworkReachability.NotReachable)
                        {
                            NetServer.getIntance().getLocl();
                            GameManager.getIntance().mInitStatus = 6;
                        }
                        else {
                            GameManager.getIntance().mInitStatus = 8;
                        }                        
                    }
                });
                th1.Start();
            }
            else
            {
                SQLManager.getIntance().init(sqlName, tabName);
                GameManager.getIntance().mInitStatus = 8;
            }

        }
        else if (GameManager.getIntance().mInitStatus == 4)//强制轮回
        {
            GameManager.getIntance().mInitDec = JsonUtils.getIntance().getStringById(100029);
            GameManager.getIntance().mInitStatus = 5;
            GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showUi(JsonUtils.getIntance().getStringById(100002),
                LuiHuiTips.TYPPE_RETURN_START);
        }
        else if (GameManager.getIntance().mInitStatus == 6)//和网络同步数据
        {
            GameManager.getIntance().mInitStatus = 7;
            
            Debug.Log("in GameManager.getIntance().mInitStatus ");
            if (isUpdate == 4) {
                GameManager.getIntance().mInitStatus = 8;
                return;
            }
            isUpdateEnd = true;
            bool isHaveNet = NetServer.getIntance().isHaveLocal();
            if (isHaveNet)
            {
                if (isUpdate == 3)
                {
                    Thread th1 = new Thread(() =>
                    {
                        Debug.Log("NetServer.getIntance().getLocal();");
                        string local = NetServer.getIntance().getLocal();
                        Debug.Log(" NetServer.getIntance().getLocal()");
                        SQLManager.getIntance().saveLocal(local);
                        Debug.Log(" SQLHelper.getIntance().saveLoacl(local);");
                        GameManager.getIntance().mInitStatus = 8;
                    });
                    th1.Start();
                }
                else if(isUpdate == 2) {

                }
                else
                {
                    GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showUi("检测到服务端有您的存档，需要进行同步吗", LuiHuiTips.TYPPE_UPDATE_LINE);
                }
            }
            else {
                GameManager.getIntance().mInitStatus = 8;
            }
        }
        else if (GameManager.getIntance().mInitStatus == 8)
        {
            GameManager.getIntance().mInitStatus = 9;
            GameManager.getIntance().mInitDec = JsonUtils.getIntance().getStringById(100030);
            if (GameManager.isAndroid)
            {
                Thread th1 = new Thread(() =>
                {
                    if (isUpdateEnd && isUpdate != 4)
                    {
                        Debug.Log(" SQLHelper.getIntance().updateIsUpdate(); ");
                        SQLHelper.getIntance().updateIsUpdate();
                        Debug.Log(" SQLHelper.getIntance().updateIsUpdate(); end");
                    }
                    Debug.Log("SQLHelper.getIntance().init();");
                    SQLHelper.getIntance().init();
                    Debug.Log("SQLHelper.getIntance().init(); end");
                    GameManager.getIntance().mInitStatus = 10;
                });
                th1.Start();
            }
            else
            {
                SQLHelper.getIntance().init();
                GameManager.getIntance().mInitStatus = 10;
            }

        }
    }
    public void init() {
        if (GameManager.getIntance().mInitStatus == -1) {
            GameManager.getIntance().mInitStatus = 0;
        }      
    }

}
