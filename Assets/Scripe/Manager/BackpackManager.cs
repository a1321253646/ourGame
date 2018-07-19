using UnityEngine;
//using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
public class BackpackManager 
{
    private List<PlayerBackpackBean> mInventoryList ;
    private List<GoodJsonBean> mGoodInfoList;

    private RectTransform mBackpack;
    private IvertoryControl mInvertoryControl;

    private BackpackManager()
    {
        mInventoryList = InventoryHalper.getIntance().getInventorys();
        mGoodInfoList = JsonUtils.getIntance().getGoodInfoList();
        mBackpack = GameObject.Find("Backpack").GetComponent<RectTransform>();
        mInvertoryControl = mBackpack.GetComponentInChildren<IvertoryControl>();
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