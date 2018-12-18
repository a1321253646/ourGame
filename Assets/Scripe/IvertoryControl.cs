using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IvertoryControl : MonoBehaviour {
    private static int ALL_TYPE = 10;
    private static int[] OTHER_TYPE = { 1,2,3};
    private long mShowUiType = -1;
    public GameObject mGood;
    private GameObject mGoods;
    private GridLayoutGroup mGrilLayout;
    private List<GameObject> mGoodsGameObject = new List<GameObject>();
    private Button mFirBt, mSecBt, mThiBt, mFouBt, mCloBt;
    private GameObject mFir, mSec, mThi, mFou, mClo;
    private Button mFirBtNo, mSecBtNo, mThiBtNo, mFouBtNo;
    private GameObject mFirNo, mSecNo, mThiNo, mFouNo;
    private int MinCount = 6;
    private int LinCount = 9;
    GoodControl[] mGoodsControl;
    private int mLevel;
    public bool isShow = false;
    private Vector2 mFri;
    private ScrollRect mScroll;
    Dictionary<long, List<PlayerBackpackBean>> mGoodDic = new Dictionary<long, List<PlayerBackpackBean>>();
    // Use this for initialization
    bool isInit = false;
    void Start () {
        if (isInit) {
            return;
        }
        mFri = gameObject.transform.localPosition;
        isInit = true;
        Debug.Log("------------------------------------IvertoryControl isInit " + isInit);
        mGoods = GameObject.Find("Goods");
        mGrilLayout = mGoods.GetComponentInChildren<GridLayoutGroup>();
        mFirBt = GameObject.Find("pack_frist_bt").GetComponent<Button>();
        mSecBt = GameObject.Find("pack_second_bt").GetComponent<Button>();
        mThiBt = GameObject.Find("pack_third_bt").GetComponent<Button>();
        mFouBt = GameObject.Find("pack_four_bt").GetComponent<Button>();
        mFirBtNo = GameObject.Find("pack_frist_bt_no").GetComponent<Button>();
        mSecBtNo = GameObject.Find("pack_second_bt_no").GetComponent<Button>();
        mThiBtNo = GameObject.Find("pack_third_bt_no").GetComponent<Button>();
        mFouBtNo = GameObject.Find("pack_four_bt_no").GetComponent<Button>();

        mFir = GameObject.Find("pack_frist_bt");
        mSec = GameObject.Find("pack_second_bt");
        mThi = GameObject.Find("pack_third_bt");
        mFou = GameObject.Find("pack_four_bt");
        mFirNo = GameObject.Find("pack_frist_bt_no");
        mSecNo = GameObject.Find("pack_second_bt_no");
        mThiNo = GameObject.Find("pack_third_bt_no");
        mFouNo = GameObject.Find("pack_four_bt_no");
        mScroll = GameObject.Find("backpack_scroll").GetComponent<ScrollRect>();
        mCloBt = GameObject.Find("pack_close").GetComponent<Button>();

        mCloBt.onClick.AddListener(() =>
        {
            removeUi();
        });

        mFirBtNo.onClick.AddListener(() => {
            chageTabShow((int)mShowUiType, false);
            chageTabShow(ALL_TYPE, true);
           
        });
        mSecBtNo.onClick.AddListener(() => {
            chageTabShow((int)mShowUiType, false);
            chageTabShow(1, true);
        });
        mThiBtNo.onClick.AddListener(() => {
            chageTabShow((int)mShowUiType, false);
            chageTabShow(2, true);
        });
        mFouBtNo.onClick.AddListener(() => {
            chageTabShow((int)mShowUiType, false);
            chageTabShow(3, true);
        });
        addGoodUi(MinCount* LinCount);
        //update();
        chageTabShow(ALL_TYPE, false); 
        chageTabShow(1, false);
        chageTabShow(2, false);
        chageTabShow(3, false);
        chageTabShow(ALL_TYPE, true);
    }

    private void chageTabShow(int type, bool show)
    {
        if (type == mShowUiType && show) {
            return;
        }
        if (show)
        {
            changeType(type);
        }
        if (show)
        {
            switch (type)
            {
                case 10:
                    mFir.SetActive(true);
                    mFirNo.SetActive(false);
                    break;
                case 1:
                    mSec.SetActive(true);
                    mSecNo.SetActive(false);
                    break;
                case 2:
                    mThi.SetActive(true);
                    mThiNo.SetActive(false);
                    break;
                case 3:
                    mFou.SetActive(true);
                    mFouNo.SetActive(false);
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (type)
            {
                case 10:
                    mFir.SetActive(false);
                    mFirNo.SetActive(true);
                    break;
                case 1:
                    mSec.SetActive(false);
                    mSecNo.SetActive(true);
                    break;
                case 2:
                    mThi.SetActive(false);
                    mThiNo.SetActive(true);
                    break;
                case 3:
                    mFou.SetActive(false);
                    mFouNo.SetActive(true);
                    break;
                default:
                    break;
            }


        }
    }
   
    private void changeType(int type) {
        mShowUiType = type;
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
        List<PlayerBackpackBean> list = InventoryHalper.getIntance().getInventorys();
        List<PlayerBackpackBean> goodList;
        if (list == null)
            return;
        mGoodDic.Clear();
        Debug.Log("=11111=============================================================list = " + list.Count);
        foreach (PlayerBackpackBean bean in list)
        {
            Debug.Log("=11111=============================================================bean goodType= " + bean.goodType + " bean.count=="+ bean.count+" goodId = "+bean.goodId);
            if (bean.goodType != SQLDate.GOOD_TYPE_BACKPACK || bean.count == 0) {
                continue;
            }
 //           Debug.Log("bean = " + bean.toString());
            if (mGoodDic.ContainsKey(bean.tabId))
            {
 //               Debug.Log("mGoodDic do have" + bean.tabId);
                goodList = mGoodDic[bean.tabId];
            }
            else
            {
     //           Debug.Log("mGoodDic don't have" + bean.tabId);
                goodList = new List<PlayerBackpackBean>();
                mGoodDic.Add(bean.tabId, goodList);
            }
            if (goodList.Count < 1)
            {
                goodList.Add(bean);
            }
            else
            {
                bool isAdd = false;
                for (int i = 0; i < goodList.Count; i++)
                {
                    if (bean.sortID < goodList[i].sortID)
                    {
                        goodList.Insert(i, bean);
                        isAdd = true;
                        break;
                    }
                }
                if (!isAdd) {
                    goodList.Add(bean);
                }
             
            }
        }
    }

    public void update() {
        update(true);
    }

    public void update(bool must)
    {
        if (!must && GameManager.getIntance().getUiCurrentLevel() > mLevel) {
            return;
        }
        upDateData();
        int mGoodIndex = 0;
        //        Debug.Log("IvertoryControl mGoodDic containsKey " + mShowUiType);
        List<PlayerBackpackBean> goodList = null;
        if (mShowUiType == ALL_TYPE)
        {
            goodList = new List<PlayerBackpackBean>();
            foreach (int i in OTHER_TYPE)
            {
           //     Debug.Log("ContainsKey" + i);
                if (mGoodDic.ContainsKey(i)) {
                    goodList.AddRange(mGoodDic[i]);
             //       Debug.Log("goodList leng" + goodList.Count);
                }
                    
            }
        }
        else if (mGoodDic.ContainsKey(mShowUiType))
        {
            goodList = mGoodDic[mShowUiType];
        }
        else {
            goodList = new List<PlayerBackpackBean>();
        }
        
        PlayerBackpackBean bean;
        long addCount = 0;
        mGoodsControl = GetComponentsInChildren<GoodControl>();
        for (int i = 0; i < goodList.Count; i++) {
            bean = goodList[i];
            addCount = bean.count;
            while (addCount != 0) {
                if (mGoodIndex >= mGoodsControl.Length) {
                    addGoodUi(LinCount);
                }
                if (bean.goodId > InventoryHalper.TABID_3_START_ID) {
                    bool isHave = false;
                    for (int iii = 0; iii < mGoodIndex;iii++) {
                        if (mGoodsControl[mGoodIndex].id == bean.goodId) {
                            mGoodsControl[mGoodIndex].addCount(bean.count);
                            addCount = 0;
                            isHave = true;
                            break;
                        }
                    }
                    if (isHave) {
                        continue;
                    }
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

    private void showUi() {

        isShow = true;
        //gameObject.transform.TransformPoint(new Vector2(0,0));
        gameObject.transform.localPosition = new Vector2(50, 0);
        mLevel = GameManager.getIntance().getUiLevel();
        gameObject.transform.SetSiblingIndex(mLevel);
        GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_SHOW, GuideManager.SHOW_BACK);
        update();

    }
    public void removeUi() {
        isShow = false;
        // gameObject.transform.TransformPoint(new Vector2(-607, -31));
        gameObject.transform.localPosition = mFri;
    }

    public void addGood(long id, long count) {
        long tabid = -1;
        if (JsonUtils.getIntance().getGoodInfoById(id) == null)
        {
            if (JsonUtils.getIntance().getAccouterInfoById(id) == null)
            {
                tabid = JsonUtils.getIntance().getCardInfoById(id).tabid;
            }
            else {
                tabid = JsonUtils.getIntance().getAccouterInfoById(id).tabid;
            }
            
        }
        else {
            tabid = JsonUtils.getIntance().getGoodInfoById(id).tabid;
        }
        if (mShowUiType != ALL_TYPE && tabid != mShowUiType) {
            return;
        }
        foreach (GoodControl good in mGoodsControl) {
            if ( good.id == id && !good.isFull()) {
                long addReturn = good.addCount(count);;
                if (addReturn != 0)
                {                 ;
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

    public void deleteGood(long id, long count)
    {
        bool isSuccess = false;
        for (int i = 0; i < mGoodsControl.Length; i++) {
            GoodControl good = mGoodsControl[i];
            if (good.id == id)
            {
                while (i < mGoodsControl.Length && mGoodsControl[i].id == id)
                {
                    i++;
                }
                if (i >= mGoodsControl.Length)
                {
                    isSuccess = mGoodsControl[mGoodsControl.Length - 1].deleteCount(count);
                }
                else {
                    isSuccess = mGoodsControl[i - 1].deleteCount(count);
                }
                if (!isSuccess) {
                    update();
                }
            }
        }
    }
}
