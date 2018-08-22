using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardControl : MonoBehaviour
{
    public static int STATUE_CARP_DEFAULT = 0;
    public static int STATUE_CARP_DOWN = 2;
    public static int STATUE_CARP_UP = 3;

    public int mIndex = 0;
    private Image mImageBottom ;
    private Image mImageTop ;
    private int mStatue = STATUE_CARP_DEFAULT;
    private bool isInit = false;
    private CardManager mManager;
    void Start()
    {
        gameObject.transform.SetSiblingIndex(99999);
    }

    private void Update()
    {
        if (!isInit) {
            return;
        }
        if (mStatue == STATUE_CARP_DEFAULT)
        {
            float targetX = 600 - (mIndex - 1) * 70;
            float distance = targetX - transform.position.x;
            if (distance > 0)
            {
                float run = 60 * Time.deltaTime;
                if (run > distance)
                {
                    run = distance;
                }
                transform.Translate(Vector2.right * run);
            }
        }

    }
    private Vector3 offset;
    public void OnpointUp() {
        if (mStatue == STATUE_CARP_UP) {
            mManager.userCard(mIndex);
            Destroy(gameObject, 0);
        }
    }

    public void OnpointDown()
    {
        mStatue = STATUE_CARP_DOWN;
        offset = transform.position -Input.mousePosition;

    }
    public void OnDrag()
    {//第二个函数

       // transform.Translate(offset + Input.mousePosition);
        transform.position = offset +Input.mousePosition;
       
        if (transform.position.y >= 130 && mStatue == STATUE_CARP_DOWN)
        {
            setStatus(STATUE_CARP_UP);
        }
        else if(transform.position.y < 130 && mStatue == STATUE_CARP_UP)
        {
            setStatus(STATUE_CARP_DOWN);
        }
        //Drag就是拖动的意思，在拖动的时候，物体跟着鼠标
    }
    Sprite tnp;

    private void setStatus(int status) {
        if (mStatue == status) {
            return;
        }
        mStatue = status;
        if (mImageBottom == null)
        {
            mImageBottom = GetComponentsInChildren<Image>()[0];
        }
        if (mImageTop == null) {
            mImageTop = GetComponentsInChildren<Image>()[1];
        }
        Debug.Log("mStatue = " + mStatue);
        if (mStatue == STATUE_CARP_UP)
        {
                     
            RectTransform rt = GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(300, 300);
            Sprite sprite = Resources.Load("icon/test", typeof(Sprite)) as Sprite;
            tnp = mImageBottom.sprite;
            mImageBottom.sprite = sprite;
            mImageTop.color = new Color(1, 1, 1, 0);

        }
        else {
            
            RectTransform rt = GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(106, 141);
            mImageBottom.sprite = tnp;
            mImageTop.color = new Color(1, 1, 1, 1);
        }
    }

    public void init(string img,int index, CardManager manage) {
        isInit = true;
        mManager = manage;
        if (mImageBottom == null) {
            mImageBottom = GetComponent<Image>();
        }
        //Sprite sprite = Resources.Load("icon/card/"+img, typeof(Sprite)) as Sprite;
        //mImageBottom.sprite = sprite;
        mIndex = index;
    }

    public void deleteCard(int index) {
        if (index < mIndex) {
            mIndex--;
        }
    }

}
