using UnityEngine;
using System.Collections;

public class AdIntance
{
    private static AdIntance mIntance = new AdIntance();

    private float mTime = 0;

    private long mType = -1;
    public static AdIntance getIntance()
    {
        return mIntance;
    }

    public void setType(long type)
    {
        mType = type;
    }

    public long getType()
    {
        return mType;
    }

    public void setTime(float time)
    {
        mTime = time;
    }
    public float getTime()
    {
        return mTime;
    }


    public string getBannerPoint() {
        /*    Vector3[] corners = new Vector3[4];
            GameObject.Find("AdBanner").GetComponent<RectTransform>().GetWorldCorners(corners);

            float a = corners[3].x - corners[0].x;
            float b = corners[1].y;
            string value = a+","+b;
            Debug.Log("corners[0].y=" + corners[0].y + " corners[1].y=" + corners[1].y + " corners[0].x=" + corners[0].x + " corners[4].x" + corners[3].x);

            Debug.Log("getBannerPoint value=" + value);
            return value;*/
        return "0,0";
    }

    public static bool isInit = false;
}
