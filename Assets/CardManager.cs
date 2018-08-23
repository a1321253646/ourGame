using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour {

    public GameObject card;

    private static float CREADT_CARD_TIME = 5;

    private List<CardControl> mList = new List<CardControl>();
    private List<NengliangkuaiControl> mNengliangList = new List<NengliangkuaiControl>();
    private float mTime = 0;
    private Transform mCanvas;
    private LocalManager mLocalManage;
    // Use this for initialization
    void Start () {
        mCanvas = GameObject.Find("Canvas").transform;
        mLocalManage = GameObject.Find("Manager").GetComponent<LevelManager>().getLocalManager();
    }
	
	// Update is called once per frame
	void Update () {
        mTime += Time.deltaTime;
        if (mTime >= CREADT_CARD_TIME) {
            addCard();
        }
        for (int i = 1; i <= 10; i++) {
            NengliangkuaiControl tmp=GameObject.Find("nengliangkuai_" + i).GetComponent<NengliangkuaiControl>();
            tmp.setCount(5.5f);
            mNengliangList.Add(tmp);
        }
    }

    private void addCard() {
        mTime = 0;
        if (mList.Count >= 8) {
            return;
         }

        GameObject newobj = GameObject.Instantiate(
            card, new Vector2(-30,50), Quaternion.Euler(0.0f, 0f, 0.0f));
        CardControl enmey = newobj.GetComponent<CardControl>();
        newobj.transform.SetParent(mCanvas);
        newobj.transform.localScale = Vector3.one;
        mList.Add(enmey);
        enmey.init("1", mList.Count + 1, this, null);
    }
    public void userCard(int index) {
        for (int i = 0; i < mList.Count;) {
            if (mList[i].mIndex != index)
            {
                mList[i].deleteCard(index);
                i++;
            }
            else {
                mList.Remove(mList[i]);
            }
        } 
    }
    public LocalManager getLocalManager() {
        return mLocalManage;
    }
}
