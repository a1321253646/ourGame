using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class NormalAttackSkillBase : AttackerSkillBase
{
    public bool isGiveup = false;
    public override void initSkill(AttackSkillManager manager, long skillId, Attacker fight, List<float> par, GameObject newobj,bool giveup, long skillIndex)
    {
        mSkillIndex = skillIndex;
        isGiveup = giveup;
        mManager = manager;
        mFight = fight;
        mSkillJson = JsonUtils.getIntance().getSkillInfoById(skillId);
        mParam = par;
        if (mParam == null) {
            mParam = mSkillJson.getSpecialParameterValue();
        }
        value = mParam[0];
    }
}