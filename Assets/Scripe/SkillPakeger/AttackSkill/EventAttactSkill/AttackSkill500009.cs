﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSkill500009 : EventAttackSkill
{

    FightManager mFit;
    float count1 = 0;

    private double mSkillValue = 1;
    public override void addValueEnd()
    {

        mSkillValue = mFight.getLunhuiValue(500009, 11, 0);
        addEachAlive(AttributePre.aggressivity);
    }



    public override void debuffBoss(Attacker monster)
    {
        monster.mAllAttributePre.updateDebuff(mSkillIndex, AttributePre.aggressivity, (long)mValue);
        monster.getAttribute(true);
    }

    public override void endSkill()
    {

        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_BOSS_DEBUFF, this);
        foreach (Attacker at in mFit.mAliveActtackers.Values)
        {
            if (at.mAttackType == Attacker.ATTACK_TYPE_BOSS && at.mAttackType != Attacker.ATTACK_TYPE_HERO)
            {
                at.mAllAttributePre.updateDebuff(mSkillIndex, AttributePre.maxBloodVolume, 0);
                at.getAttribute(true);
            }
        }
    }

    public override void startSkill()
    {
        // if (mValue == 0)
        // {
        mSkillValue = mFight.getLunhuiValue(500009, 11, 0);
        // }
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_BOSS_DEBUFF, this);
        addEachAlive(AttributePre.aggressivity);

    }
    private void addEachAlive(long type) {
        mFit = GameObject.Find("Manager").GetComponent<LevelManager>().mFightManager;

        foreach (Attacker at in mFit.mAliveActtackers.Values)
        {
            if (at.mAttackType == Attacker.ATTACK_TYPE_BOSS && at.mAttackType != Attacker.ATTACK_TYPE_HERO)
            {
                at.mAllAttributePre.updateDebuff(mSkillIndex, type, mSkillValue); ;
                at.getAttribute(true);
            }
        }

    }


}
