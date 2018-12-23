using UnityEngine;
using System.Collections;

public class AttackSkill600014 : EventAttackSkill
{


    int count1 = 0;
    public override bool endGetDrop()
    {
        if(count1 == 0) { 
            count1 = (int)(mSkillJson.getEffectsParameterValue()[0]*100);
        }

        bool isCrt = randomResult(10000, count1, false);
        return isCrt;

    }
    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_END_DROP, this);
    }

    public override void startSkill()
    {
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_END_DROP, this);
    }
}
