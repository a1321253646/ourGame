using UnityEngine;
public class NormalAttackSkill400004 : NormalAttackSkillBase
{
    public override void endSkill()
    {
        mManager.getAttacker().mAllAttributePre.minus(mSkillIndex, AttributePre.defense, (long)value);

    }

    public override void startSkill()
    {

        mManager.getAttacker().mAllAttributePre.add(mSkillIndex, AttributePre.defense, (long)value);
    }
}