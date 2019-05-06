using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSkill600002 : EventAttackSkill
{
    float count2 = 0;
    int count1 = 0;
    public override void endHurt(HurtStatus hurt, Attacker attacker)
    {

        if (count1 == 0)
        {
            count1 =(int) (mSkillJson.getSpecialParameterValue()[0] * 100);
            count2 = mSkillJson.getSpecialParameterValue()[1] / 100;
        }

        bool isSuccess = randomResult(10000, count1, false);

        mFight.AddBlood(count2 * hurt.blood);
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
