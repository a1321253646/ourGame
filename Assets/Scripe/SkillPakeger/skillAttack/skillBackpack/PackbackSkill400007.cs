using UnityEngine;
public class PackbackSkill400007 : PackbackSkillBase
{
    public override void removeSkill()
    {
        mManager.cardCardHurtPre -= ((float)value / 10000);
    }

    public override void startSkill()
    {
        mManager.cardCardHurtPre += ((float)value / 10000);
    }
}