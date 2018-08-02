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
    private ComposeListPartControl mParten;
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
        foreach(ComposeJsonBen bean in list) {
            if (!isInit) {
                GetComponentInParent<ComposeListPartControl>().listIsClick(bean.tid);
            }
            Debug.Log("ComposeEquipListControl id = " + bean.tid);
            string icon = JsonUtils.getIntance().getAccouterInfoById(bean.tid).icon;
            string name = JsonUtils.getIntance().getAccouterInfoById(bean.tid).name;
            GameObject ob = GameObject.Instantiate(mItemObject,
                 new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            ob.transform.localScale = Vector2.one;
            ob.transform.parent = gameObject.transform;
            ob.GetComponentInChildren<Text>().text = name;
            Image im = ob.GetComponentsInChildren<Image>()[1];
            im.sprite = Resources.Load("backpackIcon/" + icon, typeof(Sprite)) as Sprite; 
            im.color = Color.white;
            ob.GetComponent<Button>().onClick.AddListener(()=>{
                Debug.Log("ob.GetComponent bean.tid " + bean.tid);
                GetComponentInParent<ComposeListPartControl>().listIsClick(bean.tid); 
            });
            mItems.Add(ob);
        }
        SetGridHeight();
    }
    private void SetGridHeight()   
    {
        if (mItems.Count > 3) {
            float height = (mVerivlaLayou.spacing + 54) * mItems.Count;
            mVerivlaLayou.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }
       
    }
}
