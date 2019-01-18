using UnityEngine;
using System.Collections;

public class AttackSkill700003 : EventAttackSkill
{
    float count1 = -1;
    float count2 = -1;
    public override void beforeHurt(HurtStatus hurt)
    {
        if (count1 == -1)
        {
            count1 = mSkillJson.getSpecialParameterValue()[0];
            count2 = mSkillJson.getSpecialParameterValue()[1];
        }
        bool isCrt = randomResult(100, (int)count1, false);
        if (isCrt)
        {
            hurt.blood = hurt.blood * count2;
        }
    }
    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_BEFORE_HURT, this);
    }

    public override void startSkill()
    {
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_BEFORE_HURT, this);
    }
}
