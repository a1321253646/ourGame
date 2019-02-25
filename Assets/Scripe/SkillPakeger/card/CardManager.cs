using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : CardManagerBase
{

    


    private List<CardControl> mList = new List<CardControl>();
    private Transform mCanvas;
    
    LevelManager mLevelManager;
    private List<GameObject> mCardLoaclList = new List<GameObject>();
    
    Random mRandom = new Random();
    private float mYdel;

    public List<NengliangkuaiControl> mNengLiangKuai = new List<NengliangkuaiControl>();

    // Use this for initialization
    void Start () {
        mCanvas = GameObject.Find("Canvas").transform;
        
        mLevelManager = GameObject.Find("Manager").GetComponent<LevelManager>();
        if (mCardLoaclList.Count == 0)
        {
            for (int i = 1; i <= 6; i++)
            {
                mCardLoaclList.Add(GameObject.Find("kapai_local_" + i));
            }
            mCardLocalUp = GameObject.Find("kapai_local_up");
            mCardLocalTop = GameObject.Find("kapai_local_up_top");
            mYdel = mCardLoaclList[0].transform.position.y + 20;
        }
        initNengliangkuai();
    }

    public override float getLocalXByIndex(int index) {
        if (mCardLoaclList.Count == 0) {
            for (int i = 1; i <= 6; i++)
            {
                mCardLoaclList.Add(GameObject.Find("kapai_local_" + i));
            }
            mCardLocalUp = GameObject.Find("kapai_local_up");
            mCardLocalTop = GameObject.Find("kapai_local_up_top");
            mYdel = mCardLoaclList[0].transform.position.y + 20;
        }
        return  mCardLoaclList[index].transform.position.x;
    }


    public override void resetEnd() {

        foreach (CardControl card in mList) {
            Destroy(card.gameObject);
        }
        mList.Clear();
        mCardList.Clear();
        for (int i = 0; i < 10; i++)
        {
            mNengLiangKuai[i].setCount(nengLiangDian);
        }

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

    // Update is called once per frame





    public override void addCardUpdate(CardJsonBean addCard)
    {
        mTime = 0;
        GameObject newobj = GameObject.Instantiate(
            card, new Vector2(2500, mYdel-23), Quaternion.Euler(0.0f, 0f, 0.0f));
        newobj.AddComponent<CardControl>();
        newobj.GetComponent<CardUiControl>().init(addCard.id, 107, 146);
        newobj.GetComponent<CardUiControl>().init(addCard.id, CardUiControl.TYPE_CARD_PLAY, mLevelManager.mPlayerControl);
        newobj.GetComponent<CardUiControl>().showCard();
        CardControl enmey = newobj.GetComponent<CardControl>();
        newobj.transform.SetParent(gameObject.transform);
        newobj.transform.localScale = Vector3.one;
        //enmey.init(id, 107, 146);
        enmey.init(mList.Count, this, addCard.id);
        mList.Add(enmey);     
    }
    public override void giveUpCardDeal(int index)
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
    public override void userCardDeal(int index) {
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

    private void initNengliangkuai()
    {
        mNengLiangKuai.Clear();
        for (int i = 1; i <= 10; i++)
        {
            NengliangkuaiControl tmp1 = GameObject.Find("nengliangkuai_" + i).GetComponent<NengliangkuaiControl>();
            tmp1.init();
            tmp1.setCount(nengLiangDian);
            mNengLiangKuai.Add(tmp1);
        }
    }

    public override void nengliangShowUpdate()
    {
        if (mNengLiangKuai.Count < 10)
        {

            initNengliangkuai();
        }
        if (nengLiangDian >= 10)
        {
            nengLiangDian = 10;
        }
        for (int i = 0; i < 10; i++)
        {
            mNengLiangKuai[i].setCount(nengLiangDian);
        }
    }

    public override long getCreatCard() {
        return getRandomCard();
    }
}
