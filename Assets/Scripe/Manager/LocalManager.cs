using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalManager{

	public LocalBean mLocalLink;
	FightManager mFightManager;
	List<LocalMultiple> mMultiple = new List<LocalMultiple> ();
	public void setHeroLoacl(LocalBean local){
		mLocalLink = local;
		mLocalLink.previous = null;
		mFightManager = GameObject.Find ("Manager").GetComponent<LevelManager> ().getFightManager (); 
	}

	public void addAttack(LocalBean local){
		
		if (mLocalLink == null)
			return;
		LocalBean tmp = mLocalLink;
		
		while (tmp.next != null) {
			tmp = tmp.next;
		}
		tmp.next = local;
		local.previous = tmp;

	}

	public void upData(){
		sortLink ();
		dealMultiple ();
		heroAttackTarget ();
		foreach (LocalMultiple multiple in mMultiple) {
			multiple.updataLocal ();
		}
	}

	public void EnemyDeal(Attacker attacker){
		
		if(attacker.mLocalBean.isInMultiple){
			for (int i = 0; i < mMultiple.Count; i++) {
				LocalMultiple multiple = mMultiple [i];
				if (multiple.removeLoacal (attacker.mLocalBean)) {
					break;
				}
			}
		}
			
		LocalBean enemyLoacl = attacker.mLocalBean;
		LocalBean tmp = mLocalLink.next;
		while (tmp != null) {
			if (tmp == enemyLoacl) {
				tmp.previous.next = tmp.next;
				if (tmp.next != null) {
					tmp.next.previous = tmp.previous;
				}
				break;
			}
			tmp = tmp.next;
		}
	}

	public void removeMultiple(LocalMultiple multiple){
		Debug.Log ("removeMultiple");
		for(int i = 0; i< mMultiple.Count ;i++){
			if (mMultiple [i] == multiple) {
				mMultiple.RemoveAt (i);
				Debug.Log ("removeMultiple mMultiple size =" + mMultiple.Count);
				mFightManager.mHeroStatus = Attacker.PLAY_STATUS_RUN;
			}
		}
	}

	private void heroAttackTarget(){
		
		LocalBean tmp = mLocalLink.next;

		while (tmp != null) {
			float leng = mLocalLink.mCurrentX + mLocalLink.mAttackLeng;

			if (tmp.mCurrentX <= leng) {
				bool isHave = false;
				foreach (Attacker a in mLocalLink.mAttacker.mAttackerTargets) {
					if (a == tmp.mAttacker) {
						isHave = true;
						break;
					}
				}

				if (!isHave) {
					mLocalLink.mAttacker.mAttackerTargets.Add (tmp.mAttacker);
				}

			} else {
				break;
			}
			tmp = tmp.next;
		}
		if (mLocalLink.mAttacker.mAttackerTargets.Count > 0) {
			mFightManager.mHeroStatus = Attacker.PLAY_STATUS_FIGHT;
		}

	}

	private void sortLink(){

		if (mLocalLink == null || mLocalLink.next == null) {
			return;
		}	
		LocalBean index = mLocalLink.next;
		LocalBean tmp =index,tmp2=null;
		while (index.next != null) {

			if (index.mCurrentX > index.next.mCurrentX) {
				tmp = index.next;

				index.previous.next = tmp;
				tmp.previous = index.previous;

				index.next = tmp.next;
				if (tmp.next != null) {
					index.next.previous = index;
				}
				tmp.next = index;
				index.previous = tmp;
			} else {
				index = index.next;
			}
		}
	}
	private void dealMultiple(){
		LocalBean tmp = mLocalLink.next;
		while (tmp != null) {
			bool isAdd = false;
			foreach (LocalMultiple multiple in mMultiple) {
				if (multiple.addLocal (tmp)) {
					isAdd = true;
					break;
				}
			}

			if (!isAdd && tmp.mCurrentX - tmp.mAttackLeng <= mLocalLink.mCurrentX) {
				LocalMultiple newMultiple = new LocalMultiple ();
				newMultiple.initLocalMultiple (tmp, this);
				mMultiple.Add (newMultiple);
			}
			tmp = tmp.next;
		}
	}
		
}
