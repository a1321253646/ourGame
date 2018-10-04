using UnityEngine;
using System.Collections;

public abstract class AttackSkillNoAnimal : AttackSkillBase
{
    public abstract bool add();

    public override void init(AttackSkillManager manager, long skillId, Attacker fight)
    {
        mManager = manager;
        mFight = fight;
        mSkillJson = JsonUtils.getIntance().getSkillInfoById(skillId);
        initEnd();
        calcuator = new CalculatorUtil(mSkillJson.calculator, mSkillJson.effects_parameter);
        isInit = true;
    }


    public override void update()
    {
        upDateEnd();
    }
}
