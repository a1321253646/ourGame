using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UserCardListControl : MonoBehaviour {
    public int LinCount = 4;

    public GameObject cardObject;

    private GridLayoutGroup mGrilLayout;
    CardItemControl[] mCardControl;
    private int mGoodUiCount;
    // Use this for initialization
    void Start () {
        mGrilLayout = GetComponent<GridLayoutGroup>();
    }

    // Update is called once per frame
    float time = 0;
    int count = 0;
	void Update () {
        time += Time.deltaTime;
        if (time > 1) {
            count++;
            time = time - 1;
            addCard();
        }
    }

    public void addCard() {
        mGoodUiCount++;
        GameObject good = GameObject.Instantiate(cardObject,
            new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        good.transform.parent = gameObject.transform;
        good.transform.localScale = Vector2.one;
        mCardControl = GetComponentsInChildren<CardItemControl>();
        SetGridHeight();
    }
    private void SetGridHeight()     //每行Cell的个数
    {
        int line = 0;
        if (mGoodUiCount % LinCount != 0)
        {
            line = 1;
        }
        line += mGoodUiCount / LinCount;
        if (line < 3) {
            line = 3;
        }
        //   Debug.Log("  gridLyout line = " + line);
        float height = line * mGrilLayout.cellSize.y;  //行数乘以Cell的高度，3.0f是微调
        height += (line - 1) * mGrilLayout.spacing.y;     //每行之间有间隔
        mGrilLayout.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }
}
