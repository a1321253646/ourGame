using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GoodControl : MonoBehaviour {

    // Use this for initialization

    public static int TABID_EQUIP_TYPY = 2;
    public static int TABID_ITEM_TYPE = 1;
    public static int TABID_CARD_TYPE = 3;
    public static int TABID_COMPOSE_TYPE = 4;


    private Image mImage;
    private Image mBack;
    private Image mPoint;
    private Text mLv;
    public long id = -1;
    private long count;
    private long mMaxCout = 1;
    private long mStart = 0;
    PlayerBackpackBean bean;
    private Button mBt;
    bool isShowPoint = false;

    Image[] mStarts = new Image[5];

    void Start()
    {
        //       Debug.Log("GoodControl Start id = " + id );

        initUi();
        //       Debug.Log("mText = " + mText + "mImage = " + mImage);
    }

    private void initUi() {
        if(mPoint != null) {
            return;
        }

        Image[] ims = GetComponentsInChildren<Image>();
        mImage = ims[1];
        mBack = ims[0];
        mPoint = ims[2];

        mStarts[0] = ims[3];
        mStarts[1] = ims[4];
        mStarts[2] = ims[5];
        mStarts[3] = ims[6];
        mStarts[4] = ims[7];



        mBt = GetComponent<Button>();
        mLv = GetComponentsInChildren<Text>()[0];
        //        Debug.Log("mBt = " + mBt);
        if (mImage != null)
        {
            updateUi(id, count);
        }
        if (mBt != null)
        {
            //            Debug.Log("mBt != null ");
            mBt.onClick.AddListener(() => {

                showTip();
            });
        }
    }

    public void showTip() {
       
        if (bean == null) {
            return;
        }
        GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_OBJECT_CLICK, bean.goodId);
        int type = TipControl.COMPOSE_TYPE;
        if (bean.goodType == SQLDate.GOOD_TYPE_ZHUANGBEI)
        {
            type = TipControl.UNUSE_TYPE;
        }
        else if (bean.goodType == SQLDate.GOOD_TYPE_BACKPACK)
        {
            type = TipControl.USE_TYPE;
        }
        if (isShowPoint) {
            setPointShow(false);
            InventoryHalper.getIntance().updatePoint(bean);
        }
        Debug.Log("showTip  bean.goodid= "+bean.goodId);
        BackpackManager.getIntance().showTipUi(bean, count, type);
    }

    public void setPointShow(bool show) {
        isShowPoint = show;
        if (mPoint == null) {
            mPoint = GetComponentsInChildren<Image>()[2];
        }
        if (show)
        {
            mPoint.color = new Color(0xff, 0xff, 0xff, 0xff);
        }
        else {
            mPoint.color = new Color(0xff, 0xff, 0xff, 0);
        }
    }

    public bool isFull() {
        return count == mMaxCout;
    }
    string img = null;
    private long updateUi(long id, long count)
    {
        initUi();
        this.id = id;
        if (mImage != null && id != -1)
        {
            string back = "UI_yellow/box_gride";
          //  if (img == null) {
                if (bean.tabId == TABID_EQUIP_TYPY)
                {
                    img = InventoryHalper.getIntance().getIcon(id);
                AccouterJsonBean bean = JsonUtils.getIntance().getAccouterInfoById(id);
                    mMaxCout = bean.stacking;
                    back = "UI_yellow/box_gride_" + bean.quality;
                    mStart = bean.stars;
                }
                else if (bean.tabId == TABID_ITEM_TYPE)
                {
                    img = InventoryHalper.getIntance().getIcon(id);
                    mMaxCout = BackpackManager.getIntance().getGoodInfoById(id).stacking;
                }
                else if (bean.tabId == TABID_CARD_TYPE)
                {
                    img = InventoryHalper.getIntance().getIcon(id);
                    mMaxCout = BackpackManager.getIntance().getCardInfoById(id).stacking;
                }
       //     }
       
            // SpriteRenderer sp1 = mImage.GetComponent<SpriteRenderer>();
            //            Debug.Log("icon = " + mGoodInfo.icon + "mImage = " + mImage);
            mImage.sprite = Resources.
                Load( img, typeof(Sprite)) as Sprite;
            mImage.color = Color.white;
            mBack.sprite = Resources.
                Load(back, typeof(Sprite)) as Sprite;
        }
        else if (mImage != null && id == -1) {
            mImage.sprite = null;
            mImage.color = Color.clear;
            mBack.sprite = Resources.
                Load("UI_yellow/box_gride", typeof(Sprite)) as Sprite;
            mStart = 0;
        }
        long level = 0;
        if (bean != null && bean.attributeList != null && bean.attributeList.Count > 0) {
            foreach (PlayerAttributeBean p in bean.attributeList) {
//                Debug.Log("type = " + p.type + "value = " + p.value);
                if (p.type == 10001) {
                    level = (long)p.value;
                }
            }
            
        }
        if (mLv == null) {
            mLv = GetComponentsInChildren<Text>()[0];
        }
        if (level != 0)
        {
            mLv.text = "+" + level;
        }
        else {
            mLv.text = "" ;
        }
        if (bean != null) {
            if (bean.isShowPoint == 1)
            {
                setPointShow(true);
            }
            else
            {
                setPointShow(false);
            }
        }
        showStart();
        return 0;

    }
    long mNeed = 0;
    bool isCompose = false;

    public long updateUi(long id, long count, PlayerBackpackBean bean)
    {
        //       Debug.Log("GoodControl updateUi id = " + id);
        this.bean = bean;
 //       if (this.bean != null) {
//            Debug.Log("updateUi bean.id=" + this.bean.goodId);
 //       }
        
        return updateUi(id, count);
    }

    private void showStart()
    {

        long type = 0;
        if (mStart > 0) {
            type =( mStart-1) / 5;
        }
        long start = mStart % 5;
        if (mStart >4 && start == 0) {
            start = 5;
        }
        string path = "35";
        if (type == 0)
        {
            path = "35";
        }
        else if (type == 1)
        {
            path = "36";
        }
        else if (type == 2)
        {
            path = "42";
        }else if (type == 3)
        {
            path = "37";
        }
        path = "UI_yellow/zhujiemian/" + path;

        for (int i = 0; i < 5; i++) {
            if (i < start)
            {
                mStarts[i].transform.localScale = new Vector2(1, 1);
                mStarts[i].sprite =Resources.Load(path, typeof(Sprite)) as Sprite;
            }
            else {
                mStarts[i].transform.localScale = new Vector2(0, 0);
            }
        }

    }

}
