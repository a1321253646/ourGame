using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroRoleControl : MonoBehaviour {
    public bool isShow = false;
    Dictionary<long, PlayerBackpackBean> mHeroEquipl;
    Dictionary<long, GoodControl> mHeroGoodControl = new Dictionary<long, GoodControl>();
    private Text mText, mText2, mText3;
    private long[] mTypeAll = new long[] {1, 2, 3, 4, 5, 6};
    private Button mClose;
    private int mLevel;
    public void click()
    {
        if (isShow)
        {
            int level = GameManager.getIntance().getUiCurrentLevel();
            if (mLevel < level)
            {
                showUi();
                return;
            }
            else if (mLevel == level)
            {
                removeUi();
            }
        }
        else
        {
            showUi();
        }
    }

    private void showUi()
    {
        isShow = true;
        //gameObject.transform.TransformPoint(new Vector2(0,0));
        gameObject.transform.localPosition = new Vector2(0, 0);
        upDateUi();
        if (mClose == null) {
            mClose = GameObject.Find("hero_info_close").GetComponent<Button>();
            mClose.onClick.AddListener(()=> {
                removeUi();
            });
        }
        mLevel = GameManager.getIntance().getUiLevel();
        gameObject.transform.SetSiblingIndex(mLevel);

    }
    private void removeUi()
    {
        isShow = false;
        // gameObject.transform.TransformPoint(new Vector2(-607, -31));
        gameObject.transform.localPosition = new Vector2(-222, -411);
    }

    public void upDateUi()
    {
        mHeroEquipl = BackpackManager.getIntance().getHeroEquipInfo();
        GoodControl goodIcon = null;
        PlayerBackpackBean bean = null;
        string name = null;
        foreach (long type in mTypeAll) {
            name = "equip_gride_" + type;
            Debug.Log("hero equip type =" + name);
            goodIcon = GameObject.Find(name).GetComponent<GoodControl>();

            if (mHeroEquipl.ContainsKey(type))
            {
                PlayerBackpackBean keyValue = mHeroEquipl[type];
                goodIcon.updateUi(keyValue.goodId, keyValue.count, keyValue);
            }
            else {
                goodIcon.updateUi(-1, 0, null);
            }
        }

        if (mText == null) {
            mText = GameObject.Find("hero_information").GetComponent<Text>();
        }
        if (mText2 == null)
        {
            mText2 = GameObject.Find("hero_information2").GetComponent<Text>();
        }
        if (mText3 == null)
        {
            mText3 = GameObject.Find("hero_information3").GetComponent<Text>();
        }
        PlayControl plya = BackpackManager.getIntance().getHero();
        mText.text = "英雄等级: " + GameManager.getIntance().mHeroLv+"\n"+
            "攻击: "+plya.mAggressivity+"\n"+
            "防御: "+plya.mDefense+"\n"+
            "生命:"+plya.mMaxBloodVolume+"\n";
        mText2.text = "命中：" + plya.mRate+"\n"+
            "闪避："+plya.mEvd+"\n"+
            "暴击："+plya.mCrt+"\n"+
            "暴击伤害："+plya.mCrtHurt+"\n";
        mText3.text = "真实伤害：" + plya.mReadHurt+"\n";
    }
}
