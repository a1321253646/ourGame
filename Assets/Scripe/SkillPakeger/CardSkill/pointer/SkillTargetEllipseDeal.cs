﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillTargetEllipseDeal
{
    public static List<Attacker> getTargetList(LocalBean lives, SkillLocalBean local, int campType , bool isRed)
    {
        List<Attacker> result = new List<Attacker>();
        float a = local.leng / 2;
        float b = local.wight / 2;
        a = a * a;
        b = b * b;
        int count = 1;
        LocalBean tmp=  lives;
        while (tmp != null) {
            count++;
            if (tmp.mAttacker.mCampType == campType)
            {

                float x = tmp.mCurrentX+tmp.mAttacker.resourceData.getHurtOffset().x - local.x;
                float y = tmp.mCurrentY+tmp.mAttacker.resourceData.idel_y - local.y;
                if (x * x / a + y * y / b <= 1)
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
