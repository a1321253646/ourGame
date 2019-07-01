using UnityEngine;

public class PayControl 
{
    public static string REFRESH_TYPE_LEVEL = "2";
    public static string REFRESH_TYPE_LOGIN = "1";
    public static string REFRESH_TYPE_LOGOUT = "3";
    private bool isLogin = false;

    public void buy(string userID, string userName, string productSku, string productName, string money) {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        string[] mObject = new string[5];
        mObject[0] = userID;
        mObject[1] = userName;
        mObject[2] = productSku;
        mObject[3] = productName;
        mObject[4] = money;
        jo.Call<string>("pay", mObject);
    }
    public void reFreshGameData(string userID, string userName,string type, string level)
    {
        if (REFRESH_TYPE_LOGIN.Equals(type)) {
            if (isLogin)
            {
                return;
            }
            else {
                isLogin = true;
            }
        }
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        string[] mObject = new string[4];
        mObject[0] = userID;
        mObject[1] = userName;
        mObject[2] = type;
        mObject[3] = level;
        jo.Call<string>("reFreshGameData", mObject);
    }

    public static PayControl getIntance() {
        return mIntance;    
    }
    private static PayControl mIntance = new PayControl();
    private PayControl() {

    }
}