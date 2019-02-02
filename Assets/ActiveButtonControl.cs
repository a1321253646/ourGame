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

    public bool init(long type,long adId , string count,bool isAddSql) {

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

        mBean.adType = adId;
        mBean.count = count;
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
        }
        else if (mBean.buttonType == ACTIVE_BUTTON_TYPE_VOCATION)
        {            
            mImage.sprite = Resources.Load("UI_yellow/zhuanzhi/06", typeof(Sprite)) as Sprite;
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
            GameObject.Find("advert").GetComponent<AdUiControl>().setDate(mBean.adType, mBean.count);
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
