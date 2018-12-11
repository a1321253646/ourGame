using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SamsaraManage : MonoBehaviour {
    private int mLevel;
    public bool isShow = false;
    private Button mClose,mLunhuiClick;
    private SamSaraListControl mListControl;
    private Vector2 mFri;
    private Text mLunhuiValue, mLunhuiTx;

    private void Start()
    {
        mFri = gameObject.transform.localPosition;
        mLunhuiClick = GameObject.Find("lunhui_show_tip").GetComponent<Button>();
        mLunhuiValue = GameObject.Find("lunhui_show_tip_tx").GetComponent<Text>();
        mLunhuiTx = GameObject.Find("lunhui_show_tip_tx2").GetComponent<Text>();
        mLunhuiClick.onClick.AddListener(() =>
        {
            BackpackManager.getIntance().showLunhuiTipUi();
        });
    }

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
    private void showUi()
    {
        isShow = true;
        //gameObject.transform.TransformPoint(new Vector2(0,0));
        
        gameObject.transform.localPosition = new Vector2(0, 0);
        mLevel = GameManager.getIntance().getUiLevel();
        gameObject.transform.SetSiblingIndex(mLevel);
        if (mClose == null)
        {
            mClose = GameObject.Find("lunhui_close").GetComponent<Button>();
            mClose.onClick.AddListener(() => {
                removeUi();
            });
        }
        if (mListControl == null) {
            mListControl = GameObject.Find("lunhui_skill_list").GetComponent<SamSaraListControl>();
            mListControl.init();
        }
        if (mLunhuiClick == null) {
            mLunhuiClick = GameObject.Find("lunhui_show_tip").GetComponent<Button>();
        }
        if (mLunhuiTx == null) {
            mLunhuiTx = GameObject.Find("lunhui_show_tip_tx2").GetComponent<Text>();
        }

        float mix = JsonUtils.getIntance().getConfigValueForId(100017);
        if (GameManager.getIntance().mCurrentLevel >= mix) {
            mLunhuiClick.interactable = true;
            mLunhuiTx.text = "";
        }
        else {
            mLunhuiClick.interactable = false;
            mLunhuiTx.text = "第" + mix + "关开启轮回功能";
        }
        mLunhuiValue.text = "当前拥有轮回点：" + GameManager.getIntance().mReincarnation.toStringWithUnit();
        mListControl.isEnableLavelUp();
    }
    public void removeUi()
    {
        isShow = false;
        // gameObject.transform.TransformPoint(new Vector2(-607, -31));
        gameObject.transform.localPosition = mFri;
    }
    public void updata() {
        Debug.Log("============================轮回点获得= " + GameManager.getIntance().mReincarnation.toString());
        mLunhuiValue.text = "当前拥有轮回点：" + GameManager.getIntance().mReincarnation.toStringWithUnit();
    }
}
