using UnityEngine;

public class AdManager : MonoBehaviour
{


    // Use this for initialization
    private float timeScale = 0;
    private AdInterface mInterface = null;

    public void initAd() {
        if (GameManager.isTestVersion) {
            return;
        }
        if (mInterface == null) {
            mInterface = AdFactory.creatInterface();
        }
        mInterface.initAd();
    }

    public void playAd() {
        mInterface.playAd();
    }

    public bool isReadyToShow() {
        if (GameManager.isTestVersion)
        {
            return false;
        }
        return mInterface.isReadyToShow();
    }
}
