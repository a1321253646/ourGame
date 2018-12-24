using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TipControl : MonoBehaviour {

    private long id;
    private long count;
    private GoodControl mGoodControl;
    private Text mName;
    private Text nButtonTx;
    private Text nDepictTx;
    //private Image mTipImage;
    //private Text mTipCount;
    private Text mTipName;
    PlayerBackpackBean mBean;
    private DepictTextControl mDepoct;
    AccouterJsonBean mAccouter = null;
    GoodJsonBean mGoodJson = null;
    CardJsonBean mCardJson = null;
    private Button mClose, mClickList1Click1, mClickList1Click2;
    private Text mClickList1Text1;
    private Text mClickList1Text2;
    public static int COMPOSE_TYPE = 1;
    public static int USE_TYPE = 2;
    public static int UNUSE_TYPE = 3;
    public static int BOOK_TYPE = 4;
    public static int USE_CARD_TYPE = 5;
    public static int UNUSE_CARD_TYPE = 6;
    public static int SHOW_COMPOSE_TYPE = 7;
    public static int SALE_TYPE = 8;

    public long mCardId = -1;
    LevelManager mLevelManager;
    private int mCurrentType = 1;
    private Vector2 mFri;
    GameObject mButtonList1;
    void Start()
    {
        mLevelManager = GameObject.Find("Manager").GetComponent<LevelManager>();
        mClickList1Click1 = GameObject.Find("tip_button_list1_1").GetComponent<Button>();
        mClickList1Click2 = GameObject.Find("tip_button_list1_2").GetComponent<Button>();
        mClose = GameObject.Find("tip_close").GetComponent<Button>();
        mClickList1Text1 = GameObject.Find("tip_button_list1_1").GetComponentInChildren<Text>();
        mClickList1Text2 = GameObject.Find("tip_button_list1_2").GetComponentInChildren<Text>();
        mTipName = GameObject.Find("tipName").GetComponent<Text>();
        mButtonList1 = GameObject.Find("tip_button_list1");
        mClickList1Click2.onClick.AddListener(() =>
        {
            GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_CLICK_BUTTON, GuideManager.BUTTON_CLICK_TIP_SURE);
            use();
        });
        mClickList1Click1.onClick.AddListener(() =>
        {
            //  GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_CLICK_BUTTON, GuideManager.BUTTON_CLICK_TIP_SURE);
            sale();
        });
        mClose.onClick.AddListener(() =>
        {
            removeUi();
        });
        mFri = gameObject.transform.localPosition;
    }

    private void sale() {
        GameManager.getIntance().showDIaoLuo(mClickList1Click1.transform.position, DiaoluoDonghuaControl.SHUIJI_DIAOLUO_TYPE, "", 0,-1);
        BackpackManager.getIntance().use(mBean, count, SALE_TYPE);
        removeUi();
    }

    private void use()
    {
        if (mCurrentType != SHOW_COMPOSE_TYPE) {
            if (BackpackManager.getIntance().use(mBean, 1, mCurrentType))
            {
                removeUi();
            }
            else {
                if (mCurrentType == USE_TYPE) {
                    GameObject obj = Resources.Load<GameObject>("prefab/tip_text");
                    Vector3 v1 = mClickList1Click2.transform.position;
                    GameObject text = GameObject.Instantiate(obj,
                        new Vector2(v1.x, v1.y), Quaternion.identity);
                    Transform hp = GameObject.Find("Canvas").transform;
                    text.transform.SetParent(hp);
                    text.transform.localScale = new Vector3(1, 1, 1);
                    Text tv = text.GetComponent<Text>();
                    tv.text = "已装备全部装备，请先脱下一件";
                    tv.color = Color.red;
                    UiManager.FlyTo(tv);
                }
            }
        }
        
        
    }

    public void setShowData(PlayerBackpackBean bean,long count,int type) {
        mCurrentType = type;
         if (mCurrentType == USE_TYPE)
        {
            mClickList1Text2.text = "穿戴";
            mClickList1Text1.text = "出售";
            
        }
        else if (mCurrentType == UNUSE_TYPE)
        {
            mClickList1Text2.text = "脱下";
            mClickList1Text1.text = "出售";
        }
        mBean = bean;
        this.id = bean.goodId;
        this.count = count;
        showUi();
        updataTip();
        updataUi();
    }


    private void updataTip()
    {
/*        if (mTipImage == null) {
            mTipImage = GameObject.Find("box_icon_tip").GetComponent<Image>();
        }
        if (mTipCount == null) {
            mTipCount = GameObject.Find("box_text_tip").GetComponent<Text>();
        }*/
        if (mTipName == null)
        {
            mTipName = GameObject.Find("tipName").GetComponent<Text>();
        }
        string img = null;
        string name = null;
        long tabID = mBean.tabId;
        if (mBean.tabId == GoodControl.TABID_COMPOSE_TYPE)
        {
            Debug.Log("mBean.goodId = " + mBean.goodId);
            if (mBean.goodId > InventoryHalper.TABID_1_START_ID && mBean.goodId < InventoryHalper.TABID_2_START_ID)
            {
                mBean.tabId = GoodControl.TABID_ITEM_TYPE;
            }
            else if (mBean.goodId > InventoryHalper.TABID_2_START_ID && mBean.goodId < InventoryHalper.TABID_3_START_ID)
            {
                mBean.tabId = GoodControl.TABID_EQUIP_TYPY;
            }
            else if (mBean.goodId > InventoryHalper.TABID_3_START_ID && mBean.goodId < InventoryHalper.TABID_4_START_ID)
            {
                mBean.tabId = GoodControl.TABID_CARD_TYPE;
            }
        }
        if (mBean.tabId == GoodControl.TABID_EQUIP_TYPY)
        {
            mGoodJson = null;
            mCardJson = null;
            mAccouter = JsonUtils.getIntance().getAccouterInfoById(id);
            img = mAccouter.icon;
            name = mAccouter.name;
        }
        
        else if (mBean.tabId == GoodControl.TABID_ITEM_TYPE)
        {
            mAccouter = null;
            mCardJson = null;
            mGoodJson = JsonUtils.getIntance().getGoodInfoById(id);
            img = mGoodJson.icon;
            name = mGoodJson.name;
        }
        else if (mBean.tabId == GoodControl.TABID_CARD_TYPE)
        {
            mAccouter = null;
            mGoodJson = null;
            mCardJson = JsonUtils.getIntance().getCardInfoById(id);
            img = mCardJson.icon;
            name = mCardJson.name;
        }

        GoodControl g = GetComponentInChildren<GoodControl>();
        g.updateUi(mBean.goodId, count, mBean);
        //mTipCount.text = "" + count;
        mTipName.text = name;
        //mTipImage.sprite = Resources.
        //         Load("backpackIcon/" + img, typeof(Sprite)) as Sprite;
        //mTipImage.color = Color.white;
        mBean.tabId = tabID;
    }

    private void updataUi() {               
        creatDepictText();
       
    }
    private void creatDepictText() {
        string str = "";
        if (mGoodJson != null) {
            str = str + mGoodJson.describe + "\n";
        }
        if (mAccouter != null && mBean.attributeList != null && mBean.attributeList.Count > 0) {
            str = "";
            AccouterJsonBean aJson = JsonUtils.getIntance().getAccouterInfoById(mBean.goodId);
            List<EquipKeyAndDouble> key = aJson.getAttributeList();
            long level = 0;
            List<PlayerAttributeBean> affixList = new List<PlayerAttributeBean>();
            foreach (PlayerAttributeBean b in mBean.attributeList) {
                if (b.type == 10001)
                {
                    level =(long) b.value;
                }
                else if(b.getTypeStr() == null && b.type != 10002) {
                    affixList.Add(b);
                }
            }
            str = "<color=#18de42>基础属性</color>\n";
            foreach (PlayerAttributeBean b in mBean.attributeList) {
                bool isDouble = false;
                if (b.getTypeStr() != null) {
                    if (b.type == 100 ||
                        b.type == 101 ||
                        b.type == 102 ||
                        b.type == 103 ||
                        b.type == 105 ) {
                        isDouble = true;
                    }
                    foreach (EquipKeyAndDouble e in key) {
                        if (e.key == b.type) {
                            if (isDouble)
                            {
                                str = str + b.getTypeStr() + ":" +StringUtils.doubleToStringShow( e.value);
                            }
                            else {
                                str = str + b.getTypeStr() + ":" + e.value;
                            }
                            
                            break;
                        }                        
                    }
                    if (level != 0) {
                        if (isDouble)
                        {
                            str = str + "<color=#e1d1b0>+" + StringUtils.doubleToStringShow(aJson.getStrengthenByLevel(b.type, level)) + "</color>";
                        }
                        else {
                            str = str + "<color=#e1d1b0>+" + aJson.getStrengthenByLevel(b.type, level) + "</color>";
                        }
                        
                    }
                    str = str+ "\n";
                }              
            }

            long count = JsonUtils.getIntance().getAffixEnbleByLevel(level);
            long showCount = 1;
            if (affixList.Count > 0) {
                str = str+ "<color=#f8b551>特殊属性</color>\n";
                foreach (PlayerAttributeBean b in affixList) {
                    if (showCount <= count)
                    {
                        AffixJsonBean a = JsonUtils.getIntance().getAffixInfoById(b.type);
                        float vale = (float)b.value / 100;
                        str = str + a.dec + ":" + vale + "%\n";
                    }
                    else {
                        AffixJsonBean a = JsonUtils.getIntance().getAffixInfoById(b.type);
                        float vale = (float)b.value / 100;
                        str =  str + "<color=#cb3500>" + a.dec + ":" + vale + " %(+" + JsonUtils.getIntance().getAffixEnbleLevelByCount(showCount)+ "开启)</color>\n";
                    }
                    showCount++;
                }
            }
        }
        if (mCardJson != null) {
            SkillJsonBean sj = JsonUtils.getIntance().getSkillInfoById(mCardJson.skill_id);
            str = "";
            str += sj.skill_describe;
            Debug.Log(" SkillJsonBean.describe = " + sj.skill_describe);
            if (str.Contains("&n")) {
                Debug.Log(" str.Contains" );
                CalculatorUtil calcuator = new CalculatorUtil(sj.calculator,sj.effects_parameter);
                double value = calcuator.getValue(mLevelManager.mPlayerControl, null);
                str = str.Replace("&n", "" + value);
            }
        }
        if (nDepictTx == null)
        {
            mDepoct = GameObject.Find("depict_text").GetComponent<DepictTextControl>();
        }
        mDepoct.setText(str);
    }

    private void showUi()
    {
       
        gameObject.transform.localPosition = new Vector2(0,0);
        int level = GameManager.getIntance().getUiLevel();
        gameObject.transform.SetSiblingIndex(level);
        GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_SHOW, GuideManager.SHOW_TIP);
    }
    public void removeUi()
    {
        gameObject.transform.localPosition = mFri;
    }
}
