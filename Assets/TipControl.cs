﻿using System.Collections;
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
    public void setShowData(long id, long count) {
        if (isShow)
        {
            return;
        }
        isShow = true;
        this.id = id;
        this.count = count;
        showUi();
        updataUi();
    }

    private void updataUi() {
        
        mGoodInfo = BackpackManager.getIntance().getGoodInfoById(id);
        creatDepictText();
        if (mGoodControl == null) {
            mGoodControl = gameObject.GetComponentInChildren<GoodControl>();
        }
        mGoodControl.updateUi(id, count);
        if (mName == null) {
            mName = GameObject.Find("tipName").GetComponent<Text>();
        }
        mName.text = mGoodInfo.name;

        if (nButtonTx == null) {
            nButtonTx = GameObject.Find("tipButtonTx").GetComponent<Text>();
        }
        if (mGoodInfo.tabid == BackpackManager.ZHUANGBEI_TYPE)
        {
            nButtonTx.text = "装备";
        }
        else if (mGoodInfo.tabid == BackpackManager.CAILIAO_TYPE)
        {
            nButtonTx.text = "使用";
        }



       
    }

    private void creatDepictText() {
        string str = "";
        str = str + mGoodInfo.describe+"\n";
        str = str + mGoodInfo.describe + "\n";
        str = str + mGoodInfo.describe + "\n";

        str = str + mGoodInfo.describe + "\n";
        str = str + mGoodInfo.describe + "\n";
        str = str + mGoodInfo.describe + "\n";
        str = str + mGoodInfo.describe + "\n";
        str = str + mGoodInfo.describe + "\n";
        str = str + mGoodInfo.describe + "\n";
        str = str + mGoodInfo.describe + "\n";
        str = str + mGoodInfo.describe + "\n";
        Debug.Log("mGoodInfo.describe= "+ mGoodInfo.describe);
        if (nDepictTx == null)
        {
            mDepoct = GameObject.Find("depict_text").GetComponent<DepictTextControl>();
        }
        mDepoct.setText(str);
    }

    private void showUi()
    {
        //gameObject.transform.TransformPoint(new Vector2(0,0));
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

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
