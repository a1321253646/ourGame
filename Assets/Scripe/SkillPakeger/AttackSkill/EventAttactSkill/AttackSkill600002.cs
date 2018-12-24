using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSkill600002 : EventAttackSkill
{
    float count = 0;
    public override void Acttacking()
    {
        if (count == 0) {
            count = mSkillJson.getSpecialParameterValue()[0]/100;
        }
        int count1 = (int)(count * mManager.getAttacker().mAttribute.aggressivity);
        mManager.getAttacker().AddBlood(count1);
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
