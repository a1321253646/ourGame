using UnityEngine;
using System.Collections;

public class AttackSkill700011 : EventAttackSkill
{
    int count1 = -1;
    int count2 = -1;
    public override void beforeHurt(HurtStatus hurt,Attacker attacker)
    {
        if (attacker.mBuffList.ContainsKey(700011)) {
            return;
        }
        if (count1 == -1)
        {
            count1 = (int)mSkillJson.getSpecialParameterValue()[0];
            count2 = (int)mSkillJson.getSpecialParameterValue()[1]*100;
        }
        bool isCrt = randomResult(100, count1, false);
        if (isCrt)
        {
            attacker.mBuffList.Add(700011, true);
            attacker.mAllAttributePre.minus(mSkillIndex,AttributePre.defense,count1);
            attacker.mAllAttributePre.minus(mSkillIndex,AttributePre.aggressivity,count1);
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
