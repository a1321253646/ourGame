using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroRoleControl : MonoBehaviour {
    public bool isShow = false;
    Dictionary<long, PlayerBackpackBean> mHeroEquipl;
    Dictionary<long, GoodControl> mHeroGoodControl = new Dictionary<long, GoodControl>();
    private Text mText;
    private Button mClose;
    private int mLevel;
    public void click()
    {
        if (isShow)
        {
            int level = GameManager.getIntance().getUiLevel();
            if (mLevel < level)
            {
                gameObject.transform.SetSiblingIndex(level);
                mLevel = level;
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
        if (mHeroEquipl.Count > 0)
        {
            foreach (KeyValuePair<long, PlayerBackpackBean> key in mHeroEquipl)
            {
                long keyType = key.Key;
                PlayerBackpackBean keyValue = key.Value;
                GoodControl goodIcon = null;
                if (mHeroGoodControl.ContainsKey(keyType))
                {
                    goodIcon = mHeroGoodControl[keyType];
                }
                else
                {
                    string name = "equip_gride_" + keyType;
                    Debug.Log("hero equip type =" + name);
                    goodIcon = GameObject.Find(name).GetComponent<GoodControl>();
                    mHeroGoodControl.Add(keyType, goodIcon);
                }
                goodIcon.updateUi(key.Value.goodId, key.Value.count, key.Value);
            }
        }
        else if (mHeroGoodControl.Count > 0) {
            foreach (GoodControl icom in mHeroGoodControl.Values) {
                icom.updateUi(-1, 0, null);
            }
        }

        if (mText == null) {
            mText = GameObject.Find("hero_information").GetComponent<Text>();
        }
        PlayControl plya = BackpackManager.getIntance().getHero();
        mText.text = "英雄等级: " + GameManager.getIntance().mHeroLv+"\n"+
            "攻击: "+plya.mAggressivity+"\n"+
            "防御: "+plya.mDefense+"\n"+
            "最大血量:"+plya.mMaxBloodVolume+"\n";
    }
}
