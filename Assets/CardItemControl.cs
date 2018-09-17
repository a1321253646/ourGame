using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardItemControl : MonoBehaviour {
    CardManager mManager;
    long mCardId;
    SkillJsonBean mSkillJson;
    
    private void Start()
    {

    }

    private void Update()
    {
    }

    public void init( long cardId) {
        float x = gameObject.GetComponent<RectTransform>().rect.width * gameObject.transform.localScale.x;
        float y = gameObject.GetComponent<RectTransform>().rect.height * gameObject.transform.localScale.y;
        Transform ob = transform.GetChild(0).GetChild(0);
        float x1 = ob.gameObject.GetComponent<RectTransform>().rect.width;
        float y1 = ob.gameObject.gameObject.GetComponent<RectTransform>().rect.height;
        Debug.Log("x= " + x + " y= " + y + " x1= " + x1 + " y1=" + y1 + " Bilix=" + (x / x1) + " Biliy=" + (y / y1));
        ob.localScale = new Vector3(x / x1, y / y1, 1);
    }

}
