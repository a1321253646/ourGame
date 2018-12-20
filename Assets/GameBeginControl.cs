using System.Collections;
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

    void Update () {
        Debug.Log(" GameManager.getIntance().mInitStatus = "+ GameManager.getIntance().mInitStatus);
        if (GameManager.getIntance().mInitStatus == -1)
        {
            return;
        }
        else if (GameManager.getIntance().mInitStatus == 10)
        {
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
            else {
                JsonUtils.getIntance().initBefore();
                JsonUtils.getIntance().init();
                GameManager.getIntance().mInitStatus = 2;
            }
        }
        else if (GameManager.getIntance().mInitStatus == 2)
        {
            GameManager.getIntance().mInitStatus = 3;
            if (GameManager.isAndroid)
            {
                Thread th1 = new Thread(() =>
                {
                    isUpdate = SQLManager.getIntance().init(sqlName, tabName);
                    if (isUpdate != 0 && Application.internetReachability != NetworkReachability.NotReachable)
                    {
                        GameManager.getIntance().mInitStatus = 4;
                    }
                    else
                    {
                        GameManager.getIntance().mInitStatus = 8;
                    }
                });
                th1.Start();
            }
            else {
                SQLManager.getIntance().init(sqlName, tabName);
                GameManager.getIntance().mInitStatus = 8;
            }

        }
        else if (GameManager.getIntance().mInitStatus == 4)
        {
            GameManager.getIntance().mInitStatus = 5;
            Thread th1 = new Thread(() =>
            {
               
                NetServer.getIntance().getLocl();
                GameManager.getIntance().mInitStatus = 6;
            });
            th1.Start();
        }
        else if (GameManager.getIntance().mInitStatus == 6)
        {
            Debug.Log("in GameManager.getIntance().mInitStatus " );
            if (!NetServer.getIntance().isHaveLocal())
            {
                Debug.Log("!NetServer.getIntance().isHaveLocal() ");
                GameManager.getIntance().mInitStatus = 8;
                isUpdateEnd = true;
                return;
            }
            Debug.Log("!NetServer.getIntance().isHaveLocal() ");
            GameManager.getIntance().mInitStatus = 7;
            if (isUpdate == 2)
            {
                GameObject.Find("message_tip").GetComponent<OutLineGetMessage>().showUI(OutLineGetMessage.TYPPE_UPDATE_LINE, "检测到服务端有您的存档，需要进行同步吗", null);
            }
            else {
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
           
            isUpdateEnd = true;
        }
        else if (GameManager.getIntance().mInitStatus == 8)
        {
            GameManager.getIntance().mInitStatus = 9;
            if (GameManager.isAndroid)
            {
                Thread th1 = new Thread(() =>
                {
                    Debug.Log("SQLHelper.getIntance().init();");
                    SQLHelper.getIntance().init();
                    Debug.Log("SQLHelper.getIntance().init(); end");
                    if (isUpdateEnd)
                    {
                        Debug.Log(" SQLHelper.getIntance().updateIsUpdate(); ");
                        SQLHelper.getIntance().updateIsUpdate();
                        Debug.Log(" SQLHelper.getIntance().updateIsUpdate(); end");
                    }
                    GameManager.getIntance().mInitStatus = 10;
                });
                th1.Start();
            }
            else {
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
