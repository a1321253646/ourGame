using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetItemControl : MonoBehaviour {

    ResourceBean mRes;
    SpriteRenderer mSpriteRender;
    AnimalControlBase mAnimalControl;
    public float mTime = 0;
    public float mDelayTime = 0;
    public bool isRun = false;
    public bool isXFu = false;
    public bool isYFu = false;
    PetManager mManager ;
    Point mTarget;
    long mId;
    float mXspeed,mYspeed,mSpeed;
    bool isUp = false;
    float speed = 0.3f;
    float yy = 0;
    // Update is called once per frame
    void Update () {
        mAnimalControl.update();
        if (!isRun)
        {
            if (mTime == 0)
            {
                yy = mTarget.y + mManager.yDistance/2;
            }
            if (isUp)
            {
                if (transform.position.y + speed * Time.deltaTime >= yy)
                {
                    transform.position = new Vector3(transform.position.x, yy, transform.position.z);
                    isUp = false;
                    yy = mTarget.y - mManager.yDistance / 2;
                }
                else
                {
                    transform.Translate(Vector2.up * (speed * Time.deltaTime));
                }
            }
            else {
                if (transform.position.y - speed * Time.deltaTime <= yy)
                {
                    transform.position = new Vector3(transform.position.x, yy, transform.position.z);
                    isUp = true;
                    yy = mTarget.y + mManager.yDistance / 2;
                }
                else
                {
                    transform.Translate(Vector2.down * (speed * Time.deltaTime));
                }
            }
            mTime += Time.deltaTime;
            if (mTime >= mDelayTime)
            {
                mTime -= mDelayTime;
                getTarget();
            }
        }
        else {
            float x = mXspeed * Time.deltaTime;
            float y = mYspeed * Time.deltaTime;
            bool isxEnd = false;
            bool isyEnd = false;
            if (isXFu && transform.position.x + x <= mTarget.x)
            {
                isxEnd = true;
            }
            else if (!isXFu && transform.position.x + x >= mTarget.x) {
                isxEnd = true;
            }
            if (isYFu && transform.position.y + y <= mTarget.y)
            {
                isyEnd = true;
            }
            else if (!isYFu && transform.position.y + y >= mTarget.y)
            {
                isyEnd = true;
            }
          
            if (isxEnd && isyEnd)
            {
                transform.position = new Vector3(mTarget.x, mTarget.y,transform.position.z);
                mTime = 0;
                mAnimalControl.setStatus(ActionFrameBean.ACTION_STANDY);
                isRun = false;
            }
            else {
                transform.Translate(Vector2.up * y);
                transform.Translate(Vector2.right * x);
            }
        }
        

    }

    public void init(PetManager.PetLocalDate date,PetManager manager) {
        mId = date.id;
        mManager = manager;
        mDelayTime = JsonUtils.getIntance().getConfigValueForId(100039);
        mSpeed = JsonUtils.getIntance().getConfigValueForId(100040);
        PetJsonBean json = JsonUtils.getIntance().getPetInfoById(date.id);
        mRes = JsonUtils.getIntance().getEnemyResourceData(json.resouce);
        mSpriteRender = gameObject.GetComponent<SpriteRenderer>();
        mAnimalControl = new AnimalControlBase(mRes, mSpriteRender);
        mAnimalControl.setStatus(ActionFrameBean.ACTION_STANDY);
        mAnimalControl.start();
        gameObject.transform.position = new Vector3(date.x - mRes.getHurtOffset().x,date.y - mRes.idel_y,transform.position.z);
        getTarget();
    }
    private void getTarget() {
        isRun = true;
        mTarget = mManager.getNewMoveTarget(mId);
        mTarget.x = mTarget.x - mRes.getHurtOffset().x;
        mTarget.y = mTarget.y - mRes.idel_y;        
        
        mAnimalControl.setStatus(ActionFrameBean.ACTION_MOVE);
        if (mTarget.x < transform.position.x)
        {
            isXFu = true;
        }
        else
        {
            isXFu = false;
        }
        if (mTarget.y < transform.position.y)
        {
            isYFu = true;
        }
        else
        {
            isYFu = false;
        }

        if (mTarget.x == transform.position.x)
        {
            if (isYFu)
            {
                mYspeed = -mSpeed;
            }
            else
            {
                mYspeed = mSpeed;
            }

            mXspeed = 0;
        }
        else if ((mTarget.x - transform.position.x) * (mTarget.x - transform.position.x) < (mTarget.y - transform.position.y) * (mTarget.y - transform.position.y)) {
            if (isYFu)
            {
                mYspeed = -mSpeed;
            }
            else
            {
                mYspeed = mSpeed;
            }
            mXspeed = (mTarget.x - transform.position.x) * mYspeed / (mTarget.y - transform.position.y);
        }
        else
        {
            if (isXFu)
            {
                mXspeed = -mSpeed;
            }
            else
            {
                mXspeed = mSpeed;
            }
            mYspeed = (mTarget.y - transform.position.y) * mXspeed / (mTarget.x - transform.position.x);
        }
        if (mTarget.x < transform.position.x)
        {
            isXFu = true;
        }
        else {
            isXFu = false;
        }
        if (mTarget.y < transform.position.y)
        {
            isYFu = true;
        }
        else {
            isYFu = false;
        }
        
        
    }
}
