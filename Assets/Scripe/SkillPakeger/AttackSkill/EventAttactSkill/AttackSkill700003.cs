using UnityEngine;
using System.Collections;

public class AttackSkill700003 : EventAttackSkill
{
    public bool isDeal = false;
    float count1 = -1;
    float count2 = -1;
    public override float afterPinga()
    {
        if (count1 == -1)
        {
            count1 = mSkillJson.getSpecialParameterValue()[0];
            count2 = mSkillJson.getSpecialParameterValue()[1];
        }
        if (isDeal) {
            isDeal = false;
            return count2;
        }
        bool isCrt = randomResult(100, (int)count1, false);
        if (isCrt)
        {
            
            return count2;
        }
        return 1;
    }
    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_PING_A, this);
    }

    public override void startSkill()
    {
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_PING_A, this);
    }

    public bool getIsCret() {
        if (count1 == -1)
        {
            count1 = mSkillJson.getSpecialParameterValue()[0];
            count2 = mSkillJson.getSpecialParameterValue()[1];
        }
        isDeal = randomResult(100, (int)count1, false);
        return isDeal;
    }
}
