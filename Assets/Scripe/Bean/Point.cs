using UnityEngine;
using System.Collections;

public class Point 
{
	public float x;
	public float y;

	public Point(string str){
		string[] split = str.Split(',');
		x = float.Parse(split[0]);
		y = float.Parse(split[1]);
	}
}

