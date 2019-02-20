using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour {

    public GameObject card;
    public GameObject indicator;

    private static float CREADT_CARD_TIME = -1;
    private static float OUT_CREADT_CARD_TIME = 0.1f;
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
        for (int i = 1; i <= 6; i++ ){
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
    private bool isInit = false;
    public void init() {
        isInit = true;
        if (CREADT_CARD_TIME == -1)
        {
            CREADT_CARD_TIME = JsonUtils.getIntance().getConfigValueForId(100043);
        }
    }
    private int mMaxCardCount = 0;
	void Update () {
        if (!isInit) {
            return;
        }

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
            //return;
        }
        mTime += Time.deltaTime;
        if (mMaxCardCount == 0) {
            mMaxCardCount =(int) JsonUtils.getIntance().getConfigValueForId(100015);
        }
        if (mTime >= CREADT_CARD_TIME && mList.Count < mMaxCardCount) {
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
        mCount += count;
        mOutSendCardTime = 0;
    }

    public static int GIVEUP_CARD_ALL = 1;
    public static int GIVEUP_CARD_MAX = 2;
    public static int GIVEUP_CARD_MIX = 3;
    public static int GIVEUP_CARD_RANGE = 4;


    public long giveupCard(int type) {
        if (mList.Count == 0) {
            return 1;
        }
        long count = 0;
        if (type == GIVEUP_CARD_ALL)
        {
            count = mList.Count;
            while (mList.Count > 0)
            {
                giveUpCard(1);
            }
            return count;
        }
        else if (type == GIVEUP_CARD_MAX)
        {
            CardControl max = null;
            foreach (CardControl c in mList)
            {
                if (max == null)
                {
                    max = c;
                }
                else if (c.mCard.cost > max.mCard.cost)
                {
                    max = c;
                }
            }
            count = max.mCard.cost;
            giveUpCard(max.mIndex);
            return count;
        }
        else if (type == GIVEUP_CARD_MIX)
        {
            CardControl max = null;
            foreach (CardControl c in mList)
            {
                if (max == null)
                {
                    max = c;
                }
                else if (c.mCard.cost < max.mCard.cost)
                {
                    max = c;
                }
            }
            count = max.mCard.cost;
            giveUpCard(max.mIndex);
            return count;
        }
        else if (type == GIVEUP_CARD_RANGE) {
            int leng = mList.Count;
            int range = Random.Range(0, leng - 1);
            count = mList[range].mCard.cost;
            giveUpCard(range+1);
            return count;
        }
       // getHero().mSkillManager.addSkill()
        return -1;
    }

    private void addCard(long id) {
        mTime = 0;

        GameObject newobj = GameObject.Instantiate(
            card, new Vector2(2500, mYdel-23), Quaternion.Euler(0.0f, 0f, 0.0f));
        newobj.AddComponent<CardControl>();
        newobj.GetComponent<CardUiControl>().init(id, 107, 146);
        newobj.GetComponent<CardUiControl>().init(id, CardUiControl.TYPE_CARD_PLAY, mLevelManager.mPlayerControl);
        newobj.GetComponent<CardUiControl>().showCard();
        CardControl enmey = newobj.GetComponent<CardControl>();
        newobj.transform.SetParent(gameObject.transform);
        newobj.transform.localScale = Vector3.one;
        //enmey.init(id, 107, 146);
        enmey.init(mList.Count + 1, this, id);
        mList.Add(enmey);
    }
    public void giveUpCard(int index)
    {
        for (int i = 0; i < mList.Count;)
        {
            if (mList[i].mIndex != index)
            {
                mList[i].deleteCard(index);
                i++;
            }
            else
            {
                mList[i].giveUp();
                mList.Remove(mList[i]);
            }
        }
    }

    public bool userCard(Attacker attack, int index,float cost) {
        cost = attack.mSkillManager.mEventAttackManager.getCardCost((int)cost);
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
