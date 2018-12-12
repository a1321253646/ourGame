using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;
public class GameBeginControl : MonoBehaviour {

	// Use this for initialization
	void Start () {

      /*  float width = Screen.width;
        float height = Screen.height;
        Debug.Log("width = " + width + " height=" + height);
        if (16 * width > 9 * height)
        {
            GameObject.Find("Canvas").GetComponent<CanvasScaler>().matchWidthOrHeight = 1;
        }
        else if (16 * width < 9 * height){
            GameObject.Find("Canvas").GetComponent<CanvasScaler>().matchWidthOrHeight = 0;
        }*/
        gameObject.transform.SetSiblingIndex(GameManager.getIntance().getUiLevel());
    }
	
	// Update is called once per frame
	void Update () {
    }
    public void init() {
        initEnd();
    }
    private void initEnd() {
        GameObject.Find("Manager").GetComponent<SQLManager>().init();
        SQLHelper.getIntance().init();
        JsonUtils.getIntance().init();
        if (SQLHelper.getIntance().isFristStartGame == -1)
        {
            Destroy(GameObject.Find("game_begin_loading"));
            GetComponentInChildren<TypewriterEffect>().init(JsonUtils.getIntance().getStringById(100001));

        }
        else
        {
            AudioSource source = GameObject.Find("information").GetComponent<AudioSource>();
            GameManager.getIntance().playBgm(source, "Sounds/关卡背景音");
            Destroy(gameObject);
            GameObject.Find("Manager").GetComponent<LevelManager>().init();
        }
    }

}
