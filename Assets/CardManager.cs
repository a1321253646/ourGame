using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour {

    public GameObject card;

    private static float CREADT_CARD_TIME = 5;
    private static float OUT_CREADT_CARD_TIME = 0.5f;
    private List<CardControl> mList = new List<CardControl>();
    private float mTime = 0;
    private Transform mCanvas;
    private LocalManager mLocalManage;
    LevelManager mLevelManager;
    private List<GameObject> mCardLoaclList = new List<GameObject>();
    GameObject mCardLocalUp;
    private float mYdel;
    // Use this for initialization
    void Start () {
        mCanvas = GameObject.Find("Canvas").transform;
        mLocalManage = GameObject.Find("Manager").GetComponent<LevelManager>().getLocalManager();
        mLevelManager = GameObject.Find("Manager").GetComponent<LevelManager>();
        for (int i = 1; i <= 8; i++ ){
            mCardLoaclList.Add(GameObject.Find("kapai_local_"+i));
        }
        mCardLocalUp = GameObject.Find("kapai_local_up");
        mYdel = mCardLoaclList[0].transform.position.y;
    }

    public float getLocalXByIndex(int index) {
       return  mCardLoaclList[index-2].transform.position.x;
    }
    public float getUpLocalY() {
        return mCardLocalUp.transform.position.y;
    }
	// Update is called once per frame
	void Update () {
        if (mCount > 0) {
            mOutSendCardTime += Time.deltaTime;
            if (mOutSendCardTime >= OUT_CREADT_CARD_TIME) {
                mOutSendCardTime -= OUT_CREADT_CARD_TIME;
                mCount--;
                addCard();
            }
            return;
        }
        mTime += Time.deltaTime;
        if (mTime >= CREADT_CARD_TIME) {
            addCard();
        }
    }

    private bool isFirst = true;
    private long mCount = 0;
    private float mOutSendCardTime = 0;
    public void addCards(long count) {
        mCount = count;
        mOutSendCardTime = 0;
    }

    private void addCard() {
        mTime = 0;
        if (mList.Count >= 8) {
            return;
         }

        GameObject newobj = GameObject.Instantiate(
            card, new Vector2(-30, mYdel), Quaternion.Euler(0.0f, 0f, 0.0f));
        newobj.AddComponent<CardControl>();
        CardControl enmey = newobj.GetComponent<CardControl>();
        newobj.transform.SetParent(gameObject.transform);
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
    public bool userCard(int index,float cost) {
        if (!GameObject.Find("Manager").GetComponent<LevelManager>().delectNengliangdian(cost))
        {
            return false;
        }
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
        return true;
    }
    public LocalManager getLocalManager() {
        return mLocalManage;
    }
    public Attacker getHero() {
        return mLevelManager.mPlayerControl;    
    }

}
