using UnityEngine;
using System.Collections;

public class ResourceBean
{
	public string id;
	public string name;
	public string zoom;
	public string attack_frame;
	public string blood_offset;
	public string blood_witch;
	public string hurt_offset;
	public string fight_offset;
	public string idel_y;

	public Point BloodOffset,HurtOffset,FightOffset;


	public long getId(){
		return long.Parse (id);
	}
	public int getAttackFrame(){
		return  int.Parse (attack_frame);
	}
	public float getZoom(){
		return float.Parse(zoom);
	}
	public Point getBloodOffset(){
		if (BloodOffset == null) {
			BloodOffset = new Point (blood_offset);
		}
		return BloodOffset;
	}
	public float getBloodWitch(){
		return float.Parse(blood_witch);
	}
	public Point getHurtOffset(){
		if (HurtOffset == null) {
			HurtOffset = new Point (hurt_offset);
		}
		return HurtOffset;
	}
	public Point getFightOffset(){
		if (FightOffset == null) {
			FightOffset = new Point (fight_offset);
		}
		return FightOffset;
	}
	public float getIdelY(){
		//return float.Parse(idel_y);
		return 0;
	}

}

