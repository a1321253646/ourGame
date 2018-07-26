using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ComposeControl : MonoBehaviour {

    public bool isShow = false;
    private Button mClose;
    public void showUi()
    {
        if (isShow)
        {
            return;
        }
        isShow = true;
        //gameObject.transform.TransformPoint(new Vector2(0,0));
        gameObject.transform.localPosition = new Vector2(0, 0);
        if (mClose == null)
        {
            mClose = GameObject.Find("compose_close").GetComponent<Button>();
            mClose.onClick.AddListener(() => {
                removeUi();
            });
        }
    }
    public void removeUi()
    {
        if (!isShow)
        {
            return;
        }
        isShow = false;
        // gameObject.transform.TransformPoint(new Vector2(-607, -31));
        gameObject.transform.localPosition = new Vector2(740, -63.673f);
    }
}
