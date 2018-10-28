using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ComposeControl : MonoBehaviour {

    public bool isShow = false;
    private Button mClose;
    private ComposeListPartControl mListPartControl;
    private int mLevel;
    private Vector2 mFri;
    private void showUi()
    {
        isShow = true;
        //gameObject.transform.TransformPoint(new Vector2(0,0));
        mFri = gameObject.transform.localPosition;
        gameObject.transform.localPosition = new Vector2(0, 0);
        mLevel = GameManager.getIntance().getUiLevel();
        gameObject.transform.SetSiblingIndex(mLevel);
        if (mClose == null)
        {
            mClose = GameObject.Find("compose_close").GetComponent<Button>();
            mClose.onClick.AddListener(() => {
                removeUi();
            });
        }
        if (mListPartControl == null)
        {
            mListPartControl = GetComponentInChildren<ComposeListPartControl>();
            mListPartControl.init();
        }
        showCompose(mShowCompose);
    }
    public void click() {
        if (isShow)
        {
            int level = GameManager.getIntance().getUiCurrentLevel();
            if (mLevel < level)
            {
                showUi();
                return;
            }
            else if (mLevel == level)
            {
                removeUi();
            }
        }
        else {
            showUi();
        }  
    }

    public void updateListPart() {
        if (mListPartControl == null)
        {
            mListPartControl = GetComponentInChildren<ComposeListPartControl>();
        }
        mListPartControl.updateList();
    }

    public void removeUi()
    {
        isShow = false;
        // gameObject.transform.TransformPoint(new Vector2(-607, -31));
        gameObject.transform.localPosition = mFri;
    }

    private ComposeJsonBen mShowCompose;
    private HorizontalLayoutGroup mNeedList;
    private Button mComposeSure;
    private Text mQuitName;
    private GameObject mNeedListOj;
    private GoodControl mShowTarget;
    private List<GameObject> mNeedListObject = new List<GameObject>();
    public GameObject mComposeMetrialGride = null;
    private Text mCost;
    public void showCompose(ComposeJsonBen bean)
    {
        if (bean == null) {
            return;
        }
        mShowCompose = bean;
        if (mNeedListOj == null)
        {
            mNeedListOj = GameObject.Find("compose_material");
            mNeedList = mNeedListOj.GetComponent<HorizontalLayoutGroup>();
        }

       
        if (mComposeSure == null)
        {
            mComposeSure = GameObject.Find("compose_sure_button").GetComponent<Button>();
            mComposeSure.onClick.AddListener(() =>
            {
                BackpackManager.getIntance().addGoods(mShowCompose.tid, 1);
                foreach (ComposeNeedItemBean item in bean.getNeedList())
                {
                    BackpackManager.getIntance().deleteGoods(item.id, (int)item.num);
                }
                Debug.Log("mCurrentCrystal = " + GameManager.getIntance().mCurrentCrystal + " cost_crystal" + mShowCompose.cost_crystal);
                GameManager.getIntance().mCurrentCrystal = GameManager.getIntance().mCurrentCrystal - (float)mShowCompose.cost_crystal;
                Debug.Log("mCurrentCrystal = " + GameManager.getIntance().mCurrentCrystal);
                GameManager.getIntance().updataGasAndCrystal();
                showCompose(mShowCompose);
            });
        }

        if (mQuitName == null)
        {
            mQuitName = GameObject.Find("compose_equip_targe_text").GetComponent<Text>();
        }
        if (mShowTarget == null)
        {
            mShowTarget = GameObject.Find("compose_equip_targe_gride").GetComponent<GoodControl>();
        }
        string icon = null;
        string name = null;
        if (bean.tid < 3000001)
        {
            icon = JsonUtils.getIntance().getAccouterInfoById(bean.tid).icon;
            name = JsonUtils.getIntance().getAccouterInfoById(bean.tid).name;
        }
        else {
            icon = JsonUtils.getIntance().getCardInfoById(bean.tid).icon;
            name = JsonUtils.getIntance().getCardInfoById(bean.tid).name;
        }
        mQuitName.text = name;
        mShowTarget.updateUi(bean.tid, 0, 0,icon);
       // mShowTarget.sprite = Resources.Load("backpackIcon/" + icon, typeof(Sprite)) as Sprite;
        if (mCost == null)
        {
            mCost = GameObject.Find("compose_cost_labe").GetComponentInChildren<Text>();
        }
        mCost.text = "合成消耗：" + mShowCompose.cost_crystal + "魔晶";
        creatMaterialGride();
        refreshShow();
    }
    public void updataUi() {
        refreshShow();
    }
    private void refreshShow() {

        if (mNeedTextList.Count < 1) {
            return;
        }
        bool isSure = true;
        foreach (long id in mNeedTextList.Keys) {
            List<PlayerBackpackBean> list = InventoryHalper.getIntance().getInventorys();
            int count = 0;
            foreach (PlayerBackpackBean b in list)
            {
                if (b.goodId == id)
                {
                    count = b.count;
                    if (count < mNeedCountList[id])
                    {
                        isSure = false;
                    }
                }
            }
            if (count == 0)
            {
                isSure = false;         
            }
            mNeedTextList[id].GetComponent<GoodControl>().updateCount(count, mNeedCountList[id]);
         //   mNeedTextList[id].GetComponentInChildren<Text>().text = count + "/" + mNeedCountList[id];
        }
        if (isSure) {
            isSure = GameManager.getIntance().mCurrentCrystal >= mShowCompose.cost_crystal;
        }
        mComposeSure.interactable = isSure;
    }

    Dictionary<long, GameObject> mNeedTextList = new Dictionary<long, GameObject>();
    Dictionary<long, long> mNeedCountList = new Dictionary<long, long>();
    private bool creatMaterialGride()
    {
        bool isSure = true;
        mNeedTextList.Clear();
        mNeedCountList.Clear();
        List<ComposeNeedItemBean> needs = mShowCompose.getNeedList();
        if (mNeedListObject.Count > 0)
        {
            foreach (GameObject bj in mNeedListObject)
            {
                Destroy(bj);
            }
            mNeedListObject.Clear();

        }
        foreach (ComposeNeedItemBean been in needs)
        {
            GameObject ob = GameObject.Instantiate(mComposeMetrialGride,
                new Vector2(transform.position.x, transform.position.y), Quaternion.identity);

            ob.transform.parent = mNeedListOj.transform;
            ob.transform.localScale = new Vector3(1, 1, 1);
            ob.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            string icon;
            if (been.id > 2000000) {
                icon = JsonUtils.getIntance().getAccouterInfoById(been.id).icon;
            }
            else {
                icon = JsonUtils.getIntance().getGoodInfoById(been.id).icon;
            }
            mNeedTextList.Add(been.id, ob);
            mNeedCountList.Add(been.id, been.num);
            //   Debug.Log("合成材料icon= " + "backpackIcon/" + icon);
            ob.GetComponent<GoodControl>().updateUi(been.id, 0, 0, icon);
            mNeedListObject.Add(ob);
        }
        Debug.Log("isSure = =" + isSure);
        return isSure;
    }

}
