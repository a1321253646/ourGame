using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SamSaraListControl : MonoBehaviour {
    public GameObject mItemObject;
    private List<SamsaraItemControl> mItems = new List<SamsaraItemControl>();
    private VerticalLayoutGroup mVerivlaLayou;
    float mWitch = 0;

    public void init() {
        mVerivlaLayou = GetComponent<VerticalLayoutGroup>();
        setUiShow();
        mWitch =gameObject.GetComponent<RectTransform>().rect.width;
    }
    private void setUiShow()
    {
        Dictionary<long, SamsaraJsonBean>  list = JsonUtils.getIntance().getSamsaraInfo();
        if (list == null || list.Count == 0) {
            return;
        }
        Dictionary<long, SamsaraJsonBean>.KeyCollection listKey=  list.Keys;
        List<long> keys = new List<long>();
        foreach (long key in listKey) {
            if (keys.Count == 0)
            {
                keys.Add(key);
            }
            else {
                int i = 0;
                for (; i < keys.Count; i++) {
                    if (list[keys[i]].sort > list[key].sort){
                       
                        break;
                    }
                }
                if (i < keys.Count)
                {
                    keys.Insert(i, key);
                }
                else {
                    keys.Add(key);
                }
            }
        }
        foreach (long key in keys)
        {
            SamsaraJsonBean bean = list[key];
            GameObject ob = GameObject.Instantiate(mItemObject,
                 new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            SamsaraItemControl item = ob.GetComponent<SamsaraItemControl>();
            bool isAdd = item.init(bean.id, this);
            if (isAdd)
            {
                float witch = ob.GetComponent<RectTransform>().rect.width;
                ob.transform.parent = gameObject.transform;
                ob.transform.localScale = new Vector3(1, 1, 1);
                ob.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                mItems.Add(item);
            }
            else {
                Destroy(ob);
            }

        }
        SetGridHeight();
    }
    public void addLastItem() {
        long level = InventoryHalper.getIntance().getSamsaraLevelById(13);
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
        }
    }

    public void isEnableLavelUp() {      
        foreach (SamsaraItemControl control in mItems)
        {
            control.isEnableLevelUp();
        }
    }

    public void upDate(long id)
    {
        Debug.Log(" SamSaraListControl upDate " + id);
        InventoryHalper.getIntance().upDateSamsaraLevel(id);
        foreach (SamsaraItemControl control in mItems) {
            if (control.mId == id) {
                control.upDate();
            }
        }
        BackpackManager.getIntance().upLunhui();
        isEnableLavelUp();
    }


    private float mGridHeight = 0;
    private void SetGridHeight()
    {
        //if (mItems.Count > 3) {
        int count = 0;
        if (mItems.Count < 5)
        {
            count = 5;
        }
        else {
            count = mItems.Count;
        }
            float height = (mVerivlaLayou.spacing + 125) * count;
        mVerivlaLayou.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        if (mGridHeight != height)
        {
            mGridHeight = height;
            mVerivlaLayou.transform.Translate(Vector2.down * (height));
        }
        //}

    }

}
