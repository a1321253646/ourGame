using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZhuangBeiItemShowControl : MonoBehaviour {

    GoodControl mGoodControl;
    public Button mButton;
    Text mCost;
    public long level = 0;
    public PlayerBackpackBean mBean;
    public BigNumber updateCost;

    // Use this for initialization
    void Start () {;
    }

    private void Update()
    {

    }

    public void init() {
        init(mBean);
    }

    public void init(PlayerBackpackBean bean) {
        mBean = bean;
        if (mGoodControl == null) {
            mGoodControl = GetComponentInChildren<GoodControl>();
        }
        mGoodControl.updateUi(bean.goodId,0,bean);
        foreach (PlayerAttributeBean b in bean.attributeList) {
            if (b.type == 10001) {
                level = (long)b.value;
                break;
            }
        }
        AccouterJsonBean aj = JsonUtils.getIntance().getAccouterInfoById(bean.goodId);
        BigNumber baseCost = aj.getCost();
        BigNumber cost = aj.getCost(level);
        updateCost = BigNumber.add( cost ,baseCost);
        if (mCost == null)
        {
            mCost = GetComponentsInChildren<Text>()[2];
        }
        mCost.text = "" + updateCost.toStringWithUnit();
        if (mButton == null)
        {
            mButton = GetComponentsInChildren<Button>()[1];            
        }
        if (mButton != null)
        {
            if (GameManager.getIntance().mCurrentCrystal.ieEquit( updateCost) != -1)
            {
                mButton.interactable = true;
            }
            else
            {
                mButton.interactable = false;
            }
        }
    }

}
