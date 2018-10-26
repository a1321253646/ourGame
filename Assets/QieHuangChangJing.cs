using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class QieHuangChangJing : MonoBehaviour {

    private Text mTitle, mTimeText;
    private float mTime = 0;
    private float mTime2 = 0;

    private Vector2 mFri;
    private bool isStart = false;
    private int count = 2;
    private float friColor = 0;
    private float eveyTime = 0;
    private bool isAdd = true;
    private bool isEnd = false;
    private Image image = null;
    // Use this for initialization
    void Start () {
        image = GetComponent<Image>();
        mTitle = GetComponentsInChildren<Text>()[0];
      //  mTimeText = GetComponentsInChildren<Text>()[1];
        mFri = gameObject.transform.localPosition;
        if (GameManager.getIntance().isShowQieHuang)
        {
            friColor = 255;
            Color c = image.color;
            c.a = friColor / 255;
            image.color = c;
            isAdd = false;
            run(GameManager.getIntance().isWinQirHuang);
            eveyTime = 0.5f / 155f;
        }
        else {
            eveyTime =1f / 155f;
        }
        
        Debug.Log("eveyTime == " + eveyTime);
    }
	
	// Update is called once per frame
	void Update () {
        if (!isStart) {
            return;
        }
        mTime += Time.deltaTime;
        mTime2 += Time.deltaTime; ;
/*        if (mTime > 1) {
            mTime = mTime - 1f;
            count--;
            if (count >= 0 && isAdd) {           
                mTimeText.text = "" + count;
            }
        }*/
        Debug.Log("Time.deltaTime == " + Time.deltaTime);
        Debug.Log("Update eveyTime== " + eveyTime+ " mTime2== "+ mTime2+ " friColor== "+ friColor);
        int count1 =(int) (Time.deltaTime / eveyTime);
        if (mTime2 > eveyTime) {
            mTime2 -= eveyTime;
            if (isAdd)
            {
                friColor+= count1;
                if (friColor > 255) {
                    isEnd = true;
                }
            }
            else {
                friColor-= count1;
                if (friColor < 100)
                {
                    isEnd = true;
                }
            }
            if (isEnd)
            {
                if (!isAdd)
                {
                    Dishow();
                }
                else {
                   SceneManager.LoadScene(1);
                }              
                
            }
            else {
                Color c = image.color;
                c.a = friColor/255;
                image.color = c;
            }
        }
	}

    public void run(bool isSuccess) {
        if (isStart) {
            return;
        }

        GameManager.getIntance().isShowQieHuang = true;       
        if(isAdd) {
            friColor = 100;
            Color c = image.color;
            c.a = friColor/255;
            image.color = c;
        }
        if (isSuccess)
        {
            mTitle.text = "成功";
        }
        else {
            mTitle.text = "失败";
        }
      //  mTimeText.text = "" + count;
        if (!isAdd) {
        //    mTimeText.text = "";
            mTitle.text = "";
        }
        show();
    }

    private void show() {
        gameObject.transform.localPosition = new Vector2(0, 0);
        mTime2 = 0;
        isEnd = false;
        isStart = true;
    }
    private void Dishow()
    {
        gameObject.transform.localPosition = mFri;
        isStart = false;
        isAdd = true;
    }
}
