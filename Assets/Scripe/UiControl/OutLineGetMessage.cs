using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;

public class OutLineGetMessage : UiControlBase {

    public static int TYPPE_OUT_LINE = 1;

    Text mCountNumber;
    Button mSure, mClose;

//    private Vector2 mFri;
    private int mType;

    // Use this for initialization
    public void showUI(int type, string str1, string str2) {
        toShowUi();
        mType = type;
        mCountNumber.text = str2;
        
    }

    public override void init()
    {
        mControlType = UiControlManager.TYPE_OUTGET;
        mCountNumber = GameObject.Find("message_tip_count_number").GetComponent<Text>();
        mClose = GameObject.Find("message_tip_close").GetComponent<Button>();
        mSure = GameObject.Find("message_tip_sure").GetComponent<Button>();
        //        mFri = gameObject.transform.localPosition;
        mSure.onClick.AddListener(() =>
        {
            if (mType == TYPPE_OUT_LINE)
            {
                toremoveUi();
            }

        });
        mClose.onClick.AddListener(() =>
        {
            toremoveUi();
        });
    }

    public override void show()
    {
        gameObject.transform.localPosition = new Vector2(0, 0);
    }
}
