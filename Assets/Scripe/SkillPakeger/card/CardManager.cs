using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour {

    public GameObject card;
    public GameObject indicator;

    private static float CREADT_CARD_TIME = 5;
    private static float OUT_CREADT_CARD_TIME = 0.5f;
    private List<CardControl> mList = new List<CardControl>();
    private float mTime = 0;
    private Transform mCanvas;
    private LocalManager mLocalManage;
    LevelManager mLevelManager;
    private List<GameObject> mCardLoaclList = new List<GameObject>();
    GameObject mCardLocalUp, mCardLocalTop;
    Random mRandom = new Random();
    private float mYdel;

    // Use this for initialization
    void Start () {
        mCanvas = GameObject.Find("Canvas").transform;
        
        mLevelManager = GameObject.Find("Manager").GetComponent<LevelManager>();
        for (int i = 1; i <= 8; i++ ){
            mCardLoaclList.Add(GameObject.Find("kapai_local_"+i));
        }
        mCardLocalUp = GameObject.Find("kapai_local_up");
        mCardLocalTop = GameObject.Find("kapai_local_up_top");
        mYdel = mCardLoaclList[0].transform.position.y+20;
    }

    public float getLocalXByIndex(int index) {
        return  mCardLoaclList[index-1].transform.position.x;
    }
    class CardUser {
        public long id;
        public bool isUse = false;
    }
    private List<CardUser> mCardList = new List<CardUser>();

    private long getRandomCard() {
        List<PlayerBackpackBean> list =  InventoryHalper.getIntance().getUsercard();
        if (list.Count == 0) {
            return 0;
        }
        if (mCardList.Count != list.Count) {
            for (int i = mCardList.Count; i < list.Count; i++) {
                CardUser tmp = new CardUser();
                tmp.id = list[i].goodId;
                mCardList.Add(tmp);
            }
        }
        int noUse = 0;
        foreach (CardUser card in mCardList) {
            if (!card.isUse) {
                noUse++;
            }
        }
        if (noUse == 0)
        {
//            Debug.Log("重置卡牌");
            //  foreach (CardUser card in mCardList)
            //{
            //     card.isUse = false;           
            //  }
            return 0;// getRandomCard();
        }
        else {
            int i = Random.Range(1, noUse*10);
           // Debug.Log("读随机数 noUse = " + noUse + " i=" + i);
            i = i / 10 + (i % 10 == 0 ? 0 : 1);
            //Debug.Log("读随机数 noUse = " + noUse + " i=" + i);
            int index = 1;
            foreach (CardUser card in mCardList)
            {
                if (!card.isUse) {
                    if (index == i) { 
                        card.isUse = true;
                        return card.id;
                    }
                    else {
                        index++;
                    }
                }
            }
        }

        return 0;
    }

    public float getUpLocalY() {
        return mCardLocalUp.transform.position.y;
    }
    public GameObject getIndicator() {
        return indicator;
    }
    public float getTopLocalY() {
        return mCardLocalTop.transform.position.y;
    }

	// Update is called once per frame
	void Update () {
        if (mCount > 0) {
            mOutSendCardTime += Time.deltaTime;
            if (mOutSendCardTime >= OUT_CREADT_CARD_TIME && mList.Count < JsonUtils.getIntance().getConfigValueForId(100015)) {
                mOutSendCardTime -= OUT_CREADT_CARD_TIME;
                mCount--;
                long random = getRandomCard();
                if (random != 0) {
                    addCard(random);
                }
                
            }
            return;
        }
        mTime += Time.deltaTime;
        if (mTime >= CREADT_CARD_TIME && mList.Count < JsonUtils.getIntance().getConfigValueForId(100015)) {
            long random = getRandomCard();
            if (random != 0)
            {
                addCard(random);
            }
        }
    }
    private long mCount = 0;
    private float mOutSendCardTime = 0;
    public void addCards(long count) {
        mCount = count;
        mOutSendCardTime = 0;
    }

    private void addCard(long id) {
        mTime = 0;

        GameObject newobj = GameObject.Instantiate(
            card, new Vector2(2500, mYdel-23), Quaternion.Euler(0.0f, 0f, 0.0f));
        newobj.AddComponent<CardControl>();
        newobj.GetComponent<CardUiControl>().init(id, CardUiControl.TYPE_CARD_PLAY, mLevelManager.mPlayerControl);
        CardControl enmey = newobj.GetComponent<CardControl>();
        newobj.transform.SetParent(gameObject.transform);
        newobj.transform.localScale = Vector3.one;
        enmey.init(id, 90,127);
        enmey.init(mList.Count + 1, this, id);
        mList.Add(enmey);
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
        if (mLocalManage == null) {
            mLocalManage = GameObject.Find("Manager").GetComponent<LevelManager>().getLocalManager();
        }
        return mLocalManage;
    }
    public Attacker getHero() {
        return mLevelManager.mPlayerControl;    
    }

}
