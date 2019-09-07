using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDoubleUiControl : MonoBehaviour
{
    public CardUiControl control1;
    public CardUiControl control2;

    public void init(LevelManager mLevelManager) {

        control1.gameObject.AddComponent<ItemOnDrag>();
        control1.gameObject.GetComponent<ItemOnDrag>().init();
        control1.init(-1, 107, 146);
        control1.init(-1, CardUiControl.TYPE_CARD_PLAY, mLevelManager.mPlayerControl);

        control2.gameObject.AddComponent<ItemOnDrag>();
        control2.gameObject.GetComponent<ItemOnDrag>().init();
        control2.init(-1, 107, 146);
        control2.init(-1, CardUiControl.TYPE_CARD_PLAY, mLevelManager.mPlayerControl);
    }



    public GameObject getCardObjectById(long id) {
        if (control1.mCardId == id)
        {
            return control1.gameObject;
        }
        else if (control2.mCardId == id)
        {
            return control2.gameObject;
        }
        else {
            return null;
        }
    }


    public bool init(PlayerBackpackBean bean,float x,float y, LevelManager level, int type, Attacker hero, Transform root) {
        if (control1.mCardId == -1)
        {
            control1.init(bean.goodId, 107, 146);
            control1.init(bean.goodId, CardUiControl.TYPE_CARD_PLAY, level.mPlayerControl);
            ItemOnDrag item = control1.gameObject.GetComponent<ItemOnDrag>();
            item.mBean = bean;
            item.init(level.mPlayerControl.mCardManager, bean.goodId, true, level.mPlayerControl.mCardManager.card, root);
            control1.init(bean.goodId, type, hero);
        }
        else if (control1.mCardId == bean.goodId)
        {
            control1.addCount();
        }
        else if (control2.mCardId == -1)
        {
            control2.init(bean.goodId, 107, 146);
            control2.init(bean.goodId, CardUiControl.TYPE_CARD_PLAY, level.mPlayerControl);
            ItemOnDrag item = control2.gameObject.GetComponent<ItemOnDrag>();
            item.mBean = bean;
            item.init(level.mPlayerControl.mCardManager, bean.goodId, true, level.mPlayerControl.mCardManager.card, root);
            control2.init(bean.goodId, type, hero);
        } else if (control2.mCardId == bean.goodId) {
            control2.addCount();
        }
        else {
            return false;
        }
        return true;
    }

    public bool isHad(long cardId) {
        if (control1.mCardId == cardId)
        {
            control1.addCount();
        }
        else if (control2.mCardId == cardId)
        {
            control2.addCount();
        }
        else
        {
            return false;
        }
        return true;
    }
}
