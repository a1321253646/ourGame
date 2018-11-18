using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// 脚本挂载到每个可拖拽的Item上面即可
/// </summary>
public class ItemOnDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // 实例化的新item
    public GameObject mNewItem;
    public Transform mNewItemRoot;
    public ScrollRect mScrollRect;
    // 是否按下
    public bool mIsDown = false;
    // 按下与松开鼠标之间的距离
    public float mBorderDis = 0.5f;
    // 总的按下时间
    public float mTotalTime = 1;
    private float mCurTime = 0;
    // 当前鼠标位置
    public Vector3 mCurPos;
    // 上一次鼠标位置
    public Vector3 mPrevPos;
    private NewItemOnDrag mNewDrag;

    void Update()
    {
        if (mIsDown)
        {
            mCurTime += Time.deltaTime * 1;
            if (mCurTime >= mTotalTime)
            {
                if (Vector3.Distance(mPrevPos, mCurPos) > mBorderDis)
                {
                    mCurTime = 0f;
                    return;
                }
                mCurTime = 0f;
                mIsDown = false;
                GameObject item = Instantiate(mNewItem);
                item.transform.SetParent(mNewItemRoot);
                item.transform.localScale = Vector3.one;
                mNewDrag = item.GetComponent<NewItemOnDrag>();
                if (mNewDrag == null)
                    mNewDrag = item.AddComponent<NewItemOnDrag>();
                item.transform.position = Input.mousePosition;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        mNewDrag = null;
        mPrevPos = Input.mousePosition;
        mCurPos = Input.mousePosition;
        mIsDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        mPrevPos = Vector3.zero;
        mCurPos = Vector3.zero;
        mIsDown = false;
        mNewDrag = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        mScrollRect.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        mCurPos = eventData.position;
        if (mNewDrag == null)
            mScrollRect.OnDrag(eventData);
        else
            mNewDrag.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        mNewDrag = null;
        mScrollRect.OnEndDrag(eventData);
    }


}