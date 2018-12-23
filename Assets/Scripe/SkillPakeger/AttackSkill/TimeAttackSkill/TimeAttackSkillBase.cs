using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class TimeAttackSkillBase : AttackerSkillBase
{
    public bool isInit = false;
    public abstract void upDateSkill();
    public float mTime = 0;
    public override void initSkill(AttackSkillManager manager, long skillId, Attacker fight, List<float> value, GameObject newobj)
    {
        mManager = manager;
        mFight = fight;
        mSkillJson = JsonUtils.getIntance().getSkillInfoById(skillId);
        if (value != null)
        {
            mParam = value;
        }
        else {
            mParam = mSkillJson.getSpecialParameterValue();
        }
        
    }
    public virtual void upDateLocal(float x, float y) {

    }
}
