using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeNameTipControl : MonoBehaviour {

    Text mInputName;
    Button mClose, mSure;

	void Start () {
        mInputName = GameObject.Find("change_name_input").GetComponent<Text>();
        mClose = GameObject.Find("change_name_close").GetComponent<Button>();
        mSure = GameObject.Find("change_name_tip_sure").GetComponent<Button>();

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
    public void startEdit() {
        mOldTimeScale = Time.timeScale;
        gameObject.transform.SetSiblingIndex(GameManager.getIntance().getUiLevel());
        Time.timeScale = 0;
        transform.localScale = new Vector2(1, 1);
    }

    private void saveChangeName() {
        Debug.Log("saveChangeName newName " );
        string newName = mInputName.text;
        Debug.Log("saveChangeName newName = " + newName);
        SQLHelper.getIntance().updateName(newName);
        GameObject.Find("ranking_list").GetComponent<RankingListControl>().updateName();
    }

    private void endEdit() {
        mInputName.text = "";
        Time.timeScale = mOldTimeScale;
        transform.localScale = new Vector2(0, 0);
    }
}
