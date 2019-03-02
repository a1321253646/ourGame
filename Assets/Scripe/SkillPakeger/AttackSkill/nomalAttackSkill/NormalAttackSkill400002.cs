using UnityEngine;
public class NormalAttackSkill400002 : NormalAttackSkillBase
{
    public override void endSkill()
    {
        mManager.getAttacker().mAllAttributePre.minus(mSkillIndex, AttributePre.aggressivity, (long)value);

    }

    public override void startSkill()
    {

        mManager.getAttacker().mAllAttributePre.add(mSkillIndex, AttributePre.aggressivity, (long)value);
    }
}