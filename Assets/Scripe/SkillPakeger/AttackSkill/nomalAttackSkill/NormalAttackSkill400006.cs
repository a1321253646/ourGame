using UnityEngine;
public class NormalAttackSkill400006 : NormalAttackSkillBase
{
    public override void endSkill()
    {
        Attacker a = mManager.getAttacker();
        a.mSkillAttribute.evd -= value ;
        
    }

    public override void startSkill()
    {
        Attacker a = mManager.getAttacker();
        a.mSkillAttribute.evd += value ;
        
    }
}