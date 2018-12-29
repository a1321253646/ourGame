using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSkill600004 : EventAttackSkill
{


    int count = 0;
    float count1 = 0;

    public override void Acttacking()
    {
        if (count == 0) {
            count = (int)(mSkillJson.getSpecialParameterValue()[0] * 100);
            count1 = mSkillJson.getSpecialParameterValue()[1]/100;
        }
        
        bool isSuccess = randomResult(10000, count, false);
        if (isSuccess) {
            mManager.getAttacker().AddBlood((count1 * mManager.getAttacker().mAttribute.aggressivity));
        }
    }
    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_ATTACKING, this);
    }

    public override void startSkill()
    {
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_ATTACKING, this);
    }
}
