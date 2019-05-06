using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        transform.localScale = new Vector2(1, 1);
    }

    private void getLocalDate() {
        Debug.Log("saveChangeName newName " );
        string newId = mInputName.text;
        Debug.Log("getLocalDate newName = " + newId);

        NetServer.getIntance().getLocl(newId, true,false);
    }

    private void endEdit() {
        mInputName.text = "";
        Time.timeScale = mOldTimeScale;
        transform.localScale = new Vector2(0, 0);
    }
}
