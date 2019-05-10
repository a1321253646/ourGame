using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUiControl : UiControlBase
{

    // Use this for initialization
    Button mClose ,mVoiceClose, mVoicemOpen;
    Text mLocalIdText;
    
    public override void init()
    {
        mControlType = UiControlManager.TYPE_SETTING;
        mClose = GameObject.Find("setting_close").GetComponent<Button>();
        mLocalIdText = GameObject.Find("local_id").GetComponent<Text>();
        mClose.onClick.AddListener(() =>
        {
            toremoveUi();
        });
        mVoiceClose = GameObject.Find("close_voice_bt").GetComponent<Button>();
        mVoicemOpen = GameObject.Find("open_voice_bt").GetComponent<Button>();

        mVoiceClose.onClick.AddListener(() => {
            long isVoice = SQLHelper.getIntance().isVoice;
            AudioSource source = GameObject.Find("information").GetComponent<AudioSource>();
            GameManager.getIntance().setVoice(source, false, true);
            mVoiceClose.gameObject.transform.localScale = new Vector2(0, 0);
            mVoicemOpen.gameObject.transform.localScale = new Vector2(1, 1);
        });
        mVoicemOpen.onClick.AddListener(() => {
            long isVoice = SQLHelper.getIntance().isVoice;
            AudioSource source = GameObject.Find("information").GetComponent<AudioSource>();
            GameManager.getIntance().setVoice(source, true, true);
            mVoiceClose.gameObject.transform.localScale = new Vector2(1, 1);
            mVoicemOpen.gameObject.transform.localScale = new Vector2(0, 0);
        });
        updateLocalDateId();
        setTokenId();
        isInit = true;
    }

    public void updateLocalDateId() {
        if (string.IsNullOrEmpty(SQLHelper.getIntance().mToken)) {
            mLocalIdText.text = SQLHelper.getIntance().mToken;
        }
        else {
            mLocalIdText.text ="等待获取";
        }
       
    }

    public override void show()
    {
        gameObject.transform.localPosition = new Vector2(0, 0);
        long isVoice1 = SQLHelper.getIntance().isVoice;
        if (isVoice1 == -1 || isVoice1 == 1)
        {
            mVoiceClose.gameObject.transform.localScale = new Vector2(1, 1);
            mVoicemOpen.gameObject.transform.localScale = new Vector2(0, 0);
        }
        else
        {
            mVoiceClose.gameObject.transform.localScale = new Vector2(0, 0);
            mVoicemOpen.gameObject.transform.localScale = new Vector2(1, 1);
        }
        setTokenId();
    }

    public void setTokenId() {
        if (mLocalIdText == null) {
            mLocalIdText = GameObject.Find("local_id").GetComponent<Text>();
        }
        Debug.Log("setTokenId mToken = " + SQLHelper.getIntance().mToken);
        mLocalIdText.text = "编码:"+SQLHelper.getIntance().mToken;
    }
}
