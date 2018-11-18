using UnityEngine;
using System.Collections;
using System;

public class TimeUtils
{

    public static long GetTimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalMilliseconds);
    }

    public static long getTimeDistanceMin(long oldTime,long newTime) {
        long distance = newTime - oldTime;
        return distance / 60000;
    }
    public static long getTimeDistanceMin(long oldTime)
    {
        long distance = GetTimeStamp() - oldTime;
        long back = distance / 60000;
        if (back * 60000 < distance) {
            back++;
        }
        return back;

    }
}
