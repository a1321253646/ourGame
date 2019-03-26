using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShuoMingControl : MonoBehaviour {

    public long String_Id;
	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            GameObject.Find("game_dec_tip").GetComponentInChildren<GameDecTip>().setData(String_Id);
            UiControlManager.getIntance().show(UiControlManager.TYPE_DEC_TIP);
        });
	}
}
