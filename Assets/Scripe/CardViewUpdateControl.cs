using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardViewUpdateControl : MonoBehaviour {
    public GameObject mItemObject;
    private List<CardUpdateItemControl> mItems = new List<CardUpdateItemControl>();
    private VerticalLayoutGroup mVerivlaLayou;
    float mWitch = 0;

    public void init() {
        mVerivlaLayou = GetComponent<VerticalLayoutGroup>();
        setUiShow();
        mWitch =gameObject.GetComponent<RectTransform>().rect.width;
    }
    private void setUiShow()
    {
        List<YongjiuCardBean>  list = JsonUtils.getIntance().getYongjiuCardInfos();
        Debug.Log("YongjiuCardBean count = " + list.Count);
        if (list == null || list.Count == 0) {
            return;
        }

        List<long> keys = new List<long>();
        foreach (YongjiuCardBean key in list) {
            if (keys.Count == 0)
            {
                keys.Add(key.id);
            }
            else {
                int i = 0;
                for (; i < keys.Count; i++) {
                    if (JsonUtils.getIntance().getYongjiuCardInfoById(keys[i]).sortID > key.sortID)
                    {                 
                        break;
                    }
                }
                if (i < keys.Count)
                {
                    keys.Insert(i, key.id);
                }
                else {
                    keys.Add(key.id);
                }
            }
        }
        for (; mItems.Count > 0;)
        {
            CardUpdateItemControl cc = mItems[0];
            GameObject goj = cc.gameObject;
            mItems.Remove(cc);
            Destroy(goj);
        }

        foreach (long key in keys)
        {
            YongjiuCardBean bean = JsonUtils.getIntance().getYongjiuCardInfoById(key);
            GameObject ob = GameObject.Instantiate(mItemObject,
                 new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            CardUpdateItemControl item = ob.GetComponent<CardUpdateItemControl>();
            item.init(bean.id);

            float witch = ob.GetComponent<RectTransform>().rect.width;
            ob.transform.parent = gameObject.transform;
            ob.transform.localScale = new Vector3(1, 1, 1);
            ob.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            mItems.Add(item);

        }
        SetGridHeight();
    }
    public void addLastItem() {
       /* long level = InventoryHalper.getIntance().getSamsaraLevelById(13);
        Dictionary<long, SamsaraJsonBean> list = JsonUtils.getIntance().getSamsaraInfo();
        if (level == BaseDateHelper.encodeLong(0)) {
            SamsaraJsonBean bean = list[13];
            GameObject ob = GameObject.Instantiate(mItemObject,
                 new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            SamsaraItemControl item = ob.GetComponent<SamsaraItemControl>();
            item.init(bean.id, this);
            float witch = ob.GetComponent<RectTransform>().rect.width;
            ob.transform.parent = gameObject.transform;
            ob.transform.localScale = new Vector3(1, 1, 1);
            ob.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            mItems.Add(item);
        }*/
    }


    private float mGridHeight = 0;
    private void SetGridHeight()
    {
        //if (mItems.Count > 3) {
        int count = 0;
        if (mItems.Count < 3)
        {
            count = 3;
        }
        else {
            count = mItems.Count;
        }
        float height = (mVerivlaLayou.spacing + 196) * count;
        mVerivlaLayou.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        if (mGridHeight != height)
        {
            mGridHeight = height;
            mVerivlaLayou.transform.Translate(Vector2.down * (height));
        }
        //}

    }
    public void updateItem(long id)
    {
        if (id == -1) {
            foreach (CardUpdateItemControl item in mItems)
            {
                item.updateItem(id);
            }
            return;
        }

        Debug.Log("================================================CardViewUpdateControl add card =" + id);
        foreach (CardUpdateItemControl item in mItems) {
            if (item.updateItem(id)) {
                break;
            }
        }
    }
}
