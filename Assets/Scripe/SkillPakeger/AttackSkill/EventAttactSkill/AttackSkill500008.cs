using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSkill500008 : EventAttackSkill
{


    float count1 = 0;
    LocalManager mLocal = null;
    public override void addValueEnd()
    {

        mValue = mFight.getLunhuiValue(500008, 10, 0);
        addEachAlive(AttributePre.aggressivity);
    }

    public override void debuffLitterMonster(Attacker monster)
    {
       
        monster.mAllAttributePre.updateDebuff(mSkillIndex, AttributePre.aggressivity, (long)mValue);
        monster.getAttribute(true);
    }

    public override void endSkill()
    {

        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_LITTER_DEBUFF, this);
        LocalBean list = mLocal.mLocalLink;
        while (list != null)
        {
            if (!list.mIsHero)
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
        mValue = mFight.getLunhuiValue(500008, 10, 0);
        // }
        addEachAlive(AttributePre.aggressivity);
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_LITTER_DEBUFF, this);

    }
    private void addEachAlive(long type) {
        
        mLocal = GameObject.Find("Manager").GetComponent<LevelManager>().mLocalManager;
        LocalBean list = mLocal.mLocalLink;
        while (list != null)
        {
            if (!list.mIsHero && list.mAttacker.mAttackType != Attacker.ATTACK_TYPE_BOSS)
            {
                list.mAttacker.mAllAttributePre.updateDebuff(mSkillIndex, type, (long)mValue); ;
                list.mAttacker.getAttribute(true);
            }
            list = list.next;
        }
    }


}
