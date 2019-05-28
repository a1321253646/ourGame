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

 //   public abstract void addCardUpdate(CardJsonBean card);
 //   public abstract void giveUpCardDeal(int index);
  //  public abstract void userCardDeal(int index);
    public abstract long getCreatCard();
    public abstract void resetEnd();
    public abstract void updateEnd();


    public float CREADT_CARD_TIME = -1;
    public float FRIST_CARD_TIME = -1;
    public static float OUT_CREADT_CARD_TIME = 0.01f;

    public bool isInit = false;

    public int mMaxCardCount = 0;
    public Attacker mAttacker = null;
    public List<CardJsonBean> mCardIdList = new List<CardJsonBean>();
    public GameObject card;
    public LocalManager mLocalManage;

    public List<NengliangkuaiControl> mNengLiangKuai = new List<NengliangkuaiControl>();
    public List<GameObject> mCardLoaclList = new List<GameObject>();
    public float mYdel;
    public List<CardControl> mList = new List<CardControl>();

    public bool isShow = false;
    public bool isFristCreat = false;

    private GameObject mCreatCardX;

    public void reset() {
        isInit = false;

        mCount = 0;
        mOutSendCardTime = 0;
        mTime = 0;
        mCardIdList.Clear();      
        nengLiangDian = 0;
        isFristCreat = false;
        foreach (CardControl card in mList)
        {
            Destroy(card.gameObject);
        }
        mList.Clear();

        for (int i = 0; i < 10; i++)
        {
            mNengLiangKuai[i].setCount(nengLiangDian);
        }
        resetEnd();

        isInit = true;
    }


    void Update()
    {
        if (!isInit)
        {
            return;
        }
        if (this is BossCardManager && !isShow) {
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
        if (mTime < FRIST_CARD_TIME) {
            return;
        }
        if (!isFristCreat) {
            isFristCreat = true;
            long random = getCreatCard();
            if (random != 0)
            {
                addCard(random);
            }
        }
        if (mTime >= CREADT_CARD_TIME+ FRIST_CARD_TIME && mCardIdList.Count < mMaxCardCount)
        {
            mTime -= CREADT_CARD_TIME;
            long random = getCreatCard();
            if (random != 0)
            {
                addCard(random);
            }

        }
        updateEnd();
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
        if (mCardLoaclList.Count == 0)
        {
            string name = "";
            if (mAttacker.mAttackType == Attacker.ATTACK_TYPE_HERO)
            {
                name = "kapai_local_";
            }
            else
            {
                name = "boss_kapai_local_";
            }
            for (int i = 1; i <= 6; i++)
            {
                mCardLoaclList.Add(GameObject.Find(name + i));
            }
            mYdel = mCardLoaclList[0].transform.position.y;
            if (mAttacker.mAttackType == Attacker.ATTACK_TYPE_HERO)
            {
                mCardLocalUp = GameObject.Find("kapai_local_up");
                mCardLocalTop = GameObject.Find("kapai_local_up_top");
                // mYdel = mCardLoaclList[0].transform.position.y + 20;
            }
            else
            {

            }
        }
        if (CREADT_CARD_TIME == -1)
        {
            if (attacker.mAttackType == Attacker.ATTACK_TYPE_HERO)
            {
                CREADT_CARD_TIME = JsonUtils.getIntance().getConfigValueForId(100043);
                FRIST_CARD_TIME = JsonUtils.getIntance().getConfigValueForId(100013);
                addNengliangAttack =(int) JsonUtils.getIntance().getConfigValueForId(100014);
            }
            else
            {
                CREADT_CARD_TIME = JsonUtils.getIntance().getConfigValueForId(100052);
                FRIST_CARD_TIME = JsonUtils.getIntance().getConfigValueForId(100051);
                addNengliangAttack = (int)JsonUtils.getIntance().getConfigValueForId(100053);
            }
        }
        initNengliangkuai();
        mCreatCardX = GameObject.Find("creat_card_x");
    }

    public void addCard(long id)
    {
        CardJsonBean card = JsonUtils.getIntance().getCardInfoById(id);
        mCardIdList.Add(card);
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
//        Debug.Log("========playerAction addNengliangDian=" + mPlayActionCount + " addNengliangAttack=" + addNengliangAttack);
        if (nengLiangDian > 10)
        {
            nengLiangDian = 10;
            return;
        }
        nengliangShowUpdate();
    }

    public bool delectNengliangdian(float nengliang)
    {
//        Debug.Log("---------------------------- delectNengliangdian -------------------------------- nengliang= " + nengliang+ " nengLiangDian="+ nengLiangDian);
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
            for (int i = 0; i < count-1; i++) {
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
        return mCardLoaclList[index].transform.position.x;
    }
    public void initNengliangkuai()
    {
        string nengkuiName = "";
        if (mAttacker.mAttackType == Attacker.ATTACK_TYPE_HERO)
        {
            nengkuiName = "nengliangkuai_";
        }
        else
        {
            nengkuiName = "boss_nengliangkuai_";
        }
        mNengLiangKuai.Clear();
        for (int i = 1; i <= 10; i++)
        {
            NengliangkuaiControl tmp1 = GameObject.Find(nengkuiName + i).GetComponent<NengliangkuaiControl>();
            tmp1.init();
            tmp1.setCount(nengLiangDian);
            mNengLiangKuai.Add(tmp1);
        }
    }

    public void nengliangShowUpdate()
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

    public  void addCardUpdate(CardJsonBean addCard)
    {

        GameObject newobj = GameObject.Instantiate(
                card, new Vector2(mCreatCardX.transform.position.x, mYdel ), Quaternion.Euler(0.0f, 0f, 0.0f));;
        if (mAttacker.mAttackType == Attacker.ATTACK_TYPE_HERO)
        {
        //    newobj = GameObject.Instantiate(
        //        card, new Vector2(2500, mYdel - 23), Quaternion.Euler(0.0f, 0f, 0.0f));
            newobj.AddComponent<CardControl>();
            newobj.GetComponent<CardUiControl>().init(addCard.id, 107, 146);
        }
        else
        {
         //   newobj = 
            newobj.AddComponent<CardControl>();
            newobj.GetComponent<CardUiControl>().init(addCard.id, 27.71f, 31.79f);
        }

        newobj.GetComponent<CardUiControl>().init(addCard.id, CardUiControl.TYPE_CARD_PLAY, mAttacker);
        newobj.GetComponent<CardUiControl>().showCard();
        CardControl enmey = newobj.GetComponent<CardControl>();
        newobj.transform.SetParent(gameObject.transform);
        newobj.transform.localScale = Vector3.one;
        //enmey.init(id, 107, 146);
        enmey.init(mList.Count, this, addCard.id);
        mList.Add(enmey);
    }

    public void giveUpCardDeal(int index)
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
    public void userCardDeal(int index)
    {
        CardControl card = null;
        for (int i = 0; i < mList.Count;)
        {
            if (mList[i].mIndex != index)
            {
                mList[i].deleteCard(index);
                i++;
            }
            else
            {
                card = mList[i];
                mList.Remove(mList[i]);
            }
        }
        if(card != null) {
            useEnd(card);
        }
       
    }
    public virtual void useEnd(CardControl cardControl)
    {

    }

}
