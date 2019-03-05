using UnityEngine;
public class NormalAttackSkill400007 : NormalAttackSkillBase
{
    public override void endSkill()
    {
        mManager.carHurtPre.deletById(400007);
    }

    public override void startSkill()
    {
        mManager.carHurtPre.AddFloat(400007, 1 + ((float)value / 10000));
    }
}