using UnityEngine;
using System.Collections;

public class HurtStatus
{
    public static long TYPE_DEFAULT = 1;
    public static long TYPE_CRT = 2;
    public static long TYPE_RATE = 3;
    public static long TYPE_FANGSHANG = 4;

    public double blood;
    public long type;
    public HurtStatus(double blood, long type)
    {
        this.blood = blood;
        this.type = type;
    }
}
