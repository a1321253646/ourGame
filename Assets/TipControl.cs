using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TipControl : MonoBehaviour {

    private long id;
    private long count;
    private GoodControl mGoodControl;
    private Text mName;
    private Text nButtonTx;
    private Text nDepictTx;
    private Image mTipImage;
    private Text mTipCount;
    private Text mTipName;
    PlayerBackpackBean mBean;
    private DepictTextControl mDepoct;
    AccouterJsonBean mAccouter = null;
    GoodJsonBean mGoodJson = null;
    private Button mClose, mActionClick;
    private Text mClickText;
    public static int COMPOSE_TYPE = 1;
    public static int USE_TYPE = 2;
    public static int UNUSE_TYPE = 3;
    public static int BOOK_TYPE = 4;
    private int mCurrentType = 1;
    void Start()
    {
        mActionClick = GameObject.Find("tip_Button").GetComponent<Button>();
        mClose = GameObject.Find("tip_close").GetComponent<Button>();
        mClickText = GameObject.Find("tipButtonTx").GetComponent<Text>();
        mActionClick.onClick.AddListener(() =>
        {
            use();
        });
        mClose.onClick.AddListener(() =>
        {
            removeUi();
        });
    }

    private void use()
    {
        BackpackManager.getIntance().use(mBean, count, mCurrentType);
        removeUi();
    }

    public void setShowData(PlayerBackpackBean bean,long count,int type) {
        mCurrentType = type;
        if (mCurrentType == COMPOSE_TYPE)
        {
            mClickText.text = "合成";
        }
        else if (mCurrentType == USE_TYPE)
        {
            mClickText.text = "穿戴";
        }
        else if (mCurrentType == UNUSE_TYPE)
        {
            mClickText.text = "脱下";
        }
        else if (mCurrentType == BOOK_TYPE) {
            mClickText.text = "使用";
        }
        mBean = bean;
        this.id = bean.goodId;
        this.count = count;
        showUi();
        updataTip();
        updataUi();
    }

    private void updataTip()
    {
        if (mTipImage == null) {
            mTipImage = GameObject.Find("box_icon_tip").GetComponent<Image>();
        }
        if (mTipCount == null) {
            mTipCount = GameObject.Find("box_text_tip").GetComponent<Text>();
        }
        if (mTipName == null)
        {
            mTipName = GameObject.Find("tipName").GetComponent<Text>();
        }
        string img = null;
        string name = null;
        if (mBean.tabId == GoodControl.TABID_EQUIP_TYPY)
        {
            mAccouter = BackpackManager.getIntance().getAccouterInfoById(id);
            img = mAccouter.icon;
            name = mAccouter.name;
        }
        else if (mBean.tabId == GoodControl.TABID_ITEM_TYPE)
        {
            mGoodJson = BackpackManager.getIntance().getGoodInfoById(id);
            img = mGoodJson.icon;
            name = mGoodJson.name;
        }

        mTipCount.text = "" + count;
        mTipName.text = name;
        mTipImage.sprite = Resources.
                 Load("backpackIcon/" + img, typeof(Sprite)) as Sprite;
        mTipImage.color = Color.white;
    }

    private void updataUi() {
        
        
        creatDepictText();
       
    }

    private void creatDepictText() {
        string str = "";
        if (mGoodJson != null) {
            str = str + mGoodJson.describe + "\n";
        }
        if (mAccouter != null && mBean.attributeList != null && mBean.attributeList.Count > 0) {
            str = "";
            foreach (PlayerAttributeBean b in mBean.attributeList) {
                str = str+b.getTypeStr() + ":" + b.value+"\n";
            }
        }
        if (nDepictTx == null)
        {
            mDepoct = GameObject.Find("depict_text").GetComponent<DepictTextControl>();
        }
        mDepoct.setText(str);
    }

    private void showUi()
    {
        gameObject.transform.localPosition = new Vector2(0, 0);
        int level = GameManager.getIntance().getUiLevel();
        gameObject.transform.SetSiblingIndex(level);
    }
    public void removeUi()
    {
        gameObject.transform.localPosition = new Vector2(500f, -386.46f);
    }
}
