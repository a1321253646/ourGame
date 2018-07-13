using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtText : MonoBehaviour {

	// Use this for initialization
	float time = 0f;
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (time >= 1f) {
			DestroyObject (gameObject);
		}
	}
}
