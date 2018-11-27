using UnityEngine;
using System.Collections;

public class GuideActionCardUiShowInfo : GuideActionItem
{
    public override long notificationDeal(int eventID, long eventValue)
    {
        if (eventID == GuideManager.EVENT_OBJECT_CLICK && eventValue == mTarget.value)
        {
            start();
            return 0;
        }
        else if (eventID == GuideManager.EVENT_SHOW && eventValue == GuideManager.SHOW_CARD_BACK_INFO)
        {
            Time.timeScale = 1;
            mManager.disableGuide();
            return -3;
        }

        return -1;
    }

    public override void start()
    {
        Time.timeScale = 0;
    }
}
