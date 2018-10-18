using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ComposeListPartControl : MonoBehaviour {

    private long mNowClickButton = 1;
    private VerticalLayoutGroup mVertirclView = null;
    public GameObject mButtonObject = null;
    public GameObject mListViewObject = null;
    public GameObject mClickItem = null;
    private Button mFirst, mSecond, mThird;
    private GameObject mFri, mSec,/* mThr,*/ mList;
    private Dictionary<long, List<ComposeJsonBen>> mMap;
    private ComposeEquipListControl mListControl;
    private Sprite mClick, mNoClick;
    // Use this for initialization
    void Start () {
        mVertirclView = GetComponent<VerticalLayoutGroup>();
        mClick = Resources.Load("ui_new/hecheng_labe1" , typeof(Sprite)) as Sprite;
        mNoClick = Resources.Load("ui_new/hecheng_labe0", typeof(Sprite)) as Sprite;
        creatGameObjec();

    }
	
	// Update is called once per frame
	public void updateList () {
        mListControl.setUiShow(mMap[mNowClickButton]);
    }

    private void creatGameObjec()
    {
 //       if (mFri != null)
 //       {
 //           Destroy(mFri);
 //       }
        if (mSec != null)
        {
            mSec.transform.parent = null;
  //          Destroy(mSec);
        }
//        if (mThr != null)
//        {
//            Destroy(mFri);
//        }
        if (mList != null)
        {
            mList.transform.parent = null;
            Destroy(mList);
            mListControl = null;
            mList = null;
        }

        if (mMap == null || mMap.Count < 1)
        {
            mMap = new Dictionary<long, List<ComposeJsonBen>>();
            List<ComposeJsonBen> list = JsonUtils.getIntance().getComposeInfo();
            List<ComposeJsonBen> classType = null;
            foreach (ComposeJsonBen bean in list)
            {
                if (mMap.ContainsKey(bean.classType))
                {
                    classType = mMap[bean.classType];
                }
                else
                {
                    classType = new List<ComposeJsonBen>();
                    mMap.Add(bean.classType, classType);
                }
                classType.Add(bean);
            }
        }

        if (mFri == null) {
            mFri = GameObject.Instantiate(mButtonObject,
                new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            mFri.GetComponentInChildren<Text>().text = "装备";
            mFri.transform.localScale = Vector3.one;
            mFri.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (mClickItem == mFri) {
                    return;
                }
                if (mClickItem != null)
                {
                    mClickItem.GetComponent<Image>().sprite = mNoClick;
                }
                mClickItem = mFri;
                mClickItem.GetComponent<Image>().sprite = mClick;
                clockIndex(1);
            });
            mClickItem = mFri;
            mFri.GetComponent<Image>().sprite = mClick;
        }
        if (mSec == null)
        {
            mSec = GameObject.Instantiate(mButtonObject,
                new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            mSec.GetComponentInChildren<Text>().text = "卡牌";
            mSec.transform.localScale = Vector3.one;
            mSec.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (mClickItem == mSec)
                {
                    return;
                }
                if (mClickItem != null) {
                    mClickItem.GetComponent<Image>().sprite = mNoClick;
                }
                mClickItem = mSec;
                mClickItem.GetComponent<Image>().sprite = mClick;
                clockIndex(2);
            });
         //   if (mMap.ContainsKey(2)) {
         //       mSec.GetComponent<Button>().interactable = false;
         //   }
        }
//        if (mThr == null)
//       {
//            mThr = GameObject.Instantiate(mButtonObject,
//                new Vector2(transform.position.x, transform.position.y), Quaternion.identity); 
//            mThr.transform.localScale = Vector2.one;
//        }
        if (mList == null)
        {
            mList = GameObject.Instantiate(mListViewObject,
                new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
           
            mListControl = mList.GetComponentInChildren<ComposeEquipListControl>();
        }


        mFri.transform.parent
            = mVertirclView.transform;
       
        mFri.transform.localScale = Vector3.one;
        if ( mNowClickButton == 1) {
            mList.transform.parent = mVertirclView.transform;
            mList.transform.localScale = Vector3.one;
            mListControl.setUiShow(mMap[mNowClickButton]);
        }
        mSec.transform.parent = mVertirclView.transform;
        mSec.transform.localScale = Vector3.one;
        if (mNowClickButton == 2)
        {
            mList.transform.parent = mVertirclView.transform;
            mList.transform.localScale = Vector3.one;
            mListControl.setUiShow(mMap[mNowClickButton]);
        }
//        mSec.transform.parent = mVertirclView.transform;
//        if (mNowClickButton == 3)
//        {
//            mList.transform.parent = mVertirclView.transform;
//        }
    }
    private void clockIndex(long index) {
        if (mNowClickButton == index || !mMap.ContainsKey(index)) {
            Debug.Log("clockIndex false mNowClickButton= "+ mNowClickButton+ " index = "+ index);
            return;
        }
        mNowClickButton = index;
        creatGameObjec();
    }

    public void listIsClick(long id) {
        Debug.Log("listPart listIsClick bean.tid " + id);
        foreach (ComposeJsonBen bean in mMap[mNowClickButton]) {
            Debug.Log("listPart listIsClick bean.tid " + (bean.tid));
            if (bean.tid == id) {
                
                GetComponentInParent<ComposeControl>().showCompose(bean);
            }
        }

    }
}
