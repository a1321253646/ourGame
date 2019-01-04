using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingListItemControl : MonoBehaviour {

    List<RankingListDateBean> mList;
    int mIndex = -1;

    Image mIndexIm, mIndexBg, mNameBg, mLevelBg;
    Text mIndexTx, mNameTx, mLevelTx;

    public void init(List<RankingListDateBean> list,int index) {
        mList = list;
        mIndex = index;
        Image[] imgs = GetComponentsInChildren<Image>();
        mIndexBg = imgs[1];
        mIndexIm = imgs[2];      
        mNameBg = imgs[3];
        mLevelBg = imgs[4];

        Text[] texts = GetComponentsInChildren<Text>();
        mIndexTx = texts[0];
        mNameTx = texts[1];
        mLevelTx = texts[2];
        Sprite sprite;
        if (mIndex % 2 == 0)
        {
            sprite = Resources.Load("UI_yellow/paihang/06", typeof(Sprite)) as Sprite;
        }
        else {
            sprite = Resources.Load("UI_yellow/paihang/05", typeof(Sprite)) as Sprite;
        }

        if (mIndex < 3)
        {
            if (mIndex == 0)
            {
                mIndexIm.sprite = Resources.Load("UI_yellow/paihang/02", typeof(Sprite)) as Sprite;
            }
            else if (mIndex == 1)
            {
                mIndexIm.sprite = Resources.Load("UI_yellow/paihang/03", typeof(Sprite)) as Sprite;
            }
            else if (mIndex == 2)
            {
                mIndexIm.sprite = Resources.Load("UI_yellow/paihang/04", typeof(Sprite)) as Sprite;
            }
        }
        mIndexBg.sprite = sprite;
        mNameBg.sprite = sprite;
        mLevelBg.sprite = sprite;
        show();
    }

    public void updateList(List<RankingListDateBean> list) {
        mList = list;
        show();
    }
    private void show() {
        if (mList == null ||  mIndex >= mList.Count )
        {
            mIndexIm.transform.localScale = new Vector2(0, 0);
            mIndexTx.text = "";
            mNameTx.text = "";
            mLevelTx.text = "";
        }
        else {
            RankingListDateBean item = mList[mIndex];
            if (mIndex < 3)
            {
                mIndexIm.transform.localScale = new Vector2(1, 1);
                mIndexTx.text = "";
            }
            else {
                mIndexTx.text = item.index + "";
            }
            if (item.name.Equals("***"))
            {
                mNameTx.text = "用户" + item.user.Substring(0,4);
            }
            else {
                mNameTx.text = item.name;
            }
            if (item.level <= 0)
            {
                mLevelTx.text = (item.level + (long)JsonUtils.getIntance().getConfigValueForId(100019)) + "";
            }
            else {
                mLevelTx.text = item.level + "";
            }
           
        }
    }
}
