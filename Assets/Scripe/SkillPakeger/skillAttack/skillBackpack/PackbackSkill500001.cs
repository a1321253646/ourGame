using UnityEngine;
public class PackbackSkill500001 : PackbackSkillBase
{
    public override void removeSkill()
    {
        GameManager.getIntance().mCardOnlineGet -= (float)(value / 1000);
    }

    public override void startSkill()
    {
        GameManager.getIntance().mCardOnlineGet += (float)(value / 1000);
    }
}