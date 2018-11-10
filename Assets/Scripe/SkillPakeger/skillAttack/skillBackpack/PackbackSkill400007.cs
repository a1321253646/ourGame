using UnityEngine;
public class PackbackSkill400007 : PackbackSkillBase
{
    public override void removeSkill()
    {
        mManager.cardHurtPre -= value / 10000;
    }

    public override void startSkill()
    {
        mManager.cardHurtPre += value / 10000;
    }
}