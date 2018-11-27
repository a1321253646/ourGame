using UnityEngine;
using System.Collections;

public class GuideActionButton : GuideActionItem
{

    public override long notificationDeal(int eventID, long eventValue)
    {
        if (eventID == mValueType && eventValue == mValue) {
                start();
                return 0;
        } else if (eventID == GuideManager.EVENT_CLICK_BUTTON && eventValue == mId) {
            Time.timeScale = 1;
            mManager.disableGuide();
            return -2;
        }

        return -1;
    }

    public override void start()
    {
        Time.timeScale = 0;
        GameObject ob = GameObject.Find(mStr);
        mManager.ShowGuideNormalObject(ob);
        mManager.showGuideDec(mTargetX, mTargetY, mDecX, mDecY, mDec);
    }

    public string mStr;
    public int mId;
    public int mValueType;
    public long mValue;

    public void init(long value,int valueType ,int buttonId,string obStr) {
        mValue = value;
        mId = buttonId;
        mStr = obStr;
        mValueType = valueType;
    }
}
