using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveButtonControl : MonoBehaviour {


    public long mActiveId = -1;

    public static long ACTIVE_BUTTON_TYPE_AD = 1;
    public static long ACTIVE_BUTTON_TYPE_VOCATION = 2;

    public static long TYPE_AD_LUNHUI = 1;
    public static long TYPE_AD_HUIJING = 2;


    public static long UPDATE_SHOW_SHOW = 1;
    public static long UPDATE_SHOW_REMOVE = 2;
    public static long UPDATE_SHOW_UPDATE = 3;

    private Image mImage;
    private Button mBt;

    public ActiveButtonBean mBean = new ActiveButtonBean();

    ActiveListControl mControl;
    private void Start()
    {
        mControl = GameObject.Find("active_button_list").GetComponent<ActiveListControl>();
    }
    private float mTime = -1;
    private float mShowTime = -1;

    public void removeShowTime()
    {
        mTime = -1;
/*        AdIntance.getIntance().setTime(mTime);
        AdIntance.getIntance().setType(-1);*/
    }
    private float showVocationTime = 0;
    private bool isNeedSendNote = false;
    private void Update()
    {
        if (isNeedSendNote) {
            showVocationTime += Time.deltaTime;
            if (showVocationTime >= 1) {
                GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_SHOW_VOCATION, 5);
                isNeedSendNote = false;
                showVocationTime = 0;
            }
        }


    //    Debug.Log("==================ActiveButtonControl Update  mTime=  " + mTime + " mShowTime=" + mShowTime);
        if (mTime == -1|| mBean.buttonType != ACTIVE_BUTTON_TYPE_AD) {
            return;
        }
        if (mShowTime == -1)
        {
            mShowTime = JsonUtils.getIntance().getConfigValueForId(100050) * 60;
        }
        
        mTime += Time.deltaTime;

        
//        AdIntance.getIntance().setTime(mTime);

        if (mTime > mShowTime) {
            GameObject.Find("active_button_list").GetComponent<ActiveListControl>().removeAd();
        }
    }

    public bool init(long type,long adId , bool isAddSql) {

        if( type == mBean.buttonType) {
            return true;
 /*           if (mBean.buttonType == ACTIVE_BUTTON_TYPE_VOCATION)
            {
                return true;
            }
            else {
                mBean.adType = adId;
                mBean.count = count;
                if (isAddSql)
                {
                    updateSql(UPDATE_SHOW_UPDATE);
                }
                return true;
            }       */    
        }
        if (mBean.buttonType != -1) {
            return false;
        }
//        float time = AdIntance.getIntance().getTime();
      //  if (time == -1 && mBean.buttonType == ACTIVE_BUTTON_TYPE_AD) {
      //      time = 0;
      //  }
        mTime = 0;
        mBean.adType = adId;
        mBean.buttonType = type;
        show(isAddSql);
        return true;
    }
    private void show(bool isAddSql) {
        if (mImage == null) {
            mImage = GetComponent<Image>();
            mBt = GetComponent<Button>();
            mBt.onClick.AddListener(() =>
            {
                onclick();
            });
        }
        transform.localScale = new Vector2(1, 1);
        if (mBean.buttonType == ACTIVE_BUTTON_TYPE_AD)
        {
            mImage.sprite = Resources.Load("UI_yellow/guanggao/02", typeof(Sprite)) as Sprite;
//            AdIntance.getIntance().setType(mBean.adType);

        }
        else if (mBean.buttonType == ACTIVE_BUTTON_TYPE_VOCATION)
        {
            mImage.sprite = Resources.Load("UI_yellow/zhuanzhi/06", typeof(Sprite)) as Sprite;
            isNeedSendNote = true;
            showVocationTime = 0;
        }       
        if (isAddSql)
        {
            updateSql(UPDATE_SHOW_SHOW);
        }
    }

    private void onclick() {
        if (mBean.buttonType == ACTIVE_BUTTON_TYPE_VOCATION)
        {
            GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_CLICK_BUTTON, GuideManager.BUTTON_CLICK_OPEN_VOCATION);
            UiControlManager.getIntance().show(UiControlManager.TYPE_VOCATION);
            //            mControl.removeVocation();
        }
        else if(mBean.buttonType == ACTIVE_BUTTON_TYPE_AD) {
            GameObject.Find("advert").GetComponent<AdUiControl>().setDate(mBean.adType);
        }       
    }

    public bool removeShow(long type, bool isAddSql)
    {
        if (type != mBean.buttonType)
        {
            return false;
        }
        if (isAddSql) {
            updateSql(UPDATE_SHOW_REMOVE);
        }        
        mBean.buttonType = -1;
        transform.localScale = new Vector2(0, 0);

        return true;
    }

    public bool removeShow(long type) {
        
        return removeShow(type,true);
    }

    private void updateSql(long type) {
        if (mBean.buttonType == ACTIVE_BUTTON_TYPE_AD) {
            return;
        }
        if (type == UPDATE_SHOW_SHOW)
        {
            SQLHelper.getIntance().addActiveButton(mBean);
        }
        else if (type == UPDATE_SHOW_UPDATE) {
            SQLHelper.getIntance().updateActiveButton(mBean);
        }
        else  if(type == UPDATE_SHOW_REMOVE) {
            SQLHelper.getIntance().deleteActiveButton(mBean);
        }
    }
}
