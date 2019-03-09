using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSkill600015 : EventAttackSkill
{


    int count = 0;
    float count1 = 0;

    public override void endHurt(HurtStatus hurt)
    {
        if (count == 0)
        {
            count = (int)(mSkillJson.getSpecialParameterValue()[0] * 100);
            count1 = mSkillJson.getSpecialParameterValue()[1] / 100;
        }

        bool isSuccess = randomResult(10000, count, false);

        if (isSuccess)
        {
            mManager.getAttacker().AddBlood((count1 * hurt.blood));
        }
    }

    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_END_EHURT, this);
    }

    public override void startSkill()
    {
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_END_EHURT, this);
    }
}
