using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCardManager : CardManagerBase
{
    private Transform mCanvas;
    
    LevelManager mLevelManager;
    
    Random mRandom = new Random();
   
    // Use this for initialization
    void Start () {
        mCanvas = GameObject.Find("Canvas").transform;
        mLevelManager = GameObject.Find("Manager").GetComponent<LevelManager>();        
    }

    public override void resetEnd() {

        mCardList.Clear();
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

    public override long getCreatCard() {
        return getRandomCard();
    }

    public override void updateEnd()
    {

    }
}
