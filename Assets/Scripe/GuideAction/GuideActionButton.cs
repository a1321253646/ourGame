using UnityEngine;
using System.Collections;

public class GuideActionButton : GuideActionItem
{

   
    public override long notificationDeal(int eventID, long eventValue)
    {
        if (eventID == mValueType && eventValue == mValue) {
            isStart = true;
            start();
            return 0;
        } else if (isStart && eventID == GuideManager.EVENT_CLICK_BUTTON && eventValue == mId) {
            Time.timeScale = 1;
            mManager.disableGuide();
            return -2;
        }

        return -1;
    }

    public override void start()
    {
        setUiShow();
        Time.timeScale = 0;
        GameObject ob = null;
        if (mStr.StartsWith("find:"))
        {
            mStr = mStr.Split(':')[1];
            if (mStr.Equals("vocation"))
            {
                ob = GameObject.Find("active_button_list").GetComponent<ActiveListControl>().getVocation();
            }
        }
        else {
            ob = GameObject.Find(mStr);
        }
        
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
