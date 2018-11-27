using UnityEngine;
using System.Collections;

public class GuideActionEquiteUp : GuideActionItem
{
    public override long notificationDeal(int eventID, long eventValue)
    {
        if (eventID == GuideManager.EVENT_ENEMY_DEAL && eventValue == mList[0].value)
        {
            start();
            return 0;
        }
        else if (eventID == GuideManager.EVENT_EQUITE_UP_CLICK && eventValue == mTarget.value)
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
    }
}
