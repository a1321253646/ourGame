using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalMultiple :LocalBean
{
	public float ENEMY_X_DISTANCE = 0.3f;
	public float mMaxPointX = 0;
	public float mDistance = 0;
	ArrayList mList; 
	LocalManager mManager;
	public  void initLocalMultiple(LocalBean bean ,LocalManager mamager){
		mCurrentX = bean.mCurrentX;
		mCurrentY = bean.mCurrentY;
		mIsHero = bean.mIsHero;
		mAttacker = bean.mAttacker;
		mAttackLeng = bean.mAttackLeng;
		mDistance = ENEMY_X_DISTANCE;
		mManager = mamager;
		addLocal (bean);
	}

	public void updataLocal(){
		if (mList != null && mList.Count > 0 ) {
			LocalNote[] notes = (LocalNote[])mList [0];
			if(notes != null && notes.Length > 0){
				LocalNote note= notes [0];
				if (note != null) {
					mCurrentX = note.mLocal.mCurrentX;
					mCurrentY = note.mLocal.mCurrentY;			
				}				
			}
		}
		mMaxPointX = mDistance + mCurrentX;
	}

	public bool addLocal(LocalBean bean){
		
		if (bean.isInMultiple) {
			return true;
		}

		if (bean.mAttackLeng != mAttackLeng || bean.mCurrentX > mDistance+mCurrentX) {
			return false;
		}

		bean.isInMultiple = true;


		LocalNote note= new LocalNote();
		note.mLocal = bean;
		note.mLocal.mAttacker.mAttackerTargets.Add (mManager.mLocalLink.mAttacker);
		if (mList == null) {
			mList = new ArrayList ();
		}

		if (mList.Count == 0) {
			LocalNote[] notes = new LocalNote[1];
			note.x = mCurrentX;
			note.y = mCurrentY+note.mLocal.mAttacker.resourceData.getIdelY();
			notes [0] = note;
			mList.Add (notes);
			mDistance =ENEMY_X_DISTANCE*mList.Count;
			bean.mTargetX = note.x;
			bean.mTargetY = note.y;
			return true;
		}

		int leng = mList.Count;
		LocalNote[] tagerNotes =(LocalNote[]) mList[leng-1];
		for(int i = 0; i< leng ;i++){
			if (tagerNotes [i] == null) {
				tagerNotes [i] = note;
				note.x = mCurrentX + (leng-1) * ENEMY_X_DISTANCE;
				int a = i / 2;
				int b = i % 2;
				int c = 1;
				float d = 0;
				if (leng % 2 == 0) {
					d = ENEMY_X_DISTANCE / 2;
					if (b == 0) {
						c = 1;
					} else {
						c = -1;
						a += 1;
					}
				} else {
					d = 0;
					if (b == 0) {
						c = -1;
					} else {
						c = 1;
						a += 1;
					}
				}
				note.y =mCurrentY+ d + c * ENEMY_X_DISTANCE * a+c*+note.mLocal.mAttacker.resourceData.getIdelY();
				bean.mTargetX = note.x;
				bean.mTargetY = note.y;

				return true;
			}
		}

		LocalNote[] notes2 = new LocalNote[leng+1];
		note.x = mCurrentX + leng * ENEMY_X_DISTANCE;
		if ((leng + 1) % 2 == 1) {
			note.y = mCurrentY+note.mLocal.mAttacker.resourceData.getIdelY();
		} else {
			note.y = mCurrentY + ENEMY_X_DISTANCE / 2+note.mLocal.mAttacker.resourceData.getIdelY();
		}
		notes2 [0] = note;
		mList.Add (notes2);
		mDistance = ENEMY_X_DISTANCE*mList.Count;
		bean.mTargetX = note.x;
		bean.mTargetY = note.y;
		return true;
	}

	public bool removeLoacal(LocalBean bean){
		//Debug.Log ("removeLoacal");
		if(bean.mAttackLeng != mAttackLeng ){
			return false;
		}

		LocalNote[] targetNotes = null;
		int  targetNotesIndex =-1;
		int  targetindex =-1;

		for (int i = 0; i < mList.Count; i++) {
			targetNotes = (LocalNote[])mList [i];
			targetNotesIndex = i;
			for (int ii = 0; ii <targetNotes.Length; ii++) {
				if (targetNotes [ii]!=null &&targetNotes [ii].mLocal == bean) {
					targetindex = ii;
			//		Debug.Log ("removeLoacal i= "+i+" targetindex= "+ii);
					break;
				}
			}
			if (targetindex != -1) {
				break;
			}
		}

		if (targetindex == -1) {
			return false;
		}

		if (targetNotesIndex < (mList.Count - 1)) {
			int notesIndex = mList.Count - 1;
			LocalNote[] tmp =(LocalNote[]) mList [notesIndex];
			int leng = tmp.Length;
			for (int i = leng - 1; i >= 0; i--) {
				if (tmp [i] != null) {
				//	Debug.Log ("moveLoacal notesIndex= "+notesIndex+" i= "+i);
					targetNotes [targetindex].mLocal = tmp [i].mLocal;
					targetNotes [targetindex].mLocal.mTargetX = targetNotes [targetindex].x;
					targetNotes [targetindex].mLocal.mTargetY = targetNotes [targetindex].y;
					tmp [i] = null;
					if (i == 0) {
						mList.Remove (tmp);
						if (mList.Count == 0) {
							mManager.removeMultiple (this);
						}
			//			Debug.Log ("*mList.Count= "+mList.Count);
						mDistance = ENEMY_X_DISTANCE*mList.Count;
					}
					return true;
				}
			}
		} else {
			targetNotes [targetindex] = null;
			foreach (LocalNote note in targetNotes) {
				if (note != null) {
					return true;
				}
			}
			mList.Remove (targetNotes);
			mDistance = ENEMY_X_DISTANCE*mList.Count;
			if (mList.Count == 0) {
				mManager.removeMultiple (this);
			}
			return true;
		}
		return false;
	}
}

