using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetControl : MonoBehaviour {

    public GameObject mPetIconItem;

    GameObject mListView;
    VerticalLayoutGroup mVerticall;
    Text mName, mGetLevel, mDec, mAffix, mSkillDec,mRestText;
    Image mSkillIcon,mPetShowImg;
    Button mClose,mFightButton,mRestButton;
    GameObject mFightStatu, mRestStatu,mPetShowCenter;
	// Use this for initialization
	void Start () {
        mFri = gameObject.transform.localPosition;

    }
	
	// Update is called once per frame
	void Update () {
        if (mAnimalControl != null) {
            mAnimalControl.update();
        }
	}
    private bool isShow = false;
    private Vector2 mFri ;
    public void removeUi()
    {
  
        isShow = false;
        // gameObject.transform.TransformPoint(new Vector2(-607, -31));
        gameObject.transform.localPosition = mFri;
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

    public void showUi()
    {

        mLevel = GameManager.getIntance().getUiLevel();
        init();
        gameObject.transform.SetSiblingIndex(mLevel);
        gameObject.transform.localPosition = new Vector2(0, 0);
    }



    private bool isInit = false;
    PetManager mPetManage;
    private List<PetIconShowControl> mControlList = new List<PetIconShowControl>();
    public void init() {
        if (!isInit) {
            isInit = true;
            List<PetJsonBean> list = JsonUtils.getIntance().getPet();
            List<PlayerBackpackBean> petList = InventoryHalper.getIntance().getPet();
            int count = list.Count;
            Debug.Log("PetControl count=" + list.Count);
            PetJsonBean bean;
            mListView=  GameObject.Find("pet_list");
            mName = GameObject.Find("pet_name").GetComponent<Text>();
            mGetLevel = GameObject.Find("pet_get_level").GetComponent<Text>();
            mDec = GameObject.Find("pet_get_level").GetComponent<Text>();
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
                GameObject.Find("hero").GetComponent<HeroRoleControl>().removeUi();
                removeUi();
            });
            mVerticall = mListView.GetComponent<VerticalLayoutGroup>();
            PlayerBackpackBean play ;
            for (int i = 0;i < count; i++) {
                play = null;
                GameObject ob = GameObject.Instantiate(mPetIconItem,
                    new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                float witch = ob.GetComponent<RectTransform>().rect.width;
                ob.transform.parent = mListView.transform;
                ob.transform.localScale = new Vector3(1, 1, 1);
                ob.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                PetIconShowControl item = ob.GetComponent<PetIconShowControl>();
                bean = list[i];
                Debug.Log("PetControl id="+ bean.id);
                foreach (PlayerBackpackBean n in petList) {
                    if (n.goodId == bean.id) {
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
                else {
                    item.init(bean.id, this);
                }
                mControlList.Add(item);
            }
            onIconClick(mControlList[0]);
            SetGridHeight();
        }
    }

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
        mDec.text = mClickIcon.mJsonBean.dec;
        mAffix.text = "???????\n???????\n???????\n???????";
        if (mClickIcon.mBean == null)
        {
            mGetLevel.text = "通关" + mClickIcon.mJsonBean.activateLevel + "关拯救精灵";
            mSkillDec.text = "????????????\n????????????\n????????????\n????????????";
            mSkillIcon.sprite = Resources.Load("icon/pet/" + mClickIcon.mJsonBean.skillNoactivateIcon, typeof(Sprite)) as Sprite;
            mFightStatu.transform.localScale = new Vector2(0, 0);
            mRestStatu.transform.localScale = new Vector2(1, 1);
            mRestText.text = "未拥有";
            mRestButton.interactable = false;
            mAnimalControl = null;
            mPetShowImg.sprite = Resources.Load("icon/pet/" + mClickIcon.mJsonBean.skillNoactivateIcon, typeof(Sprite)) as Sprite;
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
            mPetShowImg.gameObject.transform.position = new Vector2(mPetShowCenter.transform.position.x - v.x, mPetShowCenter.transform.position.y - v.y);
            //  }


            mGetLevel.text = "";
            SkillJsonBean skill= JsonUtils.getIntance().getSkillInfoById(mClickIcon.mJsonBean.skillId);
            mSkillDec.text = skill.skill_describe;
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
}
