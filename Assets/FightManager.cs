using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager{

	private int id = 0;
	Dictionary<int,Attacker> mAliveActtackers = new Dictionary<int,Attacker>();

	public void registerAttacker(Attacker attcker){
		if (attcker.id != -1 || mAliveActtackers.ContainsKey(attcker.id)) {
			Debug.Log ("registerAttacker:this attcker is register");
			return;
		}
		attcker.id = id;
		id++;
		mAliveActtackers.Add (attcker.id, attcker);
	}
	public void unRegisterAttacker(Attacker attcker){
		if (id != -1 || mAliveActtackers.Count < 1) {
			Debug.Log ("unRegisterAttacker:this attcker is not register");
			return;
		}
		foreach (Attacker tmp in mAliveActtackers.Values) {
			if (tmp.mAttackerTargets != null && tmp.mAttackerTargets.Count > 0) {
				int index = tmp.mAttackerTargets.IndexOf (attcker);
				if (index >= 0) {
					tmp.mAttackerTargets.RemoveAt (index);
				}
			}
		}
		mAliveActtackers.Remove (attcker.id);
	}

	public void addAttacker(int myId,int beAttackerid){
		
		if (!mAliveActtackers.ContainsKey (myId) || !mAliveActtackers.ContainsKey (beAttackerid)) {
			return ;
		}

		Attacker my = mAliveActtackers [myId];
		Attacker beAttacker = mAliveActtackers [beAttackerid];
		if (my.mAttackerTargets == null) {
			my.mAttackerTargets = new List<Attacker> ();
		}
		my.mAttackerTargets.Add (beAttacker);
	}
	public int attackerAction(int id){
		int hurtBloodAll = 0;
		int hurtBlood = 0;
		Attacker attacker = getAttackerById (id);
		if (attacker.mAttackerTargets != null || attacker.mAttackerTargets.Count > 0) {
			foreach (Attacker tager in attacker.mAttackerTargets) {
				hurtBlood = attackBllod (attacker, tager);
				tager.BeAttack (hurtBlood);
				hurtBloodAll += hurtBlood;
			}
			return hurtBloodAll;
		}

		return 0;
	}
	private Attacker getAttackerById(int id){
		return mAliveActtackers [id];
	}
	private int attackBllod(Attacker attacker,Attacker beAttacker){
		int hurt= attacker.mAggressivity - beAttacker.mDefense;
		if (hurt <= 0) {
			hurt = 0;
		}
		return hurt;
	}
}
