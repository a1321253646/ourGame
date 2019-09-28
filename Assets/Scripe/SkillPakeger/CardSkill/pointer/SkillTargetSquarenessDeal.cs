using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillTargetSquarenessDeal
{
    public static List<Attacker> getTargetList(FightManager lives, SkillLocalBean local, int campType,bool isRed)
    {
        List<Attacker> result = new List<Attacker>();
        float maxX = local.x + local.leng / 2;
        float minX = local.x  - local.leng / 2;
        float maxY = local.y + local.wight / 2;
        float minY = local.y  - local.wight / 2;
        FightManager fit = lives;
        foreach (Attacker at in fit.mAliveActtackers.Values)
        {
            LocalBean tmp = at.mLocalBean;
            if (tmp.mAttacker.mCampType == campType)
            {
                if (isOverlap(maxX, minX, maxY, minY, tmp))
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

        }

        if (result.Count == 0)
        {
            result = null;
        }
        return result;
    }

    static bool isOverlap(float maxX, float minX, float maxY, float minY, LocalBean tmp)
    {
      //  Debug.Log("maxX = " + maxX+ " maxX2 = " + (tmp.mCurrentX + tmp.mAttacker.resourceData.getHurtOffset().x - tmp.mAttacker.resourceData.getTargetBorder()[0]));
       // Debug.Log("minX = " + minX + " minX2 = " + (tmp.mCurrentX + tmp.mAttacker.resourceData.getHurtOffset().x + tmp.mAttacker.resourceData.getTargetBorder()[1]));
        //Debug.Log("maxY = " + maxY + " maxY2 = " + (tmp.mCurrentY + tmp.mAttacker.resourceData.idel_y + tmp.mAttacker.resourceData.getTargetBorder()[2]));
        //Debug.Log("minY = " + minY + " minY2 = " + (tmp.mCurrentY + tmp.mAttacker.resourceData.idel_y));
        //Debug.Log("=============================================");

        if (maxX < tmp.mCurrentX + tmp.mAttacker.resourceData.getHurtOffset().x - tmp.mAttacker.resourceData.getTargetBorder()[0] ||
            minX > tmp.mCurrentX + tmp.mAttacker.resourceData.getHurtOffset().x + tmp.mAttacker.resourceData.getTargetBorder()[1] ||
            maxY < tmp.mCurrentY + tmp.mAttacker.resourceData.idel_y ||
            minY > tmp.mCurrentY + tmp.mAttacker.resourceData.idel_y + tmp.mAttacker.resourceData.getTargetBorder()[2])
        {
            return false;
        }
        else {
            return true;
        }
    }

}
