using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillTargetRoundDeal
{
    public static List<Attacker> getTargetList(List<LocalBean> lives, SkillLocalBean local, int campType)
    {
        List<Attacker> result = new List<Attacker>();
        float r2 = local.leng / 2;
        r2 = r2 * r2;
        foreach (LocalBean bean in lives)
        {
            if (bean.mAttacker.mCampType == campType)
            {
                float x = bean.mCurrentX - local.x;
                float y = bean.mCurrentY - local.y;
                if (x*x+ y*y <= r2) {
                    result.Add(bean.mAttacker);
                }
            }
        }
        if (result.Count == 0) {
            result = null;
        }
        return result;
    }
}
