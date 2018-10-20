using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ComposeEquipListControl : MonoBehaviour {

    private VerticalLayoutGroup mVerivlaLayou;
    private List<ComposeJsonBen> mList;
    private List<GameObject> mItems = new List<GameObject>();
    // Use this for initialization
    public GameObject mItemObject;
    public GameObject mFillingObject;
    private ComposeListPartControl mParten;
    private GameObject mClickItem,mFill;

    private Sprite mClick, mNoClick;

    private void Start()
    {

    }

    public void setParten(ComposeListPartControl part) {
        mParten = part;
    }
    private bool isInit = false;
    public void  setUiShow(List<ComposeJsonBen> list)
    {
        mList = list;
        if (mVerivlaLayou == null) {
            mVerivlaLayou = GetComponent<VerticalLayoutGroup>();
        }
        if (mItems.Count > 0) {
            foreach (GameObject ob in mItems) {
                Destroy(ob);
            }
            mItems.Clear();
        }
        if (mClick == null) {
            mClick = Resources.Load("ui_new/hecheng_labe3", typeof(Sprite)) as Sprite;
        }
        if (mNoClick == null) {
            mNoClick = Resources.Load("ui_new/hecheng_labe4", typeof(Sprite)) as Sprite;
        }
        foreach (ComposeJsonBen bean in list) {
            if (bean.isShow == 2 && !isHave(bean.id)) {
                continue;
            }
            if (!isInit) {
                isInit = true;
                GetComponentInParent<ComposeListPartControl>().listIsClick(bean.tid);
            }
            Debug.Log("ComposeEquipListControl id = " + bean.tid);
            string icon = null;
            string name = null;
            if (bean.tid < 3000001)
            {
                icon = JsonUtils.getIntance().getAccouterInfoById(bean.tid).icon;
                name = JsonUtils.getIntance().getAccouterInfoById(bean.tid).name;
            }
            else
            {
                icon = JsonUtils.getIntance().getCardInfoById(bean.tid).icon;
                name = JsonUtils.getIntance().getCardInfoById(bean.tid).name;
            }
            GameObject ob = GameObject.Instantiate(mItemObject,
                 new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            ob.transform.localScale = Vector2.one;
            ob.transform.parent = gameObject.transform;
            ob.GetComponentInChildren<Text>().text = name;
            Image im = ob.GetComponentsInChildren<Image>()[2];
            im.sprite = Resources.Load("backpackIcon/" + icon, typeof(Sprite)) as Sprite; 
            im.color = Color.white;
            ob.GetComponent<Button>().onClick.AddListener(()=>{
                if (mClickItem != null) {
                    mClickItem.GetComponent<Image>().sprite = mNoClick;
                }
                mClickItem = ob;
                mClickItem.GetComponent<Image>().sprite = mClick;
                Debug.Log("ob.GetComponent bean.tid " + bean.tid);
                GetComponentInParent<ComposeListPartControl>().listIsClick(bean.tid); 
            });
            if (mItems.Count == 0) {
                mClickItem = ob;
                mClickItem.GetComponent<Image>().sprite = mClick;
            }
            mItems.Add(ob);
        }
        SetGridHeight();
    }

    private void SetGridHeight()   
    {
        int count = mItems.Count > 4 ? mItems.Count : 4;

        if (mItems.Count > 4)
        {
            count = mItems.Count;
            if (mFill != null) {
                Destroy(mFill);
                mFill = null;
            }
        }
        else {
            count = 4;
            if (mFill != null) {
                Destroy(mFill);
                mFill = null;
            }
            mFill = GameObject.Instantiate(mFillingObject,
                 new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            mFill.transform.localScale = Vector2.one;
            mFill.transform.parent = gameObject.transform;
        }

        Debug.Log("ComposeEquipListControl SetGridHeight count = "+ count);
        float height = (mVerivlaLayou.spacing + 115) * count;
        float fillingHeight = height - (mVerivlaLayou.spacing + 115) * mItems.Count;
        if (mFill != null) {
            mFill.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, fillingHeight);
        }
        mVerivlaLayou.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);    
    }
    private bool isHave(long id) {
        List<long> idList = InventoryHalper.getIntance().getHaveBookId();
        foreach (long bookid in idList) {
            if (id == bookid) {
                return true;
            }
        }
        return false;
    }

}
