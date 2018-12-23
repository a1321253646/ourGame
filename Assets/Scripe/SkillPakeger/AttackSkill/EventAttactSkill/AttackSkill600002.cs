using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSkill600002 : EventAttackSkill
{
    public override void Acttacking()
    {
        int count = (int)(mSkillJson.getEffectsParameterValue()[0] * mManager.getAttacker().mAttribute.aggressivity);
        mManager.getAttacker().AddBlood(count);
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
