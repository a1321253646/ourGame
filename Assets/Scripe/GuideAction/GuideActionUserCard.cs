using UnityEngine;
using System.Collections;

public class GuideActionUserCard : GuideActionItem
{
    public override long notificationDeal(int eventID, long eventValue)
    {
        if (eventID == GuideManager.EVENT_ONDRAG_UP && eventValue == GuideManager.ONDRAG_UP_CARDUI_WORK)
        {
            Time.timeScale = 1;
            mManager.disableGuide();
            return -2;
        }
        else if (eventID == GuideManager.EVENT_ONDRAG_UP)
        {
            Time.timeScale = 1;
            mManager.disableGuide();
            return -1001;
        }
        else if (eventID == GuideManager.EVENT_SHOW && eventValue == GuideManager.SHOW_CARD_BACK_INFO) {
            start();
            return 0;
            
        }
        return -1;
    }

    public override void start()
    {
        Time.timeScale = 0;
        GameObject ob = GameObject.Find("user_card_list_root");
        mManager.ShowGuideNormalObject(ob);
    }
}
