using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VocationSilderControl: MonoBehaviour
{
    private float mValue = 1;
    private long mV = 100;
    private Image mBack, mSlide, mMask;
    private bool isInit = false;

    private float mBackLeng;

    public void show(long value) {
        if (mV == value && isInit) {
            return;
        }
        mV = value;
        mValue = ((float)mV) / 100;
        if (!isInit) {
            
            Image[] imgs = GetComponents<Image>();
            mBack = imgs[0];
            mSlide = imgs[1];
            mMask = imgs[2];
            mBackLeng = mBack.GetComponent<RectTransform>().rect.width;
            isInit = true;
        }
        changeSilde();
    }


    private void changeSilde() {
        float leng = mBackLeng * mValue;
        mMask.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, leng);
        mMask.transform.position = new Vector2(mBackLeng/2 - leng/2, mMask.transform.position.y);
    }
}
