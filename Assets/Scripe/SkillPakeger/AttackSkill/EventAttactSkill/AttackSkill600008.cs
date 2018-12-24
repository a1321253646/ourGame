using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSkill600008 : EventAttackSkill
{


    int count1 = 0;
    int count2 = 0;
    public override void killEnemy()
    {
        if(count1 == 0) { 
            count1 = (int)(mSkillJson.getSpecialParameterValue()[0] * 100);
        }
        count2 += count1;
        Debug.Log(" AttackSkill600008 count2 = " + count2);
        mManager.getAttacker().mSkillAttribute.crt += count1;
        mManager.getAttacker().getAttribute();
//        value++;
//        mManager.getAttacker().mSkillAttributePre.defense += (value * count1);

    }
    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_HURT_DIE, this);
        mManager.getAttacker().mSkillAttribute.crt -= count2;
        mManager.getAttacker().getAttribute();
    }

    public override void startSkill()
    {
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_HURT_DIE, this);
    }

}
