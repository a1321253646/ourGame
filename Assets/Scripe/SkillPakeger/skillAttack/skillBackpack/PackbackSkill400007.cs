using UnityEngine;
public class PackbackSkill400007 : PackbackSkillBase
{
    public override void removeSkill()
    {
        mManager.cardCardHurtPre -= value / 10000;
    }

    public override void startSkill()
    {
        mManager.cardCardHurtPre += value / 10000;
    }
}