using UnityEngine;
public class PackbackSkill400001 : PackbackSkillBase
{
    public override void removeSkill()
    {
        mManager.hurtPre -= value / 10000;
    }

    public override void startSkill()
    {
        mManager.hurtPre += ((float)value / 10000);
        Debug.Log("mManager.hurtPre="+mManager.hurtPre);
    }
}