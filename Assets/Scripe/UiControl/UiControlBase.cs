using UnityEngine;
using System.Collections;

public abstract class UiControlBase : MonoBehaviour
{
    public bool isInit = false;
    public Vector2 mFri;
    public abstract void init();
    public abstract void show();

    public long mControlType = -1;

    public virtual void toShowUi() {
        UiControlManager.getIntance().show(mControlType);
    }
    public virtual void toremoveUi()
    {
        UiControlManager.getIntance().remove(mControlType);
    }

    public void remove() {
        transform.position = mFri;
    }

    public void showUiControl() {
        if (!isInit) {
            mFri = gameObject.transform.localPosition;
            init();
            isInit = true;
        }
        show();
    }
}
