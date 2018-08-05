using UnityEngine;
using System.Collections;

public class HurtStatus
{
    public float blood;
    public bool isCrt;
    public bool isRate;
    public HurtStatus(float blood, bool isCrt, bool isRate)
    {
        this.blood = blood;
        this.isCrt = isCrt;
        this.isRate = isRate;
    }
}
