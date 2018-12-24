using UnityEngine;
using System.Collections;

public class AttackSkill600011 : EventAttackSkill
{
    public int count = 0;
    public override int getCardCost(int cost)
    {
        if (count == 0)
        {
            count = (int)(mSkillJson.getSpecialParameterValue()[0] * 100);
        }

        bool isCrt = randomResult(10000, count, true);
        if (isCrt)
        {
            Debug.Log("无消耗");
            return 0;
        }
        else {
            Debug.Log("消耗");
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
