using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZhuangbeiUpdateControl : MonoBehaviour {

    public GameObject Item;
    HorizontalLayoutGroup mZhuangbeiList;
    public List<GameObject> mItemGameObjectList = new List<GameObject>();
    ScrollRect mScroll;

    // Use this for initialization
    void Start () {
    }
    
    public void upDate(bool isDelete) {

        if (mZhuangbeiList == null) {
            mZhuangbeiList = GameObject.Find("zhuangbei_update").GetComponent<HorizontalLayoutGroup>();
        }
        List<PlayerBackpackBean>  list  = InventoryHalper.getIntance().getRoleUseList();
        if (isDelete && mItemGameObjectList.Count > 0) {
            for (int i = 0; i < mItemGameObjectList.Count;) {
                GameObject ob = mItemGameObjectList[i];
                mItemGameObjectList.Remove(ob);
                Destroy(ob);
            }
        }
        if (isDelete)
        {
            mItemGameObjectList.Clear();
            List<PlayerBackpackBean> list2 = new List<PlayerBackpackBean>();
            foreach (PlayerBackpackBean b in list) {
                AccouterJsonBean a1 = JsonUtils.getIntance().getAccouterInfoById(b.goodId);
                long level1 = getLevel(b);
                bool isAdded = false;
                if (list2.Count == 0)
                {
                    list2.Add(b);
                }
                else {
                    for (int i = 0; i < list2.Count; i++) {
                       
                        AccouterJsonBean a2 = JsonUtils.getIntance().getAccouterInfoById(list2[i].goodId);
                        if (a2.quality > a1.quality)
                        {
                            continue;
                        }
                        else if (a2.quality < a1.quality)
                        {
                            list2.Insert(i, b);
                            isAdded = true;
                            break;
                        }
                        else {
                            long level2 = getLevel(list2[i]);
                            if (level1 >= level2)
                            {
                                list2.Insert(i, b);
                                isAdded = true;
                                break;
                            }
                            else {
                                continue;
                            }
                        }
                    }
                    if (!isAdded) {
                        list2.Add(b);
                    }
                }
            }
            foreach (PlayerBackpackBean b in list2)
            {
                GameObject ob = GameObject.Instantiate(Item,
                     new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                float witch = ob.GetComponent<RectTransform>().rect.width;
                ob.transform.parent = mZhuangbeiList.transform;
                ob.transform.localScale = new Vector3(1, 1, 1);
                ob.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                ob.GetComponent<ZhuangBeiItemShowControl>().init(b);
                mItemGameObjectList.Add(ob);
            }
            setWitch(mZhuangbeiList, mItemGameObjectList.Count);
        }
        else {
            foreach (GameObject g in mItemGameObjectList) {
                g.GetComponent<ZhuangBeiItemShowControl>().init();
            }
        }
    }

    private long getLevel(PlayerBackpackBean p) {
        long level = 0;
        foreach (PlayerAttributeBean pa in p.attributeList) {
            if (pa.type == 10001) {
                return pa.value;
            }
        }
        return level;
    }

    public void gui(long id) {
        for(int i = 0; i< mItemGameObjectList.Count; i++) {
            if (mItemGameObjectList[i].GetComponent<ZhuangBeiItemShowControl>().mBean.goodId == id) {
                GameManager.getIntance().getGuideManager().ShowGuideHorizontalLayoutGroupInScroll(mItemGameObjectList[i], 
                    GameObject.Find("di_2").GetComponent<ScrollRect>(), mZhuangbeiList,i,2);
                break;
            }
        }
    }

    private void setWitch(HorizontalLayoutGroup grid, int count)     //每行Cell的个数
    {
        float w = 0;
        if (count > 4)
        {
            w = 85 * count + 20 * (count - 1) + 40;
        }
        else {
            w = 603.7f;
        }
        grid.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        grid.transform.Translate(Vector2.right * (w));
    }

}
