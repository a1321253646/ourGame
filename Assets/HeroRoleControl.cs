using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRoleControl : MonoBehaviour {
    private bool isShow = false;
    public void showUi()
    {
        if (isShow)
        {
            return;
        }
        isShow = true;
        //gameObject.transform.TransformPoint(new Vector2(0,0));
        gameObject.transform.localPosition = new Vector2(0, 0);
    }
    public void removeUi()
    {
        if (!isShow)
        {
            return;
        }
        isShow = false;
        // gameObject.transform.TransformPoint(new Vector2(-607, -31));
        gameObject.transform.localPosition = new Vector2(-222, -411);
    }

    public void upDateUi()
    {
      
    }
}
