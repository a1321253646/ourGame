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
    LevelManager mLevelManager;
    private List<GameObject> mCardLoaclList = new List<GameObject>();
    GameObject mCardLocalUp;

    // Use this for initialization
    void Start () {
        mCanvas = GameObject.Find("Canvas").transform;
        mLocalManage = GameObject.Find("Manager").GetComponent<LevelManager>().getLocalManager();
        mLevelManager = GameObject.Find("Manager").GetComponent<LevelManager>();
        for (int i = 1; i <= 8; i++ ){
            mCardLoaclList.Add(GameObject.Find("kapai_local_"+i));
        }
        mCardLocalUp = GameObject.Find("kapai_local_up");
    }

    public float getLocalXByIndex(int index) {
       return  mCardLoaclList[index-2].transform.position.x;
    }
    public float getUpLocalY() {
        return mCardLocalUp.transform.position.y;
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

    private bool isFirst = true;
    private void addCard() {
        mTime = 0;
        if (mList.Count >= 8) {
            return;
         }

        GameObject newobj = GameObject.Instantiate(
            card, new Vector2(-30,transform.position.y), Quaternion.Euler(0.0f, 0f, 0.0f));

        CardControl enmey = newobj.GetComponent<CardControl>();
        newobj.transform.SetParent(mCanvas);
        newobj.transform.localScale = Vector3.one;
        mList.Add(enmey);
        if (isFirst)
        {
            enmey.init( mList.Count + 1, this, 3000001);
        }
        else {
            enmey.init( mList.Count + 1, this, 3000002);
        }
        isFirst = !isFirst;
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
    public Attacker getHero() {
        return mLevelManager.mPlayerControl;    
    }

}
