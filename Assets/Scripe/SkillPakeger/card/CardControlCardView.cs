using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class CardControlCardView : MonoBehaviour
{

    public Vector3 mCurPos;
    CardUiControl mRootContorl;
    public GameObject mNewItem;
    private CardManagerBase mManager;
    CardJsonBean mCard = null;
    GameObject mIndicator;
    float myBili = 0;
    public Transform mNewItemRoot;
    CardUiControl mUiContorl;
    bool isUser = false;
    long mCardId = -1;
    public PlayerBackpackBean mBean;


    public void OnpointUp(BaseEventData eventData) {
        if (mCardId == -1)
        {
            return;
        }
        

        if (mIndicator != null)
        {
            Destroy(mIndicator);
            //   
        }
        GameObject.Find("kapai_click").GetComponent<CardDetailShowControl>().remove();
        bool isWork = false;
        if (mIndicator != null)
        {
            if (isUser)
            {
                if (Input.mousePosition.y > Screen.height / 2 + 30 && BackpackManager.getIntance().use(mBean, 1, TipControl.USE_CARD_TYPE))
                {
                    //  if (BackpackManager.getIntance().use(newBean, 1, TipControl.USE_CARD_TYPE))
                    //   {

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
                if (Input.mousePosition.y < Screen.height / 2)
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
            mIndicator = null;
            mUiContorl = null;
            mRootContorl = null;
            GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_ONDRAG_UP, GuideManager.ONDRAG_UP_CARDUI_WORK);
        }
        else if (mRootContorl != null)
        {
            GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_ONDRAG_UP, GuideManager.ONDRAG_UP_CARDUI_UNWORK);
            mRootContorl.showCard();
        }

    }

    private void Start()
    {
        float y = GameObject.Find("Canvas").GetComponent<CanvasScaler>().referenceResolution.y;
        myBili = Screen.height / y;

        EventTrigger tri = gameObject.AddComponent<EventTrigger>();
        tri.triggers = new List<EventTrigger.Entry>();
        EventTrigger.Entry entry1 = new EventTrigger.Entry();
        entry1.eventID = EventTriggerType.PointerDown;
        UnityAction<BaseEventData> callback1 = new UnityAction<BaseEventData>(OnpointDown);
        entry1.callback.AddListener(callback1);
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.Drag;
        UnityAction<BaseEventData> callback2 = new UnityAction<BaseEventData>(OnDrag);
        entry2.callback.AddListener(callback2);
        EventTrigger.Entry entry3 = new EventTrigger.Entry();
        entry3.eventID = EventTriggerType.PointerUp;
        UnityAction<BaseEventData> callback3 = new UnityAction<BaseEventData>(OnpointUp);
        entry3.callback.AddListener(callback3);

        tri.triggers.Add(entry1);
        tri.triggers.Add(entry2);
        tri.triggers.Add(entry3);
    }


    void OnpointDown(BaseEventData  date)
    {
        if (mCardId == -1)
        {
            return;
        }
        mIndicator = null;
        mCurPos = Input.mousePosition;

        GameObject ob = GameObject.Find("kapai_click");
        float x = 0;
        if (mCurPos.x < Screen.width / 2)
        {
            x = mCurPos.x + ob.GetComponent<RectTransform>().rect.width / 2 * myBili;
        }
        else
        {
            x = mCurPos.x - ob.GetComponent<RectTransform>().rect.width / 2 * myBili;
        }
        ob.GetComponent<CardDetailShowControl>().init(mCard.id, mManager.getAttacker(), x, mCurPos.y);
        long result = GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_OBJECT_CLICK, mCard.id);;
        if (result == 1)
        {
           // GameManager.getIntance().getGuideManager().ShowGuideNormalObject(mIndicator);
        }
        if (mRootContorl.count == 1)
        {
            //mRootContorl.showBack();
        }
        GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_SHOW, GuideManager.SHOW_CARD_BACK_INFO);

    }
    public void OnDrag(BaseEventData date)
    {
        if (mCardId == -1) {
            return;
        }
        if (mIndicator == null) {
            GameObject.Find("kapai_click").GetComponent<CardDetailShowControl>().remove();
            mIndicator  = Instantiate(mNewItem);
            mIndicator.transform.SetParent(mNewItemRoot);
            mIndicator.transform.localScale = Vector3.one;
            mUiContorl = mIndicator.GetComponent<CardUiControl>();
            mUiContorl.init(mCard.id, CardUiControl.TYPE_CARD_PLAY, mManager.getAttacker());
            mUiContorl.init(mCard.id, 113f, 166f);
            if (mRootContorl.count == 1)
            {
                mRootContorl.showBack();
            }
        }
        mIndicator.transform.position = Input.mousePosition;
    }

    public void init(CardManagerBase manage, long cardId, bool isUser, GameObject newItem, Transform newItemRoot)
    {
        mNewItem = newItem;
        mNewItemRoot = newItemRoot;

        Debug.Log("init card id =" + cardId);
        mManager = manage;
        this.isUser = isUser;
        EventTrigger tri = gameObject.AddComponent<EventTrigger>();
        //添加事件触发记录到GameObject的事件触发组件  
        CardJsonBean card = JsonUtils.getIntance().getCardInfoById(cardId);
        mCard = card;
        mRootContorl = gameObject.GetComponent<CardUiControl>();
        float x = GameObject.Find("Canvas").GetComponent<CanvasScaler>().referenceResolution.x;
        float y = GameObject.Find("Canvas").GetComponent<CanvasScaler>().referenceResolution.y;
        myBili = Screen.height / y;
    }
}
