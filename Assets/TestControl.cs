using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TestControl : MonoBehaviour {

    private long mPath = -1;
    private int mStatus = -1;
    private bool isInit = false;
    public bool isChange = false;

    public long path = -1;
    public int statu = 0;
    
    AnimalControlBase mAnimalControl;
    SpriteRenderer mSpriteRender ;
    Button mButton;
    // Use this for initialization
    void Start () {
        mSpriteRender = gameObject.GetComponent<SpriteRenderer>();
        mButton = GameObject.Find("Button").GetComponent<Button>();
        mButton.onClick.AddListener(() => {
            change();
        });
        isInit = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (!isInit) {
            return;
        }
        if (isChange) {
            isChange = false;
            change();
        }
        if (mAnimalControl != null) {
            mAnimalControl.update();
        }
        

    }

    private void change() {
        if (path != -1 && path != mPath) {
            mPath = path;
            ResourceBean resourceData = JsonUtils.getIntance().getEnemyResourceData(mPath);            
            mAnimalControl = new AnimalControlBase(resourceData, mSpriteRender);
            mAnimalControl.start();
           
        }
        if (statu != mStatus)
        {
            mStatus = statu;
            mAnimalControl.setStatus(mStatus);
        }
    }
}
