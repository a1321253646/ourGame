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
    private Text mText;
    public long id = -1;
    private long count;
    private long mMaxCout = 1;
    PlayerBackpackBean bean;
    private Button mBt;
    public bool isHero = false;
    void Start()
    {
 //       Debug.Log("GoodControl Start id = " + id );
        mImage = GetComponentsInChildren<Image>()[1];
        mText = GetComponentInChildren<Text>();
        mBt = GetComponent<Button>();
//        Debug.Log("mBt = " + mBt);
        if (mImage != null)
        {
            if (id == -1)
            {
                mImage.sprite = null;
                if (mText != null) {
                    mText.text = "";
                }
               
            }
            else
            {
                updateUi(id, count);
            }
        }
        if (mBt != null) {
//            Debug.Log("mBt != null ");
            mBt.onClick.AddListener(() => {
                showTip();
            });
        }

        //       Debug.Log("mText = " + mText + "mImage = " + mImage);
    }

    public void showTip() {
        Debug.Log("showTip");
        if (bean == null) {
            return;
        }
        int type = TipControl.COMPOSE_TYPE;
        if (isHero)
        {
            type = TipControl.UNUSE_TYPE;
        }
        else if (bean.tabId == TABID_EQUIP_TYPY)
        {
            type = TipControl.USE_TYPE;
        }
        else if (bean.tabId == TABID_ITEM_TYPE && bean.goodId > 1900000)
        {
            type = TipControl.BOOK_TYPE;
        }
        else if (bean.tabId == TABID_ITEM_TYPE)
        {
            type = TipControl.COMPOSE_TYPE;
        }
        else if (bean.tabId == TABID_CARD_TYPE) {
            type = TipControl.USE_CARD_TYPE;
        }
        else if (bean.tabId == TABID_COMPOSE_TYPE)
        {
            type = TipControl.SHOW_COMPOSE_TYPE;
        }
        BackpackManager.getIntance().showTipUi(bean, count, type);
    }

    public bool isFull() {
        return count == mMaxCout;
    }
    string img = null;
    private long updateUi(long id, long count)
    {
        //       Debug.Log("GoodControl updateUi id = " + id);
        this.id = id;
        if (mImage != null && id != -1)
        {

            if (img == null) {
                if (bean.tabId == TABID_EQUIP_TYPY)
                {
                    img = BackpackManager.getIntance().getAccouterInfoById(id).icon;
                    mMaxCout = BackpackManager.getIntance().getAccouterInfoById(id).stacking;
                }
                else if (bean.tabId == TABID_ITEM_TYPE)
                {
                    img = BackpackManager.getIntance().getGoodInfoById(id).icon;
                    mMaxCout = BackpackManager.getIntance().getGoodInfoById(id).stacking;
                }
                else if (bean.tabId == TABID_CARD_TYPE)
                {
                    img = BackpackManager.getIntance().getCardInfoById(id).icon;
                    mMaxCout = BackpackManager.getIntance().getCardInfoById(id).stacking;
                }
            }
       
            // SpriteRenderer sp1 = mImage.GetComponent<SpriteRenderer>();
            //            Debug.Log("icon = " + mGoodInfo.icon + "mImage = " + mImage);
            mImage.sprite = Resources.
                Load("backpackIcon/" + img, typeof(Sprite)) as Sprite;
            mImage.color = Color.white;
        }
        else if (mImage != null && id == -1) {
            mImage.sprite = null;
            mImage.color = Color.clear;
        }

        return setCount(count);

    }
    long mNeed = 0;
    bool isCompose = false;

    public void updateCount(long count, long need) {
        Debug.Log("updateCount count = " + count + " need=" + need);
        mNeed = need;
        setCount(count);
    }

    public void updateUi(long id,long count, long need,string im) {
        isCompose = true;
        img = im;
        PlayerBackpackBean bean = new PlayerBackpackBean();
        mNeed = need;
        bean.tabId = TABID_COMPOSE_TYPE;
        bean.goodId = id;
        updateUi(id, count,bean);
    } 

    public long updateUi(long id, long count, PlayerBackpackBean bean)
    {
 //       Debug.Log("GoodControl updateUi id = " + id);
        this.bean = bean;
        return updateUi(id, count);
    }

    public bool deleteCount(long count) {
        if (count >= this.count)
        {
            return false;
        }
        else {
            this.count -= count;
            mText.text = "" + this.count;
            return true;
        }
    }

    public long addCount(long count)
    {
        long tmp = count + this.count;
        if (tmp > mMaxCout)
        {
            this.count = mMaxCout;
            mText.text = "" + this.count;
            return tmp - mMaxCout;
        }
        else
        {
            this.count = tmp;
            mText.text = "" + this.count;
            return 0;
        }
    }

    public long setCount(long count2)
    {
        count = count2;
        Debug.Log("updateCount2 count = " + count + " need=" + mNeed);

        string text;
        long value ;
        if (mText == null) {
            mText = GetComponentInChildren<Text>();
        }
        if (isCompose ) {
            if (mNeed != 0)
            {
                mText.text = count + "/" + mNeed;
            }
            else {
                mText.text = "";
            }
            return 0;
        }
        if (id != -1 && count > mMaxCout)
        {
            this.count = mMaxCout;
            text  = "" + this.count;
            value =  count - mMaxCout;
        }
        else
        {
            this.count = count;
            text = "" + this.count;
            value =  0;
        }
        if (mText != null) {
            mText.text = text;
        }
        return value;
    }
}
