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
        isChange = false;
        Debug.Log("DepictTextControl .setText mStr= " + mStr);
    }

    void Start () {
        mText = gameObject.GetComponent<Text>();
        mRect = gameObject.GetComponent<RectTransform>();
        mText.text = mStr;
        Debug.Log("DepictTextControl .Start mStr= " + mStr);
    }

    bool isChange = false;
    void Update()
    {       
        if ( !isChange)
        {
            float h = mText.preferredHeight;
            if (h != 0 && mStr != null) {
                if (h < 224.4f)
                {
                    h = 224.4f;
                }
                GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
                isChange = true;
                //gameObject.transform.localPosition = new Vector2(0, -h);
            }

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
