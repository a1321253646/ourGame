using UnityEngine;
using System.Collections;
using Together;
using System.Collections.Generic;

public class AdManager : MonoBehaviour
{

    // Use this for initialization
    private float timeScale = 0;
  //  private bool isFristLoading = true;
    
    void Start()
    {
    //    TGSDK.SetDebugModel(true);
        TGSDK.Initialize("235M0GyJMOoX7O804h63");
        TGSDK.PreloadAd();
        // 使用 lambda 表达式
        // 广告配置数据获取成功
        TGSDK.PreloadAdSuccessCallback = (string ret) => {
            Debug.Log("============ 广告配置数据获取成功  ====================");
           /* if (isFristLoading) {
                isFristLoading = false;
                List<ActiveButtonBean> list =  SQLHelper.getIntance().getActiveList();
                if (list != null && list.Count > 0) {
                    foreach (ActiveButtonBean bean in list) {
                        if (bean.buttonType == ActiveButtonControl.ACTIVE_BUTTON_TYPE_AD) {
                            GameObject.Find("active_button_list").GetComponent<ActiveListControl>().showAd(bean.adType, bean.count, true);
                        }
                    }
                }
            }*/
        };
        // 广告配置数据获取失败
        TGSDK.PreloadAdFailedCallback = (string error) => {
            Debug.Log("============ 广告配置数据获取失败  ====================");

        };
        // 奖励视频广告已经准备好
        TGSDK.AwardVideoLoadedCallback = (string ret) => {
            Debug.Log("============ 奖励视频广告已经准备好  ====================");

        };
        // 插屏视频广告已经准备好
        TGSDK.InterstitialVideoLoadedCallback = (string ret) => {
            Debug.Log("============ 插屏视频广告已经准备好  ====================");

        };
        // 静态插屏广告已经准备好
        TGSDK.InterstitialLoadedCallback = (string ret) => {
            Debug.Log("============ 静态插屏广告已经准备好  ====================");

        };
        // 使用 lambda 表达式
        // 广告成功开始播放回调
        TGSDK.AdShowSuccessCallback = (string scene, string name) => {
            Debug.Log("============ 广告成功开始播放回调  ====================");
            timeScale = Time.timeScale;
            Time.timeScale = 0;
        };
        // 广告播放失败回调
        TGSDK.AdShowFailedCallback = (string scene, string name, string error) => {
            Debug.Log("============ 广告播放失败回调  ====================");
            if (timeScale == 0) {
                timeScale = 1;
            }
            Time.timeScale = timeScale;
            GameObject.Find("active_button_list").GetComponent<ActiveListControl>().removeAd();
        };
        // 广告关闭回调，是否奖励则在第三个参数award中
        TGSDK.AdCloseCallback = (string scene, string name, bool award) => {
            Debug.Log("============ 广告关闭回调，是否奖励则在第三个参数award中  ===================="+ award);
            if (timeScale == 0)
            {
                timeScale = 1;
            }
            Time.timeScale = timeScale;
            GameObject.Find("advert").GetComponent<AdUiControl>().playIsFinish(award);
        };
        // 广告被用户点击的回调
        TGSDK.AdClickCallback = (string scene, string name) => {
            Debug.Log("============ 广告被用户点击的回调  ====================");
        };


    }

    public void playAd() {
        if (TGSDK.CouldShowAd("JQOki1YxoGUzpWGJjCf"))
        {
             TGSDK.ShowAd("JQOki1YxoGUzpWGJjCf");
            // TGSDK.ShowTestView("JQOki1YxoGUzpWGJjCf");
        }
    }

    public bool isReadyToShow() {
        return TGSDK.CouldShowAd("JQOki1YxoGUzpWGJjCf");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
