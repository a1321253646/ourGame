using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSkill500007 : EventAttackSkill
{


    float count1 = 0;
    LocalManager mLocal = null;
    public override void addValueEnd()
    {

        mValue =  mFight.getLunhuiValue(500007, 9,0);
        addEachAlive(AttributePre.maxBloodVolume);
    }

    public override void debuffBoss(Attacker monster)
    {
        monster.mAllAttributePre.updateDebuff(mSkillIndex, AttributePre.maxBloodVolume, (long)mValue);
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
                list.mAttacker.mAllAttributePre.updateDebuff(mSkillIndex, AttributePre.maxBloodVolume, 0);
                list.mAttacker.getAttribute(true);
            }
            list = list.next;
        }
    }

    public override void startSkill()
    {
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_BOSS_DEBUFF, this);
        //   if (mValue == 0)
        //   {
        mValue = mFight.getLunhuiValue(500007, 9, 0);
        //   }

        addEachAlive(AttributePre.maxBloodVolume);

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
