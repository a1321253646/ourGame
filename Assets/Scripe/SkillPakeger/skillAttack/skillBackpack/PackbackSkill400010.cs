using UnityEngine;
public class PackbackSkill400010 : PackbackSkillBase
{
    public override void removeSkill()
    {
        GameManager.getIntance().mCardOutlineGet -= (float)(value / 1000);
    }

    public override void startSkill()
    {
        GameManager.getIntance().mCardOutlineGet += (float)(value / 1000);
    }
}