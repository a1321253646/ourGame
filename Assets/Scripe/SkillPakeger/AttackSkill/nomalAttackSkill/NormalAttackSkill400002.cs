using UnityEngine;
public class NormalAttackSkill400002 : NormalAttackSkillBase
{
    public override void endSkill()
    {
        Debug.Log("-----------------NormalAttackSkill400002 endSkill");
        mManager.getAttacker().mAllAttributePre.minus(mSkillIndex, AttributePre.aggressivity, (long)value);

    }

    public override void startSkill()
    {

        mManager.getAttacker().mAllAttributePre.add(mSkillIndex, AttributePre.aggressivity, (long)value);
    }
}