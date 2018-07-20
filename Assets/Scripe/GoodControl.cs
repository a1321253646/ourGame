using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GoodControl : MonoBehaviour {

    // Use this for initialization
    private Image mImage;
    private Text mText;
    public long id = -1;
    private long count;
    GoodJsonBean mGoodInfo;
    void Start()
    {
        mImage = GetComponentsInChildren<Image>()[1];
        mText = GetComponentInChildren<Text>();
        mImage.sprite = null;
        mText.text = "";
        Debug.Log("mText = " + mText + "mImage = " + mImage);
    }
    public bool isFull() {
        return count == mGoodInfo.stacking;
    }

    public long updateUi(long id, long count)
    {
        this.id = id;
        mGoodInfo = BackpackManager.getIntance().getGoodInfoById(id);
       // SpriteRenderer sp1 = mImage.GetComponent<SpriteRenderer>();
        Debug.Log("icon = " + mGoodInfo.icon+ "mImage = "+ mImage);
        mImage.sprite = Resources.
            Load("backpackIcon/" + mGoodInfo.icon, typeof(Sprite)) as Sprite;
        mImage.color = Color.white;
        return setCount(count);

    }

    public bool deleteCount(long count) {
        if (count >= this.count)
        {
            return false;
        }
        else {
            this.count -= count;
            mText.text = "" + this.count;
            return true;
        }
    }

    public long addCount(long count)
    {
        long tmp = count + this.count;
        if (tmp > mGoodInfo.stacking)
        {
            this.count = mGoodInfo.stacking;
            mText.text = "" + this.count;
            return tmp - mGoodInfo.stacking;
        }
        else
        {
            this.count = tmp;
            mText.text = "" + this.count;
            return 0;
        }
    }

    public long setCount(long count)
    {
        if (count > mGoodInfo.stacking)
        {
            this.count = mGoodInfo.stacking;
            mText.text = "" + this.count;
            return count - mGoodInfo.stacking;
        }
        else
        {
            this.count = count;
            mText.text = "" + this.count;
            return 0;
        }
    }
}
