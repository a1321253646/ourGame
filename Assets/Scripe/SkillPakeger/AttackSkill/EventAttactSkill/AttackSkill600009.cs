using UnityEngine;
using System.Collections;

public class AttackSkill600009 : EventAttackSkill
{


    float count1 = 0;
    float count2 = 0;
    public override void killEnemy()
    {
        if(count1 == 0) { 
            count1 = (int)(mSkillJson.getEffectsParameterValue()[0]);
        }
        count2 += count1;
        mManager.carHurtPre += count1;
//        value++;
//        mManager.getAttacker().mSkillAttributePre.defense += (value * count1);

    }
    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_HURT_DIE, this);
        mManager.carHurtPre -= count2;
    }

    public override void startSkill()
    {
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_HURT_DIE, this);
    }
}
