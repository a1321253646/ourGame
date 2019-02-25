using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardManagerBase : MonoBehaviour {

    public static int GIVEUP_CARD_ALL = 1;
    public static int GIVEUP_CARD_MAX = 2;
    public static int GIVEUP_CARD_MIX = 3;
    public static int GIVEUP_CARD_RANGE = 4;

    public float nengLiangDian = 0;
    public float mPlayActionCount = 0;

    public int addNengliangAttack = 0;

    public long mCount = 0;
    public float mOutSendCardTime = 0;
    public float mTime = 0;
    public GameObject mCardLocalUp, mCardLocalTop;
    public GameObject indicator;

    public abstract void nengliangShowUpdate();
    public abstract void addCardUpdate(CardJsonBean card);
    public abstract void giveUpCardDeal(int index);
    public abstract void userCardDeal(int index);
    public abstract long getCreatCard();
    public abstract void resetEnd();

    public static float CREADT_CARD_TIME = -1;
    public static float OUT_CREADT_CARD_TIME = 0.1f;

    public bool isInit = false;

    public int mMaxCardCount = 0;
    public Attacker mAttacker = null;
    public List<CardJsonBean> mCardIdList = new List<CardJsonBean>();
    public GameObject card;
    public LocalManager mLocalManage;


    public void reset() {
        isInit = false;

        mCount = 0;
        mOutSendCardTime = 0;
        mTime = 0;
        mCardIdList.Clear();      
        nengLiangDian = 0;
        resetEnd();
        isInit = true;
    }


    void Update()
    {
        if (!isInit)
        {
            return;
        }
        if (mMaxCardCount == 0)
        {
            mMaxCardCount = (int)JsonUtils.getIntance().getConfigValueForId(100015);
        }
        if (mCount > 0)
        {
            mOutSendCardTime += Time.deltaTime;
            if (mOutSendCardTime >= OUT_CREADT_CARD_TIME && mCardIdList.Count < mMaxCardCount)
            {
                mOutSendCardTime -= OUT_CREADT_CARD_TIME;
                mCount--;
                long random = getCreatCard();
                if (random != 0)
                {
                    addCard(random);
                }

            }
            //return;
        }
        mTime += Time.deltaTime;

        if (mTime >= CREADT_CARD_TIME && mCardIdList.Count < mMaxCardCount)
        {
            long random = getCreatCard();
            if (random != 0)
            {
                addCard(random);
            }
        }
    }

    public LocalManager getLocalManager()
    {
        if (mLocalManage == null)
        {
            mLocalManage = GameObject.Find("Manager").GetComponent<LevelManager>().getLocalManager();
        }
        return mLocalManage;
    }

    public class CardUser
    {
        public long id;
        public bool isUse = false;
    }

    public void init(Attacker attacker)
    {
        isInit = true;
        mAttacker = attacker;
        if (CREADT_CARD_TIME == -1)
        {
            if (attacker.mAttackType == Attacker.ATTACK_TYPE_HERO)
            {
                CREADT_CARD_TIME = JsonUtils.getIntance().getConfigValueForId(100043);
                addNengliangAttack =(int) JsonUtils.getIntance().getConfigValueForId(100014);
            }
            else
            {
                CREADT_CARD_TIME = JsonUtils.getIntance().getConfigValueForId(100051);
                addNengliangAttack = (int)JsonUtils.getIntance().getConfigValueForId(100014);
            }
        }
    }

    public void addCard(long id)
    {
        CardJsonBean card = JsonUtils.getIntance().getCardInfoById(id);
        mCardIdList.Add(card);
        mTime = 0;
        addCardUpdate(card);
    }



    public void playerAction()
    {
       

        mPlayActionCount++;
        if (mPlayActionCount >= addNengliangAttack)
        {
            addNengliangDian(1);
            mPlayActionCount = 0;
        }
    }

    

    public void addNengliangDian(long nengliang)
    {
        //        Debug.Log("addNengliangDian= " + nengliang);
        nengLiangDian++;
        Debug.Log("========playerAction addNengliangDian=" + mPlayActionCount + " addNengliangAttack=" + addNengliangAttack);
        if (nengLiangDian > 10)
        {
            nengLiangDian = 10;
            return;
        }
        nengliangShowUpdate();
    }

    public bool delectNengliangdian(float nengliang)
    {
        if (nengliang > nengLiangDian)
        {
            return false;
        }
        nengLiangDian -= nengliang;
        nengliangShowUpdate();
        return true;
    }

    public Attacker getAttacker()
    {
        return mAttacker;
    }

    public void addCards(long count)
    {
        mCount += count;
        mOutSendCardTime = 0;
    }
    public long giveupCard(int type)
    {
        if (mCardIdList.Count == 0)
        {
            return 1;
        }
        long count = 0;
        if (type == GIVEUP_CARD_ALL)
        {
            count = mCardIdList.Count;
            while (mCardIdList.Count > 0)
            {
                giveUpCardDeal(0);
                mCardIdList.RemoveAt(0);
            }
            return count;
        }
        else if (type == GIVEUP_CARD_MAX)
        {
            CardJsonBean max = null;
            int index = 0;
            for (int i = 0; i < mCardIdList.Count; i++) {
                CardJsonBean c = mCardIdList[i];
                if (max == null)
                {
                    max = c;
                    index = i;
                }
                else if (c.cost > max.cost)
                {
                    max = c;
                    index = i;
                }
            }
            count = max.cost;
            mCardIdList.RemoveAt(index);
            giveUpCardDeal(index);
            return count;
        }
        else if (type == GIVEUP_CARD_MIX)
        {
            CardJsonBean max = null;
            int index = 0;
            for (int i = 0; i < mCardIdList.Count; i++)
            {
                CardJsonBean c = mCardIdList[i];
                if (max == null)
                {
                    max = c;
                    index = i;
                }
                else if (c.cost < max.cost)
                {
                    max = c;
                    index = i;
                }
            }
            count = max.cost;
            mCardIdList.RemoveAt(index);
            giveUpCardDeal(index);
            return count;
        }
        else if (type == GIVEUP_CARD_RANGE)
        {
            if(mCardIdList.Count == 0) {
                return 1;    
            }
            int leng = mCardIdList.Count;
            int range = Random.Range(0, leng - 1);
            count = mCardIdList[range].cost;
            mCardIdList.RemoveAt(range);
            giveUpCardDeal(range);
            return count;
        }
        return -1;
    }
    public bool userCard(int index, float cost)
    {
        cost = mAttacker.mSkillManager.mEventAttackManager.getCardCost((int)cost);
        if (!delectNengliangdian(cost))
        {
            return false;
        }
        mCardIdList.RemoveAt(index);
        userCardDeal(index);
        return true;
    }

    public float getUpLocalY()
    {
        return mCardLocalUp.transform.position.y;
    }
    public GameObject getIndicator()
    {
        return indicator;
    }
    public float getTopLocalY()
    {
        return mCardLocalTop.transform.position.y;
    }
    public virtual float getLocalXByIndex(int index)
    {
        return 0;
    }
}
