using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DepictTextControl : MonoBehaviour {
    private Text mText;
    private RectTransform mRect;
    private float mStratY;
    private float POINT_X = -1.040001f;
    private float POINT_Y_OFFET = 15.6f + 54.76772f;
    // Use this for initialization
    private string mStr;
    public void setText(string str) {
        mStr = str;
        mText.text = mStr;
        Debug.Log("DepictTextControl .setText mStr= " + mStr);
    }

    void Start () {
        mText = gameObject.GetComponent<Text>();
        mRect = gameObject.GetComponent<RectTransform>();
        mText.text = mStr;
        Debug.Log("DepictTextControl .Start mStr= " + mStr);
        /*   float y = mRect.rect.y;
           float h = mRect.rect.height;
           mStratY = y + h / 2;
           Debug.Log("mStratY =" + mStratY);
           changePointY();*/

        //117.15
        //-75.18496
    }

    bool isChange = false;
    void Update()
    {
      //  Debug.Log("DepictTextControl .Update mStr= " + mStr);
        float h = mRect.rect.height;
        if (h != 0 && !isChange && mStr != null) {
           
            isChange = true;
            Debug.Log("mRect.rect.height = " + mRect.rect.height);
            int count = 0;
            if (h < 147.3513f)
            {
                float a = (147.3513f - h) / (166.2592f - 147.3513f);
                count = (int)a;
                if (a - count == 0)
                {
                    count++;
                }
            }
            else {


            }
            Debug.Log("count = " + count);
            for (int i = 0; i < count*3; i++)
            {
                mText.text = mText.text + "\n";
            }
            gameObject.transform.localPosition = new Vector2(0, -6677);
        }
    }

    private void changePointY() {

        float y = mRect.rect.y;
        float h = mRect.rect.height;
        float value = y + h / 2 - mStratY;
        if(value != 0){
            isChange = true;
        }
    }
}
