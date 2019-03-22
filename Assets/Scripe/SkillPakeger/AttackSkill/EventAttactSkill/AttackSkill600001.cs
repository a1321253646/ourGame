using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSkill600001 : EventAttackSkill
{
    int count = 0;
    public override EquipKeyAndValue beforeActtack()
    {
       
        if (count == 0)
        {
            count = (int)(mSkillJson.getSpecialParameterValue()[0] * 100);
        }

        bool isCrt = randomResult(10000, count, false);

        if (!isCrt)
        {
            return null;
        }
        EquipKeyAndValue value = new EquipKeyAndValue();
        value.key = 111;
        value.value = 1;
        return value;
    }

    public override void startSkill()
    {
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_BEFOR_ATTACK, this);
    }

    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_BEFOR_ATTACK, this);
    }
}
