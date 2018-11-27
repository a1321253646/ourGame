using UnityEngine;
public class PackbackSkill400005 : PackbackSkillBase
{
    public override void removeSkill()
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