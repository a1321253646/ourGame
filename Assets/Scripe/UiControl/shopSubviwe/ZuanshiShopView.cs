using UnityEngine;
using System.Collections;

public class ZuanshiShopView :  ShopSubviewBase
{
    public override int getTepy()
    {
        return ShopSubviewBase.SHOP_TYPE_ZUANSHI;
    }
    public override void initEnd()
    {
        Debug.Log("ZuanshiShopView initEnd");

        if (!SQLHelper.getIntance().isBuyThisDay())
        {
            GameObject.Find("shop_zuanshi_buy_double_root").transform.localScale = new Vector2(1, 1);
        }
        else {
            GameObject.Find("shop_zuanshi_buy_double_root").transform.localScale = new Vector2(0, 0);
        }
    }
    public override void buyEnd(ShopJsonBean item)
    {
        if (item.itemtype == ShopSubviewBase.SHOP_TYPE_ZUANSHI && !SQLHelper.getIntance().isBuyThisDay()) {

            SQLHelper.getIntance().updateBuyDayValue();
            GameObject.Find("shop_zuanshi_buy_double_root").transform.localScale = new Vector2(0, 0);
            for (int i = 0; i < mControl.Length; i++)
            {
                ShopItemControl item2 = mControl[i];
                item2.init(mList[i]);
            }
        }
    }

}