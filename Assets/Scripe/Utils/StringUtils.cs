using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringUtils
{
    public static string getKMBAString(ulong count) {
        string back = "";
        ulong point = 0;
        if (count >= 1000uL && count < 1000000uL) {
            point = count % 1000ul;
            count = count / 1000ul;
            back = "K";
        }
        else if (count >= 1000000uL && count < 1000000000uL)
        {
            point = count % 1000000uL;
            count = count / 1000000uL;
            back = "M";
        }
        else if (count >= 1000000000uL && count < 1000000000000uL)
        {
            point = count % 1000000000uL;
            count = count / 1000000000uL;
            back = "B";
        }
        else if (count >= 1000000000000uL && count < 1000000000000000uL)
        {
            point = count % 1000000000000uL;
            count = count / 1000000000000uL;
            back = "T";
        }
        else if (count > 1000000000000000uL && count < 1000000000000000000uL)
        {
            point = count % 1000000000000000uL;
            count = count / 1000000000000000uL;
            back = "aa";
        }
        else if (count > 1000000000000000000uL && count < 9223372036854775807uL)
        {
            point = count % 1000000000000000000uL;
            count = count / 1000000000000000000uL;
            back = "bb";
        }
        return count + "." + point + back;
    }
}
