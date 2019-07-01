using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemControl : MonoBehaviour
{
    ShopJsonBean mBean;
    Button mBuy;
    Text mDouble, mPrice, mCount;
    Image mIcon;
    public void init(ShopJsonBean bean) {
        if (mBean == null) {


            mBean = bean;
            Debug.Log("____________ mBean.id=" + mBean.id + "  mBean.parameter" + mBean.parameter + "  mBean.price" + mBean.price + "  mBean.itemtype" + mBean.itemtype + "  mBean.costtype" + mBean.costtype);
            mBuy = GetComponentInChildren<Button>();
            Text[] texts = GetComponentsInChildren<Text>();
            mIcon = GetComponentsInChildren<Image>()[2];
            mDouble = texts[2];
            mPrice = texts[1];
            mCount = texts[0];
            if (mBean.costtype == 1)
            {
                mPrice.text = "￥" + mBean.price;
            }
            else {
                mPrice.text =  mBean.price +"钻";
            }
            
            mDouble.transform.transform.localScale = new Vector2(0, 0);

            if (mBean.id == 2001)
            {
                mIcon.sprite = Resources.Load("UI_yellow/shop/11", typeof(Sprite)) as Sprite;
            }
            else if (mBean.id == 2002)
            {
                mIcon.sprite = Resources.Load("UI_yellow/shop/12", typeof(Sprite)) as Sprite;
            }
            else if (mBean.id == 2003)
            {
                mIcon.sprite = Resources.Load("UI_yellow/shop/13", typeof(Sprite)) as Sprite;
            }
            else if (mBean.id == 2004)
            {
                mIcon.sprite = Resources.Load("UI_yellow/shop/13", typeof(Sprite)) as Sprite;
            }
            else if (mBean.id == 3001)
            {
                mIcon.sprite = Resources.Load("UI_yellow/shop/17", typeof(Sprite)) as Sprite;
            }
            else if (mBean.id == 3002)
            {
                mIcon.sprite = Resources.Load("UI_yellow/shop/18", typeof(Sprite)) as Sprite;
            }
            else if (mBean.id == 3003)
            {
                mIcon.sprite = Resources.Load("UI_yellow/shop/19", typeof(Sprite)) as Sprite;
            }
            else if (mBean.id == 3004)
            {
                mIcon.sprite = Resources.Load("UI_yellow/shop/19", typeof(Sprite)) as Sprite;
            }
            else if (mBean.id == 4001)
            {
                mIcon.sprite = Resources.Load("UI_yellow/shop/14", typeof(Sprite)) as Sprite;
            }
            else if (mBean.id == 4002)
            {
                mIcon.sprite = Resources.Load("UI_yellow/shop/15", typeof(Sprite)) as Sprite;
            }
            else if (mBean.id == 4003)
            {
                mIcon.sprite = Resources.Load("UI_yellow/shop/16", typeof(Sprite)) as Sprite;
            }
            else if (mBean.id == 4004)
            {
                mIcon.sprite = Resources.Load("UI_yellow/shop/16", typeof(Sprite)) as Sprite;
            }

            mBuy.onClick.AddListener(() =>
            {
                if (mBean.costtype == 1)
                {
                    PayControl.getIntance().buy(SQLHelper.getIntance().mToken, SQLHelper.getIntance().mPlayName, mBean.sku, mBean.sku, mBean.price + "");
                }
                else {
                    Debug.Log("SQLHelper.getIntance().mZuanshi=" + SQLHelper.getIntance().mZuanshi.toString());
                    Debug.Log("mBean.price=" + mBean.price);
                    Debug.Log("SQLHelper.getIntance().mZuanshi.ieEquit(BigNumber.getBigNumForString(mBean.price +))=" + SQLHelper.getIntance().mZuanshi.ieEquit(BigNumber.getBigNumForString(mBean.price + "")));

                    if (SQLHelper.getIntance().mZuanshi.ieEquit(BigNumber.getBigNumForString(mBean.price + "")) != -1)
                    {

                        GameObject.Find("shop_root").GetComponent<ShopViewControl>().buySuccess(mBean);
                    }
                    else {
                        GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showUi("钻石不足，需要立即购买钻石吗？", LuiHuiTips.TYPPE_BUY_ZUANSHI);
                        GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showSelf();
                    }
                }
            });

        }
        /*if (mBean.costtype == 1) {
            if (GameManager.getIntance().mSkusList == null || GameManager.getIntance().mSkusList.Count == 0)
            {
                mBuy.interactable = false;
                mPrice.text = "???";
            }
            else {
                bool isHave = false;
                foreach (SkuJsonBean sku in GameManager.getIntance().mSkusList) {
                    if (sku.sku.Equals(mBean.sku)) {
                        mPrice.text = sku.price;
                        mBuy.interactable = true;
                        isHave = true;
                        break;
                    }
                }
                if (!isHave) {
                    mBuy.interactable = false;
                    mPrice.text = "???";
                }
            }
        }*/

        long par = long.Parse(mBean.parameter);
        BigNumber add = null;
        if (bean.itemtype == ShopSubviewBase.SHOP_TYPE_LUNHUI)
        {
            add = BigNumber.getBigNumForString(AdUiControl.getAdValue(ActiveButtonControl.TYPE_AD_LUNHUI));
            add = BigNumber.multiply(add, par);
            mCount.text = add.toStringWithUnit();
        }
        else if (bean.itemtype == ShopSubviewBase.SHOP_TYPE_HUNJING)
        {
            add = BigNumber.getBigNumForString(AdUiControl.getAdValue(ActiveButtonControl.TYPE_AD_HUIJING));
            add = BigNumber.multiply(add, par);
            mCount.text = add.toStringWithUnit();
        }
        else if (bean.itemtype == ShopSubviewBase.SHOP_TYPE_ZUANSHI)
        {
            mCount.text = mBean.parameter;
        }


        if (mBean.itemtype == ShopSubviewBase.SHOP_TYPE_ZUANSHI && !SQLHelper.getIntance().isBuyThisDay())
        {
            mDouble.transform.transform.localScale = new Vector2(1, 1);
        }
        else {
            mDouble.transform.transform.localScale = new Vector2(0, 0);
        }
        
    }
}
