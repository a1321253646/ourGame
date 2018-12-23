using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSkill600006 : EventAttackSkill
{


    int count1 = 0;
    int count2 = 0;
    public override void killEnemy()
    {
        if(count1 == 0) { 
            count1 = (int)(mSkillJson.getEffectsParameterValue()[0] * 100);
        }
        mManager.getAttacker().mSkillAttributePre.crt += count1;
//        value++;
//        mManager.getAttacker().mSkillAttributePre.defense += (value * count1);

    }
    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_HURT_DIE, this);
        mManager.getAttacker().mSkillAttributePre.crt -= count2;
    }

    public override void startSkill()
    {
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_HURT_DIE, this);
    }

}
