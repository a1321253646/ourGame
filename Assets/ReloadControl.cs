using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadControl : MonoBehaviour {

    public void reload(bool isNewLocal) {
        GameManager.getIntance().isEnd = true;
        transform.localScale = new Vector3(1, 1, 1);
        if (isNewLocal)
        {
            SQLManager.getIntance().deleteAll();
        }
        SQLHelper.getIntance().init();
        InventoryHalper.getIntance().init();
        GameObject.Find("Manager").GetComponent<LevelManager>().reset();
        transform.localScale = new Vector3(0, 0, 0);
    }
}
