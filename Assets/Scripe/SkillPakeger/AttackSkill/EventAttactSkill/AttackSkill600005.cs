using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSkill600005 : EventAttackSkill
{


    int count1 = 0;
    int count2 = 0;
    public override void killEnemy()
    {
        if(count1 == 0) { 
            count1 = (int)(mSkillJson.getSpecialParameterValue()[0])*100;
        }
        count2 += count1;
        mManager.getAttacker().mSkillAttributePre.defense += count1;
        Debug.Log("startSkill killEnemy count2=" + count2);
        mManager.getAttacker().getAttribute();
    }

    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_HURT_DIE, this);
        mManager.getAttacker().mSkillAttributePre.defense -= count2;
        mManager.getAttacker().getAttribute();
    }

    public override void startSkill()
    {
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_HURT_DIE, this);
    }

}
