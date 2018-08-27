using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SamSaraListControl : MonoBehaviour {
    public GameObject mItemObject;
    private List<SamsaraItemControl> mItems = new List<SamsaraItemControl>();
    private VerticalLayoutGroup mVerivlaLayou;
    public void init() {
        mVerivlaLayou = mVerivlaLayou = GetComponent<VerticalLayoutGroup>();
        setUiShow();
    }
    private void setUiShow()
    {
        Dictionary<long, SamsaraJsonBean>  list = JsonUtils.getIntance().getSamsaraInfo();
        if (list == null || list.Count == 0) {
            return;
        }
        Dictionary<long, SamsaraJsonBean>.KeyCollection keys=  list.Keys;
        foreach (long key in keys)
        {
            SamsaraJsonBean bean = list[key];
            GameObject ob = GameObject.Instantiate(mItemObject,
                 new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            ob.transform.localScale = Vector2.one;
            ob.transform.parent = gameObject.transform;
            SamsaraItemControl item = ob.GetComponent<SamsaraItemControl>();
            item.init(bean.id,this);
            mItems.Add(item);
        }
        SetGridHeight();
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
        isEnableLavelUp();
    }



    private void SetGridHeight()
    {
        //if (mItems.Count > 3) {
        float height = (mVerivlaLayou.spacing + 90) * mItems.Count;
        mVerivlaLayou.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        //}

    }

}
