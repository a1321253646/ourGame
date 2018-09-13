﻿using UnityEngine;
using System.Collections;

public class AttackSkill4 : AttackSkillNoAnimal
{

    public override bool add()
    {
        return false;
    }

    public override float beAction(HurtStatus status)
    {
        return -1;
    }

    public override void initEnd()
    {
        GameObject.Find("Manager").GetComponent<LevelManager>().addNengliangDian(5);
    }

    public override void upDateEnd()
    {
    }
}
