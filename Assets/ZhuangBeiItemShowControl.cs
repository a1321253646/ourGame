﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZhuangBeiItemShowControl : MonoBehaviour {

    GoodControl mGoodControl;
    Button mButton;
    Text mCost;
    long level = 0;
    PlayerBackpackBean mBean;
    long updateCost;

    // Use this for initialization
    void Start () {
    }

    private void Update()
    {
        if (mButton != null) {
            if (GameManager.getIntance().mCurrentCrystal >= updateCost)
            {
                mButton.interactable = true;
            }
            else
            {
                mButton.interactable = false;
            }
        }
    }

    public void init(PlayerBackpackBean bean) {
        mBean = bean;
        if (mGoodControl == null) {
            mGoodControl = GetComponentInChildren<GoodControl>();
        }
        mGoodControl.updateUi(bean.goodId,0,bean);
        mGoodControl.isHero = true;
        foreach (PlayerAttributeBean b in bean.attributeList) {
            if (b.type == 10001) {
                level = b.value;
                break;
            }
        }
        AccouterJsonBean aj = JsonUtils.getIntance().getAccouterInfoById(bean.goodId);
        long baseCost = aj.cost;
        long cost = aj.getCost(level);
        updateCost = cost + baseCost;
        if (mCost == null)
        {
            mCost = GetComponentsInChildren<Text>()[3];
        }
        mCost.text = "" + updateCost;
        if (mButton == null)
        {
            mButton = GetComponentsInChildren<Button>()[1];
            mButton.onClick.AddListener(() => {
                BackpackManager.getIntance().UpdateZhuangBei(mBean, updateCost, level);              
            });
        }
        if (mButton != null)
        {
            if (GameManager.getIntance().mCurrentCrystal >= updateCost)
            {
                mButton.interactable = true;
            }
            else
            {
                mButton.interactable = false;
            }
        }
    }
}
