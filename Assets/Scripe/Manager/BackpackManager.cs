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
    private List<CardJsonBean> mCardInfoList;


    private RectTransform mTipTranform;
    private RectTransform mBackpack;
    private RectTransform mHeroTranform;
    private RectTransform mComposeTranform;
    private RectTransform mSamsaraTranform;
    private RectTransform mLunHuiTips;

    private IvertoryControl mInvertoryControl;
    private TipControl mTipControl;
    private HeroRoleControl mHeroControl;
    private CardShowControl mCardControl;
    private LuiHuiTips mLunhuiControl;

    private ComposeControl mComposeControl;
    private SamsaraManage mSamsaraControl;
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
        mCardInfoList = JsonUtils.getIntance().getCardInfos();
        mBackpack = GameObject.Find("Backpack").GetComponent<RectTransform>();
        mInvertoryControl = mBackpack.GetComponentInChildren<IvertoryControl>();

        mTipTranform = GameObject.Find("tip").GetComponent<RectTransform>();
        mTipControl = mTipTranform.GetComponent<TipControl>();

        mLunHuiTips = GameObject.Find("lunhui_tips").GetComponent<RectTransform>();
        mLunhuiControl = GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>();

        mHeroTranform = GameObject.Find("hero").GetComponent<RectTransform>();
        mHeroControl = mHeroTranform.GetComponent<HeroRoleControl>();

        mComposeTranform = GameObject.Find("compose_root").GetComponent<RectTransform>();
        mComposeControl = mComposeTranform.GetComponent<ComposeControl>();

        mSamsaraTranform = GameObject.Find("lunhui").GetComponent<RectTransform>();
        mSamsaraControl = GameObject.Find("lunhui").GetComponent<SamsaraManage>();
        mCardControl = GameObject.Find("Card2").GetComponent<CardShowControl>();
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

    public CardJsonBean getCardInfoById(long id) {
        foreach (CardJsonBean bean in mCardInfoList)
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

    public bool use(PlayerBackpackBean bean, long count,int type) {
        if (type == TipControl.USE_TYPE)
        {
            InventoryHalper.getIntance().use(bean, count);
            mInvertoryControl.update();
            mLevel.ChangeEquip(InventoryHalper.getIntance().getRoleUseList());
            mHeroControl.upDateUi();
        }
        else if (type == TipControl.UNUSE_TYPE)
        {
            InventoryHalper.getIntance().unUse(bean, count);
            mInvertoryControl.update();
            mLevel.ChangeEquip(InventoryHalper.getIntance().getRoleUseList());
            mHeroControl.upDateUi();
        }
        else if (type == TipControl.COMPOSE_TYPE)
        {
            composeUiShowClick();
        }
        else if (type == TipControl.BOOK_TYPE)
        {
            long id = InventoryHalper.getIntance().useBook(bean, count);
            bool isSuccess = InventoryHalper.getIntance().getIsAddSuccess();
            mInvertoryControl.update();
            if (!isSuccess)
            {
                ComposeJsonBen book = JsonUtils.getIntance().gettComposeInfoForId(id);
                GameManager.getIntance().mCurrentCrystal += book.compensate;
                GameManager.getIntance().updataGasAndCrystal();
            }
            else
            {
                mComposeControl.updateListPart();
            }
        }
        else if (type == TipControl.USE_CARD_TYPE)
        {
            int userCardCount = InventoryHalper.getIntance().getUsercard().Count;
            float maxCount = JsonUtils.getIntance().getConfigValueForId(100016);
            Debug.Log("userCardCount== " + userCardCount + " maxCount==" + maxCount);

            if (userCardCount >= maxCount) {
                return false;
            }
            InventoryHalper.getIntance().useCard(bean, count);
            mInvertoryControl.update();
            mCardControl.upDateUi();
        }
        else if (type == TipControl.UNUSE_CARD_TYPE) {
            InventoryHalper.getIntance().removeUserCard(bean.goodId);
            mInvertoryControl.update();
            mCardControl.upDateUi();
        }
        return true;
    }

    public void upLunhui() {
        mLevel.upLunhui();
        mSamsaraControl.updata();
    }

    public void addGoods(long id, int count) {
        Debug.Log("addGoods id=" + id+ " count="+ count);
        bool isAddNiew = InventoryHalper.getIntance().addInventory(id, count);
        Debug.Log("isAddNiew ="+isAddNiew);
        if (isAddNiew)
        {
 //           Debug.Log("update " );
            mInvertoryControl.update();
        }
        else {
   //         Debug.Log("addGood ");
            mInvertoryControl.addGood(id, count);
        }
        upDataComposeControl();
    }

    public void upDataComposeControl() {
        mComposeControl.updataUi();
    }

    public void updateHeroControl() {
        mHeroControl.upDateUi();
    }

    public void deleteGoods(long id, int count) {
        InventoryHalper.getIntance().deleteIventory(id, count);
        mInvertoryControl.deleteGood(id, count);
        upDataComposeControl();
    }

    private static BackpackManager mIntance = new BackpackManager();
    public static BackpackManager getIntance()
    {
        return mIntance;
    }


    public void showTipUi(PlayerBackpackBean bean, long count,int type)
    {
        mTipControl.setShowData(bean, count, type);
        removeLunhuiTipUi();
    }
    public void removeTipUi()
    {
        mTipControl.removeUi();
    }
    public void showLunhuiTipUi()
    {
        mLunhuiControl.showUi();
        removeTipUi();
    }
    private void removeLunhuiTipUi()
    {
        mLunhuiControl.removeUi();
    }


    public void composeUiShowClick()
    {
        mComposeControl.click();
        removeTipUi();
        removeLunhuiTipUi();
    }

    public void heroUiShowClick()
    {
        mHeroControl.click();
        removeTipUi();
        removeLunhuiTipUi();
    }
    public void cardUiShowClick()
    {
        mCardControl.click();
        removeTipUi();
        removeLunhuiTipUi();
    }


    public void samsaraShowClick()
    {
        mSamsaraControl.click();
        removeTipUi();
        removeLunhuiTipUi();
    }

    public void packUiShowClick()
    {
        mInvertoryControl.click();
        removeTipUi();
        removeLunhuiTipUi();
    }

    public void removeAll() {
        removeTipUi();
        removeLunhuiTipUi();
        mInvertoryControl.removeUi();
        mSamsaraControl.removeUi();
        mCardControl.removeUi();
        mHeroControl.removeUi();
        mComposeControl.removeUi();
    }
}