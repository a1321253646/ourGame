using UnityEngine;
using System.Collections;

public class Point 
{
	public float x;
	public float y;

	public Point(string str){
		string[] split = str.Split(',');
        if (split.Length == 2)
        {
            x = float.Parse(split[0]);
            y = float.Parse(split[1]);
        }
	}
    public Point(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
}

