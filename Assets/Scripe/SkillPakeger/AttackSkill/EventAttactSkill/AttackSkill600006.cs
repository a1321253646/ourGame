using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSkill600006 : EventAttackSkill
{


    int count1 = 0;
    int count2 = 0;
    public override void killEnemy()
    {
        Debug.Log("startSkill killEnemy");
        if (count1 == 0) { 
            count1 = (int)(mSkillJson.getSpecialParameterValue()[0] *100);
        }
        count2 += count1;
        Debug.Log("startSkill killEnemy count2="+ count2);
        mManager.getAttacker().mSkillAttributePre.aggressivity += count1;
        mManager.getAttacker().getAttribute();


    }
    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_HURT_DIE, this);
        mManager.getAttacker().mSkillAttributePre.aggressivity -= count2;
        mManager.getAttacker().getAttribute();
    }

    public override void startSkill()
    {
        Debug.Log("startSkill AttackSkill600008");
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_HURT_DIE, this);
    }

}
