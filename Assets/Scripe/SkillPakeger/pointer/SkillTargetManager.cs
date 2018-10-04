using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillTargetManager
{
    public static int TYPE_SHAPE_ELLIPSE = 1;
    public static int TYPE_SHAPE_ROUND = 2;
    public static int TYPE_SHAPE_SQUARENESS = 3;
    public static int TYPE_SHAPE_POINT =4;
    public static List<Attacker> getTargetList( LocalBean  lives, SkillLocalBean local, int campType,bool isRead) {
        if (local.type == TYPE_SHAPE_ELLIPSE)
        {
            return SkillTargetEllipseDeal.getTargetList(lives, local, campType, isRead);
        }
        else if (local.type == TYPE_SHAPE_ROUND)
        {
            return SkillTargetRoundDeal.getTargetList(lives, local, campType, isRead);
        }
        else if (local.type == TYPE_SHAPE_SQUARENESS)
        {
            return SkillTargetSquarenessDeal.getTargetList(lives, local, campType, isRead);
        }
        else if (local.type == TYPE_SHAPE_POINT)
        {
            return SkillTargetPointDeal.getTargetList(lives, local, campType, isRead);
        }
        return null;
    }
}
