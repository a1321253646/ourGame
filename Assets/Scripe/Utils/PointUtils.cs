using UnityEngine;
using System.Collections;

public class PointUtils 
{
    public static Vector3 screenTransToWorld(Vector3 local) {
        return Camera.main.ScreenToWorldPoint(local);
    }
    public static Vector3 worldTransToScreen(Vector3 local)
    {
        return Camera.main.WorldToScreenPoint(local) - Camera.main.WorldToScreenPoint(new Vector3(0,0,0));
    }
    public static float screenTransToWorldForWitch(float witch)
    {
        Vector3 v = Camera.main.ScreenToWorldPoint(new Vector3(witch, 0, 0));
        return v.x;
    }
    public static float worldTransToScreenForHeight(float height)
    {
       
        Vector3 v =  Camera.main.WorldToScreenPoint(new Vector3(0, height, 0));
        return v.y;
    }


}
