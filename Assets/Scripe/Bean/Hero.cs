using UnityEngine;
using System.Collections;

public class Hero
{
	public long role_lv;
	public double role_hp;
	public double role_attack;
	public double role_defense;
	public string lvup_crystal;
	public double attack_speed;
    public double hit;
    public double dod;
    public double cri;
    public double cri_dam;
    public double speed_up;
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

