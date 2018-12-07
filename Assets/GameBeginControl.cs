using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBeginControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void init() {
        SQLHelper.getIntance().init();
        GameManager.getIntance();
        JsonUtils.getIntance().init();
        if (SQLHelper.getIntance().isFristStartGame == -1)
        {
            Destroy(GameObject.Find("game_begin_loading"));
            GetComponentInChildren<TypewriterEffect>().init(JsonUtils.getIntance().getStringById(100001));
           
        }
        else {
            Destroy(gameObject);
            GameObject.Find("Manager").GetComponent<LevelManager>().init();
        }
    }
}
