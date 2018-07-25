using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GoodControl : MonoBehaviour {

    // Use this for initialization

    public static int TABID_EQUIP_TYPY = 2;
    public static int TABID_ITEM_TYPE = 1;


    private Image mImage;
    private Text mText;
    public long id = -1;
    private long count;
    private long mMaxCout = 1;
    PlayerBackpackBean bean;
    private Button mBt;
    void Start()
    {
 //       Debug.Log("GoodControl Start id = " + id );
        mImage = GetComponentsInChildren<Image>()[1];
        mText = GetComponentInChildren<Text>();
        mBt = GetComponent<Button>();
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
        mBt.onClick.AddListener(() => {
            showTip();
        });
        //       Debug.Log("mText = " + mText + "mImage = " + mImage);
    }
    public void showTip() {
        BackpackManager.getIntance().showTipUi(bean, count);
    }

    public bool isFull() {
        return count == mMaxCout;
    }
    private long updateUi(long id, long count)
    {
        //       Debug.Log("GoodControl updateUi id = " + id);
        this.id = id;
        if (mImage != null)
        {
            string img = null ;
            if (bean.tabId == TABID_EQUIP_TYPY)
            {
                img = BackpackManager.getIntance().getAccouterInfoById(id).icon;
                mMaxCout = BackpackManager.getIntance().getAccouterInfoById(id).stacking;
            }
            else if (bean.tabId == TABID_ITEM_TYPE) {
                img = BackpackManager.getIntance().getGoodInfoById(id).icon;
                mMaxCout = BackpackManager.getIntance().getGoodInfoById(id).stacking;
            }
            // SpriteRenderer sp1 = mImage.GetComponent<SpriteRenderer>();
            //            Debug.Log("icon = " + mGoodInfo.icon + "mImage = " + mImage);
            mImage.sprite = Resources.
                Load("backpackIcon/" + img, typeof(Sprite)) as Sprite;
            mImage.color = Color.white;
        }

        return setCount(count);

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

    public long setCount(long count)
    {
        string text;
        long value ;
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
