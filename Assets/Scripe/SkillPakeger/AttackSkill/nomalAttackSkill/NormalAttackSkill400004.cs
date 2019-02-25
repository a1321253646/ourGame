using UnityEngine;
public class NormalAttackSkill400004 : NormalAttackSkillBase
{
    public override void endSkill()
    {
        Attacker a = mManager.getAttacker();
        a.mSkillAttributePre.defense -= value;

    }

    public override void startSkill()
    {
        Attacker a = mManager.getAttacker();
        a.mSkillAttributePre.defense += value;
    }
}