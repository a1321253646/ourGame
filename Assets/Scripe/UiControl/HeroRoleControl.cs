using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroRoleControl : UiControlBase
{
    
    List< PlayerBackpackBean> mHeroEquipl;
    Dictionary<long, GoodControl> mHeroGoodControl = new Dictionary<long, GoodControl>();
    Image mRoleShow;
    private Text mText, mHeroLvTx;
    private long[] mTypeAll = new long[] {1, 2, 3, 4, 5, 6};
    private Button mClose;
    private int mLevel;
    AnimalControlBase mAnimalControl;
    ResourceBean resourceData;
    GameObject mHeroEnable, mPetEnable, mHeroUnble, mPetUnble,mPet/*,mPetTable,*/,mPetTablePoint;
    Button mHeroUnbleButton, mPetUnbleButton;
    bool isShowPet = false;


    private void Update()
    {
        if (isShow) {
            mAnimalControl.update();
        }
    }

    public void showPetTable() {
//        mPetTable.transform.localScale = new Vector2(1, 1);
//        mPetTablePoint.transform.localScale = new Vector2(1, 1);
        SQLHelper.getIntance().updatePointPetTable(1);
    }

    public void upDateUi()
    {
        upDateUi(true);
    }

    public void upDateUi(bool must)
    {
        if (!must && mLevel < GameManager.getIntance().getUiCurrentLevel()) {
            return;
        }

        mHeroEquipl = BackpackManager.getIntance().getHeroEquipInfo();
        GoodControl goodIcon = null;
        PlayerBackpackBean bean = null;
        string name = null;
        for (int i = 1; i < 7; i++) {
            name = "equip_gride_" + (i);
           
            goodIcon = GameObject.Find(name).GetComponent<GoodControl>();
            if (i - 1 < mHeroEquipl.Count)
            {
                PlayerBackpackBean keyValue = mHeroEquipl[i - 1];
                goodIcon.updateUi(keyValue.goodId,0, keyValue);
            }
            else {
                goodIcon.updateUi(-1, 0, null);
            }
            
        }
        if (mText == null) {
            mText = GameObject.Find("hero_information").GetComponent<Text>();
        }
        if (mHeroLvTx == null) {
            mHeroLvTx = GameObject.Find("hero_lv_tx").GetComponent<Text>();
        }
        PlayControl plya = BackpackManager.getIntance().getHero();
        mHeroLvTx.text = "勇士等级：Lv." + BaseDateHelper.decodeLong(GameManager.getIntance().mHeroLv);
        mText.text =
            "攻    击：" + StringUtils.doubleToStringShow(plya.mAttribute.aggressivity) + "\n" +
            "防    御：" + StringUtils.doubleToStringShow(plya.mAttribute.defense) + "\n" +
            "生    命：" + StringUtils.doubleToStringShow(plya.mAttribute.maxBloodVolume) + "\n" +
            "闪    避：" + (plya.mAttribute.evd/100 )+"%\n"+
            "暴    击：" + (plya.mAttribute.crt / 100 )+"%\n" +
            "攻    速：" + StringUtils.doubleToStringShow(plya.mAttribute.attackSpeed) + "\n" +
            "暴击伤害：" + StringUtils.doubleToStringShow(plya.mAttribute.crtHurt) +"\n"+
            "真实伤害：" + StringUtils.doubleToStringShow(plya.mAttribute.readHurt) +"\n";
    }

    public override void init()
    {
        mControlType = UiControlManager.TYPE_HERO;
        //mHeroEnable, mPetEnable, mHeroUnble, mPetUnble;
        mHeroEnable = GameObject.Find("hero_table_enable");
        mPetEnable = GameObject.Find("pet_table_enble");
        mHeroUnble = GameObject.Find("hero_table_unable");
        mPetUnble = GameObject.Find("pet_table_unable");
        mPet = GameObject.Find("pet");
        //  mPetTable = GameObject.Find("pet_table");
        mPetTablePoint = GameObject.Find("pet_table_point");
        mClose = GameObject.Find("hero_info_close").GetComponent<Button>();
        mClose.onClick.AddListener(() =>
        {
            Debug.Log("HeroRoleControl  hero_info_close");
            toremoveUi();
        });

        mPetUnbleButton = mPetUnble.GetComponent<Button>();
        mPetUnbleButton.onClick.AddListener(() =>
        {
            mPetUnble.transform.localScale = new Vector2(0, 0);
            mPetEnable.transform.localScale = new Vector2(1, 1);
            mHeroEnable.transform.localScale = new Vector2(0, 0);
            mHeroUnble.transform.localScale = new Vector2(1, 1);
            mPet.transform.localScale = new Vector2(1, 1);
            mPet.GetComponent<PetControl>().toShowUi();
            SQLHelper.getIntance().updatePointPetTable(0);
            mPetTablePoint.transform.localScale = new Vector2(0, 0);
            isShowPet = true;

        });


        mHeroUnbleButton = mHeroUnble.GetComponent<Button>();
        mHeroUnbleButton.onClick.AddListener(() =>
        {
            mPetEnable.transform.localScale = new Vector2(0, 0);
            mPetUnble.transform.localScale = new Vector2(1, 1);
            mHeroUnble.transform.localScale = new Vector2(0, 0);
            mHeroEnable.transform.localScale = new Vector2(1, 1);
            mPet.GetComponent<PetControl>().toremoveUi();
            isShowPet = false;
        });
    }
    public  void vocation() {
        resourceData = null;
        if (isShow) {
            show();
        }
    }


    public override void show()
    {
        isShow = true;

        if (resourceData == null) {
            Hero mHero = JsonUtils.getIntance().getHeroData();
            long vocation = SQLHelper.getIntance().mPlayVocation;
            if (vocation == -1) {
                vocation = 1;
            }
            resourceData = JsonUtils.getIntance().getEnemyResourceData(
                JsonUtils.getIntance().getVocationById(vocation).resource);
            mRoleShow = GameObject.Find("heroRole").GetComponent<Image>();
            mAnimalControl = new AnimalControlBase(resourceData, mRoleShow);
            mAnimalControl.setStatus(ActionFrameBean.ACTION_STANDY);
            mAnimalControl.start();
        }
        //gameObject.transform.TransformPoint(new Vector2(0,0));

        gameObject.transform.localPosition = new Vector2(0, 0);
        upDateUi();
        List<PlayerBackpackBean> list = InventoryHalper.getIntance().getPet();
        mPetEnable.transform.localScale = new Vector2(0, 0);
        mPetUnble.transform.localScale = new Vector2(1, 1);
        mHeroUnble.transform.localScale = new Vector2(0, 0);
        mHeroEnable.transform.localScale = new Vector2(1, 1);
        if (SQLHelper.getIntance().isShowPetTablePoint == 1)
        {
            mPetTablePoint.transform.localScale = new Vector2(1, 1);
        }
        else
        {
            mPetTablePoint.transform.localScale = new Vector2(0, 0);
        }
    }
}
