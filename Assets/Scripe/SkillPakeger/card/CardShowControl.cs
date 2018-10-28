using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardShowControl : MonoBehaviour {

    private Button mClose;
    private int mLevel;
    public bool isShow = false;
    
    private int mUserUiCount = 0;
    private int mBacUiCount = 0;
    GridLayoutGroup  mBackListGl;
    GridLayoutGroup mUserListGl;
    List<GameObject> mUserListGb = new List<GameObject>();
    List<GameObject> mBackListGb = new List<GameObject>();
    CardItemControl[] mUserArray;
    GoodControl[] mBackArray;
    public GameObject CardObject, CardItem;
    private int USER_LINE_COUNT = 4;
    private int BACK_LINE_COUNT = 4;
    private Vector2 mFri;
    LevelManager mLevelManager;
    Text mUserCount ;

    private void Start()
    {
        mUserListGl =  GameObject.Find("user_card_list").GetComponent<GridLayoutGroup>();
        mBackListGl =  GameObject.Find("cardList").GetComponent<GridLayoutGroup>();
        mUserCount =  GameObject.Find("title01").GetComponent<Text>();
        mLevelManager = GameObject.Find("Manager").GetComponent<LevelManager>();
        mFri = gameObject.transform.localPosition;
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
    public void upDateUi() {
        updateBackCard();
        updateUserd();
    }
    private void updateUserd() {
        clearUserUi();
        mUserCount.text = "已装备卡牌（" + mUserListGb.Count + "/30）";
        List<long> user = InventoryHalper.getIntance().getUsercard();

        foreach (long id in user)
        {
            addUserUi(id);
        }
    }

    private void updateBackCard() {
        clearBackUi();
       List<PlayerBackpackBean> list =  InventoryHalper.getIntance().getInventorys();
        foreach (PlayerBackpackBean bean in list) {
            if (bean.tabId == GoodControl.TABID_CARD_TYPE) {
                addBackUi(bean);
            }
        }
    }
    private void showUi()
    {
        upDateUi();
        isShow = true;
        //gameObject.transform.TransformPoint(new Vector2(0,0));

        gameObject.transform.localPosition = new Vector2(0, 0);
        upDateUi();
        if (mClose == null)
        {
            mClose = GameObject.Find("close").GetComponent<Button>();
            mClose.onClick.AddListener(() => {
                removeUi();
            });
        }
        mLevel = GameManager.getIntance().getUiLevel();
        gameObject.transform.SetSiblingIndex(mLevel);

    }
    public void removeUi()
    {
        isShow = false;
        // gameObject.transform.TransformPoint(new Vector2(-607, -31));
        gameObject.transform.localPosition = mFri;
    }

    private void addUserUi(long id)
    {
        mUserUiCount += 1;
        GameObject good = GameObject.Instantiate(CardObject,
            new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        good.AddComponent<CardItemControl>();
        CardItemControl item = good.GetComponent<CardItemControl>();
        CardUiControl ui = good.GetComponent<CardUiControl>();
        good.transform.parent = mUserListGl.transform;
        good.transform.localScale = Vector2.one;;
        mUserListGb.Add(good);
        mUserCount.text = "已装备卡牌（" + mUserListGb.Count + "/30）";
        mUserArray = GetComponentsInChildren<CardItemControl>();
        ui.init(id, CardUiControl.TYPE_CARD_ITME, mLevelManager.mPlayerControl);
        item.init(id, 113, 166);
        setWitch(mUserListGl, mUserListGb.Count);
      //  SetGridHeight(mUserListGl,3, mUserUiCount,USER_LINE_COUNT);
    }
    private void clearUserUi()
    {
        mUserUiCount = 0;
        for (; mUserListGb.Count > 0;)
        {
            GameObject goj = mUserListGb[0];
            mUserListGb.Remove(goj);
            Destroy(goj);
        }
    }
    private void addBackUi(PlayerBackpackBean bean)
    {
        mBacUiCount += 1;
        GameObject good = GameObject.Instantiate(CardObject,
            new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        good.AddComponent<CardItemControl>();
        CardItemControl item = good.GetComponent<CardItemControl>();
        CardUiControl ui = good.GetComponent<CardUiControl>();
        good.transform.parent = mBackListGl.transform;
        good.transform.localScale = Vector2.one; ;
        mBackListGb.Add(good);
        mUserArray = GetComponentsInChildren<CardItemControl>();
        CardJsonBean card = JsonUtils.getIntance().getCardInfoById(bean.goodId);
        ui.init(bean.goodId, CardUiControl.TYPE_CARD_GOOD, mLevelManager.mPlayerControl);
        item.init(bean.goodId, 73, 108);

/*
        GoodControl ct = good.GetComponent<GoodControl>();
        ct.updateUi(bean.goodId, 1, bean);
        mBackArray = GetComponentsInChildren<GoodControl>();*/
        SetGridHeight(mBackListGl, 2, mBacUiCount, 11 );
    }
    private void clearBackUi() {
        mBacUiCount = 0;
        if (mBackListGb != null) {
            for (; mBackListGb.Count > 0;)
            {
                GameObject goj = mBackListGb[0];
                mBackListGb.Remove(goj);
                Destroy(goj);
            }
        }

    }
    private float mGridHeight = 0;
    private void SetGridHeight(GridLayoutGroup grid, int minLine,int count,int lineCount)     //每行Cell的个数
    {
        int line = 0;
        if (count % lineCount != 0)
        {
            line = 1;
        }
        line += count / lineCount;
        if (line < minLine)
        {
            line = minLine;
        }
        float height = line * grid.cellSize.y;  //行数乘以Cell的高度，3.0f是微调
        height += (line - 1) * grid.spacing.y;     //每行之间有间隔
        grid.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        if (mGridHeight != height) {
            mGridHeight = height;
            grid.transform.Translate(Vector2.down * (height));
        }
      
    }
    private void setWitch(GridLayoutGroup grid, int count)     //每行Cell的个数
    {
        grid.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 113* count+10*(count-1)+10);
    }
}
