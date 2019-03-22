using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSkill500009 : EventAttackSkill
{


    float count1 = 0;
    LocalManager mLocal = null;
    public override void addValueEnd()
    {

        mValue = mFight.getLunhuiValue(500009, 11, 0);
        addEachAlive(AttributePre.aggressivity);
    }



    public override void debuffBoss(Attacker monster)
    {
        monster.mAllAttributePre.updateDebuff(mSkillIndex, AttributePre.aggressivity, (long)mValue);
        monster.getAttribute(true);
    }

    public override void endSkill()
    {

        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_BOSS_DEBUFF, this);
        LocalBean list = mLocal.mLocalLink;
        while (list != null)
        {
            if (list.mIsHero)
            {

                list.mAttacker.mAllAttributePre.updateDebuff(mSkillIndex, AttributePre.aggressivity, 0);
                list.mAttacker.getAttribute(true);
            }
            list = list.next;
        }
    }

    public override void startSkill()
    {
        // if (mValue == 0)
        // {
        mValue = mFight.getLunhuiValue(500009, 11, 0);
        // }
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_BOSS_DEBUFF, this);
        addEachAlive(AttributePre.aggressivity);

    }
    private void addEachAlive(long type) {
        
        mLocal = GameObject.Find("Manager").GetComponent<LevelManager>().mLocalManager;
        LocalBean list = mLocal.mLocalLink;
        while (list != null)
        {
            if (!list.mIsHero && list.mAttacker.mAttackType == Attacker.ATTACK_TYPE_BOSS)
            {
                list.mAttacker.mAllAttributePre.updateDebuff(mSkillIndex, type, (long)mValue); ;
                list.mAttacker.getAttribute(true);
            }
            list = list.next;
        }
    }


}
