using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NengliangkuaiControl : MonoBehaviour {

    // Use this for initialization
    public float index;
    private Image mImage;
    AnimalControlBase mAnimalControl;
    Sprite endSprite;
    void Start () {
        if (mAnimalControl == null)
        {
            creatAnimal();
        }
    }

    void Update()
    {
        if (mAnimalControl == null)
        {
            creatAnimal();
        }
        mAnimalControl.update();
    }

    private void creatAnimal() {

        mImage = GetComponent<Image>();
        ResourceBean resourceData = JsonUtils.getIntance().getEnemyResourceData(40001);
        mAnimalControl = new AnimalControlBase(resourceData, mImage);
        mAnimalControl.start();
        endSprite = Resources.Load("ui_new/huo_hui", typeof(Sprite)) as Sprite;
        mAnimalControl.start();
        mAnimalControl.end(endSprite);
    }

    public void setCount(float count) {
        if (mAnimalControl == null) {
            creatAnimal();
        }
        if (count >= index)
        {
            mAnimalControl.start();
        }
        else {
            mAnimalControl.end(endSprite);
        }
    }
}
