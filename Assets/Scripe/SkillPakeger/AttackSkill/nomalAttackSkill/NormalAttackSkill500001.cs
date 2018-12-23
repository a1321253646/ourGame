using UnityEngine;
public class NormalAttackSkill500001 : NormalAttackSkillBase
{
    public override void endSkill()
    {
        GameManager.getIntance().mCardOnlineGet -= ((float)value / 1000);
    }

    public override void startSkill()
    {
        GameManager.getIntance().mCardOnlineGet += ((float)value / 1000);
    }
}