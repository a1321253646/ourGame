using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GetLocalDateTipControl : MonoBehaviour {

    Text mInputName;
    Button mClose, mSure;

	void Start () {
        mInputName = GameObject.Find("get_local_date_text").GetComponent<Text>();
        mClose = GameObject.Find("get_local_date_close").GetComponent<Button>();
        mSure = GameObject.Find("get_local_date_sure").GetComponent<Button>();

        mClose.onClick.AddListener(() =>
        {
            endEdit();
        });
        mSure.onClick.AddListener(() =>
        {
            Debug.Log(" mSure.onClick.AddListener");
            getLocalDate();
            endEdit();
        });

    }


    private float mOldTimeScale = -1;
    public void startEdit() {
        mOldTimeScale = Time.timeScale;
        gameObject.transform.SetSiblingIndex(GameManager.getIntance().getUiLevel());
        Time.timeScale = 0;
        //gameObject.transform.position = new Vector2(0, 0);
        transform.localScale = new Vector2(1, 1);
    }

    private void getLocalDate() {
        Debug.Log("saveChangeName newName " );
        string newId = mInputName.text;
        Debug.Log("getLocalDate newName = " + newId);

        if (string.IsNullOrEmpty(newId)) {
            GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showUi("未检测到输入有效编码", LuiHuiTips.TYPPE_EMPTY_TOKEN);
            GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showSelf();
            return;
        }
        if (newId.Equals(SQLHelper.getIntance().mToken)) {
            GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showUi("该编码存档已同步到本机", LuiHuiTips.TYPPE_EMPTY_TOKEN);
            GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showSelf();
            return;
        } else if (newId.Equals("clear")) {
            NetServer.getIntance().clearAllLocal();
            return;
        }

        NetServer.getIntance().getLocl(newId, true,false);
    }

    public void endEdit() {
        mInputName.text = "";
        Time.timeScale = mOldTimeScale;
        transform.localScale = new Vector2(0, 0);
    }
}
