using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SamsaraManage : UiControlBase
{
    private Button mClose,mLunhuiClick,mQiangzhiLunhui;
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
            GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showUi();
            UiControlManager.getIntance().show(UiControlManager.TYPE_LUIHUI);
        });
        mClose = GameObject.Find("lunhui_close").GetComponent<Button>();
        mClose.onClick.AddListener(() => {
            toremoveUi();
        });
        mQiangzhiLunhui = GameObject.Find("qiangzhilunhui").GetComponent<Button>();
        mQiangzhiLunhui.onClick.AddListener(() =>
        {
            GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showUiQiangzhi();
        });
        Debug.Log("SQLHelper.getIntance().isHadLunhui = " + SQLHelper.getIntance().isHadLunhui);


        mListControl = GameObject.Find("lunhui_skill_list").GetComponent<SamSaraListControl>();
        
        mLunhuiClick = GameObject.Find("lunhui_show_tip").GetComponent<Button>();
        mLunhuiTx = GameObject.Find("lunhui_show_tip_tx2").GetComponent<Text>();
    }
    bool isFristShow = true;
    float mTimeScale = 1;
    public void addLastItem() {
        mListControl.addLastItem();
    }

    public override void show()
    {
        if (isFristShow) {
            isFristShow = false;
            mListControl.init();
        }
        if (GameManager.getIntance().isOpenStop) {
            mTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }


        gameObject.transform.localPosition = new Vector2(0, 0);
        float mix = BaseDateHelper.decodeLong(SQLHelper.getIntance().isCanLunhui);
        if (mix == -1)
        {
            mix = 1 + JsonUtils.getIntance().getConfigValueForId(100017);
        }
        Debug.Log("================mix=" + mix);
        if (BaseDateHelper.decodeLong(GameManager.getIntance().mCurrentLevel) >= (long)mix)
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
        if (SQLHelper.getIntance().isHadLunhui == 1 || BaseDateHelper.decodeLong(GameManager.getIntance().mCurrentLevel) > 0)
        {
            mQiangzhiLunhui.transform.localScale = new Vector2(1, 1);
        }
        else
        {
            mQiangzhiLunhui.transform.localScale = new Vector2(0, 0);
        }
    }
    public override void remove()
    {
        isShow = false;
        transform.localPosition = mFri;
        if (GameManager.getIntance().isOpenStop)
        {
            GameManager.getIntance().isOpenStop = false;
            Time.timeScale = mTimeScale;
        }
    }
    public void updateLunhuiValue() {
        mLunhuiValue.text = GameManager.getIntance().mReincarnation.toStringWithUnit();
    }

}
