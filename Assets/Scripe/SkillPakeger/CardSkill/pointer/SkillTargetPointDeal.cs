using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillTargetPointDeal
{
    public static List<Attacker> getTargetList(FightManager lives, SkillLocalBean local, int campType,bool isRed)
    {
        List<Attacker> result = new List<Attacker>();
        float maxX = local.x ;
        float minX = local.x ;
        float maxY = local.y ;
        float minY = local.y;
        float maxx = 0;
        float minx = 0;
        float maxy = 0;
        float miny = 0;
        FightManager fit = lives;
        float distance = 9999999999;
        float distance2 = 0;

        foreach (Attacker at in fit.mAliveActtackers.Values)
        {
            LocalBean tmp = at.mLocalBean;
            if (at.mCampType == campType)
            {
               
                maxx = tmp.mCurrentX + tmp.mAttacker.resourceData.getHurtOffset().x + tmp.mAttacker.resourceData.getTargetBorder()[1];
                minx = tmp.mCurrentX + tmp.mAttacker.resourceData.getHurtOffset().x - tmp.mAttacker.resourceData.getTargetBorder()[0];
                miny = tmp.mCurrentY + tmp.mAttacker.resourceData.idel_y;
                maxy = miny + tmp.mAttacker.resourceData.getTargetBorder()[2];
                //                Debug.Log("maxX=" + maxX+ " minX="+ minX+ " maxY=" + maxY + " minY=" + minY +
                //                    " maxx=" + maxx + " minx=" + minx + " miny=" + miny + " maxy=" + maxy);
                if ((maxX >= minx && maxX <= maxx && maxY >= miny && maxY <= maxy) ||
                    (maxX >= minx && maxX <= maxx && minY >= miny && minY <= maxy) ||
                    (minX >= minx && minX <= maxx && maxY >= miny && maxY <= maxy) ||
                    (minX >= minx && minX <= maxx && minY >= miny && minY <= maxy))
                {
                    float x = tmp.mCurrentX + tmp.mAttacker.resourceData.getHurtOffset().x - local.x;
                    float y = tmp.mCurrentY + tmp.mAttacker.resourceData.getHurtOffset().y - local.y;

                    distance2 = x * x + y * y;
                    //                   Debug.Log("distance2 = " + distance2 + " distance" + distance);
                    if (distance2 < distance)
                    {

                        distance = distance2;

                        if (result.Count == 1)
                        {
                            result[0].setWhith();
                        }
                        result.Clear();
                        result.Add(tmp.mAttacker);
                    }
                    else
                    {
                        continue;
                    }
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
}
