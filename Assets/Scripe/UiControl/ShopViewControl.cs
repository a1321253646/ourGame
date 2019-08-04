using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopViewControl : UiControlBase
{

    public static int SHOW_ZUANSHI = 2;
    public static int SHOW_HUNJIN = 3;
    public static int SHOW_LUNHUI = 4;

    private GameObject mHunjinShow, mLunhuiShow, mZuanshiShow;
    private Button    mHunjinUnshow, mLunhuiUnshow, mZuanshiUnshow ,mClose;
    private ShopSubviewBase mHunjinControl,mLunhuiControl,mZuanshiControl;

    public override void show()
    {
        isShow = true;
        if (mZuanshiControl == null) {
            mZuanshiControl = GameObject.Find("shop_zuanshi_view").GetComponent<ShopSubviewBase>();
            mHunjinControl = GameObject.Find("shop_mojing_view").GetComponent<ShopSubviewBase>();
            mLunhuiControl = GameObject.Find("shop_lunhui_view").GetComponent<ShopSubviewBase>();           
            mZuanshiControl.init();
            mHunjinControl.init();
            mLunhuiControl.init();
        }
        gameObject.transform.localPosition = new Vector2(0, 0);

    }


    public override void init()
    {
        mControlType = UiControlManager.TYPE_SHOP;
        mZuanshiShow = GameObject.Find("shop_zuanshi_show");
        mZuanshiUnshow = GameObject.Find("shop_zuanshi_unshow").GetComponent<Button>();
        mHunjinShow = GameObject.Find("shop_mojin_show");
        mHunjinUnshow = GameObject.Find("shop_mojin_unshow").GetComponent<Button>();
        mLunhuiShow = GameObject.Find("shop_lunhui_show");
        mLunhuiUnshow = GameObject.Find("shop_lunhui_unshow").GetComponent<Button>();
        mClose = GameObject.Find("shop_root_close").GetComponent<Button>();

        mZuanshiUnshow.onClick.AddListener(() =>
        {
            showView(SHOW_ZUANSHI);
        });
        mHunjinUnshow.onClick.AddListener(() =>
        {
            showView(SHOW_HUNJIN);
        });
        mLunhuiUnshow.onClick.AddListener(() =>
        {
            showView(SHOW_LUNHUI);
        });

        mClose.onClick.AddListener(() =>
        {
            toremoveUi();
        });

    }
    private int mType = SHOW_HUNJIN;
    public void showView(int type) {

        closeAll();
        mType = type;
        if (type == SHOW_ZUANSHI) {
            mZuanshiControl.transform.localScale = new Vector2(1, 1);
            mZuanshiShow.transform.localScale = new Vector2(1, 1);

        }
        else if (type == SHOW_HUNJIN)
        {
            mHunjinControl.transform.localScale = new Vector2(1, 1);
            mHunjinShow.transform.localScale = new Vector2(1, 1);

        }
        else if (type == SHOW_LUNHUI)
        {
            mLunhuiControl.transform.localScale = new Vector2(1, 1);
            mLunhuiShow.transform.localScale = new Vector2(1, 1);

        }
        updateShow();
    }

    public void updateShow() {
        if (mZuanshiControl == null)
        {
            mZuanshiControl = GameObject.Find("shop_zuanshi_view").GetComponent<ShopSubviewBase>();
            mHunjinControl = GameObject.Find("shop_mojing_view").GetComponent<ShopSubviewBase>();
            mLunhuiControl = GameObject.Find("shop_lunhui_view").GetComponent<ShopSubviewBase>();
            mZuanshiControl.init();
            mHunjinControl.init();
            mLunhuiControl.init();
        }
        if (mType == SHOW_ZUANSHI)
        {
            mZuanshiControl.init();

        }
        else if (mType == SHOW_HUNJIN)
        {
            mHunjinControl.init();

        }
        else if (mType == SHOW_LUNHUI)
        {
            mLunhuiControl.init();

        }
    }

    private void closeAll() {

        mZuanshiShow.transform.localScale = new Vector2(0,0);
        mHunjinShow.transform.localScale = new Vector2(0,0);
        mLunhuiShow.transform.localScale = new Vector2(0,0);

        mZuanshiControl.transform.localScale = new Vector2(0,0);
        mHunjinControl.transform.localScale = new Vector2(0,0);
        mLunhuiControl.transform.localScale = new Vector2(0,0);
    }


    public void buySuccess(ShopJsonBean bean) {
        Debug.Log("buySuccess itemtype="+bean.itemtype);
        if (bean.itemtype == ShopSubviewBase.SHOP_TYPE_ZUANSHI) {
            BigNumber add = BigNumber.getBigNumForString(bean.parameter);
            BigNumber zuanshi = SQLHelper.getIntance().mZuanshi;
            if (!SQLHelper.getIntance().isBuyThisDay()) {          
                add = BigNumber.multiply(add, 2);
            }
            zuanshi = BigNumber.add(zuanshi, add);
            SQLHelper.getIntance().updateZuanshiValue(zuanshi);

            mZuanshiControl.buyEnd(bean);
     
            GameManager.getIntance().updateZuanshi();
            return;   
        }
        BigNumber zuanshi2 = SQLHelper.getIntance().mZuanshi;
        zuanshi2 = BigNumber.minus(zuanshi2, BigNumber.getBigNumForString(bean.price + ""));
        SQLHelper.getIntance().updateZuanshiValue(zuanshi2);
        GameManager.getIntance().updateZuanshi();
        long par = long.Parse(bean.parameter);

        if (bean.itemtype == ShopSubviewBase.SHOP_TYPE_LUNHUI)
        {
            BigNumber lunhui = SQLHelper.getIntance().mLunhuiValue;
            BigNumber add =BigNumber.getBigNumForString( AdUiControl.getAdValue(ActiveButtonControl.TYPE_AD_LUNHUI));
            add = BigNumber.multiply(add, par);
            GameManager.getIntance().mReincarnation = BigNumber.add(lunhui, add);
            SQLHelper.getIntance().updateLunhuiValue(GameManager.getIntance().mReincarnation);
        }
        else if (bean.itemtype == ShopSubviewBase.SHOP_TYPE_HUNJING) {
            Debug.Log("buySuccess");
            BigNumber hunjin = GameManager.getIntance().mCurrentCrystal;
            BigNumber add = BigNumber.getBigNumForString(AdUiControl.getAdValue(ActiveButtonControl.TYPE_AD_HUIJING));
            add = BigNumber.multiply(add, par);
            Debug.Log("buySuccess add="+ add.toStringWithUnit());
            GameManager.getIntance().mCurrentCrystal = BigNumber.add(hunjin, add);
            GameManager.getIntance().updateGasAndCrystal();
        }
    }

    public void getSkusUpdate() {
        if (mZuanshiControl != null)
        {
            mZuanshiControl.getSku();
        }
            
    }
}
