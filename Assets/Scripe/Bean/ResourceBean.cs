using UnityEngine;
using System.Collections;

public class ResourceBean
{
	public long id;
	public string name;
	public float zoom;
	public float attack_framce;
    public float attack_all_framce;
    public string blood_offset;
	public float blood_witch;
	public string hurt_offset;
	public string fight_offset;
	public float idel_y;

	public Point BloodOffset,HurtOffset,FightOffset;

	public Point getBloodOffset(){
		if (BloodOffset == null) {
			BloodOffset = new Point (blood_offset);
		}
		return BloodOffset;
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
}

