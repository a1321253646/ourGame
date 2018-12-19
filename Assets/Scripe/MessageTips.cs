using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;

public class MessageTips : MonoBehaviour {

    public static int TYPPE_OUT_LINE = 1;
    public static int TYPPE_UPDATE_LINE = 2;


    Text mTitle;
    Text mDec;
    Text mCountNumber;
    Text mButtonDec, mTwoLeftDec, mTwoRightDec;
    Button mSure, mClose,mTwoLeft,mTwoRight;
    Image mCountImg;

    GameObject mCount, mSureOb, mCloseOb,mTwoOb;

//    private Vector2 mFri;
    private int mType;

    // Use this for initialization
    void initUi()
    {
        if(mCount != null) {
            return;    
        }
        mCount = GameObject.Find("message_tip_count");
        mTitle = GameObject.Find("message_tip_title").GetComponent<Text>();
        mDec = GameObject.Find("message_tip_des").GetComponent<Text>();
        mCountNumber = GameObject.Find("message_tip_count_number").GetComponent<Text>();
        mClose = GameObject.Find("message_tip_close").GetComponent<Button>();
        mCloseOb = GameObject.Find("message_tip_close");
        mSure = GameObject.Find("message_tip_sure").GetComponent<Button>();
        mSureOb = GameObject.Find("message_tip_sure");

        mTwoOb = GameObject.Find("twoButton");
        mTwoLeft = GameObject.Find("message_tip_two_left").GetComponent<Button>();
        mTwoRight = GameObject.Find("message_tip_two_right").GetComponent<Button>();
        mTwoLeftDec = GameObject.Find("message_tip_two_left_tx").GetComponent<Text>();
        mTwoRightDec = GameObject.Find("message_tip_two_right_tx").GetComponent<Text>();


        mButtonDec = GameObject.Find("message_tip_ButtonTx").GetComponent<Text>();
        mCountImg = GameObject.Find("message_tip_count_img").GetComponent<Image>();
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
        mTwoLeft.onClick.AddListener(() =>
        {
            if (mType == TYPPE_UPDATE_LINE)
            {
                isUpdate(true);
                removeUi(); 
            }
        });
        mTwoRight.onClick.AddListener(() =>
        {
            if (mType == TYPPE_UPDATE_LINE)
            {
                isUpdate(false);
                removeUi();
            }
        });
    }
    public void showUI(int type, string str1, string str2) {
        initUi();
        mType = type;
        mButtonDec.text = "确定";
        mDec.text = str1;
        if (type == TYPPE_OUT_LINE)
        {
            mTitle.text = "离线奖励";
            mCountNumber.text = str2;
        }
        else if (type == TYPPE_UPDATE_LINE)
        {
            mTitle.text = "同步提示";
            mDec.text = str1;
            mTwoLeftDec.text = "需要";
            mTwoRightDec.text = "不需要";

        }
        if (type == TYPPE_OUT_LINE)
        {
            mSureOb.transform.localScale = new Vector2(1, 1);
            mTwoOb.transform.localScale = new Vector2(0, 0);
            mCount.transform.localScale = new Vector2(1, 1);
            mCloseOb.transform.localScale = new Vector2(1, 1);
        }
        else if (type == TYPPE_UPDATE_LINE)
        {
            mTwoOb.transform.localScale = new Vector2(1, 1);
            mSureOb.transform.localScale = new Vector2(0, 0);
            mCount.transform.localScale = new Vector2(0, 0);
            mCloseOb.transform.localScale = new Vector2(0, 0);
        }

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
