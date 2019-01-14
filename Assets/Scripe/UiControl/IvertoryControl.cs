using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IvertoryControl : UiControlBase
{
    public GameObject mGood;
    private GameObject mGoods;
    private GridLayoutGroup mGrilLayout;
    private List<GameObject> mGoodsGameObject = new List<GameObject>();
    private Button mCloBt;
    private GameObject  mClo;
    private GameObject  mGreedSure, mBlueSure, mPurpleSure, mOrangeSure;
    private Button mGreedBt, mBlueBt, mPurpleBt, mOrangeBt,mAllSale;
    private int MinCount = 6;
    private int LinCount = 8;
    GoodControl[] mGoodsControl;
    private int mLevel;
    private ScrollRect mScroll;
    Dictionary<long, List<PlayerBackpackBean>> mGoodDic = new Dictionary<long, List<PlayerBackpackBean>>();
    // Use this for initialization


    float time = 0;

    public void update() {
        update(true);
    }

    public void update(bool must)
    {
        if (!must && GameManager.getIntance().getUiCurrentLevel() > mLevel) {
            return;
        }
        int mGoodIndex = 0;
        
        PlayerBackpackBean bean;
        long addCount = 0;
        mGoodsControl = GetComponentsInChildren<GoodControl>();
        foreach (GoodControl g in mGoodsControl) {
            g.id = -1;
        }
        List<PlayerBackpackBean> list = InventoryHalper.getIntance().getInventorys();
        for (int i = 0; i < list.Count; i++) {
            bean = list[i];
            if (bean.goodType != SQLDate.GOOD_TYPE_BACKPACK) {
                continue;
            }
            addCount = bean.count;
            while (addCount != 0) {
                if (mGoodIndex >= mGoodsControl.Length) {
                    addGoodUi(LinCount);
                }
                GoodControl good = mGoodsControl[mGoodIndex];
                addCount = good.updateUi(bean.goodId, addCount, bean);
                mGoodIndex++;
            }
        }
        if (mGoodIndex < mGoodsGameObject.Count ) {
            for (int i = mGoodsGameObject.Count - 1; i >= mGoodIndex; i--) {
                GameObject goj = mGoodsGameObject[i];
                mGoodUiCount--;
                mGoodsGameObject.Remove(goj);
                Destroy(goj);   
            }
            mGoodsControl = GetComponentsInChildren<GoodControl>();
        }
        if (mGoodIndex < MinCount * LinCount)
        {
            addGoodUi(MinCount * LinCount - mGoodIndex);
        }
        else {
            if ( mGoodIndex % LinCount != 0) {
                addGoodUi(LinCount - mGoodIndex % LinCount);
            }

        }
    }
    private float mGridHeight = 0;

    private void SetGridHeight()     //每行Cell的个数
    {
        int line = 0;
        if (mGoodUiCount % LinCount != 0) {
            line = 1;
        }
 //       Debug.Log("  gridLyout childCount = " + mGoodUiCount);
   //     Debug.Log("  gridLyout num = " + LinCount);

    //    Debug.Log("  gridLyout line = " + line);

        line += mGoodUiCount / LinCount;
     //   Debug.Log("  gridLyout line = " + line);
        float height = line * mGrilLayout.cellSize.y ;  //行数乘以Cell的高度，3.0f是微调
        height += (line -1)* mGrilLayout.spacing.y;     //每行之间有间隔
        mGrilLayout.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        if (mGridHeight != height) {
            mGridHeight = height;
            mGrilLayout.transform.Translate(Vector2.down * (height));
        }
    }

    private int mGoodUiCount = 0;

    private void addGoodUi(int count) {
        mGoodUiCount += count;
        for (int i = 0; i < count; i++) {
            GameObject good = GameObject.Instantiate(mGood,
                new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            good.transform.parent = mGoods.transform;
            good.transform.localScale = Vector2.one;
            mGoodsControl = GetComponentsInChildren<GoodControl>();
            mGoodsGameObject.Add(good);
        }
        SetGridHeight();
    }

    public void guide(long target) {
        for (int i = 0; i < mGoodsGameObject.Count; i++)
        {
            GoodControl gc = mGoodsGameObject[i].GetComponent<GoodControl>();
            if (gc != null && gc.id == target)
            {
                GameManager.getIntance().getGuideManager().ShowGuideGrideLayoutInScroll(mGoodsGameObject[i], mScroll, mGrilLayout, i, 9);
                break;
            }
        }
    }

    public override void init()
    {
        mControlType = UiControlManager.TYPE_IVERTORY;
        isInit = true;
        Debug.Log("------------------------------------IvertoryControl isInit " + isInit);
        mGoods = GameObject.Find("Goods");
        mGrilLayout = mGoods.GetComponentInChildren<GridLayoutGroup>();

        mScroll = GameObject.Find("backpack_scroll").GetComponent<ScrollRect>();

        mCloBt = GameObject.Find("pack_close").GetComponent<Button>();
        mCloBt.onClick.AddListener(() =>
        {
            toremoveUi();
        });

        mGreedSure = GameObject.Find("select_greed_sure");
        if (GameManager.getIntance().mAllSaleGreed)
        {
            mGreedSure.transform.localScale = new Vector2(1, 1);
        }
        else
        {
            mGreedSure.transform.localScale = new Vector2(0, 0);
        }

        mBlueSure = GameObject.Find("select_blue_sure");
        if (GameManager.getIntance().mAllSaleBlue)
        {
            mBlueSure.transform.localScale = new Vector2(1, 1);
        }
        else
        {
            mBlueSure.transform.localScale = new Vector2(0, 0);
        }

        mPurpleSure = GameObject.Find("select_purple_sure");
        if (GameManager.getIntance().mAllSalePurple)
        {
            mPurpleSure.transform.localScale = new Vector2(1, 1);
        }
        else
        {
            mPurpleSure.transform.localScale = new Vector2(0, 0);
        }

        mOrangeSure = GameObject.Find("select_oringe_sure");
        if (GameManager.getIntance().mAllSaleOrange)
        {
            mOrangeSure.transform.localScale = new Vector2(1, 1);
        }
        else
        {
            mOrangeSure.transform.localScale = new Vector2(0, 0);
        }

        mGreedBt = GameObject.Find("select_greed_root").GetComponent<Button>();
        mGreedBt.onClick.AddListener(() =>
        {
            if (GameManager.getIntance().mAllSaleGreed)
            {
                GameManager.getIntance().mAllSaleGreed = false;
                mGreedSure.transform.localScale = new Vector2(0, 0);
            }
            else
            {
                GameManager.getIntance().mAllSaleGreed = true;
                mGreedSure.transform.localScale = new Vector2(1, 1);
            }
        });

        mBlueBt = GameObject.Find("select_blue_root").GetComponent<Button>();
        mBlueBt.onClick.AddListener(() =>
        {
            if (GameManager.getIntance().mAllSaleBlue)
            {
                GameManager.getIntance().mAllSaleBlue = false;
                mBlueSure.transform.localScale = new Vector2(0, 0);
            }
            else
            {
                GameManager.getIntance().mAllSaleBlue = true;
                mBlueSure.transform.localScale = new Vector2(1, 1);
            }
        });

        mPurpleBt = GameObject.Find("select_purple_root").GetComponent<Button>();
        mPurpleBt.onClick.AddListener(() =>
        {
            if (GameManager.getIntance().mAllSalePurple)
            {
                GameManager.getIntance().mAllSalePurple = false;
                mPurpleSure.transform.localScale = new Vector2(0, 0);
            }
            else
            {
                GameManager.getIntance().mAllSalePurple = true;
                mPurpleSure.transform.localScale = new Vector2(1, 1);
            }
        });

        mOrangeBt = GameObject.Find("select_oringe_root").GetComponent<Button>();
        mOrangeBt.onClick.AddListener(() =>
        {
            if (GameManager.getIntance().mAllSaleOrange)
            {
                GameManager.getIntance().mAllSaleOrange = false;
                mOrangeSure.transform.localScale = new Vector2(0, 0);
            }
            else
            {
                GameManager.getIntance().mAllSaleOrange = true;
                mOrangeSure.transform.localScale = new Vector2(1, 1);
            }
        });

        mAllSale = GameObject.Find("all_sale").GetComponent<Button>();
        mAllSale.onClick.AddListener(() =>
        {
            List<PlayerBackpackBean> list = InventoryHalper.getIntance().getInventorys();
            PlayerBackpackBean bean;
            AccouterJsonBean accouter;
            int count = 0;
            for (int i = 0; i < list.Count;)
            {

                bean = list[i];
                if (bean.goodType != SQLDate.GOOD_TYPE_BACKPACK)
                {
                    i++;
                    continue;
                }
                accouter = JsonUtils.getIntance().getAccouterInfoById(bean.goodId);
                bool isSale = false;
                if (accouter.quality == 1 && GameManager.getIntance().mAllSaleGreed)
                {
                    isSale = true;
                }
                else if (accouter.quality == 2 && GameManager.getIntance().mAllSaleBlue)
                {
                    isSale = true;
                }
                else if (accouter.quality == 3 && GameManager.getIntance().mAllSalePurple)
                {
                    isSale = true;
                }
                else if (accouter.quality == 4 && GameManager.getIntance().mAllSaleOrange)
                {
                    isSale = true;
                }
                if (isSale)
                {
                    count++;

                    BackpackManager.getIntance().use(bean, 1, TipControl.SALE_ALL_TYPE);
                }
                else
                {
                    i++;
                }
            }
            if (count > 0)
            {
                GameManager.getIntance().showDIaoLuo(mAllSale.transform.position, DiaoluoDonghuaControl.SHUIJI_DIAOLUO_TYPE, "", 0, -1, true);
                SQLHelper.getIntance().updateHunJing(GameManager.getIntance().mCurrentCrystal);
                update();
            }


        });
        addGoodUi(MinCount * LinCount);
    }

    public override void show()
    {
        gameObject.transform.localPosition = new Vector2(50, 0);
        GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_SHOW, GuideManager.SHOW_BACK);
        update();
    }
}
