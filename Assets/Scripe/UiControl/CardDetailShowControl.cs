using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDetailShowControl : MonoBehaviour {

    private Image mImageBottom;
    public SkillJsonBean mSkill;
    public Text mCostCount, mSkillDec, mSkillName;
    CalculatorUtil calcuator;
    private Vector2 mfri;
    private bool isInit = false;
    private void Start()
    {
        mfri = transform.position;
    }

    public void remove() {
        isInit = false;
        transform.position = mfri;
    }
    public void init(long cardId, Attacker hero, float x,float y)
    {
        if (cardId == -1)
        {
            transform.GetChild(0).localScale = new Vector3(0, 0, 1);
            
            return;
        }
        isInit = true;
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
        calcuator = new CalculatorUtil(mSkill.calculator, mSkill.effects_parameter, card1.skill_id);
        update(hero);
        transform.position = new Vector2(x, y);
    }
    private void Update()
    {
        if (isInit && GameManager.getIntance().isEnd) {
            remove();
        }
    }
    private void update(Attacker hero)
    {
        Debug.Log("CardUiControl update =================");

        string dec = mSkill.skill_describe;
        if (dec == null || dec.Length == 0) {
            return;
        }
        if ( dec.Contains("&n"))
        {
            double value = calcuator.getValue(hero, null);
            dec = dec.Replace("&n", "" + value);
        }
        for(int i = 0;i< mSkill.getSpecialParameterValue().Count; i++)
        {
            string str = "S" + (i + 1);
            if (dec.Contains(str)) {
                dec = dec.Replace(str, mSkill.getSpecialParameterValue()[i]+"");
            }
        }
        mSkillDec.text = dec;
    }
}
