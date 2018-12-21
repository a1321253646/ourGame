using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetControl : MonoBehaviour {

    public GameObject mPetIconItem;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private bool isInit = false;
    private List<PetIconShowControl> mControlList = new List<PetIconShowControl>();
    public void init() {
        if (isInit) {
            isInit = true;
            List<PetJsonBean> list = JsonUtils.getIntance().getPet();
            List<PlayerBackpackBean> petList = InventoryHalper.getIntance().getPet();
            int count = list.Count;
            PetJsonBean bean;
            PlayerBackpackBean play ;
            for (int i = 0;i < count; i++) {
                play = null;
                GameObject ob = GameObject.Instantiate(mPetIconItem,
                    new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                float witch = ob.GetComponent<RectTransform>().rect.width;
                ob.transform.parent = gameObject.transform;
                ob.transform.localScale = new Vector3(1, 1, 1);
                ob.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                PetIconShowControl item = ob.GetComponent<PetIconShowControl>();
                bean = list[i];
                foreach (PlayerBackpackBean n in petList) {
                    if (n.goodId == bean.id) {
                        play = n;
                        break;
                    }
                }
                if (play != null)
                {
                    item.init(play, this);
                }
                else {
                    item.init(bean.id, this);
                }
                mControlList.Add(item);
            }
        }
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
        if(mClickIcon != null) {
            mClickIcon.click(false);
        }
        mClickIcon = icon;
    }
    public void click() {

    }
}
