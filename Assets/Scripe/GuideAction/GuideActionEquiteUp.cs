using UnityEngine;
using System.Collections;

public class GuideActionEquiteUp : GuideActionItem
{
    public override long notificationDeal(int eventID, long eventValue)
    {
        if (eventID == GuideManager.EVENT_ENEMY_DEAL && eventValue == mList[0].value)
        {
            isStart = true;
            start();
            return 0;
        }
        else if (isStart && eventID == GuideManager.EVENT_EQUITE_UP_CLICK && eventValue == mTarget.value)
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
        GameObject.Find("jineng").GetComponent<ZhuangbeiUpdateControl>().gui(mTarget.value);
        mManager.showGuideDec(mTargetX, mTargetY, mDecX, mDecY, mDec);
    }
}
