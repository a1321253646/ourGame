using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossCardManager : CardManagerBase
{
    BossCardJsonBean mBean = null;
    private int cardIndex = 0;
    private Attacker mHero = null;
    BossCardControl mBossCardControl;
    List<long> mCardListTmp = new List<long>();
    public void show() {
        mHero = GameObject.Find("Manager").GetComponent<LevelManager>().mPlayerControl;
        mBossCardControl = GameObject.Find("boss_card").GetComponent<BossCardControl>();
       
        Level level = JsonUtils.getIntance().getLevelData(
            BaseDateHelper.decodeLong(GameManager.getIntance().mCurrentLevel));
        mBean = JsonUtils.getIntance().getBossCardListById(level.getCardListId());
        mCardListTmp.AddRange(mBean.getCardList());
        isShow = true;
    }

    public void disShow() {
        if (!isShow) {
            return;
        }
        GameManager.getIntance().getUiManager().showGasUi();
        reset();
        cardIndex = 0;
        isShow = false;
    }


    public override void resetEnd()
    {


    }

    private void isUserCard() {
        if (mBossCardControl.isShowIng()) {
            return;
        }
        if (mCardIdList.Count > 0 && mList[0].isInTarget)
        {
            CardJsonBean bean = mCardIdList[0];
            userCard(0, bean.cost);
        }
    }
    // Update is called once per frame
    public override void updateEnd()
    {
        isUserCard();
    }

    public override long getCreatCard()
    {
        if (mBean.random == 1)
        {
            if (mCardListTmp.Count == 0)
            {
                return 0;

            }
            else {
                int random = Random.Range(0, mCardListTmp.Count);
                long id = mCardListTmp[random];
                mCardListTmp.RemoveAt(random);
                return id;
            }
        }
        else {
            if (cardIndex < mBean.getCardList().Count)
            {
                long id = mBean.getCardList()[cardIndex];
                cardIndex++;
                return id;
            }
            else {
                return 0;
            }
        }
    }
    public override void useEnd(CardControl cardControl)
    {
        Debug.Log(" ---------------------------- useEnd");
        mBossCardControl.show(mAttacker,mHero, cardControl);
        Destroy(cardControl.gameObject);
    }

}
