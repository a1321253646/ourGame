using UnityEngine;
public class NormalAttackSkill500002 : NormalAttackSkillBase
{
    public override void endSkill()
    {
        GameManager.getIntance().mCardOutlineGet -= ((float)value / 10000);
    }

    public override void startSkill()
    {
        GameManager.getIntance().mCardOutlineGet += ((float)value / 10000);
    }
}