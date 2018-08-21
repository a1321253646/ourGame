using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillTargetSquarenessDeal
{
    public static List<Attacker> getTargetList(List<LocalBean> lives, SkillLocalBean local, int campType)
    {
        List<Attacker> result = new List<Attacker>();
        float maxX = local.x + local.leng / 2;
        float minX = local.x - local.leng / 2;
        float maxY = local.y + local.wight / 2;
        float minY = local.y - local.wight / 2;
        foreach (LocalBean bean in lives)
        {
            if (bean.mAttacker.mCampType == campType)
            {
                if (bean.mCurrentX >= minX &&
                    bean.mCurrentX <= maxX &&
                    bean.mCurrentY >= minY &&
                    bean.mCurrentY <= maxY) {
                    result.Add(bean.mAttacker);
                }
            }
        }
        if (result.Count == 0)
        {
            result = null;
        }
        return result;
    }
}
