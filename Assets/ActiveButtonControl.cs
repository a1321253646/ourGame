using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveButtonControl : MonoBehaviour {


    public long mActiveId = -1;

    public static long ACTIVE_BUTTON_TYPE_AD = 1;
    public static long ACTIVE_BUTTON_TYPE_VOCATION = 2;

    private Image mImage;
    private Button mBt;
    private long mType = -1;
    private long mAdId = -1;

    private bool init(long type,long adId ) {
        if( type == mType && mType == ACTIVE_BUTTON_TYPE_VOCATION && adId == mAdId) {
            return true;
        }

        if (type == mType && mType == ACTIVE_BUTTON_TYPE_VOCATION && adId != mAdId)
        {
            mAdId = adId;
            updateSql(true);
            return true;
        }
        if (mType != -1) {
            return false;
        }

        mAdId = adId;
        mType = type;
        show();

        return true;
    }
    private void show() {
        if (mImage == null) {
            mImage = GetComponent<Image>();
            mBt = GetComponent<Button>();
            mBt.onClick.AddListener(() =>
            {
                onclick();
            });
        }
        if (mType == ACTIVE_BUTTON_TYPE_AD)
        {
            mImage.sprite = Resources.Load("UI_yellow/guanggao/02", typeof(Sprite)) as Sprite;
        }
        else if (mType == ACTIVE_BUTTON_TYPE_VOCATION)
        {
            mImage.sprite = Resources.Load("UI_yellow/zhuanzhi/06", typeof(Sprite)) as Sprite;
        }
        transform.localScale = new Vector2(1, 1);
        updateSql(true);
    }

    private void onclick() {

    }

    public void removeShow() {
        mType = -1;
        transform.localScale = new Vector2(0, 0);
        updateSql(false);
    }

    private void updateSql(bool isShow) {

    }
}
