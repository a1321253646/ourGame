using UnityEngine;
public class NormalAttackSkill400007 : NormalAttackSkillBase
{
    public override void endSkill()
    {
        mManager.cardCardHurtPre -= ((float)value / 10000);
    }

    public override void startSkill()
    {
        mManager.cardCardHurtPre += ((float)value / 10000);
    }
}