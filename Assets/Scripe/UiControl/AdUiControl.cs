using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdUiControl : UiControlBase
{
    Button mClose, mSelect;
    Text mDec;
    Image mImg;

    long mAdType = -1;
    BigNumber mCount  = null ;
    public void setDate(long type) {
        mAdType = type;
        mCount = BigNumber.getBigNumForString(getAdValue());
        string str = "观看影片，即可免费获得S1S2";
        str=str.Replace("S1", mCount.toStringWithUnit());
        if (mAdType == ActiveButtonControl.TYPE_AD_LUNHUI)
        {
            str =str.Replace("S2", "轮回点");
            mImg.sprite = Resources.Load("UI_yellow/guanggao/04", typeof(Sprite)) as Sprite;
        }
        else
        {
            str=str.Replace("S2", "魂晶");
            mImg.sprite = Resources.Load("UI_yellow/guanggao/03", typeof(Sprite)) as Sprite;
            
        }
        mDec.text = str;

        toShowUi();
    }

    private string getAdValue() {
        string value = null;
        if (mAdType == ActiveButtonControl.TYPE_AD_LUNHUI)
        {
            long level = BaseDateHelper.decodeLong(SQLHelper.getIntance().mMaxLevel);
            if (level == -1) {
                level = BaseDateHelper.decodeLong(GameManager.getIntance().mCurrentLevel);
            }
            value = JsonUtils.getIntance().getLevelData(level).lunhui;
            BigNumber bigValue = BigNumber.getBigNumForString(value);
            bigValue = BigNumber.multiply(bigValue, GameManager.getIntance().getLunhuiGet());
            value = bigValue.toString();
        }
        else
        {
            // long level = BaseDateHelper.decodeLong(SQLHelper.getIntance().mMaxLevel);
            //if (level == -1)
            //{
            long level = BaseDateHelper.decodeLong(GameManager.getIntance().mCurrentLevel);
            //}
            value = JsonUtils.getIntance().getLevelData(level).hunjing;
            BigNumber bigValue = BigNumber.getBigNumForString(value);
            bigValue = BigNumber.multiply(bigValue, GameManager.getIntance().getOnlineGet());
            value = bigValue.toString();
        }

        return value;
    }

    public override void init()
    {
        mControlType = UiControlManager.TYPE_AD;
        mClose = GameObject.Find("advert_close").GetComponent<Button>();
        mSelect = GameObject.Find("advert_watch").GetComponent<Button>();
        mDec = GameObject.Find("advert_dec").GetComponent<Text>();
        mImg = GameObject.Find("advert_img").GetComponent<Image>();

        mClose.onClick.AddListener(()=> {
            toremoveUi();
        });
        mSelect.onClick.AddListener(() => {
            GameObject.Find("Manager").GetComponent<AdManager>().playAd();
           // toremoveUi();
        });       
    }

    public override void show()
    {
        gameObject.transform.localPosition = new Vector2(0, 0);
    }

    public void playIsFinish(bool finish) {
      //  if (finish)
      //  {
            if (mAdType == ActiveButtonControl.TYPE_AD_LUNHUI)
            {
                GameManager.getIntance().mReincarnation = BigNumber.add(GameManager.getIntance().mReincarnation, mCount);
                SQLHelper.getIntance().updateLunhuiValue(GameManager.getIntance().mReincarnation);
                GameObject.Find("lunhui").GetComponent<SamsaraManage>().updateLunhuiValue();

            }
            else
            {
                GameManager.getIntance().mCurrentCrystal = BigNumber.add(GameManager.getIntance().mCurrentCrystal, mCount);
                SQLHelper.getIntance().updateHunJing(GameManager.getIntance().mCurrentCrystal);
                GameManager.getIntance().updataGasAndCrystal();
            }
            GameObject.Find("active_button_list").GetComponent<ActiveListControl>().removeAd();
      //  }
        toremoveUi();
    }
}
