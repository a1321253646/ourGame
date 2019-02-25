using UnityEngine;
public class NormalAttackSkill400002 : NormalAttackSkillBase
{
    public override void endSkill()
    {
        Attacker a = mManager.getAttacker();
        a.mSkillAttributePre.aggressivity -= value;

    }

    public override void startSkill()
    {
        Attacker a = mManager.getAttacker();
        a.mSkillAttributePre.aggressivity += value;
    }
}