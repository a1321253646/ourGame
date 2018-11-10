using UnityEngine;
public class PackbackSkill400008 : PackbackSkillBase
{
    public override void removeSkill()
    {
        mManager.downCardCost -= value;
    }

    public override void startSkill()
    {
        mManager.downCardCost += value;
    }
}