using UnityEngine;
using System.Collections;

public class AttackSkill200001 : EventAttackSkill
{
    CalculatorUtil calcuator;

    public override void beforeBeHurt(HurtStatus status)
    {
        Debug.Log("====================AttackSkill20001 beAction value =" + value+ " status.blood="+ status.blood);
        Debug.Log("====================AttackSkill20001 beAction  mSkillJson.getEffectsParameterValue()[0]=" + mSkillJson.getEffectsParameterValue()[0]);

        status.blood = status.blood * (1+mSkillJson.getEffectsParameterValue()[0] * value);
        Debug.Log("====================AttackSkill20001 beAction =" + status.blood);
    }


    public override void startSkill()
    {
        calcuator = new CalculatorUtil(mSkillJson.calculator, mSkillJson.effects_parameter);
        calcuator.setSkill(this);
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_BEFOR_BEHURT, this);

    }

    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_BEFOR_BEHURT, this);
    }
}
