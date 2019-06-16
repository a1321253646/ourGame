using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract  class ShopSubviewBase : MonoBehaviour
{
    public static int SHOP_TYPE_ZUANSHI = 2;
    public static int SHOP_TYPE_HUNJING = 4;
    public static int SHOP_TYPE_LUNHUI = 3;

    public List<ShopJsonBean> mList = new List<ShopJsonBean>();
    public ShopItemControl[] mControl;

    public  void init() {
        long type = getTepy();
        if (mList.Count == 0)
        {
            List<ShopJsonBean> list = JsonUtils.getIntance().getShopList();
            Debug.Log("==========================list= " + list.Count);
            
            foreach (ShopJsonBean item in list)
            {
                if (item.itemtype == type)
                {
                    mList.Add(item);
                }
            }
            mControl = GetComponentsInChildren<ShopItemControl>();

        }
        for (int i = 0; i < mControl.Length; i++)
        {
            ShopItemControl item = mControl[i];
            item.init(mList[i]);
        }

        initEnd();
    }
    public virtual void initEnd()
    {
    }
    public virtual void buyEnd(ShopJsonBean item)
    {

    }

    public abstract int getTepy();
}
