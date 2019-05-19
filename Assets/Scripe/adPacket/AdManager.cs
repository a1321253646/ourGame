using System;
using UnityEngine;

public class AdManager : MonoBehaviour
{


    private bool isAdInitReady = false;
    private bool isBannerReady = false;
    private bool isFlashAdReady = false;

    // Use this for initialization
    private float timeScale = 0;

    public void initAd()
    {

    }
    public bool isAdInit()
    {
        if (isAdInitReady)
        {
            return true;
        }
        isAdInitReady = callJacaMethodReturnBoolean("isAdInit");
        return false;
    }

    public bool isReadyShowBanner()
    {
        if (isBannerReady)
        {
            return true;
        }

        if (!isAdInit())
        {
            return false;
        }
        return false;
    }

    public void showBanner()
    {
        if (!isReadyShowBanner())
        {
            return;
        }
    }
    public void stopBanner()
    {

    }
    public bool isReadyFlashAd()
    {
        if (isFlashAdReady)
        {
            return true;
        }

        if (!isAdInit())
        {
            return false;
        }
        return false;
    }

    public void showFlashAd()
    {
        if (!isReadyFlashAd())
        {
            return;
        }
    }
    public void stopFlashAd()
    {

    }
    public void cancelShowFLashAd()
    {

    }
    public void finishShowFlashAd()
    {

    }

    public bool isReadyShowInersAd()
    {
        if (!isAdInit())
        {
            return false;
        }
        return callJacaMethodReturnBoolean("isInserAdReady");
    }

    public void showInersAd()
    {
        if (!isReadyShowInersAd())
        {
            return;
        }
        callJacaMethodReturnBoolean("playInerAd");
    }
    public void cancelShowInersAd()
    {

    }
    public void finishShowInersAd()
    {

    }

    public bool isReadyToShow()
    {
        return isReadyShowInersAd();
    }

    internal void playAd()
    {
        showInersAd();
    }

    private bool callJacaMethodReturnBoolean(string neme)
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        string[] mObject = new string[1];
        return jo.Call<bool>(neme, mObject);

    }
    private string callJacaMethodReturnString(string neme)
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        string[] mObject = new string[1];
        return jo.Call<string>(neme, mObject);

    }
}
