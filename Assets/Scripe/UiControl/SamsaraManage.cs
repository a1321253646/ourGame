﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SamsaraManage : UiControlBase
{
    private Button mClose,mLunhuiClick;
    private SamSaraListControl mListControl;
    private Text mLunhuiValue, mLunhuiTx;
    public void updata() {
        Debug.Log("============================轮回点获得= " + GameManager.getIntance().mReincarnation.toString());
        mLunhuiValue.text = "" + GameManager.getIntance().mReincarnation.toStringWithUnit();
    }

    public override void init()
    {
        mControlType = UiControlManager.TYPE_SAMSARA;
        mLunhuiClick = GameObject.Find("lunhui_show_tip").GetComponent<Button>();
        mLunhuiValue = GameObject.Find("lunhui_show_tip_tx").GetComponent<Text>();
        mLunhuiTx = GameObject.Find("lunhui_show_tip_tx2").GetComponent<Text>();
        mLunhuiClick.onClick.AddListener(() =>
        {
            UiControlManager.getIntance().show(UiControlManager.TYPE_LUIHUI);
        });
        mClose = GameObject.Find("lunhui_close").GetComponent<Button>();
        mClose.onClick.AddListener(() => {
            toremoveUi();
        });
        mListControl = GameObject.Find("lunhui_skill_list").GetComponent<SamSaraListControl>();
        mListControl.init();
        mLunhuiClick = GameObject.Find("lunhui_show_tip").GetComponent<Button>();
        mLunhuiTx = GameObject.Find("lunhui_show_tip_tx2").GetComponent<Text>();
    }

    public override void show()
    {
        float mix = JsonUtils.getIntance().getConfigValueForId(100017);
        if (BaseDateHelper.decodeLong(GameManager.getIntance().mCurrentLevel) >= mix)
        {
            mLunhuiClick.interactable = true;
            mLunhuiTx.text = "";
        }
        else
        {
            mLunhuiClick.interactable = false;
            mLunhuiTx.text = "第" + mix + "关开启轮回功能";
        }
        mLunhuiValue.text = GameManager.getIntance().mReincarnation.toStringWithUnit();
        mListControl.isEnableLavelUp();
    }
}
