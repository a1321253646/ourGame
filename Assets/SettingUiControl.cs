using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUiControl : MonoBehaviour {

    // Use this for initialization
    Button mClose ,mVoiceClose, mVoicemOpen;
	void Start () {
        mFri = gameObject.transform.localPosition;
        mClose = GameObject.Find("setting_close").GetComponent<Button>();
        mClose.onClick.AddListener(() =>
        {
            removeUi();
        });
        mVoiceClose = GameObject.Find("close_voice_bt").GetComponent<Button>();
        mVoicemOpen = GameObject.Find("open_voice_bt").GetComponent<Button>();

        mVoiceClose.onClick.AddListener(() => {
            long isVoice =  SQLHelper.getIntance().isVoice;
            AudioSource source = GameObject.Find("information").GetComponent<AudioSource>();
            GameManager.getIntance().setVoice(source, false, true);
            mVoiceClose.gameObject.transform.localScale = new Vector2(0, 0);
            mVoicemOpen.gameObject.transform.localScale = new Vector2(1, 1);
        });
        mVoicemOpen.onClick.AddListener(() => {
            long isVoice =  SQLHelper.getIntance().isVoice;
            AudioSource source = GameObject.Find("information").GetComponent<AudioSource>();
            GameManager.getIntance().setVoice(source, true, true);
            mVoiceClose.gameObject.transform.localScale = new Vector2(1, 1);
            mVoicemOpen.gameObject.transform.localScale = new Vector2(0, 0);
        });

    }
    int mLevel = 0;
    bool isShow = false;
    Vector2 mFri;
    public void click()
    {
        if (isShow)
        {
            int level = GameManager.getIntance().getUiCurrentLevel();
            if (mLevel < level)
            {
                showUi();
                return;
            }
            else if (mLevel == level)
            {
                removeUi();
            }
        }
        else
        {
            showUi();
        }
    }
    public void removeUi()
    {
        isShow = false;
        // gameObject.transform.TransformPoint(new Vector2(-607, -31));
        gameObject.transform.localPosition = mFri;
    }
    private void showUi()
    {
        isShow = true;
        gameObject.transform.localPosition = new Vector2(0, 0);
        mLevel = GameManager.getIntance().getUiLevel();
        gameObject.transform.SetSiblingIndex(mLevel);
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
    }
}
