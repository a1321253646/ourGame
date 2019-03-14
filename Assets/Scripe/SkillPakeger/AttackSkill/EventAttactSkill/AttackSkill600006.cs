using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSkill600006 : EventAttackSkill
{


    int count1 = 0;
    int count2 = 0;
    int count3 = -1;
    public override void killEnemy()
    {
        if(count2 == count3)
        {
            return;
        }

        if (count1 == 0) { 
            count1 = (int)(mSkillJson.getSpecialParameterValue()[0] *100);
            count3 = (int)(mSkillJson.getSpecialParameterValue()[1]) * 100;
        }
        
        count2 += count1;
        if (count2 > count3)
        {
            mManager.getAttacker().mAllAttributePre.add(mSkillIndex, AttributePre.aggressivity, (long)(count3 - count2));
            count2 = count3;           
            return;
        }

        mManager.getAttacker().mAllAttributePre.add(mSkillIndex, AttributePre.aggressivity, (long)count1);
        mManager.getAttacker().getAttribute(true);


    }
    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_HURT_DIE, this);
        mManager.getAttacker().mAllAttributePre.minus(mSkillIndex, AttributePre.aggressivity, (long)count2);
        mManager.getAttacker().getAttribute(true);
    }

    public override void startSkill()
    {
        Debug.Log("startSkill AttackSkill600008");
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_HURT_DIE, this);
    }

}
