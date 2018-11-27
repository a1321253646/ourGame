using UnityEngine;
using System.Collections;

public class GuideActionBackpackItem : GuideActionItem
{
    public override long notificationDeal(int eventID, long eventValue)
    {
        if (eventID == GuideManager.EVENT_SHOW && eventValue == GuideManager.SHOW_BACK)
        {
            start();
            return 0;
        }
        else if (eventID == GuideManager.EVENT_OBJECT_CLICK && eventValue == mTarget.value)
        {
            Time.timeScale = 1;
            mManager.disableGuide();
            return -2;
        }

        return -1;
    }


    public override void start()
    {
        Time.timeScale = 0;
        GameObject.Find("Backpack").GetComponent<IvertoryControl>().guide(mTarget.value);
        mManager.showGuideDec(mTargetX, mTargetY, mDecX, mDecY, mDec);
    }
}
