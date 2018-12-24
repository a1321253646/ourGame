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

    public static string doubleToStringShow(double value) {
        string str = value.ToString();
        if (str.Contains("E+"))
        {
            string str1, str2;
            str = str.Replace("E+", "E");
            str1 = str.Split('E')[0];
            str2 = str.Split('E')[1];
            double count1 = double.Parse(str1);
            int count2 = int.Parse(str2);
            if (count2 >= 6)
            {
                str = (count1 * Mathf.Pow(10, count2 % 3)).ToString("f"+(3- count2 % 3)) + getUnit(count2 / 3);
            }
            else
            {
                str = (int)(count1 * Mathf.Pow(10, count2)) + "";
            }
        }
        else {

            string str1 = str.Split('.')[0];
            
            int leng = str1.Length;

            if (leng < 7)
            {
                str = str1;
            }
            else {
                int count = leng % 3;
                if (count == 0)
                {
                    count = 3;
                }
                else {
                    count = leng % 3;
                }
                str =( double.Parse(str1)/ Mathf.Pow(10, leng - count)).ToString("f"+(4-count)) + getUnit((leng-count) / 3);
            }
        }
        return str;
    }
    private static string getUnit(int index) {
        string unit = "";
        if (index < 2)
        {
            unit = "";
        }
        else if (index == 2)
        {
            unit = "M";
        }
        else if (index == 3)
        {
            unit = "B";
        }
        else if (index == 4)
        {
            unit = "T";
        }
        else if (index > 4 && index <= 30)
        {
            unit = new string(new char[] { (char)('a' + index - 5), (char)('a' + index - 5) });
        }
        else if (index > 30 && index <= 56)
        {
            unit = new string(new char[] { (char)('A' + index - 31), (char)('A' + index - 31) });
        }
        return unit;
    }
}
