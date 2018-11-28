using UnityEngine;
using UnityEditor;

public class BigNumberUnit 
{
    public string unit = "";
    public int value = 0;


    public void setUnit(int index)
    {
        if (index == 0)
        {
            unit = "";
        }
        else if (index == 1)
        {
            unit = "K";
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
        else if (index > 4 && index <= 28)
        {
            unit = (new char[] { (char)('a' + index - 4), (char)('a' + index - 4) }).ToString();
        }
        else if (index > 28 && index <= 52) {
            unit = (new char[] { (char)('A' + index - 28), (char)('A' + index - 28) }).ToString();
        }
    }
}