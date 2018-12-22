﻿using UnityEngine;
using System.Collections;

public class AttackSkill600003 : AttackSkillNoAnimal
{


    int count = 0;
    public override bool add(float count)
    {
        value = value + count;
        Debug.Log("====================AttackSkill20001 value=" + value);
        return true;
    }

    public override float beAction(HurtStatus status)
    {
        return 0;
    }
    public override void initEnd()
    {
    }

    public override void upDateEnd()
    {
    }

    public override void inAction()
    {
        count = (int)(mSkillJson.getEffectsParameterValue()[0] * mManager.getAttacker().mAttribute.aggressivity);
        string str = count + "";
        BigNumber big = BigNumber.getBigNumForString(str);
        GameManager.getIntance().mCurrentCrystal = BigNumber.add(GameManager.getIntance().mCurrentCrystal, big);
        GameManager.getIntance().updateGasAndCrystal();

    }
    
}