using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZhuangbeiUpdateControl : MonoBehaviour {

    public GameObject Item;
    HorizontalLayoutGroup mZhuangbeiList;
    public List<GameObject> mItemGameObjectList = new List<GameObject>();


    // Use this for initialization
    void Start () {
       
    }
    public void upDate() {
        if (mZhuangbeiList == null) {
            mZhuangbeiList = GameObject.Find("zhuangbei_update").GetComponent<HorizontalLayoutGroup>();
        }
        List<PlayerBackpackBean>  list  = InventoryHalper.getIntance().getRoleUseList();
        if (mItemGameObjectList.Count > 0) {
            for (int i = 0; i < mItemGameObjectList.Count;) {
                GameObject ob = mItemGameObjectList[i];
                mItemGameObjectList.Remove(ob);
                Destroy(ob);
            }
        }
        mItemGameObjectList.Clear();
        foreach (PlayerBackpackBean b in list) {
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
    private void setWitch(HorizontalLayoutGroup grid, int count)     //每行Cell的个数
    {
        float w = 85 * count + 20 * (count - 1) + 40;
        grid.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        grid.transform.Translate(Vector2.right * (w));
    }

}
