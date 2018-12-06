using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MessageTips : MonoBehaviour {

    public static int TYPPE_OUT_LINE = 1;


    Text mTitle;
    Text mDec;
    Text mCountNumber;
    Text mButtonDec;
    Button mSure, mClose;
    Image mCountImg;

    GameObject mCount;

//    private Vector2 mFri;
    private int mType;

    // Use this for initialization
    void Start()
    {
        mCount = GameObject.Find("message_tip_count");
        mTitle = GameObject.Find("message_tip_title").GetComponent<Text>();
        mDec = GameObject.Find("message_tip_des").GetComponent<Text>();
        mCountNumber = GameObject.Find("message_tip_count_number").GetComponent<Text>();
        mClose = GameObject.Find("message_tip_close").GetComponent<Button>();
        mSure = GameObject.Find("message_tip_sure").GetComponent<Button>();
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
    }

    public void showUI(int type, string str1, string str2) {
        
        mType = type;
        if (mButtonDec == null) {
            mButtonDec = GameObject.Find("message_tip_ButtonTx").GetComponent<Text>();
 //           mFri = gameObject.transform.localPosition;
        }
        if (mCountNumber == null)
        {
            mCountNumber = GameObject.Find("message_tip_count_number").GetComponent<Text>();
        }
        if (mDec == null)
        {
            mDec = GameObject.Find("message_tip_des").GetComponent<Text>();
        }
        if (mTitle == null)
        {
            mTitle = GameObject.Find("message_tip_title").GetComponent<Text>();
        }
        if (mCountImg == null)
        {
            mCountImg = GameObject.Find("message_tip_count_img").GetComponent<Image>();
        }
        if (mSure == null)
        {
            mSure = GameObject.Find("message_tip_sure").GetComponent<Button>();
            mSure.onClick.AddListener(() =>
            {
                Debug.Log(" message_tip_sure click");
                if (mType == TYPPE_OUT_LINE)
                {
                    removeUi();
                }

            });
           
        }
        
        if (mClose == null)
        {
            mClose = GameObject.Find("message_tip_close").GetComponent<Button>();
            mClose.onClick.AddListener(() =>
            {
                Debug.Log(" message_tip_close click");
                removeUi();
            });
        }
        mButtonDec.text = "确定";
        mDec.text = str1;
        if (type == TYPPE_OUT_LINE)
        {
            mTitle.text = "离线奖励";
            mCountNumber.text = str2;
        }
        else {

            Destroy(mCount);
        }
        gameObject.transform.localPosition = new Vector2(0, 0);
        int level = GameManager.getIntance().getUiLevel();
        gameObject.transform.SetSiblingIndex(level);
    }

    public void removeUi()
    {
        gameObject.transform.localPosition = new Vector2(-982, -696);
        
    }
    private void sure() {

    }
}
