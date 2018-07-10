using UnityEngine;
using System.Collections;

public class Enemy
{
	public string id;
	public string monster_hp;
	public string monster_attack;
	public string monster_defense;
	public string die_gas;
	public string die_crystal;
	public string monster_speed;
	public string attack_range;
	public string attack_speed;
	public string resource;
	public string abc;

	public int getMonsterHp(){
		
		return int.Parse (monster_hp);
	}
	public long getId(){
		return long.Parse (id);
	}
	public int getMonsterAttack(){
		return int.Parse (monster_attack);
	}
	public int getMonsterDefense(){
		return  int.Parse (monster_defense);
	}	
	public int getDieGas(){
		return  int.Parse (die_gas);
	}
	public int getDieCrystal(){
		return  int.Parse (die_crystal);
	}
	public int getMonsterSpeed(){
		return  int.Parse (monster_speed);
	}
	public float getAttackRange(){
		return  float.Parse (attack_range);
	}
	public float getAttackSpeed(){
		return float.Parse(attack_speed);
	}
	public int getResource(){
		return int.Parse (resource);
	}

}

