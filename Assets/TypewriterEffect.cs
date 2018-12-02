using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class TypewriterEffect : MonoBehaviour {

    // Use this for initialization
    private Text myText;
    private bool isInit = false;
    private string showText = "";
    private int listIndex = 0;
    private int strIndex = 0;
    public float charsPerSecond = 0.2f;//打字时间间隔
    public float mTime = 0;
    private List<TextColor> mTextList = new List<TextColor>();
    void Start() {
        myText = GetComponent<Text>();        
    }
    public void init(string  str) {
        showText = str;
        initText();
        isInit = true;
    }

    class TextColor {
        public string text;
        public string color;
        public int leng = 0;
        public string getString(int index) {
            if (color == null)
            {
                return text.Substring(0, index);
            }
            else {
                string str = color;
                str += text.Substring(0, index);
                str += "</color>";
                return str;
            }
        }
    }

    void Update()
    {
        if (!isInit && showText != null && showText.Length > 0)
        {
            Destroy( GameObject.Find("game_begin"),0.3f);
            SQLHelper.getIntance().updateFristStart(2);
            showText = null;
            GameObject.Find("Manager").GetComponent<LevelManager>().init();
            return;
        }
        mTime += Time.deltaTime;
        if (mTime >= charsPerSecond) {
            mTime = mTime - charsPerSecond;
            showTextByTime();
        }
    }

    private void showTextByTime() {
        if (listIndex < mTextList.Count)
        {
            TextColor tc = mTextList[listIndex];
            if (strIndex >= tc.leng)
            {
                listIndex++;
                strIndex = 0;
                if (tc.color == null)
                {
                    showText = showText + tc.text;
                }
                else {
                    showText = showText + tc.color + tc.text + "</color>";
                }               
                showTextByTime();
            }
            else {
                myText.text =  showText + tc.getString(strIndex);
                strIndex++;
            }
        }
        else {
            isInit = false;
        }
    }

    private void initText()
    {
        myText.text = "";
        bool isEndColor =  showText.EndsWith("</color>");
        string[] list = Regex.Split(showText, "</color>", RegexOptions.IgnoreCase);
        int count = list.Length;
        if (isEndColor) {
            count = list.Length;
        }
        else {
            count = list.Length - 1;
        }

        for(int i = 0; i< count; i++) {
            if (list[i] == null || list[i].Length == 0) {
                continue;
            }
            getTextColor(list[i]);
        }
        if (!isEndColor) {
            TextColor tx = new TextColor();
            tx.color = null;
            tx.text = list[list.Length -1];
            tx.leng = tx.text.Length;
            mTextList.Add(tx);
        }
        showText = "";
        foreach (TextColor t in mTextList) {
            Debug.Log("TextColor text = " + t.text + " color = " + t.color);
        }
    }

    private void getTextColor(string str) {
        string[] list = Regex.Split(str, "<color=#", RegexOptions.IgnoreCase);
        if (list.Length == 2)
        {
            TextColor tx1 = new TextColor();
            tx1.color = null;
            tx1.text = list[0];
            tx1.leng = tx1.text.Length;
            mTextList.Add(tx1);
            string[] list2 =  list[1].Split('>');
            if(list2.Length == 2) {
                TextColor tx2 = new TextColor();
                tx2.color = "<color=#" + list2[0] + ">";
                tx2.text = list2[1];
                tx2.leng = tx2.text.Length;
                mTextList.Add(tx2);
            }
        }
    }
    // Update is called once per frame
}
