﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VocationControl : UiControlBase
{

    private VocationCardControl mCard1,mCard2,mCard3;

    private Button mClose, mNoSelect;

    public override void show() {
        long id = SQLHelper.getIntance().mPlayVocation;
        if (id == -1) {
            id = 1;
        }
        VocationDecBean bean = JsonUtils.getIntance().getVocationById(id);
        List<long> nexts = bean.getNexts();
        mCard1.show(nexts[0]);
        mCard2.show(nexts[1]);
        mCard3.show(nexts[2]);
    }

    public void select(long id) { 
        
    }

    public override void init()
    {
        mControlType = UiControlManager.TYPE_VOCATION;
        mCard1 = GameObject.Find("vocation_card1").GetComponent<VocationCardControl>();
        mCard2 = GameObject.Find("vocation_card2").GetComponent<VocationCardControl>();
        mCard3 = GameObject.Find("vocation_card3").GetComponent<VocationCardControl>();

        mClose = GameObject.Find("vocation_close").GetComponent<Button>();
        mNoSelect = GameObject.Find("vocation_select_no").GetComponent<Button>();
        mClose.onClick.AddListener(() => {
            toremoveUi();
        });
        mNoSelect.onClick.AddListener(() => {
            toremoveUi();
        });
    }
}