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
    bool isInit = false;
    void Start () {

    }

    public void init() {
        transform.localScale = new Vector2(0, 0);
        isInit = true;
    }

    public void setCount(float count) {
        if (!isInit) {
            return;
        }
        if (count >= index)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else {
            transform.localScale = new Vector2(0, 0);
        }
    }
}
