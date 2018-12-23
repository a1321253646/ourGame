using UnityEngine;
public class NormalAttackSkill500003 : NormalAttackSkillBase
{
    public override void endSkill()
    {
        GameManager.getIntance().mCardLunhuiGet -= ((float)value / 10000);
    }

    public override void startSkill()
    {
        GameManager.getIntance().mCardLunhuiGet += ((float)value / 10000);
    }
}