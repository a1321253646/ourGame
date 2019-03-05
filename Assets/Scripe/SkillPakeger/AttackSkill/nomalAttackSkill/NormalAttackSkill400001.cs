using UnityEngine;
public class NormalAttackSkill400001 : NormalAttackSkillBase
{
    public override void endSkill()
    {
        mManager.carHurtPre.deletById(400001);
    }

    public override void startSkill()
    {

        mManager.carHurtPre.AddFloat(400001, 1 + ((float)value / 10000));
//        Debug.Log("mManager.hurtPre="+mManager.carHurtPre);
    }
}