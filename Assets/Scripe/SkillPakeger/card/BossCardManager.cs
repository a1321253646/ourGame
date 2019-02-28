using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossCardManager : CardManagerBase
{
    BossCardJsonBean mBean = null;
    private int cardIndex = 0;
    private Attacker mHero = null;
    
    public void show() {
        mHero = GameObject.Find("Manager").GetComponent<LevelManager>().mPlayerControl;
        GameManager.getIntance().getUiManager().showBossUi();
        Level level = JsonUtils.getIntance().getLevelData(
            BaseDateHelper.decodeLong(GameManager.getIntance().mCurrentLevel));
        mBean = JsonUtils.getIntance().getBossCardListById(level.card);
        isShow = true;
    }

    public void disShow() {
        if (!isShow) {
            return;
        }
        GameManager.getIntance().getUiManager().showGasUi();
        reset();
        isShow = false;
    }


    public override void resetEnd()
    {


    }

    private void isUserCard() {
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
        if (cardIndex < mBean.getCardList().Count) {
            long id =  mBean.getCardList()[cardIndex];
            cardIndex++;
            return id;
        }
        return 0;
    }
    public override void useEnd(CardControl cardControl)
    {
        Debug.Log(" ---------------------------- useEnd");

        int targetType = Attacker.CAMP_TYPE_DEFAULT;
        if (cardControl.mSkill.target_type == SkillJsonBean.TYPE_SELF)
        {
            targetType = Attacker.CAMP_TYPE_MONSTER ;
        }
        else if (cardControl.mSkill.target_type == SkillJsonBean.TYPE_ENEMY)
        {
            targetType = Attacker.CAMP_TYPE_PLAYER;
        }

        if (cardControl.mSkill.shape_type == 4)
        {
            if (mHero != null && mHero.status != Attacker.PLAY_STATUS_DIE)
            {
                mHero.mSkillManager.addSkill(cardControl.mSkill.id, mAttacker);
            }
        }
        else if (cardControl.mSkill.shape_type == 6)
        {
            mAttacker.mSkillManager.addSkill(cardControl.mSkill.id,mAttacker);
        }
        else if(mHero != null && mHero.status != Attacker.PLAY_STATUS_DIE)
        {
            SkillManage.getIntance().bossAddSkill(mAttacker, mHero,cardControl.mSkill, targetType);
        }
        Destroy(cardControl.gameObject);
    }

}
