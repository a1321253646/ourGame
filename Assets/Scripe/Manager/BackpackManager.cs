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
    private Dictionary<long, PlayerBackpackBean> mHeroEquip;
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

    LevelManager mLevel;
    private BackpackManager()
    {

    }

    public void init(LevelManager manager) {
        mLevel = manager;
        mInventoryList = InventoryHalper.getIntance().getInventorys();
        mHeroEquip = InventoryHalper.getIntance().getRoleUseList();
        mGoodInfoList = JsonUtils.getIntance().getGoodInfoList();
        mAccouterInfoList = JsonUtils.getIntance().getAccouterInfoList();
        mBackpack = GameObject.Find("Backpack").GetComponent<RectTransform>();
        mInvertoryControl = mBackpack.GetComponentInChildren<IvertoryControl>();

        mTipTranform = GameObject.Find("tip").GetComponent<RectTransform>();
        mTipControl = mTipTranform.GetComponent<TipControl>();

        mHeroTranform = GameObject.Find("hero").GetComponent<RectTransform>();
        mHeroControl = mHeroTranform.GetComponent<HeroRoleControl>();

        mComposeTranform = GameObject.Find("compose_root").GetComponent<RectTransform>();
        mComposeControl = mComposeTranform.GetComponent<ComposeControl>();
        mLevel.ChangeEquip(InventoryHalper.getIntance().getRoleUseList());
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

    public PlayControl getHero() {
        return mLevel.mPlayerControl;
    }

    public List<PlayerBackpackBean> getInventoryInfos() {
        return mInventoryList;
    }
    public Dictionary<long, PlayerBackpackBean> getHeroEquipInfo() {
        return mHeroEquip;
    }

    public void use(PlayerBackpackBean bean, long count) {
        InventoryHalper.getIntance().use(bean, count);
        mHeroControl.upDateUi();
        mInvertoryControl.update();
        mLevel.ChangeEquip(InventoryHalper.getIntance().getRoleUseList());
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


    public void showTipUi(PlayerBackpackBean bean, long count)
    {
        mTipControl.setShowData(bean, count);

    }
    private void removeTipUi()
    {
        mTipControl.removeUi();
    }

    public void composeUiShowClick()
    {
        if (mComposeControl.isShow)
        {
            removeComposeUi();
        }
        else
        {
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

    public void heroUiShowClick()
    {
        if (mHeroControl.isShow)
        {
            removeHeroUi();
        }
        else
        {
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

    public void packUiShowClick()
    {
        if (mInvertoryControl.isShow)
        {
            removeBackpackUi();
        }
        else
        {
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