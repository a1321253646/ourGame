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
    private List<AccouterJsonBean> mAccouterInfoList;

    private RectTransform mTipTranform;
    private RectTransform mBackpack;
    private RectTransform mHeroTranform;
    private RectTransform mComposeTranform;

    private IvertoryControl mInvertoryControl;
    private TipControl mTipControl;
    private HeroRoleControl mHeroControl;
    private ComposeControl mComposeControl;
    private BackpackManager()
    {
        mInventoryList = InventoryHalper.getIntance().getInventorys();
        mGoodInfoList = JsonUtils.getIntance().getGoodInfoList();

        mBackpack = GameObject.Find("Backpack").GetComponent<RectTransform>();
        mInvertoryControl = mBackpack.GetComponentInChildren<IvertoryControl>();

        mTipTranform = GameObject.Find("tip").GetComponent<RectTransform>();
        mTipControl = mTipTranform.GetComponent<TipControl>();

        mHeroTranform = GameObject.Find("hero").GetComponent<RectTransform>();
        mHeroControl = mHeroTranform.GetComponent<HeroRoleControl>();

        mComposeTranform = GameObject.Find("compose_root").GetComponent<RectTransform>();
        mComposeControl = mComposeTranform.GetComponent<ComposeControl>();
    }

    public GoodJsonBean getGoodInfoById(long id) {
        foreach (GoodJsonBean bean in mGoodInfoList) {
            if (bean.id == id) {
                return bean;
            }
        }
        return null;
    }

    public AccouterJsonBean getAccouterInfoById(long id)
    {
        foreach (AccouterJsonBean bean in mAccouterInfoList)
        {
            if (bean.id == id)
            {
                return bean;
            }
        }
        return null;
    }

    public List<PlayerBackpackBean> getInventoryInfos() {
        return mInventoryList;
    }

    public void addGoods(long id, int count) {
        bool isAddNiew = InventoryHalper.getIntance().addInventory(id, count);
        if (isAddNiew)
        {
            mInvertoryControl.update();
        }
        else {
            mInvertoryControl.addGood(id, count);
        }
        
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


    private void showTipUi(long id, long count)
    {
        mTipControl.setShowData(id, count);

    }
    private void removeTipUi()
    {
        mTipControl.removeUi();
    }

    private bool isComposeUiShow = false;
    public void composeUiShowClick()
    {
        if (isComposeUiShow)
        {
            isComposeUiShow = false;
            removeComposeUi();
        }
        else
        {
            isComposeUiShow = true;
            showComposeUi();
        }
    }
    private void showComposeUi()
    {
        mComposeControl.showUi();

    }
    private void removeComposeUi()
    {
        mComposeControl.removeUi();
    }

    private bool isHeroUiShow = false;
    public void heroUiShowClick()
    {
        if (isHeroUiShow)
        {
            isHeroUiShow = false;
            removeHeroUi();
        }
        else
        {
            isHeroUiShow = true;
            showHeroUi();
        }
    }
    private void showHeroUi()
    {
        mHeroControl.showUi();

    }
    private void removeHeroUi()
    {
        mHeroControl.removeUi();
    }


    private bool isPackUiShow = false;
    public void packUiShowClick()
    {
        if (isPackUiShow)
        {
            isPackUiShow = false;
            removeBackpackUi();
        }
        else
        {
            isPackUiShow = true;
            showBackpackUi();
        }
    }
    private void showBackpackUi()
    {
        mInvertoryControl.showUi();

    }
    private void removeBackpackUi()
    {
        mInvertoryControl.removeUi();
    }

}