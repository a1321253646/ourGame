using UnityEngine;
public class NormalAttackSkill400009 : NormalAttackSkillBase
{
    public override void endSkill()
    {
        Attacker a = mManager.getAttacker();
        a.mAllAttributePre.minus(mSkillIndex, AttributePre.attackSpeed, (long)value);
    }

    public override void startSkill()
    {
        Attacker a = mManager.getAttacker();
        a.mAllAttributePre.add(mSkillIndex, AttributePre.attackSpeed, (long)value );
        a.getAttribute();
    }
}