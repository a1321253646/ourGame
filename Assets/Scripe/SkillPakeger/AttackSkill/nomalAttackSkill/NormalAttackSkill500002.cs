using UnityEngine;
public class NormalAttackSkill500002 : NormalAttackSkillBase
{
    public override void endSkill()
    {
        GameManager.getIntance().mCardOnlineGet.deletById(500002);
    }

    public override void startSkill()
    {
        GameManager.getIntance().mCardOutlineGet.AddFloat(500002, 1 + ((float)value / 1000));

    }
}