using UnityEngine;
public class NormalAttackSkill500001 : NormalAttackSkillBase
{
    public override void endSkill()
    {
        GameManager.getIntance().mCardOnlineGet.deletById(500001);
    }

    public override void startSkill()
    {
        GameManager.getIntance().mCardOnlineGet.AddFloat(500001,1 + ((float)value / 1000));
    }
}