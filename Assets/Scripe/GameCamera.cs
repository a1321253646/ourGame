using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using LitJson;

public class GameCamera : MonoBehaviour
{

    public float devHeight = 19.2f;
    public float devWidth = 10.8f;

    // Use this for initialization

    public static float SCREEN_BILI = 0;

    void Start()
    {
        GameObject left = GameObject.Find("game_test_1");
        GameObject right = GameObject.Find("game_test_2");
        GameObject top = GameObject.Find("game_test_3");
        GameObject bottom = GameObject.Find("game_test_4");
//        Text dec = GameObject.Find("game_test_tx").GetComponent<Text>();



        float screenHeight = this.GetComponent<Camera>().pixelHeight;
        float width = this.GetComponent<Camera>().pixelWidth;

        float w = right.GetComponent<RectTransform>().localPosition.x - left.GetComponent<RectTransform>().localPosition.x;
        float h = top.GetComponent<RectTransform>().localPosition.y - bottom.GetComponent<RectTransform>().localPosition.y;

        float bili1 = h / w;
        float bili2 = devWidth / devHeight;
        SCREEN_BILI = bili2/bili1 ;

        //this.GetComponent<Camera>().orthographicSize = screenHeight / 200.0f;

        float orthographicSize = this.GetComponent<Camera>().orthographicSize;

        float aspectRatio = width * 1.0f / screenHeight;

        float cameraWidth = orthographicSize * 2 * aspectRatio;

//        Debug.Log("cameraWidth = " + cameraWidth);

        if (cameraWidth < devWidth)
        {
            orthographicSize = devWidth / (2 * aspectRatio);
            Debug.Log("new orthographicSize = " + orthographicSize);
            this.GetComponent<Camera>().orthographicSize = orthographicSize;
        }

    }

    // Update is called once per frame
    bool isFristEsc = false;
    float mEscTime = 0;
    void Update()
    {
   //     Debug.Log("new isFristEsc = " + isFristEsc+ "  mEscTime="+ mEscTime);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            if (isFristEsc && mEscTime < 2)
            {
                Application.Quit();
                return;
            }
            //else if (!isFristEsc)
            //{
                isFristEsc = true;
                mEscTime = 0;
            //}
        }
        else if (isFristEsc) {
            mEscTime+=Time.deltaTime;
            if (mEscTime >= 2) {
                isFristEsc = false;
                mEscTime = 0;
            }
        }
    }
    private float mTimeScance = 1;
    void startOrPause(string str)
    {
        Debug.Log("jackzheng:" + "startOrPause=" + str + " mTimeScance=" + mTimeScance);

        if (str.Equals("start"))
        {
            Time.timeScale = mTimeScance;
        }
        else if (str.Equals("pause"))
        {
            mTimeScance = Time.timeScale;
            Time.timeScale = 0;
        }
    }

    void removeAdShow() {
        GameObject.Find("advert").GetComponentInChildren<AdUiControl>().playIsFinish(true);
    }

    void onQueryPurchasesToUnity(string skus)
    {
        Debug.Log("jackzheng:" + "startOrPause=" + skus );
        GameManager.getIntance().mSkusList = JsonMapper.ToObject<List<SkuJsonBean>>(skus);
        GameObject.Find("shop_root").GetComponent<ShopViewControl>().getSkusUpdate();
        GameObject.Find("vip_show_view").GetComponent<VipViewControl>().updateView();
    }
    void onBuySuccess(string sku)
    {
        Debug.Log("jackzheng:" + "startOrPause=" + sku);
        List<ShopJsonBean>  list = JsonUtils.getIntance().getShopList();
        foreach (ShopJsonBean bean in list) {
            if (!string.IsNullOrEmpty(bean.sku) && bean.sku.Equals(sku)) {
                if (bean.id != 100)
                {
                    GameObject.Find("shop_root").GetComponent<ShopViewControl>().buySuccess(bean);
                    break;
                }
                else {
                    GameObject.Find("vip_show_view").GetComponent<VipViewControl>().buyVipSuccess();
                    break;
                }

            }
        }
    }
}