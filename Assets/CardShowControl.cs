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
    GridLayoutGroup mUserListGl, mBackListGl;
    List<GameObject> mUserListGb = new List<GameObject>();
    List<GameObject> mBackListGb = new List<GameObject>();
    CardItemControl[] mUserArray;
    GoodControl[] mBackArray;
    public GameObject CardObject, CardItem;
    private int USER_LINE_COUNT = 4;
    private int BACK_LINE_COUNT = 4;
    private void Start()
    {
        mUserListGl =  GameObject.Find("user_card_list").GetComponent<GridLayoutGroup>();
        mBackListGl =  GameObject.Find("cardList").GetComponent<GridLayoutGroup>();   
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
            mClose = GameObject.Find("card_close").GetComponent<Button>();
            mClose.onClick.AddListener(() => {
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
        gameObject.transform.localPosition = new Vector2(733.89f, 335.53f);
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
        mUserArray = GetComponentsInChildren<CardItemControl>();
        ui.init(id, CardUiControl.TYPE_CARD_ITME);
        item.init(id, mUserListGl.cellSize.x, mUserListGl.cellSize.y);
        SetGridHeight(mUserListGl,3, mUserUiCount,USER_LINE_COUNT);
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
        GameObject good = GameObject.Instantiate(CardItem,
            new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        good.transform.parent = mBackListGl.transform;
        good.transform.localScale = Vector2.one; ;
        mBackListGb.Add(good);
        GoodControl ct = good.GetComponent<GoodControl>();
        ct.updateUi(bean.goodId, 1, bean);
        mBackArray = GetComponentsInChildren<GoodControl>();
        SetGridHeight(mBackListGl, 5, mBacUiCount, BACK_LINE_COUNT);
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
    }

}
