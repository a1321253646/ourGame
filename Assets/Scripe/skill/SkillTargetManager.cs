using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillTargetManager
{
    public static int TYPE_SHAPE_ELLIPSE = 1;
    public static int TYPE_SHAPE_ROUND = 2;
    public static int TYPE_SHAPE_SQUARENESS = 3;
    public List<Attacker> getTargetList(List<LocalBean> lives, SkillLocalBean local, int campType) {
        if (local.type == TYPE_SHAPE_ELLIPSE)
        {
            return SkillTargetEllipseDeal.getTargetList(lives, local, campType);
        }
        else if (local.type == TYPE_SHAPE_ROUND)
        {
            return SkillTargetRoundDeal.getTargetList(lives, local, campType);
        }
        else if (local.type == TYPE_SHAPE_SQUARENESS)
        {
            return SkillTargetSquarenessDeal.getTargetList(lives, local, campType);
        }
        return null;
    }
}
