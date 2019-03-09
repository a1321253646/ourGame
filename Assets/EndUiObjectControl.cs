using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndUiObjectControl : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        if (GameManager.getIntance().isEnd) {
            Destroy(gameObject);
        }
	}
}
