using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetControl : UiControlBase
{

    public GameObject mPetIconItem;

    GameObject mListView;
    VerticalLayoutGroup mVerticall;
    Text mName, mGetLevel, mDec, mAffix, mSkillDec,mRestText;
    Image mSkillIcon,mPetShowImg;
    Button mClose,mFightButton,mRestButton;
    GameObject mFightStatu, mRestStatu,mPetShowCenter;
	
	// Update is called once per frame
	void Update () {
        if (mAnimalControl != null) {
            mAnimalControl.update();
        }
	}
    private int mLevel = 0;
    public bool isInTop() {
        int level = GameManager.getIntance().getUiCurrentLevel();
        if (mLevel < level)
        {
            
            return false;
        }
        else if (mLevel == level)
        {
            return true;
        }
        return false;
    }

    PetManager mPetManage;
    private List<PetIconShowControl> mControlList = new List<PetIconShowControl>();

    public void addPet(PlayerBackpackBean bean) {
        for (int i = 0; i < mControlList.Count; i++) {
            if (mControlList[i].mId == bean.goodId) {
                mControlList[i].init(bean, this);
            }
        }
    }

    public void changePetStatue(long status) {

        
        if (status == SQLDate.GOOD_TYPE_PET)
        {
            mPetManage.petReset(mClickIcon.mId);
            mFightStatu.transform.localScale = new Vector2(1, 1);
            mRestStatu.transform.localScale = new Vector2(0, 0);
        }
        else if(status == SQLDate.GOOD_TYPE_USER_PET){
            if (!mPetManage.petFight(mClickIcon.mId)) {
                //TODO 进行提示
                return;
            }
            mFightStatu.transform.localScale = new Vector2(0, 0);
            mRestStatu.transform.localScale = new Vector2(1, 1);
        }
        mClickIcon.mBean.goodType = status;
        InventoryHalper.getIntance().changePetStatus(mClickIcon.mBean);
        mClickIcon.init(mClickIcon.mBean, this);

        SkillJsonBean skill = JsonUtils.getIntance().getSkillInfoById(mClickIcon.mJsonBean.skillId);
        string tmp = "\n";
        if (mClickIcon.mBean.goodType == SQLDate.GOOD_TYPE_PET)
        {
            tmp = "<color=#cb3500>(出战激活)</color>\n";
        }
        string s = mClickIcon.mJsonBean.skil_name + tmp + skill.skill_describe;
        List<float> flist = skill.getSpecialParameterValue();
        if (flist != null && flist.Count > 0)
        {
            for (int i = 0; i < flist.Count; i++)
            {
                s=s.Replace("S" + (i + 1) , "" + flist[i]);
            }
        }
        mSkillDec.text = s;
     //   mSkillDec.text = mClickIcon.mJsonBean.skil_name + tmp + skill.skill_describe;
    } 


    public void update(PlayerBackpackBean bean) {
        foreach (PetIconShowControl control in mControlList) {
            if (control.mId == bean.goodId) {
                control.init(bean,this);
            }
        }
    }

    PetIconShowControl mClickIcon;
    public void onIconClick(PetIconShowControl icon) {
        if (mClickIcon != null && mClickIcon == icon) {
            return;
        }      
        if (mClickIcon != null) {
            mClickIcon.click(false);
        }
        icon.click(true);
        mClickIcon = icon;
        click();
    }
    AnimalControlBase mAnimalControl;
    public void click() {
        mName.text = mClickIcon.mJsonBean.name;
        mDec.text = mClickIcon.mJsonBean.des;
        
        if (mClickIcon.mBean == null)
        {
            mGetLevel.text = "通关" + mClickIcon.mJsonBean.activateLevel + "关拯救精灵";
            mSkillDec.text = "????????????\n????????????\n????????????\n????????????";
            mAffix.text = "???????\n???????\n???????\n???????";
            mSkillIcon.sprite = Resources.Load("icon/pet/" + mClickIcon.mJsonBean.skillNoactivateIcon, typeof(Sprite)) as Sprite;
            mFightStatu.transform.localScale = new Vector2(0, 0);
            mRestStatu.transform.localScale = new Vector2(1, 1);
            mRestText.text = "未拥有";
            mRestButton.interactable = false;
            mAnimalControl = null;
            mPetShowImg.sprite = Resources.Load("icon/pet/" + mClickIcon.mJsonBean.moactivateImage, typeof(Sprite)) as Sprite;
            mPetShowImg.SetNativeSize();
        }
        else {
            //if (mAnimalControl == null) {
           // transform.Translate(Vector2.down * y);
            ResourceBean res = JsonUtils.getIntance().getEnemyResourceData(mClickIcon.mJsonBean.resouce);
            
            //mPetShowImg.transform.Translate(Vector2.up * res.idel_y * 2);
            //mPetShowImg.transform.Translate(Vector2.left * res.getHurtOffset().x  * 2);
           // 
            
            mAnimalControl = new AnimalControlBase(res, mPetShowImg, true);
            mAnimalControl.setStatus(ActionFrameBean.ACTION_STANDY);
            mAnimalControl.start();
            Vector3 v = PointUtils.getScreenSize(new Vector3(res.getHurtOffset().x * 2, res.idel_y * 2));
            mPetShowImg.SetNativeSize();
            mPetShowImg.gameObject.transform.position = new Vector2(mPetShowCenter.transform.position.x - v.x, mPetShowCenter.transform.position.y - v.y);
            //  }


            mGetLevel.text = "";
            List<PlayerAttributeBean>  list = mClickIcon.mJsonBean.getAffixList();

            string affixstr = null;
            foreach (PlayerAttributeBean b in list)
            {
                if (affixstr == null) {
                    affixstr = "";
                }
                else {
                    affixstr = affixstr + "\n";
                }
                if (b.type < 1000)
                {
                    string vale = StringUtils.doubleToStringShow(b.value);
                    string dec = b.getTypeStr();
                    affixstr = affixstr + dec + ":" + vale ;

                }
                else {

                    float vale = (float)b.value / 100;
                    AffixJsonBean a = JsonUtils.getIntance().getAffixInfoById(b.type);
                    affixstr = affixstr + a.dec + ":" + vale + "%";
                }

            }
            mAffix.text = affixstr;

            SkillJsonBean skill= JsonUtils.getIntance().getSkillInfoById(mClickIcon.mJsonBean.skillId);
            string tmp = "\n";
            if (mClickIcon.mBean.goodType == SQLDate.GOOD_TYPE_PET) {
                tmp = "<color=#cb3500>(出战激活)</color>\n";
            }
            Debug.Log("skil_name = " + mClickIcon.mJsonBean.skil_name);
            Debug.Log("skill_describe = " + skill.skill_describe);
            string s= mClickIcon.mJsonBean.skil_name + tmp + skill.skill_describe;
            List<float>  flist = skill.getSpecialParameterValue();
            if (flist != null && flist.Count > 0) {
                for(int i = 0; i < flist.Count; i++) {
                   s= s.Replace("S" + (i + 1) , "" + flist[i]);
                }
            }
            mSkillDec.text = s;
            mSkillIcon.sprite = Resources.Load("icon/pet/" + mClickIcon.mJsonBean.skillActivateIcon, typeof(Sprite)) as Sprite;
            if ( mPetManage.isFull()  && mClickIcon.mBean.goodType != SQLDate.GOOD_TYPE_USER_PET)
            {
                mFightStatu.transform.localScale = new Vector2(0, 0);
                mRestStatu.transform.localScale = new Vector2(1, 1);
                mRestText.text = "出战已满";
                mRestButton.interactable = false;
            }
            else if (mClickIcon.mBean.goodType == SQLDate.GOOD_TYPE_PET)
            {
                mFightStatu.transform.localScale = new Vector2(1, 1);
                mRestStatu.transform.localScale = new Vector2(0, 0);
            }
            else {
                mRestStatu.transform.localScale = new Vector2(1, 1);
                mRestText.text = "休息";
                mRestButton.interactable = true;
                mFightStatu.transform.localScale = new Vector2(0, 0);
            }
            
        }
    }
    float mGridHeight;
    private void SetGridHeight()
    {
        //if (mItems.Count > 3) {
        int count =  mControlList.Count;
        
        float height = (mVerticall.spacing + 83) * count;
        mListView.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        if (mGridHeight != height)
        {
            mGridHeight = height;
            mVerticall.transform.Translate(Vector2.down * (height));
        }
        //}
    }

    public override void init()
    {
        mControlType = UiControlManager.TYPE_PET;

        mListView = GameObject.Find("pet_list");
        mName = GameObject.Find("pet_name").GetComponent<Text>();
        mGetLevel = GameObject.Find("pet_get_level").GetComponent<Text>();
        mDec = GameObject.Find("pet_dec_text").GetComponent<Text>();
        mAffix = GameObject.Find("pet_affix").GetComponent<Text>();
        mSkillDec = GameObject.Find("pet_skill_dec").GetComponent<Text>();
        mSkillIcon = GameObject.Find("pet_skill_icon").GetComponent<Image>();
        mClose = GameObject.Find("pet_info_close").GetComponent<Button>();

        mFightStatu = GameObject.Find("pet_fight");
        mRestStatu = GameObject.Find("pet_rest");
        mFightButton = mFightStatu.GetComponent<Button>();
        mRestButton = mRestStatu.GetComponent<Button>();
        mRestText = GameObject.Find("pet_rest_text").GetComponent<Text>();
        mPetShowCenter = GameObject.Find("pet_show_center");

        mPetShowImg = GameObject.Find("pet_show_img").GetComponent<Image>();
        mPetManage = GameObject.Find("Manager").GetComponent<PetManager>();
        mFightButton.onClick.AddListener(() =>
        {

            changePetStatue(SQLDate.GOOD_TYPE_USER_PET);
        });
        mRestButton.onClick.AddListener(() =>
        {
            changePetStatue(SQLDate.GOOD_TYPE_PET);
        });
        mClose.onClick.AddListener(() =>
        {
            GameObject.Find("hero").GetComponent<HeroRoleControl>().toremoveUi();
            toremoveUi();
        });
        mVerticall = mListView.GetComponent<VerticalLayoutGroup>();

    }

    public override void show()
    {
        if(mControlList.Count == 0) {
            PlayerBackpackBean play;
            List<PetJsonBean> list = JsonUtils.getIntance().getPet();
            List<PlayerBackpackBean> petList = InventoryHalper.getIntance().getPet();
            int count = list.Count;
            Debug.Log("PetControl count=" + list.Count);
            PetJsonBean bean;
            for (int i = 0; i < count; i++)
            {
                play = null;
                GameObject ob = GameObject.Instantiate(mPetIconItem,
                    new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                float witch = ob.GetComponent<RectTransform>().rect.width;
                ob.transform.parent = mListView.transform;
                ob.transform.localScale = new Vector3(1, 1, 1);
                ob.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                PetIconShowControl item = ob.GetComponent<PetIconShowControl>();
                bean = list[i];
                Debug.Log("PetControl id=" + bean.id);
                foreach (PlayerBackpackBean n in petList)
                {
                    if (n.goodId == bean.id)
                    {
                        play = n;
                        break;
                    }
                }
                if (play != null)
                {
                    item.init(play, this);
                    // if (play.goodType == SQLDate.GOOD_TYPE_USER_PET) {
                    //     mPetManage.petFight(play.goodId);
                    // }
                }
                else
                {
                    item.init(bean.id, this);
                }
                mControlList.Add(item);
            }
            onIconClick(mControlList[0]);
            SetGridHeight();

        }

        gameObject.transform.localPosition = new Vector2(0, 0);
    }
}
