using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSkill600013 : EventAttackSkill
{

    FightManager mFit;
    float count1 = 0;
    public override void debuffMonster(Attacker monster)
    {
        monster.mAllAttributePre.minus(mSkillIndex, AttributePre.defense, (long)count1);
        monster.getAttribute(true);
    }

    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_MONSTER_DEBUFF, this);
        foreach (Attacker at in mFit.mAliveActtackers.Values)
        {
            if (at.mAttackType != Attacker.ATTACK_TYPE_HERO)
            {
                at.mAllAttributePre.add(mSkillIndex, AttributePre.defense, (long)count1);
                at.getAttribute(true);
            }
        }
    }

    public override void startSkill()
    {
        if (count1 == 0) {
            count1 = mSkillJson.getSpecialParameterValue()[0]*100;
        }
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_MONSTER_DEBUFF, this);

        mFit = GameObject.Find("Manager").GetComponent<LevelManager>().mFightManager;

        foreach (Attacker at in mFit.mAliveActtackers.Values)
        {
            if (at.mAttackType != Attacker.ATTACK_TYPE_HERO)
            {
                at.mAllAttributePre.minus(mSkillIndex, AttributePre.defense, (long)count1);
                at.getAttribute(true);
            }
        }
    }

}
