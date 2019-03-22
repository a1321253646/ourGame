using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributeItemControl : MonoBehaviour {
    private Text mtext;
    private Attacker mAttacker;
    public bool initAttribute(Attacker attacker) {
        if (attacker.mAttackType == Attacker.ATTACK_TYPE_HERO)
        {
            transform.localScale = new Vector2(1, 1);
            mAttacker = attacker;
            return true;
        }
        else if(attacker.mAttackType == Attacker.ATTACK_TYPE_BOSS) {
            transform.localScale = new Vector2(1, 1);
            mAttacker = attacker;
            return true;
        }else if (mAttacker == null)
        {
            transform.localScale = new Vector2(1, 1);
            mAttacker = attacker;
            return true;
        }
        else {
            return false;
        }
    }

    private void Update()
    {
        if (mtext == null) {
            mtext = GetComponentInChildren<Text>();
        }
        if (mAttacker == null) {
            return;
        }
        if (mAttacker.getStatus() == Attacker.PLAY_STATUS_DIE) {
            mAttacker = null;
            mtext.text = "";
            transform.localScale = new Vector2(0, 0);
            return;
        }
        string str = "";
        if (mAttacker.mAttackType == Attacker.ATTACK_TYPE_BOSS) {
            str +=  "Boss\n" ;
        }
        else if (mAttacker.mAttackType == Attacker.ATTACK_TYPE_HERO)
        {
            str += "英雄\n";
        }
        else if (mAttacker.mAttackType == Attacker.ATTACK_TYPE_BOSS)
        {
            str += "小怪\n";
        }
        str += ("攻击:" + mAttacker.mAllAttributePre.getAll().aggressivity +"\n");
        str += ("防御:" + mAttacker.mAllAttributePre.getAll().defense + "\n");
        str += ("血量:" + mAttacker.mAllAttributePre.getAll().maxBloodVolume + "\n");
        str += ("闪避:" + (mAttacker.mAllAttribute.evd/10000) + "\n");
        str += ("暴击 :" + (mAttacker.mAllAttribute.crt/10000) + "\n");
        str += ("暴伤:" + mAttacker.mAllAttributePre.getAll().crtHurt + "\n");
        str += ("真伤:" + mAttacker.mAllAttributePre.getAll().readHurt + "\n");
        str += ("攻速:" + mAttacker.mAllAttributePre.getAll().attackSpeed);
        mtext.text = str;
    }
}
