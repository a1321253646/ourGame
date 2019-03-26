using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDecTip : UiControlBase
{
    private Text mContext;
    private Button mCloseBt;
    private long mId;
    public override void init()
    {
        mControlType = UiControlManager.TYPE_DEC_TIP;
        mContext = GameObject.Find("game_dec_tip_context").GetComponent<Text>();
        mCloseBt = GameObject.Find("game_dec_tip_bt").GetComponent<Button>();
        mCloseBt.onClick.AddListener(() =>
        {
            toremoveUi();
        });
    }

    public void setData(long id) {
        mId = id;
        mContext.text = JsonUtils.getIntance().getStringById(mId);
    }

    public override void show()
    {
        gameObject.transform.localPosition = new Vector2(0, 0);
    }
}
