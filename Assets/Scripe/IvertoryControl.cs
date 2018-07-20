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
    private int MinCount = 5;
    private int LinCount = 7;
    GoodControl[] mGoodsControl;
    private bool isShow = false;
    Dictionary<long, List<PlayerBackpackBean>> mGoodDic = new Dictionary<long, List<PlayerBackpackBean>>();
    // Use this for initialization
    bool isInit = false;
    void Start () {
        if (isInit) {
            return;
        }
        isInit = true;
        Debug.Log("------------------------------------IvertoryControl isInit " + isInit);
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
            Debug.Log("bean = " + bean.toString());
            if (mGoodDic.ContainsKey(bean.tabId))
            {
                Debug.Log("mGoodDic do have" + bean.tabId);
                goodList = mGoodDic[bean.tabId];
            }
            else
            {
                Debug.Log("mGoodDic don't have" + bean.tabId);
                goodList = new List<PlayerBackpackBean>();
                mGoodDic.Add(bean.tabId, goodList);
            }
            if (goodList.Count < 1)
            {
                goodList.Add(bean);
            }
            else
            {
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
            Debug.Log("IvertoryControl mGoodDic don't containsKey " + mShowUiType);
            return;

        }
        Debug.Log("IvertoryControl mGoodDic containsKey " + mShowUiType);
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
    private void SetGridHeight(int num)     //每行Cell的个数
    {

        float childCount = this.transform.childCount;  //获得Layout Group子物体个数

        float height = ((childCount + num - 1) / num) * mGrilLayout.cellSize.y + 3.0f;  //行数乘以Cell的高度，3.0f是微调
        height += (((childCount + num - 1) / num) - 1) * mGrilLayout.spacing.y;     //每行之间有间隔
        mGrilLayout.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }
    private void addGoodUi() {
        for (int i = 0; i < LinCount; i++) {
            GameObject good = GameObject.Instantiate(mGood,
                new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            good.transform.parent = mGoods.transform;
            good.transform.localScale = Vector2.one;
            mGoodsControl = GetComponentsInChildren<GoodControl>();
            mGoodsGameObject.Add(good);
        }
      //  SetGridHeight(LinCount);

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
        Debug.Log("IvertoryControl add id="+id+" count = "+count);
        foreach (GoodControl good in mGoodsControl) {
            if (good.id == id && !good.isFull()) {
                if (good.addCount(count) != 0)
                {
                    update();
                    return;
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
