using System;
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
    CardUserOrUnUserControl[] mUserArray;
    GoodControl[] mBackArray;
    public GameObject CardObject;
    private int USER_LINE_COUNT = 4;
    private int BACK_LINE_COUNT = 4;
    private Vector2 mFri;
    LevelManager mLevelManager;
    CardManager mCardManager;
    Text mUserCount ;

    private void Start()
    {
        mUserListGl =  GameObject.Find("user_card_list").GetComponent<GridLayoutGroup>();
        mBackListGl =  GameObject.Find("cardList").GetComponent<GridLayoutGroup>();
        mUserCount =  GameObject.Find("title01").GetComponent<Text>();
        mLevelManager = GameObject.Find("Manager").GetComponent<LevelManager>();
        mCardManager = GameObject.Find("jineng").GetComponent<CardManager>();
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
        if (mUserListGb.Count == 0)
        {
            for (int i = 0; i < JsonUtils.getIntance().getConfigValueForId(100016); i++)
            {
                GameObject good = GameObject.Instantiate(CardObject,
                      new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                CardUiControl ui = good.GetComponent<CardUiControl>();
                good.AddComponent<CardUserOrUnUserControl>();
                good.transform.parent = mUserListGl.transform;
                good.transform.localScale = Vector2.one; ;
                mUserListGb.Add(good);
                ui.init(-1, 113, 166);
                ui.init(-1, CardUiControl.TYPE_CARD_PLAY, mLevelManager.mPlayerControl);
            }
            setWitch(mUserListGl, mUserListGb.Count);
        }
        mUserCount.text = "已装备卡牌（" + mUserListGb.Count + "/30）";
        List<long> user = InventoryHalper.getIntance().getUsercard();

        foreach (long id in user)
        {
            addUserUi(id);
        }
    }

    private void updateBackCard() {
        clearBackUi();
        if (mBackListGb.Count == 0)
        {
            for (int i = 0; i < 20; i++)
            {
                GameObject good = GameObject.Instantiate(CardObject,
                      new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                CardUiControl ui = good.GetComponent<CardUiControl>();
                good.AddComponent<CardUserOrUnUserControl>();
                good.transform.parent = mBackListGl.transform;
                good.transform.localScale = Vector2.one; ;
                mBackListGb.Add(good);
                ui.init(-1, 73, 108);
                ui.init(-1, CardUiControl.TYPE_CARD_PLAY, mLevelManager.mPlayerControl);
                good.transform.GetChild(0).Translate(Vector2.down * (10));
            }
            SetGridHeight(mBackListGl, 2, mBackListGb.Count, 10);
        }
        List<PlayerBackpackBean> list =  InventoryHalper.getIntance().getInventorys();
        foreach (PlayerBackpackBean bean in list) {
            if (bean.tabId == GoodControl.TABID_CARD_TYPE) {
                for(int i = 0; i< bean.count; i++) {
                    addBackUi(bean);
                }              
            }
        }

    }
    private void showUi()
    {
       // upDateUi();
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
    private void addUserUi(long id)
    {
        mUserUiCount += 1;       
        for (int i = 0; i < JsonUtils.getIntance().getConfigValueForId(100016); i++) {
            CardUiControl ui = mUserListGb[i].GetComponent<CardUiControl>();
            if (ui.mCardId == -1) {
                ui.init(id, CardUiControl.TYPE_CARD_PLAY, mLevelManager.mPlayerControl);
                CardUserOrUnUserControl item = mUserListGb[i].GetComponent<CardUserOrUnUserControl>();
                mUserCount.text = "已装备卡牌（" + (i+1) + "/" + JsonUtils.getIntance().getConfigValueForId(100016) + ")";
                item.init(id, 113, 166);
                item.init(mCardManager, id, false);
                break;
            }
        }

     //   GameObject good = GameObject.Instantiate(CardObject,
       //     new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
      //  good.AddComponent<CardUserOrUnUserControl>();
     //   CardUserOrUnUserControl item = good.GetComponent<CardUserOrUnUserControl>();
    //    CardUiControl ui = good.GetComponent<CardUiControl>();
     //   good.transform.parent = mUserListGl.transform;
     //   good.transform.localScale = Vector2.one; ;
     //   mUserListGb.Add(good);
     //   mUserCount.text = "已装备卡牌（" + mUserListGb.Count + "/"+JsonUtils.getIntance().getConfigValueForId(100016)+")";
     //   mUserArray = GetComponentsInChildren<CardUserOrUnUserControl>();
      //  ui.init(id, CardUiControl.TYPE_CARD_PLAY, mLevelManager.mPlayerControl);
      //  item.init(id, 113, 166);
     //   item.init(mCardManager, id, false);
        
        //  SetGridHeight(mUserListGl,3, mUserUiCount,USER_LINE_COUNT);
    }
    private void addBackUi(PlayerBackpackBean bean)
    {
        mBacUiCount += 1;

        int count = 0;
        for (int i = 0; i < mBackListGb.Count; i++)
        {
            count++;
            CardUiControl ui = mBackListGb[i].GetComponent<CardUiControl>();
            if (ui.mCardId == -1)
            {
                ui.init(bean.goodId, CardUiControl.TYPE_CARD_PLAY, mLevelManager.mPlayerControl);
                CardUserOrUnUserControl item = mBackListGb[i].GetComponent<CardUserOrUnUserControl>();             
                item.init(bean.goodId, 73, 108);
                item.init(mCardManager, bean.goodId, true);
                break;
            }            
        }

        if(count > 10) {
            count = count - 1;
            count = (count / 10 + 2)*10;
        }
        if (count > mBackListGb.Count) {
            for (int i = 0; i < count- mBackListGb.Count; i++)
            {
                GameObject good = GameObject.Instantiate(CardObject,
                      new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                CardUiControl ui = good.GetComponent<CardUiControl>();
                good.AddComponent<CardUserOrUnUserControl>();
                good.transform.parent = mBackListGl.transform;
                good.transform.localScale = Vector2.one; ;
                mBackListGb.Add(good);
                ui.init(-1, 73, 108);
                ui.init(-1, CardUiControl.TYPE_CARD_PLAY, mLevelManager.mPlayerControl);
                good.transform.GetChild(0).Translate(Vector2.down * (10));
            }
            SetGridHeight(mBackListGl, 2, mBackListGb.Count, 10);
        }
    }



 /*   private void addBackUi(PlayerBackpackBean bean)
    {
        mBacUiCount += 1;
        GameObject good = GameObject.Instantiate(CardObject,
            new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        good.AddComponent<CardUserOrUnUserControl>();
        CardItemControl item = good.GetComponent<CardItemControl>();
        CardUserOrUnUserControl ui = good.GetComponent<CardUserOrUnUserControl>();
        good.transform.parent = mBackListGl.transform;
        good.transform.localScale = Vector2.one; ;
        mBackListGb.Add(good);
        mUserArray = GetComponentsInChildren<CardItemControl>();
        CardJsonBean card = JsonUtils.getIntance().getCardInfoById(bean.goodId);
        ui.init(bean.goodId, CardUiControl.TYPE_CARD_GOOD, mLevelManager.mPlayerControl);
        item.init(bean.goodId, 73, 108);


//        GoodControl ct = good.GetComponent<GoodControl>();
//        ct.updateUi(bean.goodId, 1, bean);
//        mBackArray = GetComponentsInChildren<GoodControl>();
        SetGridHeight(mBackListGl, 2, mBacUiCount, 11 );
    }*/
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

    public  void updateBack()
    {
        int count = 0;
        foreach (GameObject good in mBackListGb) {
            if (good.GetComponent<CardUiControl>().mCardId != -1)
            {
                count += 1;
            }
            else {
                break;
            }
        }
        int deleteCount = 0;
        if (count < 10 && mBackListGb.Count > 20)
        {
            deleteCount = mBackListGb.Count - 20;
        }
        else if(count >10)
        {
            count = count - 1;
            count = (count / 10 +2)* 10;
            if (mBackListGb.Count > count) {
                deleteCount = mBackListGb.Count - count;
            }
        }

        if (deleteCount > 0) {
            for (int i = deleteCount; i > 0; i--) {
                Destroy(mBackListGb[mBackListGb.Count - 1]);
            }
        }
    }
}
