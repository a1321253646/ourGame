using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSkill500006 : EventAttackSkill
{


    float count1 = 0;
    LocalManager mLocal = null;
    private double mSkillValue = 1;
    public override void addValueEnd()
    {

        mSkillValue = mFight.getLunhuiValue(500006, 8,0);
        addEachAlive(AttributePre.maxBloodVolume);
    }
    public override void debuffLitterMonster(Attacker monster)
    {
        monster.mAllAttributePre.updateDebuff(mSkillIndex, AttributePre.maxBloodVolume, (long)mValue);
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

                list.mAttacker.mAllAttributePre.updateDebuff(mSkillIndex, AttributePre.maxBloodVolume, 0);
                list.mAttacker.getAttribute(true);
            }
            list = list.next;
        }
    }

    public override void startSkill()
    {
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_LITTER_DEBUFF, this);
        //  if (mValue == 0)
        //  {
        mSkillValue =  mFight.getLunhuiValue(500006, 8, 0);
        //  }

        addEachAlive(AttributePre.maxBloodVolume);

    }
    private void addEachAlive(long type) {
        
        mLocal = GameObject.Find("Manager").GetComponent<LevelManager>().mLocalManager;
        LocalBean list = mLocal.mLocalLink;
        while (list != null)
        {
            if (!list.mIsHero && list.mAttacker.mAttackType != Attacker.ATTACK_TYPE_BOSS)
            {
                list.mAttacker.mAllAttributePre.updateDebuff(mSkillIndex, type, mSkillValue); ;
                list.mAttacker.getAttribute(true);
            }
            list = list.next;
        }
    }


}
