using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSkill600003 : EventAttackSkill
{
    public override void Acttacking()
    {
        long  count = (int)(mSkillJson.getEffectsParameterValue()[0] * mManager.getAttacker().mAttribute.aggressivity);
        string str = count + "";
        BigNumber big = BigNumber.getBigNumForString(str);
        GameManager.getIntance().mCurrentCrystal = BigNumber.add(GameManager.getIntance().mCurrentCrystal, big);
        GameManager.getIntance().updateGasAndCrystal();
    }
    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_ATTACKING, this);
    }

    public override void startSkill()
    {
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_ATTACKING, this);
    }
}
