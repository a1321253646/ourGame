using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillTargetSquarenessDeal
{
    public static List<Attacker> getTargetList(LocalBean lives, SkillLocalBean local, int campType,bool isRed)
    {
        List<Attacker> result = new List<Attacker>();
        float maxX = local.x + local.leng / 2;
        float minX = local.x  - local.leng / 2;
        float maxY = local.y + local.wight / 2;
        float minY = local.y  - local.wight / 2;
        LocalBean tmp = lives;
        while (tmp != null)
        {
            if (tmp.mAttacker.mCampType == campType)
            {
                if (tmp.mCurrentX >= minX &&
                    tmp.mCurrentX <= maxX &&
                    tmp.mCurrentY >= minY &&
                    tmp.mCurrentY <= maxY)
                {
                    result.Add(tmp.mAttacker);
                    if (isRed)
                    {
                        tmp.mAttacker.setRed();
                    }
                }
                else if (isRed)
                {
                    tmp.mAttacker.setWhith();
                }
            }
            else if (isRed)
            {
                tmp.mAttacker.setWhith();
            }
            tmp = tmp.next;
        }
        if (result.Count == 0)
        {
            result = null;
        }
        return result;
    }
}
