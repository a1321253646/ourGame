using UnityEngine;
using System.Collections;

public class Hero
{
	public long role_lv;
	public double role_hp;
	public double role_attack;
	public double role_defense;
	public string lvup_crystal;
	public float attack_speed;
    public float hit;
    public float dod;
    public float cri;
    public double cri_dam;
    public float speed_up;
    public double real_dam;

    public BigNumber mLvupCrystal;
    public BigNumber getLvupCrystal()
    {
        if (mLvupCrystal == null)
        {
            mLvupCrystal = BigNumber.getBigNumForString(lvup_crystal);
        }
        return mLvupCrystal;
    }
}

