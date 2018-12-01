using UnityEngine;
public class PackbackSkill400011 : PackbackSkillBase
{
    public override void removeSkill()
    {
        GameManager.getIntance().mCardLunhuiGet -= (float)(value / 1000);
    }

    public override void startSkill()
    {
        GameManager.getIntance().mCardLunhuiGet += (float)(value / 1000);
    }
}