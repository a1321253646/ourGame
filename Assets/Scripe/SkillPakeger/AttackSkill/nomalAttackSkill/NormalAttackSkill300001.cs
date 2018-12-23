using UnityEngine;
public class NormalAttackSkill300001 : NormalAttackSkillBase
{//没被应用到
    public override void endSkill()
    {
        GameManager.getIntance().mCardLunhuiGet -= ((float)value / 10000);
    }

    public override void startSkill()
    {
        
    }
}