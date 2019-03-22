using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSkill700007 : EventAttackSkill
{
    float count1 = -1;
    float count2 = -1;
    public override void beforeHurt(HurtStatus hurt,Attacker attacker)
    {
        if (count1 == -1)
        {
            count1 = mSkillJson.getSpecialParameterValue()[0];
            count2 = mSkillJson.getSpecialParameterValue()[1];
        }
        bool isCrt = randomResult(100, (int)count1, false);
        if (isCrt)
        {
            SkillJsonBean nextSkill = JsonUtils.getIntance().getSkillInfoById(200002);
            List<float> vals = new List<float>();
            vals.Add(count2);
            nextSkill.setSpecialParameterValue(vals);
            attacker.mSkillManager.addSkill(200002,mFight,
                SkillIndexUtil.getIntance().getSkillIndexByCardId(true, mSkillIndex));
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
