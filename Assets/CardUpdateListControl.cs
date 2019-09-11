using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUpdateListControl : MonoBehaviour
{
    public Text mCardMoeny;
    public CardViewUpdateControl mListControl;

    public bool isInit = false;
    public void show()
    {
        if (!isInit) {
            init();
        }
        gameObject.transform.localScale = new Vector2(1, 1);
        upDateUi(-1);
    }
    public void disShow()
    {
        gameObject.transform.localScale = new Vector2(0, 0);
    }
    public void upDateUi(long id) 
    {

        Debug.Log("================================================ add card =" + id);
        mCardMoeny.text = BigNumber.getBigNumForString("" + SQLHelper.getIntance().mCardMoney).toStringWithUnit();
        mListControl.updateItem(id);

    }
    public void init() {
        if (isInit) {
            return;
        }
        isInit = true;
        mListControl.init();
    }
    public void closed()
    {
        UiControlManager.getIntance().remove(UiControlManager.TYPE_CARD);
    }

}
