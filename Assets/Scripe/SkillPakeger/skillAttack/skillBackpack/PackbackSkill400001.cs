using UnityEngine;
public class PackbackSkill400001 : PackbackSkillBase
{
    public override void removeSkill()
    {
        mManager.carHurtPre -= value / 10000;
    }

    public override void startSkill()
    {
        mManager.carHurtPre += ((float)value / 10000);
        Debug.Log("mManager.hurtPre="+mManager.carHurtPre);
    }
}