using UnityEngine;
using System.Collections;

public class PointUtils 
{
    public static Vector3 screenTransToWorld(Vector3 local) {
        return Camera.main.ScreenToWorldPoint(local);
    }
    public static Vector3 worldTransToScreen(Vector3 local)
    {
        return Camera.main.WorldToScreenPoint(local);
    }
    public static Vector3 getScreenSize(Vector3 local) {
        return Camera.main.WorldToScreenPoint(local) - Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0));
    }
}
