using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillTargetRoundDeal
{
    public static List<Attacker> getTargetList(LocalBean lives, SkillLocalBean local, int campType,bool isRed, ResourceBean resource)
    {
        float xOffet = 0;
        float yOffet = 0;
        if (resource != null)
        {
            xOffet = resource.getHurtOffset().x;
            yOffet = resource.getHurtOffset().y;
        }
        List<Attacker> result = new List<Attacker>();
        float r2 = local.leng / 2;
        r2 = r2 * r2;
        LocalBean tmp = lives;
        while (tmp != null)
        {
            if (tmp.mAttacker.mCampType == campType)
            {
                float x = tmp.mCurrentX - (local.x-xOffet);
                float y = tmp.mCurrentY - (local.y-yOffet);
                if (x * x + y * y <= r2)
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
        if (result.Count == 0) {
            result = null;
        }
        return result;
    }
}
