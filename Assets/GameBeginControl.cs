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
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }


    private int isUpdate = 0;
    private bool isUpdateEnd = false;
    private bool isGetlocalBegin = false;
    private bool isGetlocaled= false;
    private float mGetLcalTime = 0;

    Text mLoadingDec;
    void Update () {
        Debug.Log(" GameManager.getIntance().mInitStatus = " + GameManager.getIntance().mInitStatus);
        if (mGetLcalTime > 0) {
            Debug.Log("mGetLcalTime  = " + mGetLcalTime+" isGetlocalBegin="+ isGetlocalBegin+ " isGetlocaled="+ isGetlocaled);
        }
         
        if (isGetlocalBegin && !isGetlocaled) {
            mGetLcalTime += Time.deltaTime;
            if (mGetLcalTime >= 10) {
                isGetlocaled = true;
                GameManager.getIntance().mInitStatus = 8;
            }
            return;
        }
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
            Debug.Log(" GameManager.getIntance().mInitStatus = " + GameManager.getIntance().mInitStatus);
            if (SQLHelper.getIntance().isFristStartGame == -1)
            {
                SQLHelper.getIntance().updateVersionCode(GameManager.mVersionCode);
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
            Debug.Log(" GameManager.getIntance().mInitStatus = " + GameManager.getIntance().mInitStatus);
#if UNITY_ANDROID || UNITY_IOS
            SQLManager.getIntance().initPathRoot();
            Thread th1 = new Thread(() =>
            {
                JsonUtils.getIntance().initBefore();
                JsonUtils.getIntance().init();
                GameManager.getIntance().mInitStatus = 2;
            });
            th1.Start();
#endif
#if UNITY_STANDALONE
            SQLManager.getIntance().init(sqlName, tabName);
            //  GameManager.getIntance().mInitStatus = 8;
            Debug.Log(" GameManager.getIntance().mInitStatus = " + GameManager.getIntance().mInitStatus);
            JsonUtils.getIntance().initBefore();
            JsonUtils.getIntance().init();

            GameManager.getIntance().mInitStatus = 2;
#endif
        }
        else if (GameManager.getIntance().mInitStatus == 2)
        {
            GameManager.getIntance().mInitDec = JsonUtils.getIntance().getStringById(100025);
            GameManager.getIntance().mInitStatus = 3;
            Debug.Log(" GameManager.getIntance().mInitStatus = " + GameManager.getIntance().mInitStatus);
#if UNITY_ANDROID || UNITY_IOS

            Thread th1 = new Thread(() =>
            {
                try
                {

                    isUpdate = SQLManager.getIntance().initNoNet();
                    Debug.Log("isUpdate = " + isUpdate);
                    SQLManager.getIntance().startThread();
                    if (isUpdate == 1)
                    {
                        GameManager.getIntance().mInitStatus = 4;
                        Debug.Log(" GameManager.getIntance().mInitStatus = " + GameManager.getIntance().mInitStatus);
                    }
                    else if (isUpdate == 2)
                    {
                        SQLManager.getIntance().copyToNet();
                        GameManager.getIntance().mInitStatus = 6;
                        Debug.Log(" GameManager.getIntance().mInitStatus = " + GameManager.getIntance().mInitStatus);
                    }
                    else if (isUpdate == 3)
                    {
                        GameManager.getIntance().mInitStatus = 6;
                        Debug.Log(" GameManager.getIntance().mInitStatus = " + GameManager.getIntance().mInitStatus);
                    }
                   
                }
                catch (System.Exception e)
                {
                    Debug.Log("处理存档出错");
                    Debug.Log(e.Message);

                }

            });
            th1.Start();
#endif
#if UNITY_STANDALONE
            GameManager.getIntance().mInitStatus = 8;
            Debug.Log(" GameManager.getIntance().mInitStatus = " + GameManager.getIntance().mInitStatus);
#endif

        }
        else if (GameManager.getIntance().mInitStatus == 4)//强制轮回
        {
            GameManager.getIntance().mInitDec = JsonUtils.getIntance().getStringById(100029);
            GameManager.getIntance().mInitStatus = 5;
            Debug.Log(" GameManager.getIntance().mInitStatus = " + GameManager.getIntance().mInitStatus);
            GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showUi(JsonUtils.getIntance().getStringById(100002),
                LuiHuiTips.TYPPE_RETURN_START);
            GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showSelf();
        }
        else if (GameManager.getIntance().mInitStatus == 6)//和网络同步数据
        {
            GameManager.getIntance().mInitStatus = 7;
            Debug.Log(" GameManager.getIntance().mInitStatus = " + GameManager.getIntance().mInitStatus);
            Debug.Log(" SQLManager.getIntance().isUpdateed()  = " + SQLManager.getIntance().isUpdateed());
            Debug.Log(" Application.internetReachability == NetworkReachability.NotReachable  = " + (Application.internetReachability == NetworkReachability.NotReachable));
            if (!SQLManager.getIntance().isUpdateed() || Application.internetReachability == NetworkReachability.NotReachable || GameManager.isTestVersion) {
                GameManager.getIntance().mInitStatus = 8;
                Debug.Log(" GameManager.getIntance().mInitStatus = " + GameManager.getIntance().mInitStatus);
                return;
            }

            

            if (!isGetlocalBegin && !isGetlocaled) {
                isGetlocalBegin = true;
                Thread th1 = new Thread(() =>
                {
                    if (!NetServer.getIntance().getLocl(null,false,false))
                    {
                        GameManager.getIntance().mInitStatus = 8;
                        isGetlocaled = true;
                        Debug.Log(" GameManager.getIntance().mInitStatus = " + GameManager.getIntance().mInitStatus);
                    }
                    else {
                        GameManager.getIntance().mInitStatus = 6;
                         isGetlocaled = true;
                        Debug.Log(" GameManager.getIntance().mInitStatus = " + GameManager.getIntance().mInitStatus);
                    }
                    
                   
                    
                });
                th1.Start();
                return;
            }
            isUpdateEnd = true;
            bool isHaveNet = NetServer.getIntance().isHaveLocal();
            Debug.Log("NetServer.getIntance().getLocal() isUpdate ="+ isUpdate);
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
                        Debug.Log(" GameManager.getIntance().mInitStatus = " + GameManager.getIntance().mInitStatus);
                    });
                    th1.Start();
                }
                else if(isUpdate == 2 || isUpdate == 1) {
                    GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showUi("检测到服务端有您的存档，需要进行同步吗", LuiHuiTips.TYPPE_UPDATE_LINE);
                    GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showSelf();
                    //UiControlManager.getIntance().show(UiControlManager.TYPE_LUIHUI);
                }
            }
            else {
                GameManager.getIntance().mInitStatus = 8;
                Debug.Log(" GameManager.getIntance().mInitStatus = " + GameManager.getIntance().mInitStatus);
            }
        }
        else if (GameManager.getIntance().mInitStatus == 8)
        {
            GameManager.getIntance().mInitStatus = 9;
            Debug.Log(" GameManager.getIntance().mInitStatus = " + GameManager.getIntance().mInitStatus);
            GameManager.getIntance().mInitDec = JsonUtils.getIntance().getStringById(100030);
#if UNITY_ANDROID || UNITY_IOS
            Thread th1 = new Thread(() =>
            {
                if (isUpdateEnd)
                {
                    Debug.Log(" SQLHelper.getIntance().updateIsUpdate(); ");
                    SQLHelper.getIntance().updateIsUpdate();
                    Debug.Log(" SQLHelper.getIntance().updateIsUpdate(); end");
                }
                Debug.Log("SQLHelper.getIntance().init();");
                SQLHelper.getIntance().init();

                int count = 0;
                if (SQLHelper.getIntance().mVocationCount == 0) {
                    for (int i = 0; ; i++)
                    {
                        if (SQLHelper.getIntance().mPlayVocation.ContainsKey(i))
                        {
                            count++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (count != 0)
                    {
                        SQLHelper.getIntance().updateVocationCount(count);
                    }
                }

                Debug.Log("SQLHelper.getIntance().init(); end");
                GameManager.getIntance().mInitStatus = 10;
                Debug.Log(" GameManager.getIntance().mInitStatus = " + GameManager.getIntance().mInitStatus);

            });
            th1.Start();
#endif

#if UNITY_STANDALONE
            SQLHelper.getIntance().init();
            int count = 0;
            if (SQLHelper.getIntance().mVocationCount == 0)
            {
                for (int i = 0; ; i++)
                {
                    if (SQLHelper.getIntance().mPlayVocation.ContainsKey(i))
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }
                if (count != 0)
                {
                    SQLHelper.getIntance().updateVocationCount(count);
                }
            }
            GameManager.getIntance().mInitStatus = 10;
            Debug.Log(" GameManager.getIntance().mInitStatus = " + GameManager.getIntance().mInitStatus);
#endif

        }
    }
    public void init() {
        if (GameManager.getIntance().mInitStatus == -1) {
            GameManager.getIntance().mInitStatus = 0;
        }      
    }

}
