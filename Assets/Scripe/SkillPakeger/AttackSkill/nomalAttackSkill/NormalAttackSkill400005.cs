using UnityEngine;
public class NormalAttackSkill400005 : NormalAttackSkillBase
{
    public override void endSkill()
    {
        Attacker a = mManager.getAttacker();
        a.mSkillAttribute.crt -= value;
    }

    public override void startSkill()
    {
        Attacker a = mManager.getAttacker();
        a.mSkillAttribute.crt += value ;
    }
}