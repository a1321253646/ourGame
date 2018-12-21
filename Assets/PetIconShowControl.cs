using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetIconShowControl : MonoBehaviour {

    Image mIcon, mChose, mFighe;
    Button mButton;
    PetControl mControl;
    public long mId;
    PlayerBackpackBean mBean;
    public void init(PlayerBackpackBean bean,PetControl control) {
        mId = bean.goodId;
        mBean = bean;
        if (mIcon == null) {
            mIcon = gameObject.GetComponent<Image>();
            mChose = gameObject.GetComponentsInChildren<Image>()[0];
            mFighe = gameObject.GetComponentsInChildren<Image>()[1];
        }
        if(bean.goodType == SQLDate.GOOD_TYPE_USER_PET){
            mFighe.transform.localScale = new Vector2(1, 1);
        }
        else {
            mFighe.transform.localScale = new Vector2(0, 0);
        }
        PetJsonBean pet = JsonUtils.getIntance().getPetInfoById(mId);
        mIcon.sprite = Resources.Load("icon/pet/" + pet.activateIcon, typeof(Sprite)) as Sprite;
        mChose.transform.localScale = new Vector2(0, 0);
    }
    public void init(long id, PetControl control)
    {
        mId = id;
        if (mIcon == null)
        {
            mIcon = gameObject.GetComponent<Image>();
            mChose = gameObject.GetComponentsInChildren<Image>()[0];
            mFighe = gameObject.GetComponentsInChildren<Image>()[1];
        }
        mFighe.transform.localScale = new Vector2(0, 0);
        PetJsonBean pet = JsonUtils.getIntance().getPetInfoById(mId);
        mIcon.sprite = Resources.Load("icon/pet/" + pet.noactivateIcon, typeof(Sprite)) as Sprite;
        mChose.transform.localScale = new Vector2(0, 0);
    }
    public void initEnd() {
        if (mButton == null) {
            mButton = GetComponent<Button>();
            mButton.onClick.AddListener(() =>
            {
                mControl.onIconClick(this);
                click(true);
            });
        }
    }

    public void click(bool isClick) {
        if (isClick)
        {
            mChose.transform.localScale = new Vector2(1, 1);
        }
        else {
            mChose.transform.localScale = new Vector2(0, 0);
        }
    }
}
