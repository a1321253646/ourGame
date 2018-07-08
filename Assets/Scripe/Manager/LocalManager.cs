using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalManager{

	public LocalBean mLocalLink;
	public LocalBean mFirstMin, mFristMid, mFristMax;
	FightManager mFightManager;

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
	public void removeAttack(LocalBean local){
		Debug.Log ("removeAttack");
		if (mLocalLink == null)
			return;
		LocalBean tmp = mLocalLink;
		while (tmp.next != local && tmp.next != null) {
			tmp = tmp.next;
		}
		tmp.previous.next = tmp.next;
	}

	public void upData(){
		sortLink ();
		dealMultiple ();
		heroAttackTarget ();
	}

	public void EnemyDeal(Attacker attacker){
		Debug.Log ("EnemyDeal");
		LocalBean enemyLoacl = attacker.mLocalBean;
		LocalBean tmp = mLocalLink.next;
		while (tmp != null) {
			if (tmp == enemyLoacl) {
				tmp.previous.next = tmp.next;
				if (tmp.next != null) {
					tmp.next.previous = tmp.previous;
				}
				break;
			} else if(tmp is LocalMultiple &&((LocalMultiple)tmp).removeLoacal(enemyLoacl)){
				break;
			}
			tmp = tmp.next;
		}
	}

	public void removeMultiple(LocalMultiple multiple){
		if (mFirstMin != null && mFirstMin == multiple) {
			mFirstMin.previous.next = mFirstMin.next;
			if (mFirstMin.next != null) {
				mFirstMin.next.previous = mFirstMin.previous;
			}
			mFirstMin = null;
			mFightManager.mHeroStatus = Attacker.PLAY_STATUS_RUN;
		}
	}

	private void heroAttackTarget(){
		
		if (mFirstMin == null) {
			return;
		}
		LocalBean tmp = mLocalLink;
		float leng = tmp.mCurrentX + tmp.mAttackLeng;
		Debug.Log ("mCurrentX = " +  tmp.mCurrentX+" tmp.mAttackLeng "+tmp.mAttackLeng+" leng= "+leng);
		tmp = mLocalLink.next;
		while (tmp != null) {
			if (tmp is LocalMultiple) {
				((LocalMultiple)tmp).dealHeroActtack (mLocalLink.mAttacker, leng);
				if (mLocalLink.mAttacker.mAttackerTargets.Count > 0) {
					mFightManager.mHeroStatus = Attacker.PLAY_STATUS_FIGHT;
				}
			} else {
				if (tmp.mCurrentX < leng) {
					bool isHave = false;
					foreach (Attacker a in mLocalLink.mAttacker.mAttackerTargets) {
						if (a == tmp.mAttacker) {
							isHave = true;
							break;
						}
					}
					if (!isHave) {
						mLocalLink.mAttacker.mAttackerTargets.Add (tmp.mAttacker);
						Debug.Log ("mLocalLink.mAttacker.mAttackerTargets.Count ="+mLocalLink.mAttacker.mAttackerTargets.Count);
					}
				} else {
					return;
				}
			}
			tmp = tmp.next;
		}
	}


	private void dealMultiple(){
		if (mFirstMin != null && !(mFirstMin is LocalMultiple) && mFirstMin.mCurrentX - mFirstMin.mAttackLeng <= mLocalLink.mCurrentX) {
			LocalMultiple local = new LocalMultiple ();

			mFirstMin
				.previous
				.next = local;
			local.previous = mFirstMin.previous;
			if (mFirstMin.next != null) {
				mFirstMin.next.previous = local;
				local.next = mFirstMin.next;
			}
			local.initLocalMultiple (mFirstMin,this);
			mFirstMin = local;
		}

		if (mFristMid != null && !(mFristMid is LocalMultiple) && mFristMid.mCurrentX - mFristMid.mAttackLeng <= mLocalLink.mCurrentX) {
			LocalMultiple local = new LocalMultiple ();
			local.initLocalMultiple (mFristMid,this);
			mFristMid.previous.next = local;
			local.previous = mFristMid.previous;
			if (mFristMid.next != null) {
				mFristMid.next.previous = local;
				local.next = mFristMid.next;
			}
			mFristMid = local;
		}

		if (mFristMax != null && !(mFristMax is LocalMultiple) && mFristMax.mCurrentX - mFristMax.mAttackLeng <= mLocalLink.mCurrentX) {
			LocalMultiple local = new LocalMultiple ();
			local.initLocalMultiple (mFristMax,this);
			mFristMax.previous.next = local;
			local.previous = mFristMax.previous;
			if (mFristMax.next != null) {
				mFristMax.next.previous = local;
				local.next = mFristMax.next;
			}
			mFristMax = local;
		}
		if (mFirstMin != null && (mFirstMin is LocalMultiple) ) {
			((LocalMultiple)mFirstMin).updataLocal ();
		}
		if (mFristMid != null && (mFristMid is LocalMultiple) ) {
			((LocalMultiple)mFristMid).updataLocal ();
		}
		if (mFristMax != null && (mFristMax is LocalMultiple) ) {
			((LocalMultiple)mFristMax).updataLocal ();
		}

		LocalBean tmp = mLocalLink.next;
		LocalBean tmp2 = null;
		while (tmp != null) {
			if (tmp is LocalMultiple) {
				tmp = tmp.next;
				continue;
			}
			if (tmp.mAttackType == LocalBean.MIX_TYPE && mFirstMin is LocalMultiple) {
				if (tmp.mCurrentX <= ((LocalMultiple)mFirstMin).mMaxPointX) {
					tmp.previous.next = tmp.next;
					if (tmp.next != null) {
						tmp.next.previous = tmp.previous;
					}
					tmp2 = tmp.next;
					((LocalMultiple)mFirstMin).addLocal (tmp);
					tmp = tmp2;
					continue;

				}
				tmp = tmp.next;
				continue;
			}
			if (tmp.mAttackType == LocalBean.MID_TYPE && mFristMid is LocalMultiple) {
				if (tmp.mCurrentX<= ((LocalMultiple)mFristMid).mMaxPointX) {
					
					tmp.previous.next = tmp.next;
					if (tmp.next != null) {
						tmp.next.previous = tmp.previous;
					}
					tmp2 = tmp.next;
					((LocalMultiple)mFristMid).addLocal (tmp);
					tmp = tmp2;
					continue;
				}
				tmp = tmp.next;
				continue;
			}
			if (tmp.mAttackType == LocalBean.MAX_TYPE && mFristMax is LocalMultiple) {
				if (tmp.mCurrentX <= ((LocalMultiple)mFristMax).mMaxPointX) {
					
					tmp.previous.next = tmp.next;
					if (tmp.next != null) {
						tmp.next.previous = tmp.previous;
					}
					tmp2 = tmp.next;
					((LocalMultiple)mFristMax).addLocal (tmp);
					tmp = tmp2;
					continue;
				}
				tmp = tmp.next;
				continue;
			}
			tmp = tmp.next;
		}
	}

	private void sortLink(){

		if (mLocalLink == null || mLocalLink.next == null) {
			return;
		}	
		mFirstMin = null;
		mFristMax = null;
		mFristMid = null;
		LocalBean index = mLocalLink.next;
		LocalBean tmp =index,tmp2=null;
		if(index.mAttackType == LocalBean.MIX_TYPE){
			if (mFirstMin == null) {
				mFirstMin = index;
			}

		}else if(  LocalBean.MID_TYPE == index.mAttackType ){
			if (mFirstMin == null) {
				mFristMid = index;
			}

		}else if( index.mAttackType == LocalBean.MAX_TYPE){
			if (mFirstMin == null) {
				mFristMax =index;
			}
		}

		while (index.next != null) {
			if(index.mAttackType == LocalBean.MIX_TYPE){
				if (mFirstMin == null) {
					mFirstMin = index;
				}

			}else if(  LocalBean.MID_TYPE == index.mAttackType ){
				if (mFirstMin == null) {
					mFristMid = index;
				}

			}else if( index.mAttackType == LocalBean.MAX_TYPE){
				if (mFirstMin == null) {
					mFristMax =index;
				}
			}

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
}
