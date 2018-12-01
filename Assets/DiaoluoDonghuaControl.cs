using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaoluoDonghuaControl : MonoBehaviour {

    public static int GOOD_DIAOLUO_TYPE = 1;
    public static int SHUIJI_DIAOLUO_TYPE = 2;

    EnemyBase mEnemy;
    long mType;
    bool isInit = false;
    bool isDown = true;

    float mHight = 0;
    float mXspeed = 0;
    float mYspeed = 0;
    public float mDownSpeed = 0;

    Vector2 mUpTarget;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (mUpTarget.x != 0) {
            if (transform.position.y >= mUpTarget.y - 0.05 && transform.position.y <= mUpTarget.y + 0.05)
            {
                Destroy(gameObject);
                GameManager.getIntance().updateGasAndCrystal();
                if (mId != -1) {
                    GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_OBJECT_DROP, mId);
                    if (mId < InventoryHalper.TABID_3_START_ID)
                    {
                        GameManager.getIntance().uiManager.setPackPointShow(1);
                    }
                    else
                    {
                        GameManager.getIntance().uiManager.setCardPointShow(1);
                    }
                }
            }
        }
        if (!isInit) {
            return;
        }
        if (isDown)
        {
            float down = Time.deltaTime * mDownSpeed;
            if (down < mHight)
            {
                mHight = mHight - down;
            }
            else
            {
                mHight = 0;
                down = mHight;
                isDown = false;
            }
            transform.Translate(Vector2.down * down);
            if (mBackManager.isRun)
            {
                transform.Translate(Vector2.left * (mBackManager.moveSpeed * Time.deltaTime));
            }
        }
        else {
            if (mYspeed == 0) {
                if (mType == SHUIJI_DIAOLUO_TYPE)
                {
                    mYspeed =  JsonUtils.getIntance().getConfigValueForId(100018);
                }
                else
                {
                    mYspeed =  JsonUtils.getIntance().getConfigValueForId(100030);
                }
                mXspeed = (mUpTarget.x - transform.position.x) / (mUpTarget.y - transform.position.y) * mYspeed;
            }
            float up = mYspeed * Time.deltaTime;
            float x = Time.deltaTime * mXspeed;

            //            Debug.Log("transform.position.x = " + mUpTarget.x + " transform.position.y=" + transform.position.y+ " up="+ up);
            if (transform.position.y + up > mUpTarget.y) {
                up = mUpTarget.y - transform.position.y;
                x = mUpTarget.x - transform.position.x;
                isInit = false;
            }
            transform.Translate(Vector2.up * (up));
            transform.Translate(Vector2.right * (x));
        }
    }
    BackgroundManager mBackManager;


    private long mId = -1;
    public void init(EnemyBase enemy, int type, string imaPath, float count,long id){
        mId = id;
        init(enemy,type,imaPath,count);
    }
    public void init(EnemyBase enemy,int type,string imaPath,float count) {
        mType = type;
        List<float> list = enemy.resourceData.getTargetBorder();
        if (list != null && list.Count == 3) {
            mHight = list[2] / 2;
        }
        mUpTarget = GameManager.getIntance().uiManager.mgetTarget(type);
        Vector3 v1 =  PointUtils.worldTransToScreen(new Vector3(enemy.mLocalBean.mCurrentX + enemy.resourceData.getHurtOffset().x,
            enemy.mLocalBean.mCurrentY + enemy.resourceData.getHurtOffset().y ,0));
        Vector3 v2 = PointUtils.worldTransToScreen(new Vector3(enemy.mLocalBean.mCurrentX + enemy.resourceData.getHurtOffset().x,
            enemy.mLocalBean.mCurrentY + enemy.resourceData.getHurtOffset().y - mHight , 0));
        mHight = v1.y - v2.y;
        Debug.Log("mHight = " + mHight);
        Vector2 v = v1;
        transform.position = v;
        
        if (type == SHUIJI_DIAOLUO_TYPE) {
            imaPath = "ui_new/mojing";
        }
        Debug.Log("DiaoluoDonghuaControl imaPath = " + imaPath);
        GetComponent<Image>().sprite = Resources.Load(imaPath, typeof(Sprite)) as Sprite;
        mBackManager = enemy.mBackManager;
        isInit = true;
    }
}
