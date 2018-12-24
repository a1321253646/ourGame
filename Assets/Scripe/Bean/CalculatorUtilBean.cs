using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CalculatorUtilBean
{
    public static int TYPE_ADD = 1;
    public static int TYPE_MINUS = 2;
    public static int TYPE_MULTIPLY = 3;
    public static int TYPE_DIVIDE = 4;

    public List<CalculatorUtilBean> list;
    public double bean = 0;
    public int type = 0;
    public string valueKey;
    public string getString() {
        string s = "bean = " + bean + " type=" + type + " valueKey = " + valueKey+" list:\n";
        if(list != null && list.Count > 0) {
            foreach(CalculatorUtilBean c in list) {
                s = s + c.getString();
                s = s + "===========================";
            }
        }
        return s;
    }
}
