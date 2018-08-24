using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillTargetSquarenessDeal
{
    public static List<Attacker> getTargetList(LocalBean lives, SkillLocalBean local, int campType,bool isRed,ResourceBean resource)
    {
        float xOffet = 0;
        float yOffet = 0;
        if (resource != null)
        {
            xOffet = resource.getHurtOffset().x;
            yOffet = resource.getHurtOffset().y;
        }
        List<Attacker> result = new List<Attacker>();
        float maxX = local.x-xOffet + local.leng / 2;
        float minX = local.x - xOffet - local.leng / 2;
        float maxY = local.y-yOffet + local.wight / 2;
        float minY = local.y - yOffet - local.wight / 2;
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
