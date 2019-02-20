using UnityEngine;
using System.Collections;

public class AdIntance 
{
    private static  AdIntance mIntance = new AdIntance();

    private float mTime = 0;

    private long mType = -1;
    public static AdIntance getIntance() {
        return mIntance;
    }

    public void setType(long type) {
        mType = type;
    }

    public long getType() {
        return mType;
    }

    public void setTime(float time) {
        mTime = time;
    }
    public float getTime() {
        return mTime;
    }

    public static bool isInit = false;
}
