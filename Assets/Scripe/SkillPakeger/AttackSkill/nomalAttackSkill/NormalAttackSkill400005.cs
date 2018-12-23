using UnityEngine;
public class NormalAttackSkill400005 : NormalAttackSkillBase
{
    public override void endSkill()
    {
        Attacker a = mManager.getAttacker();
        if (a is PlayControl)
        {
            PlayControl play = (PlayControl)a;
            play.mSkillAttribute.crt -= value;
        }
    }

    public override void startSkill()
    {
        Attacker a = mManager.getAttacker();
        if (a is PlayControl) {
            PlayControl play = (PlayControl)a;
            play.mSkillAttribute.crt += value ;
        }
    }
}