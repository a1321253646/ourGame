using UnityEngine;
using System.Collections;

public class AttackSkill700009 : EventAttackSkill
{
    float count1 = -1;
    float count2 = -1;
    public override void endHurt(HurtStatus hurt, Attacker attacker)
    {
        if (count1 == -1)
        {
            count1 = mSkillJson.getSpecialParameterValue()[0];
            count2 = mSkillJson.getSpecialParameterValue()[1]/100;
        }
        bool isCrt = randomResult(100, (int)count1, false);
        if (isCrt)
        {
            hurt.blood =mManager.getAttacker().mAttribute.maxBloodVolume * count2;
            attacker.allHurt(hurt, mFight);
          
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
