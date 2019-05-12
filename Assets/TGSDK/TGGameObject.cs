using UnityEngine;
using System.Collections;

namespace Together
{
    public class TGGameObject : MonoBehaviour
    {
        private static TGGameObject instance = null;
        public static TGGameObject Instance()
        {
            if (instance == null)
            {
                GameObject go = new GameObject("TGSDK");
                go.hideFlags = HideFlags.HideAndDontSave;
                DontDestroyOnLoad(go);

                instance = go.AddComponent<TGGameObject>();
            }
            return instance;
        }

        void initSDK(string msg)
        {
            if (TGSDK.SDKInitFinishedCallback != null)
                TGSDK.SDKInitFinishedCallback(msg);
        }

        void userPartnerBindSuccess(string msg)
        {
            if (TGSDK.PartnerBindSuccessCallback != null)
                TGSDK.PartnerBindSuccessCallback(msg);
        }

        void userPartnerBindFailed(string msg)
        {
            if (TGSDK.PartnerBindFailedCallback != null)
                TGSDK.PartnerBindFailedCallback(msg);
        }

        void onPreloadSuccess(string msg)
        {
            if (TGSDK.PreloadAdSuccessCallback != null)
                TGSDK.PreloadAdSuccessCallback(msg);
        }

        void onPreloadFailed(string msg)
        {
            if (TGSDK.PreloadAdFailedCallback != null)
                TGSDK.PreloadAdFailedCallback(msg);
        }

        void onCPADLoaded(string msg)
        {
            if (TGSDK.CPAdLoadedCallback != null)
                TGSDK.CPAdLoadedCallback(msg);
        }

        void onVideoADLoaded(string msg)
        {
            if (TGSDK.VideoAdLoadedCallback != null)
                TGSDK.VideoAdLoadedCallback(msg);
        }

        void onADShowSuccess(string msg)
        {
            if (msg.Contains("|"))
            {
                string[] args = msg.Split('|');
                if (TGSDK.AdShowSuccessCallback != null)
                    TGSDK.AdShowSuccessCallback(args[0], args[1]);
            }
        }

        void onADShowFailed(string msg)
        {
			if (msg.Contains("|"))
            {
                string[] args = msg.Split('|');
                if (TGSDK.AdShowFailedCallback != null)
                    TGSDK.AdShowFailedCallback(args[0], args[1], args[2]);
            }
        }

        void onADComplete(string msg)
        {
            if (TGSDK.AdCompleteCallback != null)
                TGSDK.AdCompleteCallback(msg);
        }

        void onADClose(string msg)
        {
			if (msg.Contains("|"))
            {
                string[] args = msg.Split('|');
				bool couldReward = false;
				if ("yes".Equals(args[2])){
					couldReward = true;
				}
                if (TGSDK.AdCloseCallback != null)
                    TGSDK.AdCloseCallback(args[0], args[1], couldReward);
            }
        }

        void onADClick(string msg)
        {
			if (msg.Contains("|"))
            {
                string[] args = msg.Split('|');
                if (TGSDK.AdClickCallback != null)
                    TGSDK.AdClickCallback(args[0], args[1]);
            }
        }

        void onADRewardSuccess(string msg)
        {
            if (TGSDK.AdRewardSuccessCallback != null)
                TGSDK.AdRewardSuccessCallback(msg);
        }

        void onADRewardFailed(string msg)
        {
            if (TGSDK.AdRewardFailedCallback != null)
                TGSDK.AdRewardFailedCallback(msg);
        }

        void onBannerLoaded(string msg)
        {
            if (TGSDK.BannerLoadedCallback != null)
            {
                string[] args = msg.Split('|');
                TGSDK.BannerLoadedCallback(args[0], args[1]);
            }
        }

        void onBannerFailed(string msg)
        {
            if (TGSDK.BannerFailedCallback != null)
            {
                string[] args = msg.Split('|');
                TGSDK.BannerFailedCallback(args[0], args[1], args[2]);
            }
        }

        void onBannerClick(string msg)
        {
            if (TGSDK.BannerClickCallback != null)
            {
                string[] args = msg.Split('|');
                TGSDK.BannerClickCallback(args[0], args[1]);
            }
        }

        void onBannerClose(string msg)
        {
            if (TGSDK.BannerCloseCallback != null)
            {
                string[] args = msg.Split('|');
                TGSDK.BannerCloseCallback(args[0], args[1]);
            }
        }

        void onAwardVideoLoaded(string msg)
        {
            if (TGSDK.AwardVideoLoadedCallback != null)
            {
                TGSDK.AwardVideoLoadedCallback(msg);
            }
        }

        void onInterstitialVideoLoaded(string msg)
        {
            if (TGSDK.InterstitialVideoLoadedCallback != null)
            {
                TGSDK.InterstitialVideoLoadedCallback(msg);
            }
        }

        void onInterstitialLoaded(string msg)
        {
            if (TGSDK.InterstitialLoadedCallback != null)
            {
                TGSDK.InterstitialLoadedCallback(msg);
            }
        }
    }
}