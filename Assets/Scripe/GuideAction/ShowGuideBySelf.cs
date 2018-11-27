using UnityEngine;
using System.Collections;

public class ShowGuideBySelf : GuideActionButton
{

    public void init(long value, int buttonId, string obStr)
    {
        mValueType = GuideManager.EVENT_SHOW;
        mValue = value;
        mId = buttonId;
        mStr = obStr;
    }
}
