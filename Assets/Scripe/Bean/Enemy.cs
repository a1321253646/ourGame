using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy
{
	public long id;
	public double monster_hp;
	public double monster_attack;
	public double monster_defense;
    public double attack_speed;
    public double die_gas;
	public string die_crystal;
    public string death_fell;
    public double monster_speed;
    public double attack_range;
    public long resource;
    public long trajectory_resource;
    public long hit_resource;
    public double hit;
    public double dod;
    public double cri;
    public double cri_dam;
    public double speed_up;
    public double real_dam;
    public double range_type;
    public List<long> fellList = new List<long>();

    public BigNumber mDieCrystal;

    public BigNumber getDieCrystal() {
//        Debug.Log(" id = " + id);
        if (mDieCrystal == null) {
            mDieCrystal = BigNumber.getBigNumForString(die_crystal);
        }
        return mDieCrystal;
    }

    public List<FellObjectBean> fell() {
        List<FellObjectBean> list = new List<FellObjectBean>();
        List<FellObjectBean> tmp;
        fellList = getFellList();
        foreach(long id  in fellList){        
            if (id == 0) {
                continue;
            }
   //         Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>掉落器ID=" + id);
            tmp = JsonUtils.getIntance().getDropDevoiceByID(id).fell();
            if (tmp != null) {
                list.AddRange(tmp);
            }
    //        Debug.Log("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<掉落器ID=" + id);
        }
        if (list.Count == 0)
        {
            return null;
        }
        else {
            return list;
        }
    }

    public List<long> getFellList() {
        if (fellList.Count == 0)
        {
            if (death_fell == null || death_fell.Length < 1)
            {
                return null;
            }
            string[] array = death_fell.Split(',');

            foreach (string str2 in array)
            {
                if (str2 == null || str2.Length < 1)
                {
                    continue;
                }
                fellList.Add(long.Parse(str2));
            }
        }
        return fellList;
    }
}

