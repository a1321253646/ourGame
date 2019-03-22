using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSkill600002 : EventAttackSkill
{
    float count = 0;

    public override void endHurt(HurtStatus hurt, Attacker attacker)
    {

        if (count == 0)
        {
            count = mSkillJson.getSpecialParameterValue()[0] / 100;
        }
        mFight.AddBlood(count * hurt.blood);
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
