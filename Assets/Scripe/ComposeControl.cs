using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ComposeControl : MonoBehaviour {

    public bool isShow = false;
    private Button mClose;
    public void showUi()
    {
        if (isShow)
        {
            return;
        }
        isShow = true;
        //gameObject.transform.TransformPoint(new Vector2(0,0));
        gameObject.transform.localPosition = new Vector2(0, 0);
        if (mClose == null)
        {
            mClose = GameObject.Find("compose_close").GetComponent<Button>();
            mClose.onClick.AddListener(() => {
                removeUi();
            });
        }
        showCompose(mShowCompose);
    }
    public void removeUi()
    {
        if (!isShow)
        {
            return;
        }
        isShow = false;
        // gameObject.transform.TransformPoint(new Vector2(-607, -31));
        gameObject.transform.localPosition = new Vector2(740, -63.673f);
    }

    private ComposeJsonBen mShowCompose;
    private HorizontalLayoutGroup mNeedList;
    private Button mComposeSure;
    private Text mQuitName;
    private GameObject mNeedListOj;
    private Image mShowTarget;
    private List<GameObject> mNeedListObject = new List<GameObject>();
    public GameObject mComposeMetrialGride = null;
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
                BackpackManager.getIntance().addGoods(bean.tid, 1);
                foreach (ComposeNeedItemBean item in bean.getNeedList())
                {
                    BackpackManager.getIntance().deleteGoods(item.id, (int)item.num);
                }
                mShowCompose = null;
                showCompose(bean);
            });
        }
        if (mQuitName == null)
        {
            mQuitName = GameObject.Find("compose_equip_targe_text").GetComponent<Text>();
        }
        if (mShowTarget == null)
        {
            mShowTarget = GameObject.Find("compose_equip_targe_gride").GetComponent<Image>();
        }
        string icon = JsonUtils.getIntance().getAccouterInfoById(bean.tid).icon;
        string name = JsonUtils.getIntance().getAccouterInfoById(bean.tid).name;
        mQuitName.text = name;
        mShowTarget.sprite = Resources.Load("backpackIcon/" + icon, typeof(Sprite)) as Sprite;
        mComposeSure.interactable = creatMaterialGride();
    }
    public void updataUi() {
        creatMaterialGride();
    }
    private bool creatMaterialGride()
    {
        bool isSure = true;
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
            ob.transform.localScale = Vector2.one;
            ob.transform.parent = mNeedListOj.transform;
            string icon;
            if (been.id > 2000000) {
                icon = JsonUtils.getIntance().getAccouterInfoById(been.id).icon;
            }
            else {
                icon = JsonUtils.getIntance().getGoodInfoById(been.id).icon;
            }
           
            ob.GetComponentsInChildren<Image>()[1].sprite = Resources.Load("backpackIcon/" + icon, typeof(Sprite)) as Sprite;
            List<PlayerBackpackBean> list = InventoryHalper.getIntance().getInventorys();
            int count = 0;
            foreach (PlayerBackpackBean b in list)
            {
                if (b.goodId == been.id)
                {
                    count = b.count;
                    if (count >= been.num)
                    {
                        ob.GetComponentInChildren<Text>().text = count + "/" + been.num;
                    }
                    else
                    {
                        isSure = false;
                        ob.GetComponentInChildren<Text>().text =  count + " / " + been.num;
                    }
                }
            }
            mNeedListObject.Add(ob);
            if (count == 0)
            {
                isSure = false;
                ob.GetComponentInChildren<Text>().text = count + " / " + been.num;
            }
        }
        Debug.Log("isSure = =" + isSure);
        return isSure;
    }

}
