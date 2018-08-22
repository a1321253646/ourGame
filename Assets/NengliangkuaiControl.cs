using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NengliangkuaiControl : MonoBehaviour {

    // Use this for initialization
    public float index;
    private Image liang, mie;
	void Start () {
        liang = GetComponentsInChildren<Image>()[1];
        mie = GetComponentsInChildren<Image>()[2];

    }
    public void setCount(float count) {
        if (count >= index)
        {
            liang.color = new Color(1, 1, 1, 1);
            mie.color = new Color(1, 1, 1, 0);
        }
        else {
            mie.color = new Color(1, 1, 1, 1);
            liang.color = new Color(1, 1, 1, 0);
        }
    }
}
