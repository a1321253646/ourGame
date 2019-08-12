using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeGifTipControl : MonoBehaviour
{

    Text mInputName;
    Button mClose, mSure;

    void Start()
    {
        mInputName = GameObject.Find("change_git_input").GetComponent<Text>();
        mClose = GameObject.Find("change_git_close").GetComponent<Button>();
        mSure = GameObject.Find("change_gif_tip_sure").GetComponent<Button>();

        mClose.onClick.AddListener(() =>
        {
            endEdit();
        });
        mSure.onClick.AddListener(() =>
        {
            Debug.Log(" mSure.onClick.AddListener");
            saveChangeName();
            endEdit();
        });

    }
    private float mOldTimeScale = -1;
    public void startEdit()
    {
        mOldTimeScale = Time.timeScale;
        gameObject.transform.SetSiblingIndex(GameManager.getIntance().getUiLevel());
        Time.timeScale = 0;
        transform.localScale = new Vector2(1, 1);
        isShow = true;
    }

    private void saveChangeName()
    {
        Debug.Log("saveChangeName newName ");
        string newName = mInputName.text;
        Debug.Log("saveChangeName newName = " + newName);
        if (string.IsNullOrEmpty(newName))
        {
            GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showUi("兑换码为空", LuiHuiTips.TYPPE_EMPTY_TOKEN);
            GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showSelf();
        }
        else {
            NetServer.getIntance().getGif(newName);
        }        
    }

    private void endEdit()
    {
        mInputName.text = "";
        Time.timeScale = mOldTimeScale;
        isShow = false;
        transform.localScale = new Vector2(0, 0);
    }


    bool isShow = false;
    public void OnPointerClick(BaseEventData eventData)
    {
        if (isShow)
        {
            endEdit();

        }
        else
        {
            startEdit();
        }

    }
}
