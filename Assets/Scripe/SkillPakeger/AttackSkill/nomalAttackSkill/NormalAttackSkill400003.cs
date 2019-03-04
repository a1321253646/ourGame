using UnityEngine;
public class NormalAttackSkill400003 : NormalAttackSkillBase
{
    public override void endSkill()
    {

        mManager.getAttacker().mAllAttributePre.minus(mSkillIndex, AttributePre.maxBloodVolume, (long)value);

    }

    public override void startSkill()
    {
        mManager.getAttacker().mAllAttributePre.add(mSkillIndex, AttributePre.maxBloodVolume, (long)value);

    }
}