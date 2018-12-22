﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetIconShowControl : MonoBehaviour {

    Image mIcon, mChose, mFighe;
    Button mButton;
    PetControl mControl;
    public long mId;
    public PlayerBackpackBean mBean;
    public PetJsonBean mJsonBean;
    public void init(PlayerBackpackBean bean,PetControl control) {
        mControl = control;
        mId = bean.goodId;
        mBean = bean;
        if (mIcon == null) {
            mIcon = gameObject.GetComponentsInChildren<Image>()[1];
            mChose = gameObject.GetComponentsInChildren<Image>()[2];
            mFighe = gameObject.GetComponentsInChildren<Image>()[3];
        }
        if(bean.goodType == SQLDate.GOOD_TYPE_USER_PET){
            mFighe.transform.localScale = new Vector2(1, 1);
        }
        else {
            mFighe.transform.localScale = new Vector2(0, 0);
        }
        mJsonBean = JsonUtils.getIntance().getPetInfoById(mId);
        mIcon.sprite = Resources.Load("icon/pet/" + mJsonBean.activateIcon, typeof(Sprite)) as Sprite;
        mChose.transform.localScale = new Vector2(0, 0);
        initEnd();
    }
    public void init(long id, PetControl control)
    {
        mId = id;
        mControl = control;
        if (mIcon == null)
        {
            mIcon = gameObject.GetComponentsInChildren<Image>()[1];
            mChose = gameObject.GetComponentsInChildren<Image>()[2];
            mFighe = gameObject.GetComponentsInChildren<Image>()[3];
        }
        mFighe.transform.localScale = new Vector2(0, 0);
        mJsonBean = JsonUtils.getIntance().getPetInfoById(mId);
        mIcon.sprite = Resources.Load("icon/pet/" + mJsonBean.noactivateIcon, typeof(Sprite)) as Sprite;
        mChose.transform.localScale = new Vector2(0, 0);
        initEnd();
    }
    public void initEnd() {
        if (mButton == null) {
            mButton = GetComponent<Button>();
            mButton.onClick.AddListener(() =>
            {
                mControl.onIconClick(this);
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
