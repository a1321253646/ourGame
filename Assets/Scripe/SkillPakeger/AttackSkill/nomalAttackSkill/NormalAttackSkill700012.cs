using UnityEngine;
public class NormalAttackSkill700012 : NormalAttackSkillBase
{
    public override void endSkill()
    {
        mManager.getAttacker().mSkillAttribute.crt -= (long)value;
    }

    public override void startSkill()
    {

        value = mParam[0]*100;
        mManager.getAttacker().mSkillAttribute.crt += (long)value;
    }
}