using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;

public class OutLineGetMessage : MonoBehaviour {

    public static int TYPPE_OUT_LINE = 1;
    public static int TYPPE_UPDATE_LINE = 2;

    Text mCountNumber;
    Button mSure, mClose;

//    private Vector2 mFri;
    private int mType;

    // Use this for initialization
    void initUi()
    {
        if(mCountNumber != null) {
            return;    
        }

        mCountNumber = GameObject.Find("message_tip_count_number").GetComponent<Text>();
        mClose = GameObject.Find("message_tip_close").GetComponent<Button>();
        mSure = GameObject.Find("message_tip_sure").GetComponent<Button>();
//        mFri = gameObject.transform.localPosition;
        mSure.onClick.AddListener(() =>
        {
            if (mType == TYPPE_OUT_LINE)
            {
                removeUi();
            }
            
        });
        mClose.onClick.AddListener(() =>
        {
            removeUi();
        });
    }
    public void showUI(int type, string str1, string str2) {
        initUi();
        mType = type;
        mCountNumber.text = str2;

        gameObject.transform.localPosition = new Vector2(0, 0);
        int level = GameManager.getIntance().getUiLevel();
        gameObject.transform.SetSiblingIndex(level);
    }

    public void isUpdate(bool isNeed) {
        if (isNeed)
        {
            Thread th1 = new Thread(() =>
            {
                string local = NetServer.getIntance().getLocal();
                Debug.Log("local == " + local);
                SQLManager.getIntance().saveLocal(local);
                Debug.Log("local save end " );
                GameManager.getIntance().mInitStatus = 8;
            });
            th1.Start();
        }
        else {
            Thread th1 = new Thread(() =>
            {
                NetServer.getIntance().clearAllNet();
                GameManager.getIntance().mInitStatus = 8;
            });
            th1.Start();
        }
    }

    public void removeUi()
    {
        gameObject.transform.localPosition = new Vector2(-982, -696);
        
    }
    private void sure() {

    }
}
