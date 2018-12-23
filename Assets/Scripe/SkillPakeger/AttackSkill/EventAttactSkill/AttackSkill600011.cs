using UnityEngine;
using System.Collections;

public class AttackSkill600011 : EventAttackSkill
{
    public int count = 0;
    public override int getCardCost(int cost)
    {
        if (count == 0)
        {
            count = (int)(mSkillJson.getEffectsParameterValue()[0] * 100);
        }

        bool isCrt = randomResult(10000, count, false);
        if (isCrt)
        {
            return 0;
        }
        else {
            return cost;
        }     
    }
    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_BEFOE_CARD_COST, this);
    }

    public override void startSkill()
    {
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_BEFOE_CARD_COST, this);
    }
}
