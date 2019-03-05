using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class TimeEventAttackSkillBase : EventAttackSkill
{
    public float mTime = 0;
    public bool isInit = false;
    public abstract void upDateSkill();

}
