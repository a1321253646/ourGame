using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        initText();
    }

    public void initText() {
        Text[] ts = GetComponentsInChildren<Text>();
        foreach (Text t in ts) {
            if (!string.IsNullOrEmpty(t.text) && t.text.Contains("&")) {
                string[] s = t.text.Split('&');
                if (s.Length == 3) {
                    string str = s[1];
                    if (string.IsNullOrEmpty(str)) {
                        continue;
                    }
                    try
                    {
                        long id = long.Parse(str);
                        t.text = JsonUtils.getIntance().getStringById(id);
                    }
                    catch (Exception e) {

                    }
                }
            }
        }
    }
}
