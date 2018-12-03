using UnityEngine;
public class PackbackSkill500003 : PackbackSkillBase
{
    public override void removeSkill()
    {
        GameManager.getIntance().mCardLunhuiGet -= ((float)value / 10000);
    }

    public override void startSkill()
    {
        GameManager.getIntance().mCardLunhuiGet += ((float)value / 10000);
    }
}