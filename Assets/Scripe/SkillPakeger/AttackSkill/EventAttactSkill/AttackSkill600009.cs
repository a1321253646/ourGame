using UnityEngine;
using System.Collections;

public class AttackSkill600009 : EventAttackSkill
{


    float count1 = 0;
    float count2 = 0;
    float count3 = 0;
    public override void killEnemy()
    {

        if (count1 == 0) { 
            count1 = (float)(mSkillJson.getSpecialParameterValue()[0]/100);
            count3 = (float)(mSkillJson.getSpecialParameterValue()[1]) / 100;
        }
        count2 += count1;
        if (count2 > count3)
        {
            count2 = count3;
            return;
        }
        

        mManager.carHurtPre.AddFloat(60009, 1+count2);
        mManager.getAttacker().getAttribute(true);
        //        value++;
        //        mManager.getAttacker().mSkillAttributePre.defense += (value * count1);

    }
    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_HURT_DIE, this);
        mManager.carHurtPre.deletById(60009);
        mManager.getAttacker().getAttribute(true);
    }

    public override void startSkill()
    {
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_HURT_DIE, this);
    }
}
