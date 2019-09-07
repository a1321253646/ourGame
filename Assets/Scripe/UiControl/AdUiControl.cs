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
        setShowContext();
        toShowUi();
    }

    private void setShowContext()
    {
        mCount = BigNumber.getBigNumForString(getAdValue(mAdType));
        string str = "观看影片，即可免费获得S1S2";
        str = str.Replace("S1", mCount.toStringWithUnit());
        if (mAdType == ActiveButtonControl.TYPE_AD_LUNHUI)
        {
            str = str.Replace("S2", "轮回点");
            mImg.sprite = Resources.Load("UI_yellow/guanggao/04", typeof(Sprite)) as Sprite;
        }
        else
        {
            str = str.Replace("S2", "魂晶");
            mImg.sprite = Resources.Load("UI_yellow/guanggao/03", typeof(Sprite)) as Sprite;

        }
        mDec.text = str;

    }

    public static string getAdValue(long type) {
        string value = null;
        if (type == ActiveButtonControl.TYPE_AD_LUNHUI)
        {
            long level = BaseDateHelper.decodeLong(SQLHelper.getIntance().mMaxLevel);
            if (level == -1) {
                level = BaseDateHelper.decodeLong(GameManager.getIntance().mCurrentLevel);
            }
            Debug.Log("getAdValue level = " + level);
            BigNumber bigValue = getLunhuiAd();
            Debug.Log("getAdValue bigValue = " + bigValue.toString());
            Debug.Log("getAdValue GameManager.getIntance().getLunhuiGet() = " + GameManager.getIntance().getLunhuiGet());

            bigValue = BigNumber.multiply(bigValue, GameManager.getIntance().getLunhuiGet());
            Debug.Log("getAdValue end bigValue = " + bigValue.toString());
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

    private static BigNumber getLunhuiAd() {
        BigNumber adLunhui = null;
        Debug.Log("SQLHelper.getIntance().mMaxLevel= " + BaseDateHelper.decodeLong(SQLHelper.getIntance().mMaxLevel));
        long current = BaseDateHelper.decodeLong(SQLHelper.getIntance().mGameLevel);
        if (SQLHelper.getIntance().mMaxLevel != BaseDateHelper.encodeLong(-1))
        {
            long maxLevel = BaseDateHelper.decodeLong(SQLHelper.getIntance().mMaxLevel);
           
            long maxIndex = 0;
            long gameindex = 0;
            if (maxLevel < 0)
            {
                maxIndex = 1;
            }
            else
            {
                maxIndex = (maxLevel - 1) / 1000 + 1;
            }

            if (current < 0)
            {
                gameindex = 1;
            }
            else
            {
                gameindex = (current - 1) / 1000 + 1;
            }

            if (gameindex < maxIndex)
            {
                Debug.Log("getAdValue maxIndex = " + maxIndex + " maxLevel=" + maxLevel);
                adLunhui = JsonUtils.getIntance().readMaxLevelLunhuiAdValue(maxIndex, maxLevel);

            }
            else
            {
                adLunhui = JsonUtils.getIntance().getLevelData(maxLevel).getAdLunhui();
            }

        }
        else {
            adLunhui = JsonUtils.getIntance().getLevelData(current).getAdLunhui();
        }
        
        return adLunhui;
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

    public void update() {
        if (!isShow) {
            return;
        }
        getAdValue(mAdType);
        setShowContext();
    }
}
