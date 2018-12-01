using UnityEngine;
public class PackbackSkill400008 : PackbackSkillBase
{
    public override void removeSkill()
    {
        mManager.cardDownCardCost -= value;
    }

    public override void startSkill()
    {
        mManager.cardDownCardCost += value;
    }
}