using UnityEngine;
using System.Collections;

public class TimeEventAttackSkill200003 : TimeEventAttackSkillBase
{
    public CalculatorUtil mCalcuator;
    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_ATTACKING, this);
        mStatus = SKILL_STATUS_END;
    }

    public override void startSkill()
    {
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_ATTACKING, this);

        mCalcuator = new CalculatorUtil(mSkillJson.calculator, mSkillJson.effects_parameter);
        mCalcuator.setSkill(this);
        isInit = true;
    }

    public override void Acttacking()
    {
        double hurt = mCalcuator.getValue(mManager.getAttacker(), mFight);
        mFight.AddBlood(hurt);
    }

    public override void upDateSkill()
    {
        if (!isInit)
        {
            return;
        }
        mTime += Time.deltaTime;
        //}
        if (mTime > value)
        {
            endSkill();
        }
    }
}
