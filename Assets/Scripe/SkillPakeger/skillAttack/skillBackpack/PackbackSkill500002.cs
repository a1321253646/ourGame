using UnityEngine;
public class PackbackSkill500002 : PackbackSkillBase
{
    public override void removeSkill()
    {
        GameManager.getIntance().mCardOutlineGet -= ((float)value / 10000);
    }

    public override void startSkill()
    {
        GameManager.getIntance().mCardOutlineGet += ((float)value / 10000);
    }
}