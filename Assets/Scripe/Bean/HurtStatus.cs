using UnityEngine;
using System.Collections;

public class HurtStatus
{
    public double blood;
    public bool isCrt;
    public bool isRate;
    public HurtStatus(double blood, bool isCrt, bool isRate)
    {
        this.blood = blood;
        this.isCrt = isCrt;
        this.isRate = isRate;
    }
}
