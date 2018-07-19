using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IvertoryControl : MonoBehaviour {
    private long mShowUiType = 1;
    public GameObject mGood;
    private GameObject mGoods;
    private GridLayoutGroup mGrilLayout;
    private List<GameObject> mGoodsGameObject = new List<GameObject>();
    private int MinCount = 24;
    GoodControl[] mGoodsControl;
    private bool isShow = false;
    Dictionary<long, List<PlayerBackpackBean>> mGoodDic = new Dictionary<long, List<PlayerBackpackBean>>();
    // Use this for initialization
    void Start () {
        mGoods = GameObject.Find("Goods");
        mGrilLayout = mGoods.GetComponentInChildren<GridLayoutGroup>();
        for (int i = 0; i < MinCount; i++) {
            addGoodUi();
        }
        update();
    }
    public void showTypeUi(long type)
    {
        if (mShowUiType == type) {
            return;
        }
        mShowUiType = type;
        update();
    }
    float time = 0;

    private void upDateData() {
        List<PlayerBackpackBean> list = BackpackManager.getIntance().getInventoryInfos();
        List<PlayerBackpackBean> goodList;
        if (list == null)
            return;
        foreach (PlayerBackpackBean bean in list)
        {
            if (mGoodDic.ContainsKey(bean.tabId))
            {
                goodList = mGoodDic[bean.tabId];
            }
            else
            {
                goodList = new List<PlayerBackpackBean>();
                mGoodDic.Add(bean.tabId, goodList);
            }
            if (goodList.Count < 1)
            {
                goodList[0] = bean;
            }
            else
            {
                int index = 0;
                for (int i = 0; i < goodList.Count; i++)
                {
                    if (bean.sortID > goodList[i].sortID)
                    {
                        goodList.Insert(i, bean);
                        break;
                    }
                }
            }
        }
    }

    private void update()
    {
        upDateData();
        int mGoodIndex = 0;
        if (!mGoodDic.ContainsKey(mShowUiType)) {
            return;

        }
        List<PlayerBackpackBean> goodList = mGoodDic[mShowUiType];
        PlayerBackpackBean bean;
        long addCount = 0;
        mGoodsControl = GetComponentsInChildren<GoodControl>();
        for (int i = 0; i < goodList.Count; i++) {
            bean = goodList[i];
            addCount = bean.count;
            while (addCount != 0) {
                if (mGoodIndex >= mGoodsControl.Length) {
                    addGoodUi();
                }
                GoodControl good = mGoodsControl[mGoodIndex];
                addCount = good.updateUi(bean.goodId, addCount);
                mGoodIndex++;
            }
        }
        if (mGoodIndex < mGoodsControl.Length - 1 ) {
            for (int i = mGoodsControl.Length - 1; i > mGoodIndex; i--) {
                GameObject goj = mGoodsGameObject[i];
                mGoodsGameObject.Remove(goj);
                Destroy(goj);   
            }
        }
        if (mGoodIndex < MinCount -1) {
            int count = MinCount - 1 - mGoodIndex;
            for (int i = 0; i < count; i++) {
                addGoodUi();
            }
        }
    }
    private void addGoodUi() {
        for (int i = 0; i < 6; i++) {
            GameObject good = GameObject.Instantiate(mGood,
                new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            good.transform.parent = mGoods.transform;
            good.transform.localScale = Vector2.one;
            mGoodsControl = GetComponentsInChildren<GoodControl>();
            mGoodsGameObject.Add(good);
        }

    }
    public void showUi() {
        if (isShow) {
            return;
        }
        isShow = true;
        gameObject.transform.TransformPoint(new Vector2(0,0));
    }
    public void removeUi() {
        if (!isShow)
        {
            return;
        }
        isShow = false;
        gameObject.transform.TransformPoint(new Vector2(-607, -31));
    }
    public void addGood(long id, long count) {
        foreach (GoodControl good in mGoodsControl) {
            if (good.id == id && !good.isFull()) {
                if (good.addCount(count) != 0)
                {
                    update();
                }
                else {
                    return;
                }
            }
        }
        update();
    }
    public void deleteGood(long id, long count)
    {
        foreach (GoodControl good in mGoodsControl)
        {
            if (good.id == id)
            {
                if (good.deleteCount(count))
                {
                    return;
                }
                else
                {
                    update();
                }
            }
        }
    }
}
