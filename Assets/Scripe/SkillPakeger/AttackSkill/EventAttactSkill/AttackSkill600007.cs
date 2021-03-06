﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSkill600007 : EventAttackSkill
{


    float count1 = 0;
    public override void killEnemy()
    {

        if (count1 == 0) { 
            count1 = mSkillJson.getSpecialParameterValue()[0] /100;
        }
        mManager.getAttacker().AddBlood(count1 * mManager.getAttacker().mAttribute.aggressivity);

    }
    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_HURT_DIE, this);
    }

    public override void startSkill()
    {
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_HURT_DIE, this);
    }
}
