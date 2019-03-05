using UnityEngine;
public class NormalAttackSkill500003 : NormalAttackSkillBase
{
    public override void endSkill()
    {
        GameManager.getIntance().mCardOnlineGet.deletById(500003);
    }

    public override void startSkill()
    {
        GameManager.getIntance().mCardLunhuiGet.AddFloat(500003, 1 + ((float)value / 1000));
    }
}