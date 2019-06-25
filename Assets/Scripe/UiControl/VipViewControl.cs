using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VipViewControl : UiControlBase
{

    public static int SHOW_ZUANSHI = 2;
    public static int SHOW_HUNJIN = 3;
    public static int SHOW_LUNHUI = 4;

    private Text mBuyGet, mEachGet, mAllGet, mLeftDay,mSureTx;
    private Button mClose, mSure;
    private ShopJsonBean mBean = null;
    BigNumber mBuyGetCount, mEachGetCount, mAllGetCount;
    long mGetDate = 0;
    long mIndex = 0;
    public override void show()
    {
        isShow = true;
        if (mBean == null) {
            foreach (ShopJsonBean tmp in JsonUtils.getIntance().getShopList()) {
                if (tmp.itemtype == 1) {
                    mBean = tmp;
                    break;
                }
            }
            string[] list = mBean.parameter.Split(',');
            mBuyGetCount = BigNumber.getBigNumForString(list[0]);
            mEachGetCount = BigNumber.getBigNumForString(list[1]);
            mGetDate = long.Parse(list[2]);
            mAllGetCount = BigNumber.multiply(mEachGetCount, mGetDate);
            mAllGetCount = BigNumber.add(mAllGetCount, mBuyGetCount);
            mBuyGet.text = mBuyGetCount.toStringWithUnit();
            mEachGet.text = mEachGetCount.toStringWithUnit();
            mAllGet.text = mAllGetCount.toStringWithUnit();
        }
        gameObject.transform.localPosition = new Vector2(0, 0);
        updateView();
    }

    public void updateView()
    {
        if (!SQLHelper.getIntance().isVipDate())
        {
            if (GameManager.getIntance().mSkusList == null || GameManager.getIntance().mSkusList.Count == 0)
            {
                GameObject.Find("shop_vip_google_connet").transform.localScale = new Vector2(1, 1);
                mSure.interactable = false ; 
            }
            else
            {
                GameObject.Find("shop_vip_google_connet").transform.localScale = new Vector2(0, 0);
                ShopJsonBean item = JsonUtils.getIntance().getShopItemById(1001);
                foreach (SkuJsonBean sku in GameManager.getIntance().mSkusList)
                {
                    if (item.sku.Equals(sku.sku))
                    {
                        mSureTx.text = sku.price;
                        mSure.interactable = true;
                        break;
                    }
                }

            }

        }
        else if (SQLHelper.getIntance().isNoGetVip())
        {
            mSureTx.text = "领取";
            mSure.interactable = true;
        }
        else
        {
            mSure.interactable = false;
        }
        int day = SQLHelper.getIntance().vipLeftDay();
        mLeftDay.text = day + "";

        
    }

    public void updateVipAdd() {
        if (SQLHelper.getIntance().isVipDate())
        {
            if (mIndex == 0) {
                mIndex = SkillIndexUtil.getIntance().getSamIndexBySamId(false, 88888888);
            }
            GameManager.getIntance().mLunhuiOutlineGet.AddFloat(mIndex, 1 + 1);
            GameManager.getIntance().mLunhuiLunhuiGet.AddFloat(mIndex, 1 + 1);

            PlayControl  play=  GameObject.Find("Manager").GetComponent<LevelManager>().mPlayerControl;
            play.mAllAttributePre.add(mIndex, AttributePre.maxBloodVolume,5000);
            play.mAllAttributePre.add(mIndex, AttributePre.defense,5000);
            play.mAllAttributePre.add(mIndex, AttributePre.aggressivity,5000);
            play.getAttribute(true);
        }
    }

    public override void init()
    {
        mControlType = UiControlManager.TYPE_VIP;
        mClose = GameObject.Find("vip_close_bt").GetComponent<Button>();
        mBuyGet = GameObject.Find("vip_buy_get").GetComponent<Text>();
        mEachGet = GameObject.Find("vip_each_get").GetComponent<Text>();
        mAllGet = GameObject.Find("vip_all_get").GetComponent<Text>();
        mLeftDay = GameObject.Find("vip_left_day").GetComponent<Text>();
        mSureTx = GameObject.Find("vip_view_sure").GetComponent<Text>();
        mSure = GameObject.Find("vip_sure_bt").GetComponent<Button>();


        mClose.onClick.AddListener(() =>
        {
            toremoveUi();
        });
        mSure.onClick.AddListener(() =>
        {
            sure();
        });

    }

    private void sure() {
        if (!SQLHelper.getIntance().isVipDate())
        {
             Debug.Log(" BillingControl buySku");
             AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
             Debug.Log(" BillingControl AndroidJavaClass");
             AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
             Debug.Log(" BillingControl AndroidJavaObject");
             string[] param = new string[1];
             param[0] = mBean.sku;
             bool isBuyIng = jo.Call<bool>("buySku", param);
            //buyVipSuccess();
        }
        else if (SQLHelper.getIntance().isNoGetVip())
        {

            SQLHelper.getIntance().updateVipGetValue();
            BigNumber zuanshi2 = SQLHelper.getIntance().mZuanshi;
            zuanshi2 = BigNumber.add(zuanshi2, mEachGetCount);
            SQLHelper.getIntance().updateZuanshiValue(zuanshi2);
            GameManager.getIntance().updateZuanshi();

        }
        updateView();
    }

    public void buyVipSuccess() {
        SQLHelper.getIntance().updateVipDateValue(mGetDate);
        BigNumber zuanshi2 = SQLHelper.getIntance().mZuanshi;
        zuanshi2 = BigNumber.add(zuanshi2, mBuyGetCount);
        SQLHelper.getIntance().updateZuanshiValue(zuanshi2);
        GameManager.getIntance().updateZuanshi();
    }

}
