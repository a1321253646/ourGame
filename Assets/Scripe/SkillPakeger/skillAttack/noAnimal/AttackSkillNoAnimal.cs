using UnityEngine;
using System.Collections;

public abstract class AttackSkillNoAnimal : AttackSkillBase
{
    public override void init(AttackSkillManager manager, long skillId, Attacker fight)
    {
        mManager = manager;
        mFight = fight;
        mSkillJson = JsonUtils.getIntance().getSkillInfoById(skillId);
        calcuator = new CalculatorUtil(mSkillJson.calculator, mSkillJson.effects_parameter);
        isInit = true;
        initEnd();
    }


    public override void update()
    {
        upDateEnd();
    }
    //key说明 1 为无视防御

    public virtual EquipKeyAndValue beforeHurt()
    {
        return null;
    }
    public virtual void enemyDie()
    {
    }
}
