﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
//用于动画的状态控制，也是实际控制每一个状态动画的实现类
public class AnimalStatu 
{

    public delegate void animalBegin(int status);
    public delegate void animalEnd(int status);
    public delegate void animalIndexCallback(int status);
    public delegate void animalCountTimeCallback(int count);

    private static float DEFAULT_EACH_TIME = 1f / 12f;
    private List<Sprite> spriteList = new List<Sprite>();
    
    private float eachTime = DEFAULT_EACH_TIME;

    public int statue = -1;
    public int delayStatue = -1;
    public float loopDelayTime = 0;

    private float speedDate = 0;
    private float mScale =1;
    private float mAnimalTime = 0;
    private int index = 0;
    private bool mIsLopp = true;
    private int mFrameCount= 0;
    private SpriteRenderer mSpriteRender;
    private Image mImageRender;
    private string mPathRoot;
    private string mPath;

    private long mTimeCount = -1;
    private int mTimeCountIndex = 1;
    private float mCountTimeBackEach = -1;
    private float mCountTime = 0;
    private float mSetSpeed = 1;
    private bool mIsNativeSize = false;
    public AnimalStatu(int statue, int frame, string resultPath, SpriteRenderer spriteRender, bool isLoop, float setSpeed, bool isNativeSize)
    {
        mIsLopp = isLoop;
        mIsNativeSize = isNativeSize;
        mSetSpeed = setSpeed;
        this.statue = statue;
        mSpriteRender = spriteRender;
        mFrameCount = frame;
        mPathRoot = resultPath.Split('_')[0];
        mPath = resultPath;
        for(int i = 0; i < mFrameCount; i++) {
            string index;
            if (i > 8) {
                index = "" + (i + 1);
            }
            else{
                index = "0" + (i+1);
            }
            string path = "animal/" + mPathRoot + "/" + mPath + "/" + statue + "/" + index;
            //Debug.Log("AnimalStatu " + path);
            Sprite sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
            spriteList.Add(sprite);
        }
        eachTime = DEFAULT_EACH_TIME/ mSetSpeed;
    }
    public AnimalStatu(int statue, int frame, string resultPath, Image spriteRender, bool isLoop, float setSpeed, bool isNativeSize)
    {
        mIsNativeSize = isNativeSize;
        mIsLopp = isLoop;
        mSetSpeed = setSpeed;
        this.statue = statue;
        mImageRender = spriteRender;
        mFrameCount = frame;
        mPathRoot = resultPath.Split('_')[0];
        mPath = resultPath;
        for (int i = 0; i < mFrameCount; i++)
        {
            string index;
            if (i > 8)
            {
                index = "" + (i + 1);
            }
            else
            {
                index = "0" + (i + 1);
            }
            Sprite sprite = Resources.Load("animal/" + mPathRoot + "/" + mPath + "/" + statue + "/" + index, typeof(Sprite)) as Sprite;
            spriteList.Add(sprite);
        }
        eachTime = DEFAULT_EACH_TIME/ mSetSpeed;
    }


    public void upDateTime() {
        if (mCountTimeBackEach > 0)
        {
            mCountTime += Time.deltaTime;
            if (mCountTime >= mCountTimeBackEach)
            {
                mCountTime -= mCountTimeBackEach;
                timeBack(mTimeCountIndex);
                mTimeCountIndex++;
                if (mTimeCountIndex > mTimeCount)
                {
                    mTimeCountIndex = 1;
                }
            }
        }
        mAnimalTime += Time.deltaTime;
        if (mAnimalTime >= eachTime) {
            mAnimalTime = mAnimalTime- eachTime;
            index++;
            changeSprite();
        }
    }
    private void changeSprite() {
        if (index > spriteList.Count) {
            return;
        }

        if (index == 0 && begin != null)
        {
            begin(statue);
        }
        else if (index == spriteList.Count  && end != null)
        {
            end(statue);
        }
        if (mIndexCall.ContainsKey(index)) {
            mIndexCall[index](statue);        
        }
        if (index == spriteList.Count)
        {
            if (mIsLopp)
            {
                index = 0;
            }
            else {
                return;
            }
        }
        if (mSpriteRender != null)
        {
            mSpriteRender.sprite =
                spriteList[index];
        }
        else if(mImageRender!=null)
        {
            mImageRender.sprite = spriteList[index];
            if (mIsNativeSize)
            {
                mImageRender.SetNativeSize();
            }
        }
        

    }

    internal void setBeginCallBack(Func<object> p)
    {
        throw new NotImplementedException();
    }

    public void initAnimal() {
        mAnimalTime = 0;
        index = 0;
        changeSprite();
    }
    public void setSpeedDate(float speed) {
        speedDate = speed;
        speedChange();
    }
    private void speedChange() {
        float oneTime = 1 / speedDate;
        float animalTime = spriteList.Count * DEFAULT_EACH_TIME;
        if (oneTime >= animalTime)
        {
            loopDelayTime = oneTime - animalTime;
            eachTime = DEFAULT_EACH_TIME/ mSetSpeed;
        }
        else
        {
            loopDelayTime = 0;
            eachTime = oneTime / spriteList.Count/ mSetSpeed ;
        }
//        Debug.Log(" each == " + eachTime + " loopDelayTime= "+ loopDelayTime+ " speedDate="+ speedDate);
    }

    public void setScale(float scale) {
        mScale = scale;
        changeScale();
    }
    private void changeScale() {
        foreach (Sprite sprite in spriteList) {
            sprite.texture.width = sprite.texture.width * (int)(mScale * 10) / 10;
            sprite.texture.height = sprite.texture.height * (int)(mScale * 10) / 10;
        }
    }


    private animalBegin begin;
    private animalEnd end;
    private Dictionary<int, animalIndexCallback> mIndexCall = new Dictionary<int, animalIndexCallback>();
    private animalCountTimeCallback timeBack;
    public void setTimeCountBack(long count, animalCountTimeCallback back) {
        this.timeBack = back;
        mTimeCount = count;
        mCountTimeBackEach = eachTime * spriteList.Count / mTimeCount/ mSetSpeed;
        mCountTime = 0;
    }
    public void setEndCallBack(animalEnd end) {
        this.end = end;
    }
    public void setBeginCallBack(animalBegin begin)
    {
        this.begin = begin;
    }
    public void addIndexCallBack(int frame , animalIndexCallback index)
    {
        mIndexCall.Add(frame, index);
    }
    public void setIsLoop(bool isLoop) {
        mIsLopp = isLoop;
    }
}
