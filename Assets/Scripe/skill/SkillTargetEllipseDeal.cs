using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillTargetEllipseDeal
{
    public static List<Attacker> getTargetList(List<LocalBean> lives, SkillLocalBean local, int campType)
    {
        List<Attacker> result = new List<Attacker>();
        float a = local.leng / 2;
        float b = local.wight / 2;
        a = a * a;
        b = b * b;
        foreach (LocalBean bean in lives)
        {
            if (bean.mAttacker.mCampType == campType)
            {

                float x = bean.mCurrentX - local.x;
                float y = bean.mCurrentY - local.y;
                if (x*x / a +y*y / b <= 1) {
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
