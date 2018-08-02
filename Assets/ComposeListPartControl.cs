﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ComposeListPartControl : MonoBehaviour {

    private long mNowClickButton = 1;
    private VerticalLayoutGroup mVertirclView = null;
    public GameObject mButtonObject = null;
    public GameObject mListViewObject = null;
    private Button mFirst, mSecond, mThird;
    private GameObject mFri, mSec,/* mThr,*/ mList;
    private Dictionary<long, List<ComposeJsonBen>> mMap;
    private ComposeEquipListControl mListControl;
	// Use this for initialization
	void Start () {
        mVertirclView = GetComponent<VerticalLayoutGroup>();
        creatGameObjec();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void creatGameObjec()
    {
        if (mFri != null)
        {
            Destroy(mFri);
        }
        if (mSec != null)
        {
            Destroy(mSec);
        }
//        if (mThr != null)
//        {
//            Destroy(mFri);
//        }
        if (mList != null)
        {
            Destroy(mList);
            mListControl = null;
        }

        if (mFri == null) {
            mFri = GameObject.Instantiate(mButtonObject,
                new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            mFri.GetComponentInChildren<Text>().text = "装备";
            mFri.transform.localScale = Vector2.one;
            mFri.GetComponent<Button>().onClick.AddListener(() =>
            {
                clockIndex(1);
            });
        }
        if (mSec == null)
        {
            mSec = GameObject.Instantiate(mButtonObject,
                new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            mSec.GetComponentInChildren<Text>().text = "卡牌";
            mSec.transform.localScale = Vector2.one;
            mSec.GetComponent<Button>().onClick.AddListener(() =>
            {
                clockIndex(2);
            });
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
            mList.transform.localScale = Vector2.one;
            mListControl = mList.GetComponentInChildren<ComposeEquipListControl>();
        }
        if (mMap == null || mMap.Count < 1) {
            mMap = new Dictionary<long, List<ComposeJsonBen>>();
            List<ComposeJsonBen> list = JsonUtils.getIntance().getComposeInfo();
            List<ComposeJsonBen> classType = null; 
            foreach (ComposeJsonBen bean in list) {
                if (mMap.ContainsKey(bean.classType))
                {
                    classType = mMap[bean.classType];
                }
                else {
                    classType = new List<ComposeJsonBen>();
                    mMap.Add(bean.classType,classType);
                }
                classType.Add(bean);
            }
        }

        mFri.transform.parent = gameObject.transform;
        if ( mNowClickButton == 1) {
            mList.transform.parent = gameObject.transform;
            mListControl.setUiShow(mMap[mNowClickButton]);
        }
        mSec.transform.parent = gameObject.transform;
        if (mNowClickButton == 2)
        {
            mList.transform.parent = gameObject.transform;
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