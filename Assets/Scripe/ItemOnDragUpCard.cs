using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// 脚本挂载到每个可拖拽的Item上面即可
/// </summary>
public class ItemOnDragUpCard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler { 

    float myBili = 0;
    private CardManagerBase mManager;
    long mCardId = -1;
    bool isDown = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (mCardId == -1 || isDown) {
            return;
        }
        isDown = true;
        GameObject ob = GameObject.Find("kapai_click");
        float x = 0;
        if (eventData.position.x < Screen.width / 2)
        {
            x = eventData.position.x + ob.GetComponent<RectTransform>().rect.width / 2 * myBili;
        }
        else
        {
            x = eventData.position.x - ob.GetComponent<RectTransform>().rect.width / 2 * myBili;
        }
        float y = eventData.position.y;
        if (eventData.position.y < ob.GetComponent<RectTransform>().rect.height / 2)
        {
            y = ob.GetComponent<RectTransform>().rect.height / 2 * myBili;
        }
        else if (eventData.position.y > Screen.height - ob.GetComponent<RectTransform>().rect.height / 2 * myBili)
        {
            y = Screen.height - ob.GetComponent<RectTransform>().rect.height / 2 * myBili;
        }

        Debug.Log(" ===========================eventData.position.y =" + eventData.position.y+ " Screen.height - ob.GetComponent<RectTransform>().rect.height / 2 * myBili=" + (Screen.height - ob.GetComponent<RectTransform>().rect.height / 2 * myBili));

        ob.GetComponent<CardDetailShowControl>().init(mCardId,GameObject.Find("Manager").GetComponent<LevelManager>().mPlayerControl.mCardManager.getAttacker(), x, y);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isDown = false;
        GameObject.Find("kapai_click").GetComponent<CardDetailShowControl>().remove();
    }


    public void init( long cardId)
    {
        Debug.Log("init card id =" + cardId);
        mCardId = cardId;

        float x = GameObject.Find("Canvas").GetComponent<CanvasScaler>().referenceResolution.x;
        float y = GameObject.Find("Canvas").GetComponent<CanvasScaler>().referenceResolution.y;
        myBili = Screen.height / y;
    }


}