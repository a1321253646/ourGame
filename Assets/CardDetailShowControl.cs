﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDetailShowControl : MonoBehaviour {

    private Image mImageBottom;
    public SkillJsonBean mSkill;
    public Text mCostCount, mSkillDec, mSkillName;
    CalculatorUtil calcuator;


    public void init(long cardId, Attacker hero)
    {
        if (cardId == -1)
        {
            transform.GetChild(0).localScale = new Vector3(0, 0, 1);
            
            return;
        }
        CardJsonBean card1 = JsonUtils.getIntance().getCardInfoById(cardId);
        gameObject.transform.SetSiblingIndex(50000);
        mSkill = JsonUtils.getIntance().getSkillInfoById(card1.skill_id);
        if (mImageBottom == null)
        {
            mImageBottom = GameObject.Find("big_card_icon").GetComponent<Image>();
        }
        Sprite sprite1 = Resources.Load("icon/card/" + card1.center_resource, typeof(Sprite)) as Sprite;
        mImageBottom.sprite = sprite1;

        if (mCostCount == null)
        {
            mSkillDec = GameObject.Find("big_card_dec").GetComponent<Text>();
            mSkillName = GameObject.Find("big_card_name").GetComponent<Text>();
            mCostCount = GameObject.Find("big_card_cost").GetComponent<Text>();
        }

        mCostCount.text = card1.cost + "";
        mSkillName.text = card1.name;
        mSkillDec.text = mSkill.skill_describe;
        calcuator = new CalculatorUtil(mSkill.calculator, mSkill.effects_parameter);
        update(hero);
    }
    private void update(Attacker hero)
    {
        Debug.Log("CardUiControl update =================");

        string dec = mSkill.skill_describe;
        if (dec != null && dec.Contains("&n"))
        {
            double value = calcuator.getValue(hero, null);
            dec = dec.Replace("&n", "" + value);
            mSkillDec.text = dec;
        }
    }
}
