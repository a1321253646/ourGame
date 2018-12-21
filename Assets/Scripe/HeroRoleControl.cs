using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroRoleControl : MonoBehaviour {
    public bool isShow = false;
    List< PlayerBackpackBean> mHeroEquipl;
    Dictionary<long, GoodControl> mHeroGoodControl = new Dictionary<long, GoodControl>();
    Image mRoleShow;
    private Text mText, mHeroLvTx;
    private long[] mTypeAll = new long[] {1, 2, 3, 4, 5, 6};
    private Button mClose;
    private int mLevel;
    private Vector2 mFri;
    AnimalControlBase mAnimalControl;
    ResourceBean resourceData;
    GameObject mHeroEnable, mPetEnable, mHeroUnble, mPetUnble,mPet;
    Button mHeroUnbleButton, mPetUnbleButton;

    private void Start()
    {

    }
    bool isInit = false;
    public void init() {
        Hero mHero = JsonUtils.getIntance().getHeroData();
        resourceData = JsonUtils.getIntance().getEnemyResourceData(mHero.resource);
        mRoleShow = GameObject.Find("heroRole").GetComponent<Image>();
        mAnimalControl = new AnimalControlBase(resourceData, mRoleShow);
        mAnimalControl.setStatus(ActionFrameBean.ACTION_STANDY);
        mAnimalControl.start();
        mFri = gameObject.transform.localPosition;
    }

    private void Update()
    {
        if (isShow) {
            mAnimalControl.update();
        }
    }
    public void click()
    {
        if (isShow)
        {
            int level = GameManager.getIntance().getUiCurrentLevel();
            if (mLevel < level)
            {
                showUi();
                return;
            }
            else if (mLevel == level)
            {
                removeUi();
            }
        }
        else
        {
            showUi();
        }
    }

    private void showUi()
    {
        isShow = true;
        if (!isInit) {
            init();
        }
        //gameObject.transform.TransformPoint(new Vector2(0,0));
        
        gameObject.transform.localPosition = new Vector2(0, 0);
        upDateUi();
        if (mClose == null) {

            mClose = GameObject.Find("hero_info_close").GetComponent<Button>();
            mClose.onClick.AddListener(()=> {
                removeUi();
            });
        }
        if(mHeroEnable == null) {
            //mHeroEnable, mPetEnable, mHeroUnble, mPetUnble;
            mHeroEnable = GameObject.Find("hero_table_enable");
            mPetEnable = GameObject.Find("pet_table_enble");
            mHeroUnble = GameObject.Find("hero_table_unable");
            mPetUnble = GameObject.Find("pet_table_unable");
            mPet = GameObject.Find("pet");


            mPetUnbleButton = mPetUnble.GetComponent<Button>();
            mPetUnbleButton.onClick.AddListener(() =>
            {
                mPetUnble.transform.localScale = new Vector2(0, 0);
                mPetEnable.transform.localScale = new Vector2(1, 1);
                mHeroEnable.transform.localScale = new Vector2(0, 0);
                mHeroUnble.transform.localScale = new Vector2(1, 1);
                mPet.transform.localScale = new Vector2(1, 1);
                GetComponentInChildren<PetControl>().init();

            });


            mHeroUnbleButton = mHeroUnble.GetComponent<Button>();
            mHeroUnbleButton.onClick.AddListener(() =>
            {
                mPetEnable.transform.localScale = new Vector2(0, 0);
                mPetUnble.transform.localScale = new Vector2(1, 1);
                mHeroUnble.transform.localScale = new Vector2(0, 0);
                mHeroEnable.transform.localScale = new Vector2(1, 1);
                mPet.transform.localScale = new Vector2(0, 0);
            });
        }


        mLevel = GameManager.getIntance().getUiLevel();
        gameObject.transform.SetSiblingIndex(mLevel);

    }
    public void removeUi()
    {
        isShow = false;
        // gameObject.transform.TransformPoint(new Vector2(-607, -31));
        gameObject.transform.localPosition = mFri;
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
        mHeroLvTx.text = "勇士等级：Lv." + GameManager.getIntance().mHeroLv;
        mText.text =
            "攻    击：" + plya.mAttribute.aggressivity + "\n" +
            "防    御：" + plya.mAttribute.defense + "\n" +
            "生    命：" + plya.mAttribute.maxBloodVolume + "\n" +
            "闪    避：" + (plya.mAttribute.evd/100 )+"%\n"+
            "暴    击：" + (plya.mAttribute.crt / 100 )+"%\n" +
            "攻    速：" + plya.mAttribute.attackSpeed + "\n" +
            "暴击伤害：" + plya.mAttribute.crtHurt +"\n"+
            "真实伤害：" + plya.mAttribute.readHurt +"\n";
    }


}
