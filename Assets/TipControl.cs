using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TipControl : MonoBehaviour {


    private bool isShow = false;

    private long id;
    private long count;
    private GoodJsonBean mGoodInfo;
    private GoodControl mGoodControl;
    private Text mName;
    private Text nButtonTx;
    private Text nDepictTx;
    private DepictTextControl mDepoct;
    public void setShowData(PlayerBackpackBean bean,long count) {
        if (isShow)
        {
            return;
        }
        isShow = true;
        this.id = bean.goodId;
        this.count = count;
        showUi();
        updataUi();
    }

    private void updataUi() {
        
        mGoodInfo = BackpackManager.getIntance().getGoodInfoById(id);
        creatDepictText();
       
    }

    private void creatDepictText() {
        string str = "";
        
        Debug.Log("mGoodInfo.describe= "+ mGoodInfo.describe);
        if (nDepictTx == null)
        {
            mDepoct = GameObject.Find("depict_text").GetComponent<DepictTextControl>();
        }
        mDepoct.setText(str);
    }

    private void showUi()
    {
        gameObject.transform.localPosition = new Vector2(0, 0);
    }
    public void removeUi()
    {
        if (!isShow)
        {
            return;
        }
        isShow = false;
        // gameObject.transform.TransformPoint(new Vector2(-607, -31));
        gameObject.transform.localPosition = new Vector2(500f, -386.46f);
    }
}
