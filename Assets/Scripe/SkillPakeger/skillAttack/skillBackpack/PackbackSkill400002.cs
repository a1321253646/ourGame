using UnityEngine;
public class PackbackSkill400002 : PackbackSkillBase
{
    public override void removeSkill()
    {
        Attacker a = mManager.getAttacker();
        if (a is PlayControl)
        {
            PlayControl play = (PlayControl)a;
            play.mSkillAttributePre.aggressivity -= value;
        }
    }

    public override void startSkill()
    {
        Attacker a = mManager.getAttacker();
        if (a is PlayControl) {
            PlayControl play = (PlayControl)a;
            play.mSkillAttributePre.aggressivity += value;
        }
    }
}