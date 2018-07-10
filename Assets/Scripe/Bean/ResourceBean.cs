using UnityEngine;
using System.Collections;

public class ResourceBean
{
	public string id;
	public string role;
	public string zoom;
	public string attack_frame;
	public long getId(){
		return long.Parse (id);
	}
	public int getAttackFrame(){
		return  int.Parse (attack_frame);
	}
	public float getZoom(){
		return float.Parse(zoom);
	}
}

