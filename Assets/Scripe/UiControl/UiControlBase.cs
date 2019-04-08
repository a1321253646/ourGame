using UnityEngine;
using System.Collections;

public abstract class UiControlBase : MonoBehaviour
{

    public bool isShow = false;

    public bool isInit = false;
    public Vector2 mFri;
    public abstract void init();
    public abstract void show();

    public long mControlType = -1;

    private void Start()
    {
        mFri = gameObject.transform.localPosition;
        init();
        isInit = true;
    }

    public virtual void toShowUi() {
        UiControlManager.getIntance().show(mControlType);
       
    }
    public virtual void toremoveUi()
    {
        UiControlManager.getIntance().remove(mControlType);
    }

    public virtual void remove() {
        isShow = false;
        transform.localPosition = mFri;
    }

    public void showUiControl() {
        isShow = true;
        show();
    }
}
