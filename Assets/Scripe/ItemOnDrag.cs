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
    public PlayerBackpackBean mBean;
    // 是否按下
    public bool mIsDown = false;
    public bool mIsOndragDown = false;
    // 按下与松开鼠标之间的距离
    public float mBorderDis = 0.5f;
    // 总的按下时间
    private float mCurTime = 0;
    // 当前鼠标位置
    public Vector3 mCurPos;
    // 上一次鼠标位置
    public Vector3 mPrevPos;
    private NewItemOnDrag mNewDrag;
    GameObject mIndicator;
    CardUiControl mUiContorl;
    CardUiControl mRootContorl;
    void Update()
    {
        if (!isInit) {
            return;
        }
        if (mIsDown)
        {
            mCurTime += Time.deltaTime * 1;
            if (mCurTime >= JsonUtils.getIntance().getConfigValueForId(100020)/1000)
            {
                if (Vector3.Distance(mPrevPos, mCurPos) > mBorderDis)
                {
                    mCurTime = 0f;
                    return;
                }
                mIsOndragDown = true;
                GameObject ob = GameObject.Find("kapai_click");
                ob.GetComponent<CardDetailShowControl>().init(mCard.id, mManager.getHero(), mCurPos.x, mCurPos.y);
                long result = GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_OBJECT_CLICK, mCard.id);
                mCurTime = 0f;
                mIsDown = false;
                if (result == 1) {
                    GameManager.getIntance().getGuideManager().ShowGuideNormalObject(mIndicator);
                }
                if (mRootContorl.count == 1) {
                    mRootContorl.showBack();
                }
                
                GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_SHOW, GuideManager.SHOW_CARD_BACK_INFO);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        mNewDrag = null;
        mPrevPos = Input.mousePosition;
        mCurPos = Input.mousePosition;
        mIsDown = true;
        mCurTime = 0;
    }
    private bool isMove = false;
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("=================================eventData x = " + eventData.position.x + " eventData y =" + eventData.position.y);
        if (mIndicator != null) {
            Destroy(mIndicator);
         //   
        }
        GameObject.Find("kapai_click").GetComponent<CardDetailShowControl>().remove();
        mScrollRect.OnEndDrag(eventData);
        bool isWork = false;
        mIsDown = false;
        mIsOndragDown = false;
        mPrevPos = Vector3.zero;
        mCurPos = Vector3.zero;
        if (!isInit) {
            return;
        }
        if (mIndicator != null) {
            if (isUser)
            {
                if (eventData.position.y > Screen.height / 2 + 30)
                {
                    //  if (BackpackManager.getIntance().use(newBean, 1, TipControl.USE_CARD_TYPE))
                    //   {
                    BackpackManager.getIntance().use(mBean, 1, TipControl.USE_CARD_TYPE);
                    Destroy(mIndicator);
                    //  Destroy(gameObject);

                    CardUiControl ui = mIndicator.GetComponent<CardUiControl>();
                    ui.init(-1, 0, null);
                    isWork = true;
                    //   }
                }
            }
            else
            {
                if (eventData.position.y < Screen.height / 2)
                {
                    BackpackManager.getIntance().use(mBean, 1, TipControl.UNUSE_CARD_TYPE);
                    Destroy(mIndicator);
                    // Destroy(gameObject);
                    CardUiControl ui = mIndicator.GetComponent<CardUiControl>();
                    ui.init(-1, 0, null);
                    BackpackManager.getIntance().updateCardBackShow();
                    isWork = true;
                }
            }
        }

        if (isWork)
        {          
            mNewDrag = null;
            mIndicator = null;
            mUiContorl = null;
            isMove = false;
            mRootContorl = null;
            isInit = false;
            GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_ONDRAG_UP, GuideManager.ONDRAG_UP_CARDUI_WORK);
        }
        else if(mRootContorl != null)
        {
            isMove = false;
            GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_ONDRAG_UP, GuideManager.ONDRAG_UP_CARDUI_UNWORK);
            mRootContorl.showCard();
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        mScrollRect.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {

        mCurPos = eventData.position;
        if (!mIsOndragDown) {
            mScrollRect.OnDrag(eventData);
        }
        else
        {            
            if (!isMove)
            {

               // GameObject.Find("kapai_click").transform.position = mClickCardV;
                GameObject.Find("kapai_click").GetComponent<CardDetailShowControl>().remove();
                mIndicator = Instantiate(mNewItem);
                mIndicator.transform.SetParent(mNewItemRoot);
                mIndicator.transform.localScale = Vector3.one;
                mNewDrag = mIndicator.GetComponent<NewItemOnDrag>();
                if (mNewDrag == null)
                    mNewDrag = mIndicator.AddComponent<NewItemOnDrag>();
                mIndicator.transform.position = Input.mousePosition;
                mUiContorl = mIndicator.GetComponent<CardUiControl>();
                mUiContorl.init(mCard.id, CardUiControl.TYPE_CARD_PLAY, mManager.getHero());
                mUiContorl.init(mCard.id, 113f, 166f);
                if (mRootContorl.count == 1)
                {
                    mRootContorl.showBack();
                }
                isMove = true;
            }
            else {
                mNewDrag.transform.position = Input.mousePosition;
            }
        }
          
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        mNewDrag = null;
        mIsOndragDown = false;
        mScrollRect.OnEndDrag(eventData);
    }
    private CardManager mManager;
    private bool isUser;
    private bool isInit = false;
    private CardJsonBean mCard;
    private SkillJsonBean mSkill;
    public void init(ScrollRect scrollRect) {
        mScrollRect = scrollRect;
    }

    public void init(CardManager manage, long cardId, bool isUser, GameObject newItem, Transform newItemRoot, ScrollRect scrollRect)
    {
        mNewItem = newItem;
        mNewItemRoot = newItemRoot;
        mScrollRect = scrollRect;
        this.isUser = isUser;
        Debug.Log("init card id =" + cardId);
        mManager = manage;

        EventTrigger tri = gameObject.AddComponent<EventTrigger>();
        if (cardId == -1)
        {       
             isInit = false;
            //isInit = true;
            return;
        }
        isInit = true;
        //添加事件触发记录到GameObject的事件触发组件  
        CardJsonBean card = JsonUtils.getIntance().getCardInfoById(cardId);
        mCard = card;
        mSkill = JsonUtils.getIntance().getSkillInfoById(mCard.skill_id);

        mRootContorl = gameObject.GetComponent<CardUiControl>();
    }
}