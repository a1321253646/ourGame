using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopViewControl : UiControlBase
{

    private static int SHOW_TEHUI = 1;
    private static int SHOW_ZUANSHI = 2;
    private static int SHOW_HUNJIN = 3;
    private static int SHOW_LUNHUI = 4;

    private GameObject mTehuiShow, mHunjinShow, mLunhuiShow, mZuanshiShow;
    private Button  mTehuiUnshow,  mHunjinUnshow, mLunhuiUnshow, mZuanshiUnshow ,mClose;
    private ShopSubviewBase mTehuiContro,mHunjinControl,mLunhuiControl,mZuanshiControl;
    public override void init()
    {
        mControlType = UiControlManager.TYPE_SHOP;
        mTehuiShow = GameObject.Find("shop_tehui_show");
        mTehuiUnshow = GameObject.Find("shop_tehui_unshow").GetComponent<Button>();
        mZuanshiShow = GameObject.Find("shop_zuanshi_show");
        mZuanshiUnshow = GameObject.Find("shop_zuanshi_unshow").GetComponent<Button>();
        mHunjinShow = GameObject.Find("shop_mojin_show");
        mHunjinUnshow = GameObject.Find("shop_mojin_unshow").GetComponent<Button>();
        mLunhuiShow = GameObject.Find("shop_lunhui_show");
        mLunhuiUnshow = GameObject.Find("shop_lunhui_unshow").GetComponent<Button>();

        mTehuiUnshow.onClick.AddListener(() =>
        {
            showView(SHOW_TEHUI);
        });
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


        mClose = GameObject.Find("shop_root_close").GetComponent<Button>();

        mTehuiContro = GameObject.Find("shop_tehui_view").GetComponent<ShopSubviewBase>();
        mZuanshiControl = GameObject.Find("shop_zuanshi_view").GetComponent<ShopSubviewBase>();
        mHunjinControl = GameObject.Find("shop_mojing_view").GetComponent<ShopSubviewBase>();
        mLunhuiControl = GameObject.Find("shop_lunhui_view").GetComponent<ShopSubviewBase>();

        mTehuiContro.init();
        mZuanshiControl.init();
        mHunjinControl.init();
        mLunhuiControl.init();

    }

    private void showView(int type) {
        closeAll();
        if (type == SHOW_TEHUI)
        {
            mTehuiContro.transform.localScale = new Vector2(1, 1);
            mTehuiShow.transform.localScale = new Vector2(1, 1);


        }
        else if (type == SHOW_ZUANSHI) {
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
    }

    private void closeAll() {

        mTehuiShow.transform.localScale = new Vector2(0,0);
        mZuanshiShow.transform.localScale = new Vector2(0,0);
        mHunjinShow.transform.localScale = new Vector2(0,0);
        mLunhuiShow.transform.localScale = new Vector2(0,0);


        mTehuiContro.transform.localScale = new Vector2(0,0);
        mZuanshiControl.transform.localScale = new Vector2(0,0);
        mHunjinControl.transform.localScale = new Vector2(0,0);
        mLunhuiControl.transform.localScale = new Vector2(0,0);
    }

    public override void show()
    {
        gameObject.transform.localPosition = new Vector2(0, 0);
    }
}
