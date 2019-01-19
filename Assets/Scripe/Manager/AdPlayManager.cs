using UnityEngine;
using System.Collections;

public class AdPlayManager : MonoBehaviour
{
    public string iosGameID = "3008523";
    public string androidGameID = "3008522";

    public bool enableTestMode = true;


    private void Start()
    {
/*        string gameID = null;

#if UNITY_IOS
		gameID = iosGameID;
#elif UNITY_ANDROID
        gameID = androidGameID;
#endif
        Debug.Log("=============AdPlayManager gameID=" + gameID);
        if (!Advertisement.isInitialized &&gameID != null) {

            Advertisement.Initialize(gameID, false);
            StartCoroutine(LogWhenUnityAdsIsInitialized());
        }
      */  
    }/*
    private IEnumerator LogWhenUnityAdsIsInitialized()
    {
        float initStartTime = Time.time;

        do yield return new WaitForSeconds(0.1f);
        while (!Advertisement.isInitialized);

        Debug.Log(string.Format("Unity Ads was initialized in {0:F1} seconds.", Time.time - initStartTime));
        yield break;
    }
    public void ShowRewardedAd()
    {
        Debug.Log(" UnityAds ShowRewardedAd");
        string zoneID = "rewardedVideo";
        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;
        Advertisement.Show(zoneID, options);
        if (Advertisement.IsReady("rewardedVideo") || Advertisement.IsReady("rewardedVideo") )
        {
            Debug.Log("UnityAds Showing ad now...");
            if (Advertisement.IsReady("rewardedVideo")) {
                Debug.Log("UnityAds Showing ad now...");
            }


        //    Advertisement.Show(zoneID, options);
        }
        else
        {
            Debug.LogWarning(string.Format("UnityAds Unable to show ad. The ad placement zone {0} is not ready.",
                                            object.ReferenceEquals(zoneID, null) ? "default" : zoneID));
         
        }
        //Advertisement.Show("rewardedVideo", options);
        Debug.Log("UnityAds===========================ShowRewardedAd");
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("UnityAds========================The ad was successfully shown.");
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                break;
            case ShowResult.Skipped:
                Debug.Log("UnityAds=====================The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("UnityAds========================The ad failed to be shown.");
                break;
        }
    }
    */
    /*public void playAdStart() {
        Debug.Log("playAdStart start");
        AndroidJavaClass AJC = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject AJO = AJC.GetStatic<AndroidJavaObject>("currentActivity");
        AJO.Call("playAd","");
        Debug.Log("playAdStart end");
    }

    public void videoDidStartLoad()//视频开始加载
    {
       
    }
    public void videoDidFinishLoad(string var1)//视频是否加载完成 1 为是2为否
    {

    }
    public void videoDidLoadError(string var1)//视频加载失败
    {

    }
    public void videoDidClosed()//视频关闭
    {

    }
    public void videoCompletePlay()//视频播放完成
    {

    }
    public void videoPlayError(string var1)//视频播放出错
    {

    }
    public void videoWillPresent()//视频开始播放
    {

    }
    public void videoVailable(long value)//检查视频是否可用
    {

    }*/
}
