using UnityEngine;
using System.Collections;
using UnityEngine.UI;
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
}