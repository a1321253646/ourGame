using UnityEngine;
using System.Collections;

public class Hero
{
	public string role_lv;
	public string role_hp;
	public string role_attack;
	public string role_defense;
	public string lvup_crystal;

	public int getRoleLv(){
		return  int.Parse(role_lv);
	}
	public int getRoleHp(){
		return  int.Parse (role_hp);
	}
	public int getRoleAttack(){
		return  int.Parse (role_attack);
	}
	public int getRoleDefense(){
		return  int.Parse (role_defense);
	}	
	public int getLvupCrystal(){
		return  int.Parse (lvup_crystal);
	}
}

