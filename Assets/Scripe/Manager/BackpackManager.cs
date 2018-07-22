using UnityEngine;
//using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
public class BackpackManager 
{
    public static long ALL_TYPE = 10;
    public static long CAILIAO_TYPE = 1;
    public static long ZHUANGBEI_TYPE = 2;
    public static long KAPAI_TYPE = 3;

    private List<PlayerBackpackBean> mInventoryList ;
    private List<GoodJsonBean> mGoodInfoList;

    private RectTransform mTipTranform;
    private RectTransform mBackpack;
    private IvertoryControl mInvertoryControl;
    private TipControl mTipControl;
    private BackpackManager()
    {
        mInventoryList = InventoryHalper.getIntance().getInventorys();
        mGoodInfoList = JsonUtils.getIntance().getGoodInfoList();
        mBackpack = GameObject.Find("Backpack").GetComponent<RectTransform>();
        mInvertoryControl = mBackpack.GetComponentInChildren<IvertoryControl>();
        mTipTranform = GameObject.Find("tip").GetComponent<RectTransform>();
        mTipControl = mTipTranform.GetComponent<TipControl>();
    }

    public void showTipUi(long id ,long count)
    {
        mTipControl.setShowData(id,count);

    }
    public void removeTipUi()
    {
        mTipControl.removeUi();
    }


    public void showBackpackUi() {
        mInvertoryControl.showUi();

    }
    public void removeUi() {
        mInvertoryControl.removeUi();
    }
    public GoodJsonBean getGoodInfoById(long id) {
        foreach (GoodJsonBean bean in mGoodInfoList) {
            if (bean.id == id) {
                return bean;
            }
        }
        return null;
    }

    public List<PlayerBackpackBean> getInventoryInfos() {
        return mInventoryList;
    }

    public void addGoods(long id, int count) {
        InventoryHalper.getIntance().addInventory(id, count);
        mInvertoryControl.addGood(id, count);
    }

    public void deleteGoods(long id, int count) {
        InventoryHalper.getIntance().deleteIventory(id, count);
        mInvertoryControl.deleteGood(id, count);
    }

    private static BackpackManager mIntance = new BackpackManager();
    public static BackpackManager getIntance()
    {
        return mIntance;
    }


}