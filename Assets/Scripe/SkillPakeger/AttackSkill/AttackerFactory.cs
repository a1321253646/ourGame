﻿using UnityEngine;
using System.Collections;

public class AttackerFactory 
{
    public static AttackerSkillBase getSkillById(long id) {
        if (id == 100002) {
            return new AnimalAttackSkill100002();
        }
        else if (id == 100008)
        {
            return new AnimalAttackSkill100008();
        }
        else if (id == 200001)
        {
            return new AttackSkill200001();
        }
        else if (id == 200002)
        {
            return new AnimalAttackSkill200002();
        }
        else if (id == 200003)
        {
            return new TimeEventAttackSkill200003();
        }
        else if (id == 300001)
        {
            return new NormalAttackSkill300001();
        }
        else if (id == 400001)
        {
            return new NormalAttackSkill400001();
        }
        else if (id == 400002)
        {
            return new NormalAttackSkill400002();
        }
        else if (id == 400003)
        {
            return new NormalAttackSkill400003();
        }
        else if (id == 400004)
        {
            return new NormalAttackSkill400004();
        }
        else if (id == 400005)
        {
            return new NormalAttackSkill400005();
        }
        else if (id == 400006)
        {
            return new NormalAttackSkill400006();
        }
        else if (id == 400007)
        {
            return new NormalAttackSkill400007();
        }
        else if (id == 400008)
        {
            return new NormalAttackSkill400008();
        }
        else if (id == 400009)
        {
            return new NormalAttackSkill400009();
        }
        else if (id == 500001)
        {
            return new NormalAttackSkill500001();
        }
        else if (id == 500002)
        {
            return new NormalAttackSkill500002();
        }
        else if (id == 500003)
        {
            return new NormalAttackSkill500003();
        }
        else if (id == 600001)
        {
            return new AttackSkill600001();
        }
        else if (id == 600002)
        {
            return new AttackSkill600002();
        }
        else if (id == 600003)
        {
            return new AttackSkill600003();
        }
        else if (id == 600004)
        {
            return new AttackSkill600004();
        }
        else if (id == 600005)
        {
            return new AttackSkill600005();
        }
        else if (id == 600006)
        {
            return new AttackSkill600006();
        }
        else if (id == 600007)
        {
            return new AttackSkill600007();
        }
        else if (id == 600008)
        {
            return new AttackSkill600008();
        }
        else if (id == 600009)
        {
            return new AttackSkill600009();
        }
        else if (id == 600011)
        {
            return new AttackSkill600011();
        }
        else if (id == 600014)
        {
            return new AttackSkill600014();
        }
        else if (id == 600010)
        {
            return new UpdateSkill600010();
        }
        return null;
    }
}