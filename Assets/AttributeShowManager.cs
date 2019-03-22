using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeShowManager : MonoBehaviour {
    AttributeItemControl[] mList;
    public void showEnemy(Attacker a) {
        if (mList == null) {
            mList = GetComponentsInChildren<AttributeItemControl>();
        }
        int index = 0;
        if (a.mAttackType == Attacker.ATTACK_TYPE_HERO)
        {
            index = 0;
        }else if (a.mAttackType == Attacker.ATTACK_TYPE_BOSS)
        {
            index = 1;
        }
        else {
            index = 2;
        }
        if (index == 0)
        {
            mList[0].initAttribute(a);
        }
        else if (index == 1) {
            mList[1].initAttribute(a);
        }
        else {
            for (; index < mList.Length; index++) {
                if (mList[index].initAttribute(a)) {
                    break;
                }
            }
        }
    }
}
