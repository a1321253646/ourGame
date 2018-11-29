

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
        else if (index > 4 && index <= 30)
        {
            unit = new string(new char[] { (char)('a' + index - 5), (char)('a' + index - 5) });
        }
        else if (index >30 && index <= 56) {
            unit = new string(new char[] { (char)('A' + index - 31), (char)('A' + index - 31) });
        }
    }
}