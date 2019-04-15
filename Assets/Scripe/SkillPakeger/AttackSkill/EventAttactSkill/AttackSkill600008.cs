using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSkill600008 : EventAttackSkill
{


    int count1 = 0;
    int count2 = 0;
    int count3 = 0;
    public override void killEnemy()
    {

        if (count1 == 0) { 
            count1 = (int)(mSkillJson.getSpecialParameterValue()[0] * 100);
            count3 = (int)(mSkillJson.getSpecialParameterValue()[1]) * 100;
        }
        if (count2 == count3)
        {
            return;
        }
        count2 += count1;


        if (count2 > count3)
        {
            mManager.getAttacker().mSkillAttribute.crt +=(count3 - (count2- count1)) ;
            mManager.getAttacker().getAttribute(true);
            count2 = count3;
            return;
        }
       
        mManager.getAttacker().mSkillAttribute.crt += count1;
        mManager.getAttacker().getAttribute(true);
//        value++;
//        mManager.getAttacker().mSkillAttributePre.defense += (value * count1);

    }
    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_HURT_DIE, this);
        mManager.getAttacker().mSkillAttribute.crt -= count2;
        mManager.getAttacker().getAttribute(true);
    }

    public override void startSkill()
    {
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_HURT_DIE, this);
    }

}
