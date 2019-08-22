using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardShowControl : MonoBehaviour
{

    private Button mClose,mLeftUp,mLeftDown,mRightUp,mRightDown;    
    private int mUserUiCount = 0;
    GridLayoutGroup  mBackListGl;
    GridLayoutGroup mUserListGl;
    List<GameObject> mUserListGb = new List<GameObject>();
    List<GameObject> mBackListGb = new List<GameObject>();
    CardUserOrUnUserControl[] mUserArray;
    GoodControl[] mBackArray;
    public GameObject CardObject;
    public GameObject DoubleCardObject;
    public GameObject mUserShow;
    public GameObject mBackShow;
    private int USER_LINE_COUNT = 4;
    private int BACK_LINE_COUNT = 4;

    public void guideBack(long value)
    {
        for (int i = 0; i < mBackListGb.Count; i++)
        {
            CardUiControl ui = mBackListGb[i].GetComponent<CardUiControl>();
            if (ui.mCardId == value)
            {
               // GameManager.getIntance().getGuideManager().ShowGuideGrideLayoutInScroll(mBackListGb[i], mBackScroll, mBackListGl, i, 10);
                break;
            }
        }
    }

    LevelManager mLevelManager;
    Text mUserCount ;
    //ScrollRect mUserScroll;
    //ScrollRect mBackScroll;
    Transform mRoot;

    private long mAllUserListCount = 0;

    public void upDateUi() {
        if (mUserListGl == null)
        {
            return;
        }
        updateBackCard();
        updateUserd();
    }
    private void updateUserd() {
        clearUserUi();
        if (mUserListGb.Count == 0)
        {
            upDataCardCount();
            mUserCount.text = 0 + "/" + mAllUserListCount;
        }
        
        List<PlayerBackpackBean> user = InventoryHalper.getIntance().getUsercard();

        foreach (PlayerBackpackBean bean in user)
        {
            addUserUi(bean);
        }
    }
    public void upDataCardCount() {
        if (mUserListGl == null)
        {
            return;
        }
        long count =  getCardCount();
        long oldCount = mUserListGb.Count;
        for (int i = 0; i < count - oldCount; i++)
        {
            GameObject good = GameObject.Instantiate(CardObject,
                  new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            CardUiControl ui = good.GetComponent<CardUiControl>();
          
            good.transform.parent = mUserListGl.transform;
            good.transform.localScale = Vector2.one;

            good.AddComponent<ItemOnDrag>();
            good.GetComponent<ItemOnDrag>().init();
            mUserListGb.Add(good);
            ui.init(-1, 105, 143);
            ui.init(-1, CardUiControl.TYPE_CARD_PLAY, mLevelManager.mPlayerControl);
        }
        setWitch(mUserListGl, mUserListGb.Count);
        

    }

    private long getCardCount() {
        long cardCount = (long)JsonUtils.getIntance().getConfigValueForId(100016);
        long luihuiLevel = InventoryHalper.getIntance().getSamsaraLevelById(7);
        long value = 0;

        if (luihuiLevel != BaseDateHelper.encodeLong(0))
        {
            List<SamsaraValueBean> list = JsonUtils.getIntance().getSamsaraVulueInfoByIdAndLevel(7, BaseDateHelper.decodeLong(luihuiLevel));
            foreach (SamsaraValueBean bean in list)
            {
                if (bean.type == 500005)
                {
                    value = (long)bean.value;
                    break;
                }
            }
            if (value != 0)
            {

                cardCount += value;
            }
            
        }
        mAllUserListCount = cardCount;
        return cardCount;
    }

    private void updateBackCard() {
        clearBackUi();
        if (mBackListGb.Count == 0)
        {
            for (int i = 0; i < 7; i++)
            {
                GameObject good = GameObject.Instantiate(DoubleCardObject,
                      new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                CardDoubleUiControl ui = good.GetComponent<CardDoubleUiControl>();

               
                good.transform.parent =
                    mBackListGl.transform;
                good.transform.localScale = Vector2.one; ;
                mBackListGb.Add(good);
                ui.init(mLevelManager);
//                good.transform.GetChild(0).Translate(Vector2.down * (10));
            }
          //  SetGridHeight(mBackListGl, 1, mBackListGb.Count, mBackListGb.Count/2);
        }
        List<PlayerBackpackBean> list =  InventoryHalper.getIntance().getInventorys();
        foreach (PlayerBackpackBean bean in list) {
            Debug.Log("bean.id = " + bean.goodId + " bean.goodType=" + bean.goodType);
            if ( bean.goodType == SQLDate.GOOD_TYPE_CARD) {
                for(int i = 0; i< bean.count; i++) {
                    addBackUi(bean);
                }              
            }
        }

    }
    bool isGuide = false;


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

    private int mUserCardCount = 0;
    private void addUserUi(PlayerBackpackBean card)
    {
        mUserUiCount += 1;       
        for (int i = 0; i < getCardCount(); i++) {
            CardUiControl ui = mUserListGb[i].GetComponent<CardUiControl>();
            if (ui.mCardId == -1) {
                ui.init(card.goodId, CardUiControl.TYPE_CARD_PLAY, mLevelManager.mPlayerControl);
                ui.init(card.goodId, 105, 143);

                mUserCount.text = (i + 1) + "/" + mAllUserListCount;
                // item.init(id, 113, 166);
                ItemOnDrag item = mUserListGb[i].GetComponent<ItemOnDrag>();
                item.mBean = card;
                item.init(mLevelManager.mPlayerControl.mCardManager, card.goodId, false, mLevelManager.mPlayerControl.mCardManager.card, mRoot);
                mUserCardCount = i + 1;
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
        

        int count = 0;
        bool isAdd = false;
        for (; count < mBackListGb.Count; count++)
        {

            CardDoubleUiControl ui = mBackListGb[count].GetComponent<CardDoubleUiControl>();

            if (ui.init(bean, 91, 125, mLevelManager, CardUiControl.TYPE_CARD_PLAY, mLevelManager.mPlayerControl, mRoot)) {
                isAdd = true;
                break;
            }          
        }
        Debug.Log("addBackUi = " + count+ "=================================");
        if (!isAdd) {
            GameObject good = GameObject.Instantiate(DoubleCardObject,
                new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            CardDoubleUiControl ui = good.GetComponent<CardDoubleUiControl>();


            good.transform.parent = mBackListGl.transform;
            good.transform.localScale = Vector2.one; ;
            mBackListGb.Add(good);
            ui.init(mLevelManager);
            SetGridHeight(mBackListGl, 1, mBackListGb.Count, mBackListGb.Count );
            ui.init(bean, 91, 125, mLevelManager, CardUiControl.TYPE_CARD_PLAY, mLevelManager.mPlayerControl, mRoot);
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

        float height = lineCount * grid.cellSize.x;  //行数乘以Cell的高度，3.0f是微调
        height += lineCount * grid.spacing.x;     //每行之间有间隔
        grid.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, height);
        if (mGridHeight != height) {
            mGridHeight = height;
         //   grid.transform.Translate(Vector2.down * (height));
        }
      
    }
    private void setWitch(GridLayoutGroup grid, int count)     //每行Cell的个数
    {
        grid.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 113* count+10*(count-1)+10);
    }

    public void updateBack() {
        updateBack(true);
    }
    public  void updateBack(bool isMust)
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

    public void init()
    {
        mUserListGl = GameObject.Find("user_card_list").GetComponent<GridLayoutGroup>();
        mBackListGl = GameObject.Find("cardList").GetComponent<GridLayoutGroup>();
        mUserCount = GameObject.Find("title01").GetComponent<Text>();
        mLevelManager = GameObject.Find("Manager").GetComponent<LevelManager>();
   //     mUserScroll = GameObject.Find("user_card_list_root").GetComponent<ScrollRect>();
   //     mBackScroll = GameObject.Find("back_card_list_root").GetComponent<ScrollRect>();
        mRoot = GameObject.Find("Canvas").GetComponent<Transform>();
        mClose = GameObject.Find("card_show_card_close").GetComponent<Button>();
       
        mClose.onClick.AddListener(() => {
            UiControlManager.getIntance().remove(UiControlManager.TYPE_CARD);
        });

        mRightDown = GameObject.Find("card2_right_down").GetComponent<Button>();
        mRightUp = GameObject.Find("card2_right_up").GetComponent<Button>();
        mLeftDown = GameObject.Find("card2_left_down").GetComponent<Button>();
        mLeftUp = GameObject.Find("card2_left_up").GetComponent<Button>();

        mLeftDown.onClick.AddListener(() => {
            Debug.Log("move mRightDown");
            if (7 > mBackListGb.Count || downMove == 0)
            {
                return;
            }
            if (downMove > 7)
            {
                move(false, mBackListGl, mBackShow, 7);
                downMove -= 7;
            }
            else
            {
                move(false, mBackListGl, mBackShow, downMove);
                downMove = 0;
            }

        });
        mRightDown.onClick.AddListener(() => {
            Debug.Log("move mLeftUp");
            if (7 >= mBackListGb.Count || mBackListGb.Count - downMove == 7)
            {
                return;
            }
            if (mBackListGb.Count - downMove - 7 > 7)
            {
                move(true, mBackListGl, mBackShow, 7);
                downMove += 7;
            }
            else
            {
                move(true, mBackListGl, mBackShow, mBackListGb.Count - 7 - downMove);
                downMove = mBackListGb.Count - 7;
            }
        });
        mLeftUp.onClick.AddListener(() => {
            Debug.Log("move mRightUp upMove="+ upMove);
            if (6 >= mUserCardCount || upMove == 0)
            {
                return;
            }
            if ( upMove > 6)
            {
                move(false, mUserListGl, mUserShow, 6);
                upMove -= 6;
            }
            else
            {
                move(false, mUserListGl, mUserShow,  upMove);
                upMove = 0;
            }


        });
        mRightUp.onClick.AddListener(() => {
            Debug.Log("move mLeftUp");
            if (6 >= mUserCardCount || mUserListGb.Count - upMove == 6)
            {
                return;
            }
            if (mUserListGb.Count -6 - upMove > 6)
            {
                move(true, mUserListGl, mUserShow, 6);
                upMove += 6;
            }
            else
            {
                move(true, mUserListGl, mUserShow, mUserListGb.Count - 6 - upMove);
                upMove = mUserListGb.Count - 6;
            }
        });



    }
    int upMove = 0;
    int downMove = 0;
    private void move(bool isLeft , GridLayoutGroup grid,GameObject show, int count) {

        Debug.Log("move show.gameObject.GetComponent<RectTransform>().rect.width / 2 = "+ show.gameObject.GetComponent<RectTransform>().rect.width / 2);
        Debug.Log("move grid.gameObject.transform.position.x = " + grid.gameObject.transform.position.x);
        Debug.Log("move grid.gameObject.GetComponent<RectTransform>().rect.width / 2= " + grid.gameObject.GetComponent<RectTransform>().rect.width / 2);


        Debug.Log("move GameCamera.SCREEN_BILI= " + myBili);

        if (!isLeft)
        {
            float distance2 = grid.cellSize.x * count + grid.spacing.x * count;
            Debug.Log("move distance2= " + distance2);
            distance2 = distance2* myBili;

            Debug.Log("move distance2= " + distance2);
            grid.transform.Translate(Vector2.right * distance2);
        }
        else {


            float distance3 = grid.cellSize.x * count + grid.spacing.x * count;
            Debug.Log("move distance3= " + distance3);
            distance3 = distance3 * myBili;
            Debug.Log("move distance3= " + distance3);
           // grid.transform.SetPositionAndRotation(new  Vector2(grid.transform.position.x - distance, grid.transform.position.y), grid.transform.rotation);
            //grid.transform.position.x = grid.transform.position.x - distance;
            grid.transform.Translate(Vector2.left * distance3);
        }
    }
    float myBili = 0;
    private void Start()
    {
        float x = GameObject.Find("Canvas").GetComponent<CanvasScaler>().referenceResolution.x;
        myBili = Screen.width / x;
    }
    public void show()
    {
        if (mUserListGl == null) {
            init();
        }
        gameObject.transform.localScale = new Vector2(1, 1);
        upDateUi();
       // GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_SHOW, GuideManager.SHOW_CARD);
    }
    public void disShow() {
        gameObject.transform.localScale = new Vector2(0, 0);
    }

}
