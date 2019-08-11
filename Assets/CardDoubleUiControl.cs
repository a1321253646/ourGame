using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDoubleUiControl : MonoBehaviour
{
    public CardUiControl control1;
    public CardUiControl control2;

    public bool init(long cardId, int type, Attacker hero) {
        if (control1.mCardId == -1)
        {
            control1.init(cardId, type, hero);
        }
        else if (control2.mCardId == -1)
        {
            control2.init(cardId, type, hero);
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
