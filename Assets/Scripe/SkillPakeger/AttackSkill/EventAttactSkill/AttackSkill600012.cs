﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSkill600012 : EventAttackSkill
{


    float count1 = 0;
    LocalManager mLocal = null;
    public override void debuffMonster(Attacker monster)
    {
        monster.mSkillAttributePre.attackSpeed -= count1;
        monster.getAttribute();
    }

    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_MONSTER_DEBUFF, this);
        LocalBean list = mLocal.mLocalLink;
        while (list != null)
        {
            if (!list.mIsHero)
            {
                list.mAttacker.mSkillAttributePre.attackSpeed += count1;
                list.mAttacker.getAttribute();
            }
            list = list.next;
        }
    }

    public override void startSkill()
    {
        if (count1 == 0) {
            count1 = mSkillJson.getSpecialParameterValue()[0]*100;
        }
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_MONSTER_DEBUFF, this);
        mLocal =  GameObject.Find("Manager").GetComponent<LevelManager>().mLocalManager;
        LocalBean list = mLocal.mLocalLink;
        while (list != null) {
            if (!list.mIsHero) {
                list.mAttacker.mSkillAttributePre.attackSpeed -= count1;
                list.mAttacker.getAttribute();
            }
            list = list.next;
        }
    }

}