using UnityEngine;
public class NormalAttackSkill400001 : NormalAttackSkillBase
{
    public override void endSkill()
    {
        mManager.carHurtPre -= ((float)value / 10000);
    }

    public override void startSkill()
    {
        mManager.carHurtPre += ((float)value / 10000);
//        Debug.Log("mManager.hurtPre="+mManager.carHurtPre);
    }
}