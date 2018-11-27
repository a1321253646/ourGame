using UnityEngine;
using System.Collections;

public class GuideActionCardUiBackItem : GuideActionItem
{
    public override long notificationDeal(int eventID, long eventValue)
    {
        if (eventID == GuideManager.EVENT_SHOW && eventValue == GuideManager.SHOW_CARD)
        {
            start();
            return 0;
        }
        else if (eventID == GuideManager.EVENT_OBJECT_CLICK && eventValue == mTarget.value)
        {
            GameManager.getIntance().isGuide = false;
            mManager.disableGuide();
            return -3;
        }

        return -1;
    }


    public override void start()
    {
        GameManager.getIntance().isGuide = true;
        GameObject.Find("Card2").GetComponent<CardShowControl>().guideBack(mTarget.value);
    }
}
