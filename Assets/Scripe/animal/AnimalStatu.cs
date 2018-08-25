using UnityEngine;
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
    private string mPathRoot;
    private string mPath;


    public AnimalStatu(int statue, int frame, string resultPath, SpriteRenderer spriteRender, bool isLoop)
    {
        mIsLopp = isLoop;
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
            Sprite sprite = Resources.Load("animal/"+mPathRoot+"/"+ mPath+"/"+statue+"/"+ index, typeof(Sprite)) as Sprite;
            spriteList.Add(sprite);
        }
        eachTime = DEFAULT_EACH_TIME;
    }



    public void upDateTime() {
        mAnimalTime += Time.deltaTime;
        if (mAnimalTime >= eachTime) {
            mAnimalTime = mAnimalTime- eachTime;
            index++;
            if (mIsLopp && index == spriteList.Count) {
                index = 0;
            }
            changeSprite();
        }
       

    }
    private void changeSprite() {
        if (index >= spriteList.Count) {
            return;
        }

        if (index == 0 && begin != null)
        {
            begin(statue);
        }
        else if (index == spriteList.Count - 1 && end != null)
        {
            end(statue);
        }
        if (mIndexCall.ContainsKey(index)) {
            mIndexCall[index](statue);        
        }
        mSpriteRender.sprite = 
            spriteList[index];

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
            eachTime = DEFAULT_EACH_TIME;
        }
        else
        {
            loopDelayTime = 0;
            eachTime = oneTime / spriteList.Count;
        }
        Debug.Log(" each == " + eachTime + " loopDelayTime= "+ loopDelayTime+ " speedDate="+ speedDate);
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
